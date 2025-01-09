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
    // ������������ҲҪ�� ģ�幤��������֧��.md �и���
    enum EErrorCode
    {
        Success = 0,
        LackInputFile = 101,        // ȱ�� input_file
        LackInputDir = 102,         // ȱ�� input_dir
        LackOutputPath1 = 103,      // ȱ�� output_path1
        LackOutputPath2 = 104,      // ȱ�� output_path2
        InvalidGenType = 105,       // �Ƿ���gen_type
        LackAnyInputPath = 106,     // δָ���κ�����·��
        TooManyInputPath = 107,     // ָ���˶������·��
        ExcelFileNotFound = 108,    // Excelģ���ļ�������
        ExcelDirNotFound = 109,     // Excelģ��Ŀ¼������
        TableListEmpty = 110,       // δ�ҵ�ģ��������Ϣ
        CFCFInvalid = 111,          // CodeFileContentFormat ȱʧ�ַ���"__CodeContent__"
        CFCF2Invalid = 112,         // CodeFileContentFormat2 ȱʧ�ַ���"__CodeContent__"
        WrongHost = 113,            // �����������ַ
        EmptyPort = 114,            // �˿ںŲ���Ϊ��
        EmptyUser = 115,            // �û�������Ϊ��
        EmptyDatabase = 116,        // ���ݿⲻ��Ϊ��
        MySQLConnectFail = 117,     // ����MySQLʧ��
        OpenMySQLConnectionFaild = 118, // ��������ʧ��
        FileMissing = 119,          // ���ļ�û�б��ϴ�
        ClearOldDirFail = 120,         // ��վ��ļ���ʱʧ��
        HaveInvalidChar = 121,          // �����Ƿ��ַ�
        Excel2JsonFail = 122,           // �������ʧ�ܣ����һ�±��
        Excel2JsonGetTypeFail = 123,    // ��ȡ��������ͳ���
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

            string tag = "as3";    // ��һ����־λ��tagΪ0��ʾ����as3���룬tagΪ1��ʾ����cpp��json����
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

                // ���config.xml�е������Ƿ���ȷ
                if (ToolConfig.Instance.CodeFileContentFormat.Trim().IndexOf(Global.CodeReplaceString) == -1)
                {
                    Environment.Exit((int)EErrorCode.CFCFInvalid);
                    //MessageBox.Show(string.Format("��һ������ṹ�ڱ������ {0} �ַ���! ����config.xml CodeFileContentFormat�ֶ�", Global.CodeReplaceString));
                }

                if (string.IsNullOrEmpty(ToolConfig.Instance.CodeFileContentFormat2.Trim()) ||
                    ToolConfig.Instance.CodeFileContentFormat2.Trim().IndexOf(Global.CodeReplaceString) == -1)
                {
                    Environment.Exit((int)EErrorCode.CFCF2Invalid);
                    //MessageBox.Show(string.Format("�ڶ�������ṹ����Ϊ�� �� ����ṹ���ڱ������ {0} �ַ�����\n ����config.xml�ļ��е�CodeFileContentFormat2�ֶ�"), Global.CodeReplaceString);
                }
            }

            if (Directory.Exists(options.InputDir) == false)
            {
                Environment.Exit((int)EErrorCode.ExcelDirNotFound);
            }

            IList<TableInfo> list = ExcelUtil.ParseTableList(options.InputDir, "����", false);
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
            if (options.InputFile != null)//�����ļ�
            {
                IList<TableInfo> list = ParseTableList(options.InputFile, "����", true);
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
                //������ṹ��д������
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
                        XLogger.ErrorFormat("���ݱ���ʧ��!`{0}`.`{1}`", options.Db, table.TableName);
                        continue;
                    }

                    InsertDataToTable(table, options, _connStr);
                }
            }
            else   // Ŀ¼
            {
                //openTimer();
                if (Directory.Exists(options.InputDir) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelDirNotFound);
                }
                //����ձ�
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
                    IList<TableInfo> list = ParseTableList(file, "����", true);
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
                    //������ṹ��д������
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
                            XLogger.ErrorFormat("���ݱ���ʧ��!`{0}`.`{1}`", options.Db, table.TableName);
                            continue;
                        }

                        InsertDataToTable(table, options, _connStr);
                    }
                    // TODO: ʹ���첽���صķ�ʽ�Ż����ѭ��
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
                    //XLogger.InfoFormat("InsertDataToTable��{0}", _connStr);
                    if (MySQLUtil.InsertTableRecord(conn, options.Db, table, insertValueStr))
                    {
                        count += 1;
                    }
                    else
                    {
                        StopUpload();
                        XLogger.InfoFormat("����ʧ�ܣ���ֹ");
                        break;
                    }
                }
            }
            //XLogger.InfoFormat("�ɹ����� {0}/{1} �����ݵ�{2}.{3}���ݱ���!", count, table.RowCount, _db, table.TableName);
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
                        XLogger.ErrorFormat("�޷�ʶ������ͣ�Ĭ��ʹ��text����\r\nSheetName��{0}��FieldName��{1}��FieldType��{2}", sheetName, field.FieldName, field.FieldType);
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
            if (options.InputFile != null)        // ָ���ļ�
            {
                path = options.InputFile;

                if (File.Exists(path) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelFileNotFound);
                }
            }
            else if (options.InputDir != null)        // ָ��Ŀ¼
            {
                path = options.InputDir;
                if (Directory.Exists(path) == false)
                {
                    Environment.Exit((int)EErrorCode.ExcelDirNotFound);
                }
            }

            IList<TableInfo> list = ExcelUtil.ParseTableList(path, "����", true);

            if (list.Count == 0)
            {
                Environment.Exit((int)EErrorCode.TableListEmpty);
            }

            if (options.ErrorLogPath == null) // ����Ҫ������־
            {
                // Ŀǰֻ��һ��������ѡ�����ֱ��д����indexΪ1�������и���ѡ��ʱ���Լ��������в�����
                IDataPacker packer = new BinaryDataPacker();
                packer.PackData(list, options.OutputPath1, null, null);
            }
            else        // ��Ҫ������־ 
            {
                string errorLogFilePath = null;

                string extension = Path.GetExtension(options.ErrorLogPath);
                if (extension == string.Empty)
                {
                    // ���ɴ���ʱ����Ĵ�����־�ļ��� ƴ�Ӵ�����־�ļ�������·��
                    errorLogFilePath = Path.Combine(options.ErrorLogPath, $"ErrorLog_{DateTime.Now:yyyyMMddHHmmss}.log");
                }
                else if (extension != string.Empty)
                {
                    errorLogFilePath = options.ErrorLogPath;
                }

                // Ŀǰֻ��һ��������ѡ�����ֱ��д����indexΪ1�������и���ѡ��ʱ���Լ��������в�����
                IDataPacker packer = new BinaryDataPacker();
                packer.PackData(list, options.OutputPath1, null, null);

                // ��������־�ļ��Ƿ�Ϊ��
                if (new FileInfo(errorLogFilePath).Length > 0)
                {
                    // ����������־�ļ�
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

            // ����ļ����µľ��ļ�
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
                if (extension == string.Empty)  // Ŀ¼
                {
                    // ���ɴ���ʱ����Ĵ�����־�ļ��� ƴ�Ӵ�����־�ļ�������·��
                    errorLogFilePath = Path.Combine(options.ErrorLogPath, $"ErrorLog_{DateTime.Now:yyyyMMddHHmmss}.log");

                }
                else if (extension != string.Empty) // �ļ�
                {
                    errorLogFilePath = options.ErrorLogPath;
                }
               
                ExcelUtil.TraverseDirectory(options.InputDir, options.OutputPath1, errorLogFilePath);
            }

            return;
        }
    }
}
