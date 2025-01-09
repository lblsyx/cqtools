#if !USE_SCRIPT
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityLight.Loggers;
using CSScriptApp.TemplateCore;

namespace CSScriptApp.Scripts.PackTemplates
{
    [DataPacker((int)PackerType.MySQLPacker, "MySQL上传", false)]
    public class MySQLDataPacker : IDataPacker
    {
        private const string MYSQL_COLUMN_SPLITTER = ",\r\n  ";

        /// <summary>
        /// {0}：列名
        /// {1}：类型
        /// {2}：说明
        /// </summary>
        private const string MYSQL_COLUMN_FORMAT = "`{0}` {1} NOT NULL COMMENT '{2}'";

        public void PackData(IList<TableInfo> schemas, string connStr, object others)
        {
            string db = "arpg_tmpldb";

            MySqlConnection conn = MySQLUtil.OpenMySQLConnection(connStr);
            
            if (conn == null) return;

            MySQLUtil.CreateDatabaseIfNotExists(conn, db);

            MySQLUtil.DropTables(conn, db, true);

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

                if (MySQLUtil.CreateTable(conn, db, table, columnStr) == false)
                {
                    XLogger.ErrorFormat("数据表创建失败!`{0}`.`{1}`", db, table.TableName);
                    continue;
                }

                XLogger.InfoFormat("数据表 {0}.{1} 创建成功!", db, table.TableName);

                uint count = 0;
                for (int i = 0; i < table.RowCount; i++)
                {
                    fieldValList.Clear();
                    foreach (FieldInfo field in table.TableFields)
                    {
                        string val = MySQLUtil.AddSlashes(Convert.ToString(field.FieldValues[i]));
                        fieldValList.Add(string.Format("'{0}'", val));
                    }

                    string insertValueStr = string.Join(",", fieldValList.ToArray());
                    if (MySQLUtil.InsertTableRecord(conn, db, table, insertValueStr))
                    {
                        count += 1;
                    }
                }

                XLogger.InfoFormat("成功插入 {0}/{1} 条数据到{2}.{3}数据表中!", count, table.RowCount, db, table.TableName);
            }

            Program.WriteToConsole("上传到MySQL数据库完成,结果请查看日志框!");
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
#endif