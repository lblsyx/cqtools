using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using TemplateTool.Utils;
using UnityLight.Loggers;
using TemplateTool.Scripts;

namespace TemplateTool.Gens.Generators
{
    [Generator((int)GeneratorType.JsonServer, "Json代码", true)]
    public class JsonServerGenerator : IGenerator
    {
        public string ReplaceStr
        {
            get { return "$$CodeContent$$"; }
        }

        public bool UseFolder
        {
            get { return false; }
        }

        public bool RequireSecondCode
        {
            get { return true; }
        }

        /// <summary>
        /// {0}：类名
        /// {1}：字段声明代码
        /// </summary>
        public const string CLASS_H_FORMAT =
            "typedef struct {0} \r\n" +
            "    {1}\r\n" +
            "{0}\r\n";
        /// <summary>
        /// {0}：字段名
        /// {1}：字段类型
        /// {2}：字段说明
        /// </summary>
        public const string CLASS_FIELD_FORMAT =
            "/* {2} */\r\n" +
            "    {1} {0};";
        public const string CLASS_H_SPLITTER = "\r\n    ";

        /// <summary>
        /// {0}：类名
        /// {1}：字段映射代码
        /// </summary>
        public const string CLASS_CPP_FORMAT =
            "DB_FIELD_BEGIN({0})\r\n" +
            "    {1}\r\n" +
            "DB_FIELD_END({0})\r\n";
        /// <summary>
        /// {0}：类名
        /// {1}：构造函数代码
        /// </summary>
        public const string CLASS_CTOR_FORMAT =
            "DB_CONSTRUCTOR_BEGIN({0})\r\n" +
            "    {1}\r\n" +
            "DB_CONSTRUCTOR_END({0})\r\n";
        /// <summary>
        /// {0}：类名
        /// {1}：字段名
        /// {2}：数据库对应类型
        /// </summary>
        public const string CLASS_IMP_FORMAT = "DB_FIELD({0}, {1}, {2})";
        public const string CLASS_CPP_SPLITTER = "\r\n    ";
        public const string CLASS_H_SUMMARY = "/* [{1}] */";


        public void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others)
        {
            StringBuilder sbHCode = new StringBuilder();
            StringBuilder sbCPPCode = new StringBuilder();

            string[] splitter = new string[] { "\r\n" };

            //结构头文件
            StringBuilder sbSHFile = new StringBuilder();
            //结构头文件(序列化/反序列化)
            StringBuilder sbSSHFile = new StringBuilder();
            //结构实现文件(序列化/反序列化)
            StringBuilder sbSCFile = new StringBuilder();
            //结构体重置
            StringBuilder sbReset = new StringBuilder();

            for (int i = 0; i < schemas.Count; i++)
            {

                TableInfo table = schemas[i];
                string tn = table.TableName.Substring(0, 1).ToUpper() + table.TableName.Substring(1).ToLower();
                
                //结构体重置
                sbReset.Clear();
                sbReset.Append("    void reset()\r\n");
                sbReset.Append("    {\r\n");
                //结构体声明
                int beginPos = schemas[i].ExcelFile.LastIndexOf("\\");
                int endPos = schemas[i].ExcelFile.Length - beginPos - 1;                
                string fileName = schemas[i].ExcelFile.Substring(beginPos + 1, endPos);
                sbSHFile.Append("/* ").Append(fileName).Append(" */").Append("\r\n");
                sbSHFile.Append(string.Format("typedef struct ST{0}\r\n", tn));
                sbSHFile.Append("{\r\n");
                sbSHFile.Append(string.Format("    ST{0}()\r\n", tn));
                sbSHFile.Append("    {\r\n");
                sbSHFile.Append("        reset();\r\n");
                sbSHFile.Append("    }\r\n");
                //foreach (var fielditem in stitem.Fields)
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //声明代码
                    string cppfield = CodeUtil.FieldData2CPPField(field, 4);
                    sbSHFile.Append(string.Format("{0}\r\n", cppfield));
                    //重置代码
                    string ctorfield = CodeUtil.FieldData2CtorField(field, 8);
                    sbReset.Append(string.Format("{0}\r\n", ctorfield));
                }
                sbReset.Append("    }\r\n");
                sbSHFile.Append(sbReset.ToString());
                sbSHFile.Append("}");
                sbSHFile.Append(string.Format(" {0}, *LP{0};\r\n\r\n", tn));

                //结构体序列化/反序列化运算符重载
                sbSSHFile.Append(string.Format("ServerLight::ByteArray& operator >> (ServerLight::ByteArray& stream, ST{0}& oST{0});\r\n", tn));
                sbSSHFile.Append(string.Format("ServerLight::ByteArray& operator << (ServerLight::ByteArray& stream, ST{0}& oST{0});\r\n\r\n", tn));
                sbSSHFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, ST{0}& oST{0});\r\n", tn));
                sbSSHFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, ST{0}& oST{0});\r\n\r\n", tn));

                //结构体序反列化运算符重载
                sbSCFile.Append(string.Format("ServerLight::ByteArray& operator >> (ServerLight::ByteArray& stream, ST{0}& oST{0})\r\n", tn));
                sbSCFile.Append("{\r\n");
                
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    
                    //反序列化代码
                    string serfield = CodeUtil.FieldData2CPPDeserializeBin(field, string.Format("oST{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return stream;\r\n}\r\n\r\n");

                //结构体序列化运算符重载
                sbSCFile.Append(string.Format("ServerLight::ByteArray& operator << (ServerLight::ByteArray& stream, ST{0}& oST{0})\r\n", tn));
                sbSCFile.Append("{\r\n");
                
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //序列化代码
                    string serfield = CodeUtil.FieldData2CPPSerializeBin(field, string.Format("oST{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return stream;\r\n}\r\n\r\n");

                //结构体反序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, ST{0}& oST{0})", tn));
                sbSCFile.Append("{\r\n");

                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //反序列化代码
                    string serfield = CodeUtil.FieldData2CPPDeserializeJson(field, string.Format("oST{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");

                //结构体序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, ST{0}& oST{0})", tn));
                sbSCFile.Append("{\r\n");

                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //序列化代码
                    string serfield = CodeUtil.FieldData2CPPSerializeJson(field, string.Format("oST{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");

            }

            using (Stream fStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);

                StringBuilder sb = new StringBuilder();
                sb.Append("#ifndef DB_STRUCTS_ARPG_TMPLDB_H_\r\n").Append("#define DB_STRUCTS_ARPG_TMPLDB_H_\r\n\r\n");
               
                sb.Append(sbSHFile.ToString());
                sb.Append(sbSSHFile.ToString());
                sb.Append("\r\n#endif");
               
                //string content = oGeneratorSetting.ContentFormat1.Replace("\n", "\r\n");
                string content = sb.ToString();// content.Replace(ReplaceStr, sb.ToString());

                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }

            using (Stream fStream = new FileStream(outPath2, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);

                StringBuilder sb = new StringBuilder();
                sb.Append("#include \"stdafx.h\"\r\n\r\n");
                
                sb.Append(sbSCFile.ToString());

                //string content = oGeneratorSetting.ContentFormat2.Replace("\n", "\r\n");
                //content = content.Replace(ReplaceStr, sb.ToString());
                string content = sb.ToString();
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }

            MessageBox.Show("[Json]C++代码生成成功！");

        }

        private string ColumnTypeDefaultValue(string columnName, string columnType)
        {
            switch (columnType.ToLower())
            {
                case "bit":
                case "bool":
                    return string.Format("{0} = false;", columnName);
                case "int":
                case "int64":
                case "uint":
                case "short":
                case "ushort":
                case "byte":
                case "sbyte":
                case "bigint":
                case "integer":
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "float":
                case "double":
                case "numeric":
                case "decimal":
                case "date":
                case "datetime":
                case "timestamp":
                    return string.Format("{0} = 0;", columnName);
                case "text":
                case "string":
                case "tinytext":
                case "longtext":
                case "mediumtext":
                case "binary":
                    return string.Format("{0} = \"\";", columnName);
            }

            return string.Format("{0} = unknow;", columnName);
        }

        private string GetCPPSummary(string summary)
        {
            string[] list = summary.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string rlt = string.Join("\r\n    ", list);
            return rlt;
        }

        private string GetCPPType(string fieldType)
        {
            switch (fieldType)
            {
                case "bool":
                case "boolean":
                    return "bool";
                case "sbyte":
                case "short":
                case "int":
                    return "int";
                case "byte":
                case "ushort":
                case "uint":
                    return "uint";
                case "float":
                    return "float";
                case "int64":
                case "long":
                    return "int64";
                case "uint64":
                case "ulong":
                    return "uint64";
                case "double":
                    return "double";
                case "utf":
                case "string":
                case "varchar":
                case "text":
                case "longtext":
                    return "std::string";
                default:
                    return fieldType;
            }
        }

        private string GetDBType(string fieldType)
        {
            switch (fieldType)
            {
                case "bool":
                case "boolean":
                    return "eFieldTypeBool";
                case "sbyte":
                case "short":
                case "int":
                    return "eFieldTypeInt";
                case "byte":
                case "ushort":
                case "uint":
                    return "eFieldTypeUInt";
                case "float":
                    return "eFieldTypeFloat";
                case "int64":
                case "long":
                    return "eFieldTypeInt64";
                case "uint64":
                case "ulong":
                    return "eFieldTypeUInt64";
                case "double":
                    return "eFieldTypeDouble";
                case "utf":
                case "string":
                case "varchar":
                case "text":
                case "longtext":
                    return "eFieldTypeText";
                default:
                    return "eFieldTypeUnknow";
            }
        }
    }
}
