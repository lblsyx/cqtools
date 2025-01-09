using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using TemplateTool.Utils;
using UnityLight.Loggers;

namespace TemplateTool.Gens.Generators
{
    [Generator((int)GeneratorType.Unity3DClient, "Unity3D代码", true)]
    public class Unity3DClientGenerator : IGenerator
    {
        /// <summary>
        /// {0}：模板类名
        /// {1}：字段代码
        /// {2}：读取代码
        /// {3}：写入代码
        /// __LK__替换为 {
        /// __RK__替换为 }
        /// </summary>
        public const string TPL_CLASS_FORMAT =
            "public class {0} : Tpl\r\n" +
            "    __LK__\r\n" +
            "        {1}\r\n\r\n" +

            "        public override void ReadFrom(ByteArray bytes)\r\n" +
            "        __LK__\r\n" +
            "            base.ReadFrom(bytes);\r\n" +
            "            {2}\r\n" +
            "        __RK__\r\n\r\n" +

            "        public override void WriteTo(ByteArray bytes)\r\n" +
            "        __LK__\r\n" +
            "            base.WriteTo(bytes);\r\n" +
            "            {3}\r\n" +
            "        __RK__\r\n" +
            "    __RK__\r\n\r\n" +
            "    public class {0}Mode : TplMode\r\n" +
            "    __LK__\r\n" +
            "        private IList<{0}> mList = null;\r\n\r\n" +

            "        public {0}Mode(int step) : base(\"{0}\", typeof({0}), step) __LK__ __RK__\r\n\r\n" +
            
            "        public {0} this[int id]\r\n" +
            "        __LK__\r\n" +
            "            get __LK__ return Find<{0}>(id); __RK__\r\n" +
            "        __RK__\r\n\r\n" +

            "        public {0} Find(int id)\r\n" +
            "        __LK__\r\n" +
            "            return Find<{0}>(id);\r\n" +
            "        __RK__\r\n\r\n" +

            "        public IList<{0}> FindAll()\r\n" +
            "        __LK__\r\n" +
            "            if (mList == null) mList = FindAll<{0}>();\r\n" +
            "            return mList;\r\n" +
            "        __RK__\r\n" +
            "    __RK__\r\n";

        /// <summary>
        /// {0}：模板模型全局静态变量代码
        /// </summary>
        public const string TEMPLATES_CLASS_FORMAT =
            "    [TplSearchable]\r\n" +
            "    public class Templates\r\n" +
            "    __LK__\r\n" +
            "        {0}\r\n" +
            "    __RK__\r\n";

        /// <summary>
        /// 字段列表分隔符
        /// </summary>
        public const string TPL_FIELD_SPLITTER = "\r\n        ";

        /// <summary>
        /// {0}：字段名称
        /// {1}：字段类型
        /// {2}：字段说明
        /// </summary>
        public const string TPL_FIELD_FORMAT =
            "/// <summary>\r\n        " +
            "/// {2}\r\n        " +
            "/// </summary>\r\n        " +
            "public {1} {0};";


        /// <summary>
        /// 读写列表分隔符
        /// </summary>
        public const string TPL_READWRITE_SPLITTER = "\r\n            ";

        /// <summary>
        /// {0}：字段名称
        /// {1}：字段类型对应的读取方法
        /// </summary>
        public const string TPL_READ_FORMAT = "{0} = bytes.Read{1}();";
        /// <summary>
        /// {0}：字段名称
        /// {1}：字段类型对应的读取方法
        /// </summary>
        public const string TPL_WRITE_FORMAT = "bytes.Write{1}({0});";

        /// <summary>
        /// 模板模型变量列表分隔符
        /// </summary>
        public const string TPL_MODE_VAR_SPLITTER = "\r\n        ";

        /// <summary>
        /// {0}：模板类名
        /// </summary>
        public const string TPL_MODE_VAR_FORMAT = "public static {0}Mode {0}s = new {0}Mode(200);";

        public bool UseFolder
        {
            get { return false; }
        }

        public bool RequireSecondCode
        {
            get { return false; }
        }

        public void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others)
        {
            StringBuilder sbTPLModeCode = new StringBuilder();
            StringBuilder sbTPLClassCode = new StringBuilder();

            List<string> fieldList = new List<string>();
            List<string> readList = new List<string>();
            List<string> writeList = new List<string>();
            List<string> modeVarList = new List<string>();
            string fieldCode = string.Empty;
            string readCode = string.Empty;
            string writeCode = string.Empty;
            string modeVarCode = string.Empty;

            for (int i = 0; i < schemas.Count; i++)
            {
                TableInfo table = schemas[i];
                fieldList.Clear();
                readList.Clear();
                writeList.Clear();

                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];
                    if (field.FieldName == "TID") continue;
                    fieldList.Add(string.Format(TPL_FIELD_FORMAT, field.FieldName, GetFieldType(field.FieldType), GetFieldSummary(field.FieldSummary)));
                    readList.Add(string.Format(TPL_READ_FORMAT, field.FieldName, GetReadWriteMethodName(field.FieldType)));
                    writeList.Add(string.Format(TPL_WRITE_FORMAT, field.FieldName, GetReadWriteMethodName(field.FieldType)));
                }
                modeVarList.Add(string.Format(TPL_MODE_VAR_FORMAT, table.TableName));

                fieldCode = string.Join(TPL_FIELD_SPLITTER, fieldList.ToArray());
                readCode = string.Join(TPL_READWRITE_SPLITTER, readList.ToArray());
                writeCode = string.Join(TPL_READWRITE_SPLITTER, writeList.ToArray());

                if (sbTPLClassCode.Length != 0) sbTPLClassCode.Append("\r\n");
                sbTPLClassCode.Append(string.Format(TPL_CLASS_FORMAT, table.TableName, fieldCode, readCode, writeCode));
            }
            modeVarCode = string.Join(TPL_MODE_VAR_SPLITTER, modeVarList.ToArray());

            sbTPLModeCode.Append(string.Format(TEMPLATES_CLASS_FORMAT, modeVarCode));

            string allCode = string.Format("{0}\r\n{1}", sbTPLClassCode.ToString(), sbTPLModeCode.ToString());
            allCode = allCode.Replace("__LK__", "{");
            allCode = allCode.Replace("__RK__", "}");

            MainForm mainForm = others as MainForm;
            string fileContent = mainForm.codeFormatTextBox.Text;
            fileContent = fileContent.Replace(Global.CodeReplaceString, allCode);
            fileContent += "\r\n";

            try
            {
                using (StreamWriter sw = new StreamWriter(outPath))
                {
                    sw.Write(fileContent);
                }

                if (SeqRun.Instance.isRunning())
                {
                    SeqRun.Instance.onCurTaskFinished();
                }
                else
                {
                    GenerateParameter param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.Unity3DClient);
                    if (param != null && param.AutoCommit)
                    {
                        SvnUtil.CoverCommit(param.CodeFileOutputPath, ToolConfig.Instance.UseFolder);
                    }
                    MessageBox.Show("Unity3D代码生成成功！");
                }
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }
        }

        private string GetFieldType(string type)
        {
            switch (type)
            {
                case "int64":
                case "long":
                    return "Int64";
                default:
                    return type;
            }
        }

        private string GetFieldSummary(string summary)
        {
            string[] sum = summary.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join("\r\n        /// ", sum);
        }

        private string GetReadWriteMethodName(string type)
        {
            switch (type)
            {
                case "sbyte":
                    return "SByte";
                case "byte":
                    return "Byte";
                case "bool":
                case "boolean":
                    return "Boolean";
                case "int64":
                case "long":
                    return "Int64";
                case "uint64":
                case "ulong":
                    return "UInt64";
                case "double":
                    return "Double";
                case "float":
                    return "Float";
                case "int":
                    return "Int";
                case "uint":
                    return "UInt";
                case "short":
                    return "Short";
                case "ushort":
                    return "UShort";
                case "utf":
                case "string":
                case "varchar":
                case "text":
                case "longtext":
                    return "UTF";
                //case "string":
                    //return "String";
            }

            return string.Format("{0}{1}", type.Substring(0, 1).ToUpper(), type.Substring(1));
        }
    }
}
