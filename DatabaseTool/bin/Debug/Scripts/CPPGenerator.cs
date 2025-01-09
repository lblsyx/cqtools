using DatabaseTool.DatabaseCore;
using DatabaseTool.DatabaseCore.Generates;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool.Scripts
{
    [StructGenerator(true)]
    public class CPPGenerator : IStructGenerator
    {
        public string Type
        {
            get { return "C++ServerMappingCode"; }
        }

        public string Name
        {
            get { return "C++映射代码"; }
        }

        public string ReplaceStr
        {
            get { return "$$CodeContent$$"; }
        }

        public string OutputFilter1
        {
            get { return "C++头文件(*.h;*.hpp)|*.h;*.hpp"; }
        }

        public PathType OutputPathType1
        {
            get { return PathType.FilePath; }
        }

        public string OutputFilter2
        {
            get { return "C++代码文件(*.c;*.cpp)|*.c;*.cpp"; }
        }

        public PathType OutputPathType2
        {
            get { return PathType.FilePath; }
        }

        public void Generate(DBInfo oDBInfo, TBInfo[] oTBInfos, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2)
        {
            if (oDBInfo == null)
            {
                MessageBox.Show("请选择要映射的数据库");
                return;
            }

            if (string.IsNullOrEmpty(sPath1))
            {
                MessageBox.Show("请选择C++数据库映射头文件");
                return;
            }

            if (string.IsNullOrEmpty(sPath2))
            {
                MessageBox.Show("请选择C++数据库映射代码文件");
                return;
            }

            //头文件代码
            StringBuilder sbHFile = new StringBuilder();
            //实现文件代码
            StringBuilder sbCFile = new StringBuilder();
            //构造函数代码
            StringBuilder sbCtorCode = new StringBuilder();
            //字段信息定义代码
            StringBuilder sbFieldCode = new StringBuilder();
            //二进制数据字段绑定代码
            StringBuilder sbBindCode = new StringBuilder();
            //二进制数据字段数量
            int nBinCount = 0;

            ////预编译头
            //sbHFile.Append(string.Format("#ifndef DB_STRUCTS_{0}_H_\r\n#define DB_STRUCTS_{0}_H_\r\n\r\n", oDBInfo.DBName.ToUpper()));
            ////引用头文件
            //sbCFile.Append("#include \"stdafx.h\"\r\n\r\n");

            IList<string> tblist = new List<string>();

            foreach (var item in oTBInfos)
            {
                if (item.Selected)
                {
                    tblist.Add(item.TBName);
                }
            }

            string sql = string.Empty;

            foreach (var item in tblist)
            {
                string clsname = CodeUtil.TBName2CLSName(item);

                //头文件代码
                sbHFile.Append(string.Format("DB_TABLE_BEGIN({0})\r\n", clsname));
                //构造函数代码
                sbCtorCode.Clear();
                sbCtorCode.Append(string.Format("DB_CONSTRUCTOR_BEGIN({0})\r\n", clsname));
                //字段信息定义代码
                sbFieldCode.Clear();
                sbFieldCode.Append(string.Format("DB_FIELD_BEGIN({0})\r\n", clsname));
                //二进制数据字段绑定代码
                sbBindCode.Clear();
                //二进制数据字段绑定代码
                nBinCount = 0;

                //查询字段列表
                sql = string.Format("SELECT `COLUMN_NAME`,`DATA_TYPE`,`CHARACTER_MAXIMUM_LENGTH`,`COLUMN_TYPE`,`COLUMN_DEFAULT`,`IS_NULLABLE`,`COLUMN_KEY`,`COLUMN_COMMENT`,`EXTRA` FROM `information_schema`.`COLUMNS` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='{1}'", oDBInfo.DBName, item);
                MySqlDataReader reader = DBHelper.ExecuteReader(sql);

                if (reader != null)
                {
                    try
                    {
                        while (reader.Read())
                        {
                            string columnname = reader.GetString(0);
                            //头文件代码
                            string unsign = reader.GetString(3).ToLower();
                            string cppfield = CodeUtil.ColumnField2CPPField(columnname, reader.GetString(1), unsign.IndexOf("unsigned") != -1, reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                            sbHFile.Append(string.Format("    /* {0} */\r\n", reader.IsDBNull(7) ? "" : reader.GetString(7)));
                            sbHFile.Append(string.Format("    {0}\r\n", cppfield));

                            //构造函数代码
                            string cppassign = CodeUtil.ColumnTypeDefaultValue(columnname, reader.GetString(1), reader.IsDBNull(2) ? 0 : reader.GetInt32(2));
                            sbCtorCode.Append(string.Format("    {0}\r\n", cppassign));

                            //字段信息定义代码
                            string sqltype = CodeUtil.ColumnType2CPPSQLType(reader.GetString(1), reader.GetString(3), reader.IsDBNull(8) ? "" : reader.GetString(8), reader.GetString(6));
                            sbFieldCode.Append(string.Format("    DB_FIELD({0}, {1}, {2})\r\n", clsname, columnname, sqltype));

                            if (CodeUtil.IsBinColumn(reader.GetString(1)))
                            {
                                sbBindCode.Append(string.Format("    Binds[{0}].buffer_type = MYSQL_TYPE_BLOB;\r\n", nBinCount));
                                sbBindCode.Append(string.Format("    Binds[{0}].buffer = {1}.data();\r\n", nBinCount, columnname));
                                sbBindCode.Append(string.Format("    Binds[{0}].buffer_length = {1}.size();\r\n", nBinCount, columnname));
                                nBinCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        reader.Close();
                        MessageBox.Show(ex.Message);
                    }

                }

                if (nBinCount > 0)
                {
                    sbHFile.Append(string.Format("#ifdef MYSQL_BIN_BIND\r\npublic:\r\n    MYSQL_BIND* BinVariableBind() override;\r\n#endif\r\n"));
                }
                //头文件代码
                sbHFile.Append(string.Format("DB_TABLE_END({0})\r\n\r\n", clsname));
                //二进制数据字段绑定代码
                if (nBinCount > 0)
                {
                    sbCtorCode.Append(string.Format("#ifdef MYSQL_BIN_BIND\r\n    Binds.resize({0});\r\n{1}#endif\r\n", nBinCount, sbBindCode.ToString()));
                }
                //字段信息定义代码
                sbCtorCode.Append(string.Format("DB_CONSTRUCTOR_END({0})\r\n\r\n", clsname));
                //字段信息定义代码
                sbFieldCode.Append(string.Format("DB_FIELD_END({0})\r\n\r\n", clsname));

                //实现文件代码
                sbCFile.Append(sbCtorCode.ToString());
                sbCFile.Append(sbFieldCode.ToString());
                if (nBinCount > 0)
                {
                    sbCFile.Append(string.Format("#ifdef MYSQL_BIN_BIND\r\nMYSQL_BIND* {0}::BinVariableBind()\r\n{{\r\n{1}    return Binds.data();\r\n}}\r\n#endif\r\n\r\n", clsname, sbBindCode.ToString()));
                }
            }

            //sbHFile.Append("\r\n\r\n#endif");

            using (FileStream fStream = new FileStream(sPath1, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.CodeContentFormat1.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sbHFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }

            using (Stream fStream = new FileStream(sPath2, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.CodeContentFormat2.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sbCFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }
        }
    }
}
