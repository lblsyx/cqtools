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
    [Generator((int)GeneratorType.CPPServer, "C++代码", true)]
    public class CPPServerGenerator : IGenerator
    {
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

        public const string TEMPLATE_MGR_H = "\r\n" +
            "class TemplateMgr\r\n" +
            "{{\r\n" +
            "public:\r\n" +
            "    static void LoadTemplate();\r\n" +
            "    static void ReloadTemplate();\r\n" +
            "public:\r\n" +
            "{0}\r\n" +
            "    static const Conditiontemplate* pConditionTemplate;\r\n" +
            "    static const Conditiontwotemplate* pConditionTwoTemplate;\r\n" +
            "}};\r\n";

        public const string TEMPLATE_MGR_CPP = "\r\n" +
            "{0}\r\n" +
            "const Conditiontemplate* TemplateMgr::pConditionTemplate = nullptr;\r\n" +
            "const Conditiontwotemplate* TemplateMgr::pConditionTwoTemplate = nullptr;\r\n\r\n" +
            "void TemplateMgr::LoadTemplate()\r\n" +
            "{{\r\n" +
            "    if (SERVER_CONFIG.LoadFromDB == 0)\r\n" +
            "    {{\r\n" +
            "{1}" +
            "    }}\r\n" +
            "    else\r\n" +
            "    {{\r\n" +
            "{2}" +
            "    }}\r\n" +
            "    pConditionTemplate = ConditionTemplate.GetTMPL(1);\r\n" +
            "    pConditionTwoTemplate = ConditionTwoTemplate.GetTMPL(1);\r\n" +
            "}}\r\n\r\n" +
            "void TemplateMgr::ReloadTemplate()\r\n" +
            "{{\r\n" +
            "    if (SERVER_CONFIG.LoadFromDB == 0)\r\n" +
            "    {{\r\n" +
            "{3}" +
            "    }}\r\n" +
            "    else\r\n" +
            "    {{\r\n" +
            "{4}" +
            "    }}\r\n" +
            "    SEND_EVENT(EVT_RELOAD_TEMPLATE);\r\n" +
            "    SEND_EVENT(EVT_RELOAD_TEMPLATE_COMPLETE);\r\n" +
            "}};\r\n";

        public void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others)
        {
            StringBuilder sbHCode = new StringBuilder();
            StringBuilder sbCPPCode = new StringBuilder();

            IList<string> hCodeList = new List<string>();
            IList<string> cppCodeList = new List<string>();
            IList<string> hFieldCodeList = new List<string>();
            IList<string> cppFieldCodeList = new List<string>();
            IList<string> ctorFieldCodeList = new List<string>();

            //结构头文件(序列化/反序列化)
            StringBuilder sbSSHFile = new StringBuilder();
            //结构实现文件(序列化/反序列化)
            StringBuilder sbSCFile = new StringBuilder();

            //TemplateMgr代码
            StringBuilder sbTemplateMgrMembers = new StringBuilder();
            StringBuilder sbTemplateMgrMembersDef = new StringBuilder();
            StringBuilder sbTemplateMgrLoadJson = new StringBuilder();
            StringBuilder sbTemplateMgrReloadJson = new StringBuilder();
            StringBuilder sbTemplateMgrLoadDB = new StringBuilder();
            StringBuilder sbTemplateMgrReloadDB = new StringBuilder();

            for (int i = 0; i < schemas.Count; i++)
            {
                TableInfo table = schemas[i];
                string tn = table.TableName.Substring(0, 1).ToUpper() + table.TableName.Substring(1).ToLower();
                string tn2 = table.TableName;
                tn2 = tn2.Replace("template", "Template");

                sbTemplateMgrMembers.Append(string.Format("    static TMPLMode<{0}> {1};\r\n", tn, tn2));
                sbTemplateMgrMembersDef.Append(string.Format("TMPLMode<{0}> TemplateMgr::{1};\r\n", tn, tn2));
                sbTemplateMgrLoadJson.Append(string.Format("        TEMPLATE_LOAD_FROM_JSON({0});\r\n", tn2));
                sbTemplateMgrReloadJson.Append(string.Format("        TEMPLATE_RELOAD_FROM_JSON({0});\r\n", tn2));
                sbTemplateMgrLoadDB.Append(string.Format("        TEMPLATE_LOAD_FROM_DB({0});\r\n", tn2));
                sbTemplateMgrReloadDB.Append(string.Format("        TEMPLATE_RELOAD_FROM_DB({0});\r\n", tn2));

                hFieldCodeList.Clear();
                cppFieldCodeList.Clear();
                ctorFieldCodeList.Clear();
                sbSSHFile.Clear();
                sbSCFile.Clear();

                //结构体反序列化JSON运算符重载
                sbSSHFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, {0}& o{0});\r\n", tn));
                sbSSHFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, {0}& o{0});\r\n\r\n", tn));

                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    hFieldCodeList.Add(string.Format(CLASS_FIELD_FORMAT, field.FieldName, GetCPPType(field.FieldType), GetCPPSummary(field.FieldSummary)));
                    cppFieldCodeList.Add(string.Format(CLASS_IMP_FORMAT, tn, field.FieldName, GetDBType(field.FieldType)));
                    ctorFieldCodeList.Add(ColumnTypeDefaultValue(field.FieldName, field.FieldType));
                }

                //结构体反序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, {0}& o{0})", tn));
                sbSCFile.Append("{\r\n");
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //反序列化代码
                    string serfield = CodeUtil.FieldData2CPPDeserializeJson(field, string.Format("o{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");

                //结构体序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, {0}& o{0})", tn));
                sbSCFile.Append("{\r\n");
                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    //序列化代码
                    string serfield = CodeUtil.FieldData2CPPSerializeJson(field, string.Format("o{0}.", tn), 4);

                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");

                string hExcelName = Path.GetFileName(table.ExcelFile);
                string hFieldCode = string.Join(CLASS_H_SPLITTER, hFieldCodeList.ToArray());
                string cppFieldCode = string.Join(CLASS_CPP_SPLITTER, cppFieldCodeList.ToArray());
                string ctorFieldCode = string.Join(CLASS_CPP_SPLITTER, ctorFieldCodeList.ToArray());

                hCodeList.Add(string.Format(CLASS_H_SUMMARY, tn, hExcelName));
                hCodeList.Add(string.Format(CLASS_H_FORMAT, tn, hFieldCode));
                hCodeList.Add(sbSSHFile.ToString());
                cppCodeList.Add(string.Format(CLASS_CTOR_FORMAT, tn, ctorFieldCode));
                cppCodeList.Add(string.Format(CLASS_CPP_FORMAT, tn, cppFieldCode));
                cppCodeList.Add(sbSCFile.ToString());
            }

            hCodeList.Add(string.Format(TEMPLATE_MGR_H, sbTemplateMgrMembers.ToString()));
            cppCodeList.Add(string.Format(TEMPLATE_MGR_CPP, sbTemplateMgrMembersDef.ToString(), sbTemplateMgrLoadJson.ToString(), sbTemplateMgrLoadDB.ToString(), sbTemplateMgrReloadJson.ToString(), sbTemplateMgrReloadDB.ToString()));

            string hCode = string.Join("\r\n", hCodeList.ToArray());
            string cppCode = string.Join("\r\n", cppCodeList.ToArray());

            string hFileContent;
            string cppFileContent;
            if (Global.args.Length > 0)
            {
                hFileContent = ToolConfig.Instance.CodeFileContentFormat;
                cppFileContent = ToolConfig.Instance.CodeFileContentFormat2;
            }
            else
            {
                MainForm mainForm = others as MainForm;
                hFileContent = mainForm.codeFormatTextBox.Text;
                cppFileContent = mainForm.codeFormat2TextBox.Text;
            }
            hFileContent = hFileContent.Replace(Global.CodeReplaceString, hCode);
            hFileContent += "\r\n";
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

                if (SeqRun.Instance.isRunning())
                {
                    SeqRun.Instance.onCurTaskFinished();
                }
                else
                {
                    GenerateParameter param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.CPPServer);
                    if (param != null && param.AutoCommit)
                    {
                        string[] paths = new string[2] { param.CodeFileOutputPath, param.CodeFileOutputPath2 };
                        SvnUtil.CoverCommit(paths);
                    }
                    MessageBox.Show("C++代码生成成功！");
                }
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }
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
