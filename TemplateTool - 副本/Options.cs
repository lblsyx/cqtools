using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityLight.Tpls;
using CommandLine;
using System.Windows.Forms;
using TemplateTool.Utils;
using System.IO;
using UnityLight.Loggers;
using TemplateTool.Datas;
using TemplateTool.Packs;
using TemplateTool.Packs.Packers;
using TemplateTool.Gens;
using TemplateTool.Gens.Generators;
using System.Drawing.Drawing2D;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;
using System.Drawing;
using static TemplateTool.Utils.ExcelUtil;
using System.Threading;

namespace TemplateTool
{
    // 错误码新增后，也要在 模板工具命令行支持.md 中更新
    enum EErrorCode
    {
        Success = 0,
        LackInputFile = 101,        // 缺少 input_file
        LackInputDir = 102,         // 缺少 input_dir
        LackOutputPath1 = 103,      // 缺少 output_path1
        LackOutputPath2 = 104,      // 缺少 output_path2
        InvalidGenType = 105,       // 非法的gen_type
        LackAnyInputPath = 106,     // 未指定任何输入路径
        TooManyInputPath = 107,     // 指定了多个输入路径
        ExcelFileNotFound = 108,    // Excel模板文件不存在
        ExcelDirNotFound = 109,     // Excel模板目录不存在
        TableListEmpty = 110,       // 未找到模板配置信息
        CFCFInvalid = 111,          // CodeFileContentFormat 缺失字符串"__CodeContent__"
        CFCF2Invalid = 112,         // CodeFileContentFormat2 缺失字符串"__CodeContent__"
        WrongHost = 113,            // 错误的主机地址
        EmptyPort = 114,            // 端口号不能为空
        EmptyUser = 115,            // 用户名不能为空
        EmptyDatabase = 116,        // 数据库不能为空
        MySQLConnectFail = 117,     // 连接MySQL失败
        OpenMySQLConnectionFaild = 118, // 建立连接失败
        FileMissing = 119,          // 有文件没有被上传
        ClearOldDirFail = 120,         // 清空旧文件夹时失败
        HaveInvalidChar = 121,          // 包含非法字符
        Excel2JsonFail = 122,           // 表格生成失败，检查一下表格
        Excel2JsonGetTypeFail = 123,    // 获取表格中类型出错
    }

    public class Options
    {
        [Option('t', "gen_type", Required = true, HelpText = "generate type such as: as3_flash, as3_h5, cpp, json, upload, package_data, excel_to_json")]
        public string GenType { get; set; }

        [Option('i', "input_file", Required = false, HelpText = "Input file path.")]
        public string InputFile { get; set; }

        [Option('n', "input_dir", Required = false, HelpText = "Input dir path.")]
        public string InputDir { get; set; }

        [Option('o', "output_path1", Required = false, HelpText = "Output file/dir path1.")]
        public string OutputPath1 { get; set; }

        [Option('u', "output_path2", Required = false, HelpText = "Output file/dir path2.")]
        public string OutputPath2 { get; set; }

        [Option('p', "ip", Required = false, HelpText = "IP address.")]
        public string Ip { get; set; }

        [Option('r', "port", Required = false, HelpText = "Port.")]
        public string Port { get; set; }

        [Option('s', "user", Required = false, HelpText = "UserName.")]
        public string User { get; set; }

        [Option('w', "pwd", Required = false, HelpText = "Passward.")]
        public string Pwd { get; set; }

        [Option('d', "db", Required = false, HelpText = "Database.")]
        public string Db { get; set; }

        [Option('e', "error_log", Required = false, HelpText = "ErrorLog.")]
        public string ErrorLogPath { get; set; }

        public static void Parse(Options options)
        {
            IGenerator iGenerator;
            switch (options.GenType)
            {
                case "as3_flash":
                    break;

                case "as3_h5":
                    iGenerator = new AS3ClientGenerator();
                    OnGenerateCode(options, (int)GeneratorType.AS3Client, iGenerator);
                    break;

                case "cpp":
                    iGenerator = new CPPServerGenerator();
                    OnGenerateCode(options, (int)GeneratorType.CPPServer, iGenerator);
                    break;

                case "json":
                    iGenerator = new JsonServerGenerator();
                    OnGenerateCode(options, (int)GeneratorType.JsonServer, iGenerator);
                    break;

                case "upload":
                    OnUploadDB(options);
                    break;

                case "package_data":
                    OnPackageData(options);
                    break;

                case "excel_to_json":
                    OnExcelToJson(options);
                    break;

                default:
                    Environment.Exit((int)EErrorCode.InvalidGenType);
                    break;

            }

            Environment.Exit((int)EErrorCode.Success);
        }

        /*
         * input_dir
         * output_path1, /output_path2
         */
        public static void OnGenerateCode(Options options, int genType, IGenerator iGenerator)
        {
            if (options.InputDir == null)
            {
                Environment.Exit((int)EErrorCode.LackInputDir);
            }

            if (options.OutputPath1 == null)
            {
                Environment.Exit((int)EErrorCode.LackOutputPath1);
            }

            string tag = "as3";    // 做一个标志位，tag为0表示生成as3代码，tag为1表示生成cpp和json代码
            if (genType == (int)GeneratorType.CPPServer || genType == (int)GeneratorType.JsonServer)
            {
                tag = "cpp";
            }

            ToolConfig.Load();

            if (tag == "cpp")
            {
                if (options.OutputPath2 == null)
                {
                    Environment.Exit((int)EErrorCode.LackOutputPath2);
                }

                // 检查config.xml中的配置是否正确
                if (ToolConfig.Instance.CodeFileContentFormat.Trim().IndexOf(Global.CodeReplaceString) == -1)
                {
                    Environment.Exit((int)EErrorCode.CFCFInvalid);
                    //MessageBox.Show(string.Format("第一个代码结构内必须包含 {0} 字符串! 请检查config.xml CodeFileContentFormat字段", Global.CodeReplaceString));
                }

                if (string.IsNullOrEmpty(ToolConfig.Instance.CodeFileContentFormat2.Trim()) ||
                    ToolConfig.Instance.CodeFileContentFormat2.Trim().IndexOf(Global.CodeReplaceString) == -1)
                {
                    Environment.Exit((int)EErrorCode.CFCF2Invalid);
                    //MessageBox.Show(string.Format("第二个代码结构不能为空 且 代码结构体内必须包含 {0} 字符串！\n 请检查config.xml文件中的CodeFileContentFormat2字段"), Global.CodeReplaceString);
                }
            }

            if (Directory.Exists(options.InputDir) == false)
            {
                Environment.Exit((int)EErrorCode.ExcelDirNotFound);
            }

            IList<TableInfo> list = ExcelUtil.ParseTableList(options.InputDir, "导出", false);
            if (list.Count == 0)
            {
                Environment.Exit((int)EErrorCode.TableListEmpty);
            }

            iGenerator.Generate(list, options.OutputPath1, options.OutputPath2, null);

            return;
        }

        /*
         * input_file / input_dir
         */
        public static void OnUploadDB(Options options)
        {
            if (string.IsNullOrEmpty(options.Ip) || Regex.Matches(options.Ip, "[a-zA-Z]").Count > 0)
            {
                Environment.Exit((int)EErrorCode.WrongHost);
            }
            if (string.IsNullOrEmpty(options.Port))
            {
                Environment.Exit((int)EErrorCode.EmptyPort);
            }
            if (string.IsNullOrEmpty(options.User))
            {
                Environment.Exit((int)EErrorCode.EmptyUser);
            }
            if (string.IsNullOrEmpty(options.Db))
            {
                Environment.Exit((int)EErrorCode.EmptyDatabase);
            }
            if (options.InputFile != null && options.InputDir != null)
            {
                Environment.Exit((int)EErrorCode.TooManyInputPath);
            }

            string errMsg = string.Empty;

            if (MySQLUtil.TestMySQLConnect(options.Ip, options.Port, options.User, options.Pwd, ref errMsg) == false)
            {
                Environment.Exit((int)EErrorCode.MySQLConnectFail);
            }

            string _connStr = string.Format(Global.MYSQL_CONNECTION_FORMAT, options.Ip, options.Port, options.User, options.Pwd, "mysql");
            if (options.InputFile != null)//单个文件
            {
                IList<TableInfo> list = ParseTableList(options.InputFile, "导出", true);
                if (list.Count == 0)
                {
                    Environment.Exit((int)EErrorCode.TableListEmpty);
                }

                var _conn = MySQLUtil.OpenMySQLConnection(_connStr);
                if (_conn == null)
                {
                    Environment.Exit((int)EErrorCode.MySQLConnectFail);
                }

                MySQLUtil.CreateDatabaseIfNotExists(_conn, options.Db);
                MySQLUtil.DropTables(_conn, options.Db, true);

                var tableName = list[0].TableName as string;
                if (tableName != null)
                {
                    MySQLUtil.DropTable(_conn, options.Db, tableName);
                }
                //创建表结构和写入数据
                List<string> columnList = new List<string>();

                foreach (TableInfo table in list)
                {
                    columnList.Clear();
                    foreach (Datas.FieldInfo field in table.TableFields)
                    {
                        columnList.Add(GetMySQLColumnString(table.SheetName, field));
                    }
                    string columnStr = string.Join(",\r\n  ", columnList.ToArray());

                    if (MySQLUtil.CreateTable(_conn, options.Db, table, columnStr) == false)
                    {
                        XLogger.ErrorFormat("数据表创建失败!`{0}`.`{1}`", options.Db, table.TableName);
                        continue;
                    }

                    InsertDataToTable(table, options, _connStr);
                }
            }
            else   // 目录
            {
                //openTimer();
                if (Directory.Exists(options.InputDir) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelDirNotFound);
                }
                //先清空表
                //startUpload();
                var _conn = MySQLUtil.OpenMySQLConnection(_connStr);
                if (_conn == null)
                {
                    SeqRun.Instance.onCurTaskFinished(false);
                    Environment.Exit((int)EErrorCode.MySQLConnectFail);
                }

                MySQLUtil.CreateDatabaseIfNotExists(_conn, options.Db);
                MySQLUtil.DropTables(_conn, options.Db, true);

                IList<string> files = new List<string>();
                GetDirectoryChildren(options.InputDir, files, "*.xls|*.xlsx");
                
                foreach (string file in files)
                {
                    IList<TableInfo> list = ParseTableList(file, "导出", true);
                    if (list.Count == 0)
                    {
                        Environment.Exit((int)EErrorCode.FileMissing);
                        continue;
                    }

                    var tableName = list[0].TableName as string;
                    if (tableName != null)
                    {
                        MySQLUtil.DropTable(_conn, options.Db, tableName);
                    }
                    //创建表结构和写入数据
                    List<string> columnList = new List<string>();
                    List<string> fieldValList = new List<string>();

                    foreach (TableInfo table in list)
                    {
                        columnList.Clear();
                        foreach (Datas.FieldInfo field in table.TableFields)
                        {
                            columnList.Add(GetMySQLColumnString(table.SheetName, field));
                        }
                        string columnStr = string.Join(",\r\n  ", columnList.ToArray());

                        if (MySQLUtil.CreateTable(_conn, options.Db, table, columnStr) == false)
                        {
                            XLogger.ErrorFormat("数据表创建失败!`{0}`.`{1}`", options.Db, table.TableName);
                            continue;
                        }

                        InsertDataToTable(table, options, _connStr);
                    }
                    // TODO: 使用异步加载的方式优化这个循环
                    //AsyncDelegate funcNewTask = new AsyncDelegate(ParseTableAsync);
                    //funcNewTask.BeginInvoke(file, ignoreNameList, true, null, null);
                }
            }
        }

        public static void InsertDataToTable(object t, Options options, string _connStr)
        {
            TableInfo table = t as TableInfo;
            uint count = 0;
            using (var conn = MySQLUtil.OpenMySQLConnection(_connStr))
            {
                List<string> fieldValList = new List<string>();
                for (int i = 0; i < table.RowCount; i++)
                {
                    fieldValList.Clear();
                    foreach (Datas.FieldInfo field in table.TableFields)
                    {
                        string val = MySQLUtil.AddSlashes(Convert.ToString(field.FieldValues[i]));
                        fieldValList.Add(string.Format("'{0}'", val));
                    }
                    string insertValueStr = string.Join(",", fieldValList.ToArray());
                    //XLogger.InfoFormat("InsertDataToTable：{0}", _connStr);
                    if (MySQLUtil.InsertTableRecord(conn, options.Db, table, insertValueStr))
                    {
                        count += 1;
                    }
                    else
                    {
                        StopUpload();
                        XLogger.InfoFormat("插入失败，终止");
                        break;
                    }
                }
            }
            //XLogger.InfoFormat("成功插入 {0}/{1} 条数据到{2}.{3}数据表中!", count, table.RowCount, _db, table.TableName);
        }

        public static string GetMySQLColumnString(string sheetName, Datas.FieldInfo field)
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

            return string.Format("`{0}` {1} NOT NULL COMMENT '{2}'", field.FieldName, type, MySQLUtil.AddSlashes(field.FieldSummary));
        }

        /*
         * input_file / input_dir
         * output_path1
         */
        public static void OnPackageData(Options options)
        {
            if (options.OutputPath1 == null)
            {
                Environment.Exit((int)EErrorCode.LackOutputPath1);
            }

            if (options.InputFile == null && options.InputDir == null)
            {
                Environment.Exit((int)EErrorCode.LackAnyInputPath);
            }

            if (options.InputFile != null && options.InputDir != null)
            {
                Environment.Exit((int)EErrorCode.TooManyInputPath);
            }

            string path = null;
            if (options.InputFile != null)        // 指定文件
            {
                path = options.InputFile;

                if (File.Exists(path) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelFileNotFound);
                }
            }
            else if (options.InputDir != null)        // 指定目录
            {
                path = options.InputDir;
                if (Directory.Exists(path) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelDirNotFound);
                }
            }

            IList<TableInfo> list = ExcelUtil.ParseTableList(path, "导出", true);

            if (list.Count == 0)
            {
                Environment.Exit((int)EErrorCode.TableListEmpty);
            }

            if (options.ErrorLogPath == null) // 不需要错误日志
            {
                // 目前只有一个二进制选项，所以直接写死了index为1，后期有更多选项时可以加入命令行参数。
                IDataPacker packer = new BinaryDataPacker();
                packer.PackData(list, options.OutputPath1, null, null);
            }
            else        // 需要错误日志 
            {
                string errorLogFilePath = null;

                string extension = Path.GetExtension(options.ErrorLogPath);
                if (extension == string.Empty)
                {
                    // 生成带有时间戳的错误日志文件名 拼接错误日志文件的完整路径
                    errorLogFilePath = Path.Combine(options.ErrorLogPath, $"ErrorLog_{DateTime.Now:yyyyMMddHHmmss}.log");
                }
                else if (extension != string.Empty)
                {
                    errorLogFilePath = options.ErrorLogPath;
                }

                // 目前只有一个二进制选项，所以直接写死了index为1，后期有更多选项时可以加入命令行参数。
                IDataPacker packer = new BinaryDataPacker();
                packer.PackData(list, options.OutputPath1, null, null);

                // 检查错误日志文件是否为空
                if (new FileInfo(errorLogFilePath).Length > 0)
                {
                    // 创建错误日志文件
                    File.Create(errorLogFilePath).Close();
                }
            }

            return;
        }

        /*
         * input_dir
         * output_path1
         */
        public static void OnExcelToJson(Options options)
        {
            if (options.InputDir == null)
            {
                Environment.Exit((int)EErrorCode.LackInputDir);
            }

            if (options.OutputPath1 == null)
            {
                Environment.Exit((int)EErrorCode.LackOutputPath1);
            }

            // 清空文件夹下的旧文件
            if (Directory.Exists(options.OutputPath1))
            {
                string[] fileList = Directory.GetFiles(options.OutputPath1);

                foreach (var item in fileList)
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        Environment.Exit((int)EErrorCode.ClearOldDirFail);
                    }
                }
            }

            if (options.ErrorLogPath == null)
            {
                ExcelUtil.TraverseDirectory(options.InputDir, options.OutputPath1, null);
            }
            else
            {
                string errorLogFilePath = null;

                string extension = Path.GetExtension(options.ErrorLogPath);
                if (extension == string.Empty)  // 目录
                {
                    // 生成带有时间戳的错误日志文件名 拼接错误日志文件的完整路径
                    errorLogFilePath = Path.Combine(options.ErrorLogPath, $"ErrorLog_{DateTime.Now:yyyyMMddHHmmss}.log");

                }
                else if (extension != string.Empty) // 文件
                {
                    errorLogFilePath = options.ErrorLogPath;
                }
               
                ExcelUtil.TraverseDirectory(options.InputDir, options.OutputPath1, errorLogFilePath);
            }

            return;
        }
    }
}
