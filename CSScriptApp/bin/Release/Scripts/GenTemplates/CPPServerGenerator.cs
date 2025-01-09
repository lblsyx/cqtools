#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityLight.Loggers;
using CSScriptApp.TemplateCore;

namespace CSScriptApp.Scripts.GenTemplates
{
    [Generator((int)GeneratorType.CPPServer, "C++代码", true)]
    public class CPPServerGenerator : IGenerator
    {
        public const string CodeFormat1 = @"#ifndef DB_STRUCTS_ARPG_TMPLDB_H_\r\n#define DB_STRUCTS_ARPG_TMPLDB_H_\r\n\r\n__CodeContent__\r\n\r\n#endif";

        public const string CodeFormat2 = "#include \"stdafx.h\"\r\n\r\n__CodeContent__";

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
            "DB_TABLE_BEGIN({0})\r\n" +
            "    {1}\r\n" +
            "DB_TABLE_END({0})\r\n";
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
        /// {1}：字段名
        /// {2}：数据库对应类型
        /// </summary>
        public const string CLASS_IMP_FORMAT = "DB_FIELD({0}, {1}, {2})";
        public const string CLASS_CPP_SPLITTER = "\r\n    ";

        public void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others)
        {
            StringBuilder sbHCode = new StringBuilder();
            StringBuilder sbCPPCode = new StringBuilder();

            IList<string> hCodeList = new List<string>();
            IList<string> cppCodeList = new List<string>();
            IList<string> hFieldCodeList = new List<string>();
            IList<string> cppFieldCodeList = new List<string>();

            for (int i = 0; i < schemas.Count; i++)
            {
                TableInfo table = schemas[i];
                string tn = table.TableName.Substring(0, 1).ToUpper() + table.TableName.Substring(1).ToLower();

                hFieldCodeList.Clear();
                cppFieldCodeList.Clear();
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    hFieldCodeList.Add(string.Format(CLASS_FIELD_FORMAT, field.FieldName, GetCPPType(field.FieldType), GetCPPSummary(field.FieldSummary)));
                    cppFieldCodeList.Add(string.Format(CLASS_IMP_FORMAT, tn, field.FieldName, GetDBType(field.FieldType)));
                }

                string hFieldCode = string.Join(CLASS_H_SPLITTER, hFieldCodeList.ToArray());
                string cppFieldCode = string.Join(CLASS_CPP_SPLITTER, cppFieldCodeList.ToArray());

                hCodeList.Add(string.Format(CLASS_H_FORMAT, tn, hFieldCode));
                cppCodeList.Add(string.Format(CLASS_CPP_FORMAT, tn, cppFieldCode));
            }
            
            string hCode = string.Join("\r\n", hCodeList.ToArray());
            string cppCode = string.Join("\r\n", cppCodeList.ToArray());

            string hFileContent = CodeFormat1;
            hFileContent = hFileContent.Replace(Global.CodeReplaceString, hCode);
            hFileContent += "\r\n";
            string cppFileContent = CodeFormat2;
            cppFileContent = cppFileContent.Replace(Global.CodeReplaceString, cppCode);
            cppFileContent += "\r\n";

            try
            {
                using (StreamWriter sw = new StreamWriter(outPath, false, Encoding.Default))
                {
                    sw.Write(hFileContent);
                }

                using (StreamWriter sw = new StreamWriter(outPath2, false, Encoding.Default))
                {
                    sw.Write(cppFileContent);
                }

                Program.WriteToConsole("C++模板代码生成成功！");
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }
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
#endif