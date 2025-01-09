using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using UnityLight.Loggers;
using TemplateTool.Utils;

namespace TemplateTool.Gens.Generators
{
    [Generator((int)GeneratorType.AS3Client, "AS3代码", true)]
    public class AS3ClientGenerator : IGenerator
    {
        public bool UseFolder
        {
            get { return true; }
        }

        public bool RequireSecondCode
        {
            get { return false; }
        }

        public const string RegisterFormat = "/**\r\n" +
                                             " * Created by Tool\r\n" +
                                             " */\r\n" +
                                             "package game.tpls {\r\n" +
                                             "public class TPLRegister {\r\n" +
                                             "    private static var _dict:Object = {};\r\n\r\n" +

                                             "    public static function init():void {\r\n" +
                                             "        __CodeContent__\r\n" +
                                             "    }\r\n\r\n" +

                                             "    public static function getTPLClass(tplName:String):Class {\r\n" +
                                             "        return _dict[tplName] as Class;\r\n" +
                                             "    }\r\n\r\n" +

                                             "    public static function setTPLClass(tplName:String, tClass:Class):void {\r\n" +
                                             "        _dict[tplName] = tClass;\r\n" +
                                             "    }\r\n" +

                                             "}\r\n" +
                                             "}\r\n";

        public const string FileCodeFormat = @"/**
 * Created by Tool
 */
package game.tpls {
import com.tpl.Tpl;

import laya.utils.Byte;
__CodeContent__
}";

        /// <summary>
        /// {0}：模板类名
        /// {1}：字段代码
        /// {2}：读取代码
        /// {3}：写入代码
        /// {4}：文件名
        /// </summary>
        public const string TPL_FILE_FORMAT =
            "/* [{4}] */\r\n" +
            "public class {0} extends Tpl __LK__\r\n" +
            "    {1}\r\n\r\n" +

            "    override public function readFrom(bytes:Byte):void __LK__\r\n" +
            "        super.readFrom(bytes);\r\n" +
            "        {2}\r\n" +
            "    __RK__\r\n\r\n" +

            "    override public function writeTo(bytes:Byte):void __LK__\r\n" +
            "        super.writeTo(bytes);\r\n" +
            "        {3}\r\n" +
            "    __RK__\r\n" +
            "__RK__";

        /// <summary>
        /// {0}：字段名
        /// {1}：字段类型
        /// {2}：字段说明
        /// </summary>
        public const string TPL_FIELD_FORMAT =
                "/**\r\n" +
            "     * {2}\r\n" +
            "     */\r\n" +
            "    public var {0}:{1};";
        public const string TPL_FIELD_SPLITTER = "\r\n    ";
        /// <summary>
        /// {0}：字段名
        /// {1}：字段类型
        /// </summary>
        public const string TPL_READ_FORMAT = "{0} = bytes.read{1}();";
        /// <summary>
        /// {0}：字段名
        /// {1}：字段类型
        /// </summary>
        public const string TPL_WRITE_FORMAT = "bytes.write{1}({0});";
        /// <summary>
        /// 读写方法分隔符
        /// </summary>
        public const string TPL_READWRITE_SPLITTER = "\r\n        ";
        /// <summary>
        /// 模板注册代码
        /// </summary>
        public const string TPL_REGISTER_FORMAT = "_dict[\"{0}\"] = {0};";
        public const string TPL_REGISTER_SPLITTER = "\r\n        ";

        public void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others)
        {
            IList<string> regList = new List<string>();
            IList<string> fieldList = new List<string>();
            IList<string> readList = new List<string>();
            IList<string> writeList = new List<string>();

            string fileFormat = FileCodeFormat;


            if (Directory.Exists(outPath) == false)
            {//创建不存在的目录
                Directory.CreateDirectory(outPath);
            }

            string[] files = Directory.GetFiles(outPath, "*.as");
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    File.Delete(files[i]);
                }
                catch (Exception ex)
                {
                    XLogger.ErrorFormat("删除旧文件 {0} 失败!原因：{1}", files[i], ex.Message);
                }
            }

            byte[] temp3 = null;
            string filePath = string.Empty;
            string fileContent = string.Empty;
            for (int i = 0; i < schemas.Count; i++)
            {
                readList.Clear();
                fieldList.Clear();
                writeList.Clear();

                TableInfo table = schemas[i];

                regList.Add(string.Format(TPL_REGISTER_FORMAT, table.TableName));

                for (int j = 0; j < table.TableFields.Count; j++)
                {
                    FieldInfo field = table.TableFields[j];

                    if (field.FieldName.ToUpper() == "TID") continue;

                    String strFieldName = field.FieldName;
                    if (strFieldName == "Number")
                    {
                        strFieldName = "i" + strFieldName;
                    }
                    else if (strFieldName == "Date")
                    {
                        strFieldName = "str" + strFieldName;
                    }
                    else if (strFieldName == "Function")
                    {
                        strFieldName = "o" + strFieldName;
                    }

                    fieldList.Add(string.Format(TPL_FIELD_FORMAT, strFieldName, GetAS3Type(field.FieldType), GetAS3Summary(field.FieldSummary)));

                    readList.Add(string.Format(TPL_READ_FORMAT, strFieldName, GetAS3ReadType(field.FieldType)));
                    writeList.Add(string.Format(TPL_WRITE_FORMAT, strFieldName, GetAS3WriteType(field.FieldType)));
                }

                string hExcelName = Path.GetFileName(table.ExcelFile);
                string fieldCode = string.Join(TPL_FIELD_SPLITTER, fieldList.ToArray());
                string readCode = string.Join(TPL_READWRITE_SPLITTER, readList.ToArray());
                string writeCode = string.Join(TPL_READWRITE_SPLITTER, writeList.ToArray());
                string classCode = string.Format(TPL_FILE_FORMAT, table.TableName, fieldCode, readCode, writeCode, hExcelName);

                fileContent = fileFormat.Replace(Global.CodeReplaceString, classCode);
                fileContent = fileContent.Replace("__LK__", "{");
                fileContent = fileContent.Replace("__RK__", "}");
                if (fileContent.LastIndexOf("\r\n") != (fileContent.Length - 2))
                {
                    fileContent += "\r\n";
                }

                filePath = Path.Combine(outPath, table.TableName + ".as");
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.Write(fileContent);
                }
                temp3 = FileUtil.ReadFileBytes(filePath);
                FileUtil.WriteFileBytes(filePath, temp3, 3);
            }

            string regCode = string.Join(TPL_REGISTER_SPLITTER, regList.ToArray());
            fileContent = RegisterFormat.Replace(Global.CodeReplaceString, regCode);
            filePath = Path.Combine(outPath, "TPLRegister.as");
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.Write(fileContent);
            }
            temp3 = FileUtil.ReadFileBytes(filePath);
            FileUtil.WriteFileBytes(filePath, temp3, 3);

            if (Global.args.Length > 0)
            {
                System.Environment.Exit(0);
            }
            else
            {
                if (SeqRun.Instance.isRunning())
                {
                    SeqRun.Instance.onCurTaskFinished();
                }
                else
                {
                    GenerateParameter param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.AS3Client);
                    if (param != null && param.AutoCommit)
                    {
                        SvnUtil.CoverCommit(param.CodeFileOutputPath, ToolConfig.Instance.UseFolder);
                    }
                    MessageBox.Show("AS3模板代码生成成功！");
                }
            }
        }

        private string GetAS3Type(string t)
        {
            switch (t)
            {
                case "bool":
                case "boolean":
                    return "Boolean";
                case "char":
                case "sbyte":
                case "short":
                case "int":
                    return "int";
                case "uchar":
                case "byte":
                case "ushort":
                case "uint":
                    return "uint";
                case "float":
                case "double":
                case "int64":
                    return "Number";
                case "utf":
                case "text":
                case "string":
                case "varchar":
                case "longtext":
                    return "String";
            }
            return t;
        }

        private string GetAS3ReadType(string t)
        {
            switch (t)
            {
                case "bool":
                case "boolean":
                    return "Boolean";
                case "char":
                case "sbyte":
                    return "Byte";
                case "uchar":
                case "byte":
                    return "UnsignedByte";
                case "short":
                    return "Short";
                case "ushort":
                    return "UnsignedShort";
                case "int":
                    return "Int";
                case "uint":
                    return "UnsignedInt";
                case "float":
                    return "Float";
                case "double":
                case "int64":
                    return "Double";
                case "utf":
                case "string":
                case "varchar":
                case "text":
                case "longtext":
                    return "UTF";
            }
            return t.Substring(0, 1).ToUpper() + t.Substring(1);
        }

        private string GetAS3WriteType(string t)
        {
            switch (t)
            {
                case "bool":
                case "boolean":
                    return "Boolean";
                case "char":
                case "sbyte":
                    return "Byte";
                case "uchar":
                case "byte":
                    return "Byte";
                case "short":
                    return "Short";
                case "ushort":
                    return "Short";
                case "int":
                    return "Int";
                case "uint":
                    return "UnsignedInt";
                case "float":
                    return "Float";
                case "double":
                case "int64":
                    return "Double";
                case "utf":
                case "string":
                case "varchar":
                case "text":
                case "longtext":
                    return "UTF";
            }
            return t.Substring(0, 1).ToUpper() + t.Substring(1);
        }

        private string GetAS3Summary(string s)
        {
            string[] list = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string rlt = string.Join("\r\n     * ", list.ToArray());
            return rlt;
        }
    }
}
