using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using TemplateTool.Utils;
using UnityLight.Loggers;
using System.Threading;

namespace TemplateTool.Packs.Packers
{
    [DataPacker((int)PackerType.MySQLPacker, "MySQL上传", false)]
    public class MySQLDataPacker : IDataPacker
    {
        public MySQLDataPacker()
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(1000, 1000);
            Control.CheckForIllegalCrossThreadCalls = false; //加载时 取消跨线程检查

        }
        private const string MYSQL_COLUMN_SPLITTER = ",\r\n  ";

        /// <summary>
        /// {0}：列名
        /// {1}：类型
        /// {2}：说明
        /// </summary>
        private const string MYSQL_COLUMN_FORMAT = "`{0}` {1} NOT NULL COMMENT '{2}'";

        public MySqlConnection _conn = null;
        public string _db = null;
        public string _connStr = null;

        public bool initData(string connStr, object others,bool needDelete)
        {
            MainForm mainForm = others as MainForm;
            _db = mainForm.dbTextBox.Text.Trim();
            _connStr = connStr;
            _conn = MySQLUtil.OpenMySQLConnection(connStr);
 
            if (_conn == null) return false;
            MySQLUtil.CreateDatabaseIfNotExists(_conn, _db);
            if (needDelete)
            {
                MySQLUtil.DropTables(_conn, _db, true);
            }
            return true;
        }
        public void PackData(IList<TableInfo> schemas, string connStr, object others, string errorlogPath)
        {
            //XLogger.InfoFormat("PackData :", schemas.Count);

            if (_conn == null) return;
            var tableName = others as string;
            if (tableName != null)
            {
                MySQLUtil.DropTable(_conn, _db, tableName);
            }
            //创建表结构和写入数据
            List<string> columnList = new List<string>();
            List<string> fieldValList = new List<string>();
            
            foreach (TableInfo table in schemas)
            {
                columnList.Clear();
                foreach (FieldInfo field in table.TableFields)
                {
                    columnList.Add(GetMySQLColumnString(table.SheetName, field));
                }
                string columnStr = string.Join(MYSQL_COLUMN_SPLITTER, columnList.ToArray());

                if (MySQLUtil.CreateTable(_conn, _db, table, columnStr) == false)
                {
                    XLogger.ErrorFormat("数据表创建失败!`{0}`.`{1}`", _db, table.TableName);
                    continue;
                }

                //XLogger.InfoFormat("数据表 {0}.{1} 创建成功!", _db, table.TableName);
                ThreadPool.QueueUserWorkItem(new WaitCallback(InsertDataToTable), table);

            }
            //XLogger.InfoFormat("上传中：{0}", schemas.Count);
            
        }
        private void InsertDataToTable(object t)
        {
            TableInfo table = t as TableInfo;
            uint count = 0;
            using (var conn = MySQLUtil.OpenMySQLConnection(_connStr))
            {
                List<string> fieldValList = new List<string>();
                for (int i = 0; i < table.RowCount; i++)
                {
                    fieldValList.Clear();
                    foreach (FieldInfo field in table.TableFields)
                    {
                        string val = MySQLUtil.AddSlashes(Convert.ToString(field.FieldValues[i]));
                        fieldValList.Add(string.Format("'{0}'", val));
                    }
                    string insertValueStr = string.Join(",", fieldValList.ToArray());
                    //XLogger.InfoFormat("InsertDataToTable：{0}", _connStr);
                    if (MySQLUtil.InsertTableRecord(conn, _db, table, insertValueStr))
                    {
                        count += 1;
                    }
                    else
                    {
                        ExcelUtil.StopUpload();
                        XLogger.InfoFormat("插入失败，终止");
                        break;
                    }
                }
            }
            //XLogger.InfoFormat("成功插入 {0}/{1} 条数据到{2}.{3}数据表中!", count, table.RowCount, _db, table.TableName);
        }
        private string GetMySQLColumnString(string sheetName, FieldInfo field)
        {
            string type = string.Empty;

            switch (field.FieldType)
            {
                case "sbyte":
                case "short":
                case "int":
                    type = "int";
                    break;
                case "byte":
                case "uint":
                case "ushort":
                    type = "int unsigned";
                    break;
                case "int64":
                    type = "bigint";
                    break;
                case "uint64":
                    type = "bigint unsigned";
                    break;
                case "float":
                    type = "float";
                    break;
                case "double":
                    type = "double";
                    break;
                case "varchar":
                    type = "varchar(255)";
                    break;
                case "string":
                    type = "text";
                    break;
                default:
                    {
                        XLogger.ErrorFormat("无法识别的类型，默认使用text类型\r\nSheetName：{0}，FieldName：{1}，FieldType：{2}", sheetName, field.FieldName, field.FieldType);
                        type = "text";
                    }
                    break;
            }

            return string.Format(MYSQL_COLUMN_FORMAT, field.FieldName, type, MySQLUtil.AddSlashes(field.FieldSummary));
        }
    }
}
