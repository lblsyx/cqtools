using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Utils;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using UnityLight.Loggers;
using TemplateTool.Datas;
using TemplateTool.Gens;
using TemplateTool.Packs;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Threading;
using Timer = System.Windows.Forms.Timer;
using System.Text.RegularExpressions;


namespace TemplateTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Global.CurrentAssembly = typeof(Global).Assembly;
            XLogger.AddLogger(new TextBoxLogger(summaryTextBox));
            GenMgr.SearchAssembly(Global.CurrentAssembly);
            PackMgr.SearchAssembly(Global.CurrentAssembly);

            packerTypeComboBox.DataSource = PackMgr.GetPackerInfoList();
            generatorTypeComboBox.DataSource = GenMgr.GetGeneratorInfoList();

            if (ToolConfig.Exists == false)
            {
                updateConfig();

                ToolConfig.Save();
            }
            else
            {
                ToolConfig.Load();

                updateFormData();
            }

            packerTypeComboBox.SelectedIndex = ToolConfig.Instance.PackSelectedIndex;
            generatorTypeComboBox.SelectedIndex = ToolConfig.Instance.GenSelectedIndex;


            if (Global.args.Length != 0)
            {
                string args = Global.args[0];
                if (args.IndexOf("PackTemplate") > -1)
                {
                    packTemplateDataButton_Click(null, null);
                }
                else if (args.IndexOf("GenerateCode") > -1)
                {
                    generateCodeContentButton_Click(null, null);
                }
            }
        }

        private void updateConfig()
        {
            ToolConfig.Instance.UseFolder = dirRadioButton.Checked;
            ToolConfig.Instance.ExcelFilePath = excelFileTextBox.Text.Trim();
            ToolConfig.Instance.ExcelFolderPath = excelDirTextBox.Text.Trim();
            ToolConfig.Instance.IgnoreSheetNames = ignoreNamesTextBox.Text.Trim();
            ToolConfig.Instance.CodeFileOutputPath = codeOutFileTextBox.Text.Trim();
            ToolConfig.Instance.CodeFileOutputPath2 = codeOutFile2TextBox.Text.Trim();
            ToolConfig.Instance.CodeFileContentFormat = codeFormatTextBox.Text.Trim();
            ToolConfig.Instance.CodeFileContentFormat2 = codeFormat2TextBox.Text.Trim();
            ToolConfig.Instance.PackDataFileOutputPath = packTemplateDataTextBox.Text.Trim();
            ToolConfig.Instance.ExcelFileSplitOutputPath = excelOutTextBox.Text.Trim();
            ToolConfig.Instance.ExcelToJsonOutputPath = jsonOutputTextBox.Text.Trim();

            ToolConfig.Instance.GenSelectedIndex = generatorTypeComboBox.SelectedIndex;
            ToolConfig.Instance.PackSelectedIndex = packerTypeComboBox.SelectedIndex;

            PackerInfo packerInfo = packerTypeComboBox.SelectedItem as PackerInfo;
            if (packerInfo != null)
            {
                PackParameter param = ToolConfig.Instance.GetPackParameter(packerInfo.PackerType);
                if (param == null)
                {
                    param = new PackParameter();
                    param.PackerType = packerInfo.PackerType;
                    ToolConfig.Instance.PackParameterList.Add(param);
                }

                param.PackDataFileOutputPath = ToolConfig.Instance.PackDataFileOutputPath;
                param.PackDataAutoCommit = cBDatAutoCommit.CheckState == CheckState.Checked ? true : false;
            }

            GeneratorInfo generatorInfo = generatorTypeComboBox.SelectedItem as GeneratorInfo;
            if (generatorInfo != null)
            {
                GenerateParameter param = ToolConfig.Instance.GetGenerateParameter(generatorInfo.GeneratorType);
                if (param == null)
                {
                    param = new GenerateParameter();
                    param.GeneratorType = generatorInfo.GeneratorType;
                    ToolConfig.Instance.CodeParameterList.Add(param);
                }

                param.CodeFileContentFormat2 = ToolConfig.Instance.CodeFileContentFormat2;
                param.CodeFileContentFormat = ToolConfig.Instance.CodeFileContentFormat;
                param.CodeFileOutputPath2 = ToolConfig.Instance.CodeFileOutputPath2;
                param.CodeFileOutputPath = ToolConfig.Instance.CodeFileOutputPath;
                param.AutoCommit = checkBoxCommit.CheckState == CheckState.Checked ? true : false;
            }

            ToolConfig.Instance.MySQLHost = hostTextBox.Text.Trim();
            ToolConfig.Instance.MySQLPort = portTextBox.Text.Trim();
            ToolConfig.Instance.MySQLUser = userTextBox.Text.Trim();
            ToolConfig.Instance.MySQLPass = pwdTextBox.Text.Trim();
            ToolConfig.Instance.MySQLDB = dbTextBox.Text.Trim();
        }

        private void updateFormData()
        {
            if (ToolConfig.Instance.UseFolder) dirRadioButton.Checked = true;
            else fileRadioButton.Checked = true;

            excelFileTextBox.Text = ToolConfig.Instance.ExcelFilePath;
            excelDirTextBox.Text = ToolConfig.Instance.ExcelFolderPath;
            ignoreNamesTextBox.Text = ToolConfig.Instance.IgnoreSheetNames;
            codeOutFileTextBox.Text = ToolConfig.Instance.CodeFileOutputPath;
            codeOutFile2TextBox.Text = ToolConfig.Instance.CodeFileOutputPath2;
            codeFormatTextBox.Text = ToolConfig.Instance.CodeFileContentFormat;
            codeFormat2TextBox.Text = ToolConfig.Instance.CodeFileContentFormat2;
            packTemplateDataTextBox.Text = ToolConfig.Instance.PackDataFileOutputPath;
            excelOutTextBox.Text = ToolConfig.Instance.ExcelFileSplitOutputPath;
            jsonOutputTextBox.Text = ToolConfig.Instance.ExcelToJsonOutputPath;
            hostTextBox.Text = ToolConfig.Instance.MySQLHost;
            portTextBox.Text = ToolConfig.Instance.MySQLPort;
            userTextBox.Text = ToolConfig.Instance.MySQLUser;
            pwdTextBox.Text = ToolConfig.Instance.MySQLPass;
            dbTextBox.Text = ToolConfig.Instance.MySQLDB;

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, ToolConfig.Instance.isCheckBoxListItemSelected(i));
            }

            GeneratorInfo gen_info = generatorTypeComboBox.SelectedItem as GeneratorInfo;
            GenerateParameter gen_param = null;
            if (gen_info != null) gen_param = ToolConfig.Instance.GetGenerateParameter(gen_info.GeneratorType);
            if (gen_info == null || gen_param == null)
            {
                checkBoxCommit.CheckState = CheckState.Unchecked;
            }
            else
            {
                checkBoxCommit.CheckState = gen_param.AutoCommit ? CheckState.Checked : CheckState.Unchecked;
            }

            PackerInfo info = packerTypeComboBox.SelectedItem as PackerInfo;
            PackParameter param = null;
            if (info != null) param = ToolConfig.Instance.GetPackParameter(info.PackerType);

            if (info == null || param == null)
            {
                cBDatAutoCommit.CheckState = CheckState.Unchecked;
            }
            else
            {
                cBDatAutoCommit.CheckState = param.PackDataAutoCommit ? CheckState.Checked : CheckState.Unchecked;
            }
        }
        private void browseExcelFileButton_Click(object sender, EventArgs e)
        {
            string f = FileUtil.BrowseFile("Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*");
            if (string.IsNullOrEmpty(f) == false)
            {
                excelFileTextBox.Text = f;
                fileRadioButton.Checked = true;
            }
        }

        private void browseExcelDirButton_Click(object sender, EventArgs e)
        {
            string d = FileUtil.BrowseFolder();

            if (string.IsNullOrEmpty(d) == false)
            {
                excelDirTextBox.Text = d;
                dirRadioButton.Checked = true;
            }
        }

        private void browseCodeOutFileButton_Click(object sender, EventArgs e)
        {
            string f = string.Empty;

            GeneratorInfo info = generatorTypeComboBox.SelectedItem as GeneratorInfo;

            if (GenMgr.IsUseFolder(info.GeneratorType))
            {
                f = FileUtil.BrowseFolder();
            }
            else
            {
                f = FileUtil.BrowseFile("代码文件(*.as;*.cs;*.h;*.cpp;*.hpp)|*.as;*.cs;*.h;*.cpp;*.hpp|所有文件(*.*)|*.*");
            }

            if (string.IsNullOrEmpty(f) == false)
            {
                codeOutFileTextBox.Text = f;
            }
        }

        private void browseCodeOutFile2Button_Click(object sender, EventArgs e)
        {
            string f = string.Empty;

            GeneratorInfo info = generatorTypeComboBox.SelectedItem as GeneratorInfo;

            if (GenMgr.IsUseFolder(info.GeneratorType))
            {
                f = FileUtil.BrowseFolder();
            }
            else
            {
                f = FileUtil.BrowseFile("代码文件(*.as;*.cs;*.h;*.cpp;*.hpp)|*.as;*.cs;*.h;*.cpp;*.hpp|所有文件(*.*)|*.*");
            }

            if (string.IsNullOrEmpty(f) == false)
            {
                codeOutFile2TextBox.Text = f;
            }
        }

        private void browsePackOutFileButton_Click(object sender, EventArgs e)
        {
            string f = FileUtil.BrowseFile("所有文件(*.*)|*.*");

            if (string.IsNullOrEmpty(f) == false)
            {
                packTemplateDataTextBox.Text = f;
            }
        }

        private void connectTestButton_Click(object sender, EventArgs e)
        {
            if (checkMySQLInfo() == false) return;

            string errMsg = string.Empty;

            if (MySQLUtil.TestMySQLConnect(hostTextBox.Text.Trim(), portTextBox.Text.Trim(), userTextBox.Text.Trim(), pwdTextBox.Text.Trim(), ref errMsg))
            {
                MessageBox.Show("连接成功!");
            }
            else
            {
                MessageBox.Show(string.Format("连接失败!"));
                //MessageBox.Show(string.Format("连接失败，错误如下：\r\n{0}", errMsg));
            }
        }

        private void saveAllConfigButton_Click(object sender, EventArgs e)
        {
            try
            {
                updateConfig();

                ToolConfig.Save();

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存失败，错误如下：\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            summaryTextBox.Text = "";
        }
        private void excelToTableInfoListAsync(bool bReadValue)
        {
            string path = string.Empty;
            if (dirRadioButton.Checked)
            {
                if (string.IsNullOrEmpty(excelDirTextBox.Text.Trim()))
                {
                    XLogger.Error("请选择Excel模板所在目录!");
                    return;
                }
                path = excelDirTextBox.Text.Trim();
                if (Directory.Exists(path) == false)
                {
                    XLogger.Error("Excel模板所在目录不存在!");
                    return;
                }
                //先清空表
                startUpload();
                var result = PackMgr.initPackData((int)PackerType.MySQLPacker, _connStr, this);
                if (result)
                {
                    ExcelUtil.ParseTableListAsync(path, ignoreNamesTextBox.Text.Trim(), bReadValue, this);
                }

            }
            else
            {
                XLogger.Error("请选择Excel模板路径!");
            }

            if (string.IsNullOrEmpty(path))
            {
                XLogger.Error("Excel源路径不能为空!");
            }

        }
        public static void FuncCallBack(IAsyncResult ar)
        {
            //获得调用委托实例的引用

            Console.WriteLine(string.Format("这里是回调函数"));
        }

        private IList<TableInfo> excelToTableInfoList(bool bReadValue)
        {
            string path = string.Empty;

            if (fileRadioButton.Checked)
            {
                if (string.IsNullOrEmpty(excelFileTextBox.Text.Trim()))
                {
                    XLogger.Error("请选择Excel模板文件路径!");
                    return new List<TableInfo>();
                }
                path = excelFileTextBox.Text.Trim();

                if (File.Exists(path) == false)
                {
                    XLogger.Error("Excel模板文件不存在!");
                    return new List<TableInfo>();
                }
            }
            else if (dirRadioButton.Checked)
            {
                if (string.IsNullOrEmpty(excelDirTextBox.Text.Trim()))
                {
                    XLogger.Error("请选择Excel模板所在目录!");
                    return new List<TableInfo>();
                }
                path = excelDirTextBox.Text.Trim();
                if (Directory.Exists(path) == false)
                {
                    XLogger.Error("Excel模板所在目录不存在!");
                    return new List<TableInfo>();
                }
            }
            else
            {
                XLogger.Error("请选择Excel模板路径!");
                return new List<TableInfo>();
            }

            if (string.IsNullOrEmpty(path))
            {
                XLogger.Error("Excel源路径不能为空!");
                return new List<TableInfo>();
            }

            IList<TableInfo> list = ExcelUtil.ParseTableList(path, ignoreNamesTextBox.Text.Trim(), bReadValue);

            if (list.Count == 0) XLogger.Error("未找到模板配置信息!");

            return list;
        }

        private void generatorTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GeneratorInfo info = generatorTypeComboBox.SelectedItem as GeneratorInfo;
            GenerateParameter param = null;
            if (info != null) param = ToolConfig.Instance.GetGenerateParameter(info.GeneratorType);

            bool requireSecondPath = GenMgr.IsRequireSecondPath(info.GeneratorType);
            codeFormat2TextBox.Enabled = requireSecondPath;
            codeOutFile2TextBox.Enabled = requireSecondPath;
            browseCodeOutFile2Button.Enabled = requireSecondPath;


            if (requireSecondPath == false)
            {
                codeFormat2TextBox.Text = string.Empty;
                codeOutFile2TextBox.Text = string.Empty;
            }

            if (info == null || param == null)
            {
                ToolConfig.Instance.CodeFileOutputPath = string.Empty;
                ToolConfig.Instance.CodeFileContentFormat = string.Empty;
                ToolConfig.Instance.CodeFileContentFormat2 = string.Empty;
                codeOutFile2TextBox.Text = string.Empty;
                codeOutFileTextBox.Text = string.Empty;
                codeFormat2TextBox.Text = string.Empty;
                codeFormatTextBox.Text = string.Empty;
                checkBoxCommit.CheckState = CheckState.Unchecked;

            }
            else
            {
                ToolConfig.Instance.CodeFileContentFormat = param.CodeFileContentFormat;
                ToolConfig.Instance.CodeFileContentFormat2 = param.CodeFileContentFormat2;
                ToolConfig.Instance.CodeFileOutputPath = param.CodeFileOutputPath;
                ToolConfig.Instance.CodeFileOutputPath2 = param.CodeFileOutputPath2;
                codeFormat2TextBox.Text = param.CodeFileContentFormat2;
                codeFormatTextBox.Text = param.CodeFileContentFormat;
                codeOutFileTextBox.Text = param.CodeFileOutputPath;
                codeOutFile2TextBox.Text = param.CodeFileOutputPath2;

                checkBoxCommit.CheckState = param.AutoCommit ? CheckState.Checked : CheckState.Unchecked;

            }
        }
        public void StopTask()
        {
            if (SeqRun.Instance.isRunning())
            {
                stopUpload();
            }
            checkedListBox1.Enabled = true;
            tabPage1.Enabled = true;
            tabPage2.Enabled = true;
            tabPage3.Enabled = true;
            fileRadioButton.Enabled = true;
            dirRadioButton.Enabled = true;
            onKeyRun.Text = "一键执行";

        }
        public void RunTask(SeqRun.TASK_ID id)
        {
            onKeyRun.Text = "停止";

            switch (id)
            {
                case SeqRun.TASK_ID.TASK_ID_AS3:
                case SeqRun.TASK_ID.TASK_ID_CPP:
                case SeqRun.TASK_ID.TASK_ID_UNITY:
                    //tabControl1.SelectedIndex = 0;
                    tabPage1.Enabled = false;
                    var gType = SeqRun.getGenCodeTypeByTaskID(id);
       
                    var param = ToolConfig.Instance.GetGenerateParameter(gType);
                    generatorTypeComboBox.SelectedIndex = SeqRun.getComboBoxItemIdxByTaskID(id);
                    generateCodeContentButton_Click(null, null);
                    if (SeqRun.Instance.getCurID() == id)
                    {
                        SeqRun.Instance.onCurTaskFinished(false);
                    }
                    break;
                case SeqRun.TASK_ID.TASK_ID_PACK:
                    //tabControl1.SelectedIndex = 1;
                    tabPage2.Enabled = false;
                    packTemplateDataButton_Click(null, null);
                    if(SeqRun.Instance.getCurID()== id)
                    {
                        SeqRun.Instance.onCurTaskFinished(false);
                    }
                    break;
                case SeqRun.TASK_ID.TASK_ID_UPLOAD_DB:
                    //tabControl1.SelectedIndex = 2;
                    tabPage3.Enabled = false;
                    uploadButton_Click(null, null);
                    break;
                default:
                    break;
            }
        }
        public void OnAllTasksFinished()
        {

            var needDeleteLostFile = ToolConfig.Instance.UseFolder;
            GenerateParameter param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.AS3Client);
            if (param != null && param.AutoCommit)
            {
                SvnUtil.CoverCommit(param.CodeFileOutputPath, needDeleteLostFile);
            }
            param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.CPPServer);
            if (param != null && param.AutoCommit)
            {
                string[] paths = new string[2] { param.CodeFileOutputPath, param.CodeFileOutputPath2 };
                SvnUtil.CoverCommit(paths);
            }
            param = ToolConfig.Instance.GetGenerateParameter((int)GeneratorType.Unity3DClient);
            if (param != null && param.AutoCommit)
            {
                SvnUtil.CoverCommit(param.CodeFileOutputPath, needDeleteLostFile);
            }
            var packParam = ToolConfig.Instance.GetPackParameter((int)PackerType.BinaryPacker);
            if (packParam != null && packParam.PackDataAutoCommit)
            {
                SvnUtil.CoverCommit(packParam.PackDataFileOutputPath, false);
            }
        }
        private void generateCodeContentButton_Click(object sender, EventArgs e)
        {
            if (generatorTypeComboBox.SelectedIndex == -1)
            {
                //XLogger.Error("请选择代码生成类型!");
                MessageBox.Show("请选择代码生成类型!");
                generatorTypeComboBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(codeOutFileTextBox.Text.Trim()))
            {
                //XLogger.Error("请选择代码输出文件路径后生成!");
                MessageBox.Show("第一个代码输出文件路径不能为空!");
                browseCodeOutFileButton.Focus();
                return;
            }

            if (string.IsNullOrEmpty(codeFormatTextBox.Text.Trim()))
            {
                MessageBox.Show("第一个代码结构不能为空!");
                codeFormatTextBox.Focus();
                return;
            }

            if (codeFormatTextBox.Text.Trim().IndexOf(Global.CodeReplaceString) == -1)
            {
                MessageBox.Show(string.Format("第一个代码结构内必须包含 {0} 字符串!", Global.CodeReplaceString));
                codeFormatTextBox.Focus();
                return;
            }

            GeneratorInfo info = generatorTypeComboBox.SelectedItem as GeneratorInfo;

            if (GenMgr.IsRequireSecondPath(info.GeneratorType))
            {
                if (string.IsNullOrEmpty(codeOutFile2TextBox.Text.Trim()))
                {
                    MessageBox.Show("第二个代码输出文件路径不能为空!");
                    browseCodeOutFile2Button.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(codeFormat2TextBox.Text.Trim()))
                {
                    MessageBox.Show("第二个代码结构不能为空!");
                    codeFormat2TextBox.Focus();
                    return;
                }

                if (codeFormat2TextBox.Text.Trim().IndexOf(Global.CodeReplaceString) == -1)
                {
                    MessageBox.Show(string.Format("第二个代码结构内必须包含 {0} 字符串!", Global.CodeReplaceString));
                    codeFormat2TextBox.Focus();
                    return;
                }
            }
            if(e==null && sender!= null)
            {
                var onlyCheck = (bool)sender;
                if (onlyCheck)
                {
                    return;
                }
            }
            IList<TableInfo> list = excelToTableInfoList(false);

            if (list.Count == 0) return;

            GenMgr.Generate(info.GeneratorType, list, codeOutFileTextBox.Text.Trim(), codeOutFile2TextBox.Text.Trim(), this);
        }

        private void packerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackerInfo info = packerTypeComboBox.SelectedItem as PackerInfo;
            PackParameter param = null;
            if (info != null) param = ToolConfig.Instance.GetPackParameter(info.PackerType);

            if (info == null || param == null)
            {
                ToolConfig.Instance.PackDataFileOutputPath = string.Empty;
                packTemplateDataTextBox.Text = string.Empty;

                cBDatAutoCommit.CheckState = CheckState.Unchecked;
            }
            else
            {
                ToolConfig.Instance.PackDataFileOutputPath = param.PackDataFileOutputPath;
                packTemplateDataTextBox.Text = param.PackDataFileOutputPath;

                cBDatAutoCommit.CheckState = param.PackDataAutoCommit ? CheckState.Checked : CheckState.Unchecked;
            }
        }

        private void packTemplateDataButton_Click(object sender, EventArgs e)
        {
            if (packerTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("请选择打包器类型!");
                packerTypeComboBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(packTemplateDataTextBox.Text.Trim()))
            {
                MessageBox.Show("请选择数据打包后的输出路径!");
                browsePackOutFileButton.Focus();
                return;
            }
            if (e == null && sender != null)
            {
                var onlyCheck = (bool)sender;
                if (onlyCheck)
                {
                    return;
                }
            }
            IList<TableInfo> list = excelToTableInfoList(true);

            if (list.Count == 0) return;

            PackerInfo info = packerTypeComboBox.SelectedItem as PackerInfo;

            PackMgr.PackData(info.PackerType, list, packTemplateDataTextBox.Text.Trim());
        }

        private bool checkMySQLInfo()
        {
            var hostStr = hostTextBox.Text.Trim();
            if (string.IsNullOrEmpty(hostStr))
            {
                MessageBox.Show("主机地址不能为空!");
                return false;
            }
            if(Regex.Matches(hostStr, "[a-zA-Z]").Count > 0)
            {
                MessageBox.Show("主机地址只能填IP!");
                return false;
            }
            if (string.IsNullOrEmpty(portTextBox.Text.Trim()))
            {
                MessageBox.Show("端口号不能为空!");
                return false;
            }

            if (string.IsNullOrEmpty(userTextBox.Text.Trim()))
            {
                MessageBox.Show("用户名不能为空!");
                return false;
            }

            if (string.IsNullOrEmpty(dbTextBox.Text.Trim()))
            {
                MessageBox.Show("数据库不能为空!");
                return false;
            }

            return true;
        }
        private Timer _timer = null;
        private string _connStr = null;
        private int _uploadState = 0;//0：未上传，1：上传中 

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (_uploadState == 1)//点击停止
            {
                stopUpload();
                return;
            }
            if (checkMySQLInfo() == false) return;

            string errMsg = string.Empty;

            if (MySQLUtil.TestMySQLConnect(hostTextBox.Text.Trim(), portTextBox.Text.Trim(), userTextBox.Text.Trim(), pwdTextBox.Text.Trim(), ref errMsg) == false)
            {
                MessageBox.Show(string.Format("连接失败,请检测MySQL连接参数!"));
            }
            _connStr = string.Format(Global.MYSQL_CONNECTION_FORMAT, hostTextBox.Text.Trim(), portTextBox.Text.Trim(), userTextBox.Text.Trim(), pwdTextBox.Text.Trim(), "mysql");
            if (e == null && sender != null)
            {
                var onlyCheck = (bool)sender;
                if (onlyCheck)
                {
                    return;
                }
            }
            if (fileRadioButton.Checked)//单个文件
            {
                uploadButton.Text = "上传中...";
                IList<TableInfo> list = excelToTableInfoList(true);
                if (list.Count == 0) return;
                var result = PackMgr.initPackData((int)PackerType.MySQLPacker, _connStr, this, false);
                if (result)
                {
                    PackMgr.PackData((int)PackerType.MySQLPacker, list, _connStr,list[0].TableName);
                    SeqRun.Instance.onCurTaskFinished();
                }
                else
                {
                    SeqRun.Instance.onCurTaskFinished(false);
                }
                uploadButton.Text = "上传(&D)";
            }
            else
            {
                openTimer();
                //每解析一张表，执行一条插入语句
                //用子线程做异步解析和异步插入
                excelToTableInfoListAsync(true);

            }

        }
        public void openTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer();
                _timer.Tick += new EventHandler(onTick);
                _timer.Interval = 200;//越大运算越密集
                _timer.Enabled = true;
                _timer.Start();
            }
        }
        public void startUpload()
        {
            uploadButton.Text = "停止";
            _uploadState = 1;

        }
        public void stopUpload()
        {
            uploadButton.Text = "上传(&D)";
            _uploadState = 0;
            ExcelUtil._mut.WaitOne();
            ExcelUtil._parsedTableList.Clear();
            ExcelUtil._mut.ReleaseMutex();
        }
        private void onTick(object sender, EventArgs e)
        {
            if (_isOnKeyRunStart)
            {
                _isOnKeyRunStart = false;
                SeqRun.Instance.startRun(this);
            }
            if (_uploadState==0)
            {
                return;
            }
            var list = ExcelUtil._parsedTableList;

            if (list.Count > 0)
            {
                //XLogger.InfoFormat("onTick :{0}", list.Count);
                if (list.Count == 0) return;
                if (_connStr == null) return;
                ExcelUtil._mut.WaitOne();
                var tempList = new List<TableInfo>(list);
                ExcelUtil._parsedTableList.Clear();
                ExcelUtil._mut.ReleaseMutex();
                PackMgr.PackData((int)PackerType.MySQLPacker, tempList, _connStr);


            }
            else if(ExcelUtil._leftFileCount<=0)
            {
                int work = 0;
                int io = 0;
                int maxWork = 0;
                int maxIO = 0;
                ThreadPool.GetAvailableThreads(out work, out io);
                ThreadPool.GetMaxThreads(out maxWork, out maxIO);
                //XLogger.InfoFormat("onTick work:{0} io:{1} maxWork:{2} maxIO:{3}", work,io, maxWork, maxIO);
                if (work == maxWork&& io == maxIO)
                {
                    stopUpload();
                    if (SeqRun.Instance.isRunning())
                    {
                        SeqRun.Instance.onCurTaskFinished();
                    }
                    else
                    {
                        MessageBox.Show("上传到MySQL数据库完成,结果请查看日志框!");
                    }

                }
            }

        }
        private void assignRevertButton_Click(object sender, EventArgs e)
        {
            string contents = assignCodeTextBox.Text.Trim();

            if (string.IsNullOrEmpty(contents)) return;

            string[] replaceArray = new string[] { ".trim()", ".Trim()", ".ToLower()", ".toLower", ".ToUpper", ".toUpper" };

            string[] array = contents.Split(new string[] { "\r\n", "\r", "\n", ";" }, StringSplitOptions.RemoveEmptyEntries);

            List<string> assignList = new List<string>();
            char[] splitter = new char[] { '=' };
            foreach (string item in array)
            {
                string[] temp = item.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                if (temp.Length == 1)
                {
                    assignList.Add(temp[0]);
                }
                else if (temp.Length == 2)
                {
                    string tmp = temp[1].Trim();
                    foreach (string oldStr in replaceArray)
                    {
                        tmp = tmp.Replace(oldStr, string.Empty);
                    }
                    assignList.Add(string.Format("{0} = {1}", tmp, temp[0].Trim()));
                }
                else
                {
                    assignList.Add(item.Trim());
                }
            }

            assignCodeTextBox.Text = string.Join(";\r\n", assignList.ToArray()) + ";";
        }

        private void dropAllTableButton_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;

            if (MySQLUtil.TestMySQLConnect(hostTextBox.Text.Trim(), portTextBox.Text.Trim(), userTextBox.Text.Trim(), pwdTextBox.Text.Trim(), ref errMsg) == false)
            {
                MessageBox.Show(string.Format("连接失败,请检测MySQL连接参数!"));
            }

            string connStr = string.Format(Global.MYSQL_CONNECTION_FORMAT, hostTextBox.Text.Trim(), portTextBox.Text.Trim(), userTextBox.Text.Trim(), pwdTextBox.Text.Trim(), "mysql");

            MySqlConnection conn = MySQLUtil.OpenMySQLConnection(connStr);

            if (conn == null) return;

            string db = dbTextBox.Text.Trim();

            MySQLUtil.DropTables(conn, db, true);
        }

        private string getExcelFolder()
        {
            string upFolder = string.Empty;
            if (fileRadioButton.Checked)
            {
                upFolder = excelFileTextBox.Text.Trim();
                upFolder = upFolder.Substring(0, upFolder.LastIndexOf("\\"));
            }
            else
            {
                upFolder = excelDirTextBox.Text.Trim();
            }
            return upFolder;
        }

        private bool execCommand(string cmd, string arguments, bool showWindow = false)
        {
            try
            {
                Process oProcess = new Process();
                oProcess.StartInfo.FileName = cmd;
                oProcess.StartInfo.Arguments = arguments;
                oProcess.StartInfo.CreateNoWindow = !showWindow;
                oProcess.StartInfo.UseShellExecute = false;
                oProcess.Start();
                oProcess.WaitForExit();
                oProcess.Close();
                //Process.Start(cmd, string.Format("up -q {0}", upFolder));
                return true;
            }
            catch (Exception ex)
            {
                XLogger.ErrorFormat("{0}执行失败!ErrMsg:\r\n{0}", ex.Message, cmd);
                return false;
            }
        }

        private void svnUpdateButton_Click(object sender, EventArgs e)
        {
            string svnFolder = Path.GetFullPath("svn/svn.exe");

            string cmd = string.Empty;
            if (File.Exists(svnFolder))
            {
                cmd = svnFolder;
            }
            else
            {
                cmd = "svn";
            }

            string upFolder = getExcelFolder();

            if (execCommand(cmd, string.Format("up -q {0}", upFolder)))
            {
                MessageBox.Show("更新成功!", "更新SVN", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void openExcelButton_Click(object sender, EventArgs e)
        {
            string upFolder = getExcelFolder();

            if (Directory.Exists(upFolder))
            {
                execCommand("Explorer.exe", upFolder);
            }
            else
            {
                MessageBox.Show(string.Format("目录不存在！Path:{0}", upFolder));
            }
        }

        private void browseExcelOutButton_Click(object sender, EventArgs e)
        {
            string f = FileUtil.BrowseFolder("选择输出目录");
            if (string.IsNullOrEmpty(f) == false)
            {
                excelOutTextBox.Text = f;
                fileRadioButton.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fileRadioButton.Checked == false)
            {
                MessageBox.Show("非Excel文件,不需要拆分!");
                return;
            }

            if (string.IsNullOrEmpty(excelOutTextBox.Text))
            {
                MessageBox.Show("请选择输出目录!");
                return;
            }

            string output = excelOutTextBox.Text;

            IList<TableInfo> list = ExcelUtil.ParseTableList(excelFileTextBox.Text, ignoreNamesTextBox.Text, true);

            for (int i = 0; i < list.Count; i++)
            {
                TableInfo oTableInfo = list[i];

                ExcelUtil.CreateExcelFile(output, oTableInfo);
            }

            MessageBox.Show("拆分完成！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IList<TableInfo> list = excelToTableInfoList(true);

            XLogger.InfoFormat("num:{0}", list.Count);
        }
        public void setTaskFinish(SeqRun.TASK_ID id,bool succeed)
        {
            var idx = (int)id;
            var str = checkedListBox1.Items[idx] as string;
            var strResult = succeed ? "√" : "×";

            checkedListBox1.Items[idx] = str + strResult;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = checkedListBox1.SelectedIndex;//获取此时鼠标点击了哪个项
        }
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var id = (SeqRun.TASK_ID)e.Index;//获取被选中的项的索引
            //CheckState checkState = e.CurrentValue;//获取该项被选中之前的状态
            CheckState newState = e.NewValue;//获取该项被选中后的状态
            if (newState == CheckState.Checked)
            {
                switch (id)
                {
                    case SeqRun.TASK_ID.TASK_ID_AS3:
                    case SeqRun.TASK_ID.TASK_ID_CPP:
                    case SeqRun.TASK_ID.TASK_ID_UNITY:
                        generateCodeContentButton_Click(true, null);//只有当传true,null时候只做表单检查，不执行后续
                        break;
                    case SeqRun.TASK_ID.TASK_ID_PACK:
                        packTemplateDataButton_Click(true, null);//只有当传true,null时候只做表单检查，不执行后续
                        break;
                    case SeqRun.TASK_ID.TASK_ID_UPLOAD_DB:
                        uploadButton_Click(true, null);//只有当传true,null时候只做表单检查，不执行后续
                        break;
                    default:
                        break;
                }
                ToolConfig.Instance.setCheckBoxListSelected(e.Index, true);
            }
            else
            {
                ToolConfig.Instance.setCheckBoxListSelected(e.Index, false);
            }

        }
        private bool _isOnKeyRunStart = false;
        private void onKeyRun_Click(object sender, EventArgs e)
        {
            openTimer();
            if (SeqRun.Instance.isRunning())
            {
                SeqRun.Instance.stop();
                return;
            }
            SeqRun.Instance.clearID();
            int count = 0;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    ++count;
                    SeqRun.Instance.addID((SeqRun.TASK_ID)i);
                }
              
            }
            if (count > 0)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.Items[i] = SeqRun.getTaskName((SeqRun.TASK_ID)i);
                }
                checkedListBox1.Enabled = false;
                fileRadioButton.Enabled = false;
                dirRadioButton.Enabled = false;
                onKeyRun.Text = "停止";
                _isOnKeyRunStart = true;


            }
            else
            {
                MessageBox.Show("没有勾选任务！");
            }
        }
        private GenerateParameter getCurSelectedGenParameter()
        {
            GeneratorInfo genInfo = packerTypeComboBox.SelectedItem as GeneratorInfo;
            if (genInfo != null)
            {
                GenerateParameter param = ToolConfig.Instance.GetGenerateParameter(genInfo.GeneratorType);
                return param;
            }
            return null;
        }
        private void autoCommit_CheckedChanged(object sender, EventArgs e)
        {
            GeneratorInfo genInfo = generatorTypeComboBox.SelectedItem as GeneratorInfo;
            if (genInfo != null)
            {
                GenerateParameter param = ToolConfig.Instance.GetGenerateParameter(genInfo.GeneratorType);
                if (param != null)
                {
                    param.AutoCommit = checkBoxCommit.CheckState == CheckState.Checked;
                }
            }
        }

        private void coverCommit_Click(object sender, EventArgs e)
        {
            var path = textBoxCommitPath.Text.Trim();
            //path = " E:\\svn\\CQ_BUILD\\test\\222.txt";
            SvnUtil.CoverCommit(path,true);

        }

        private void cBDatAutoCommit_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void dirRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ToolConfig.Instance.UseFolder = dirRadioButton.Checked;
        }

        // Excel转Json 生成按钮
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(excelDirTextBox.Text.Trim()))
            {
                MessageBox.Show("请选择输入目录。");
                return;
            }

            if (string.IsNullOrEmpty(jsonOutputTextBox.Text.Trim()))
            {
                MessageBox.Show("请选择输出目录!");
                return;
            }

            string inputDir = excelDirTextBox.Text.Trim();
            string outputDir = jsonOutputTextBox.Text.Trim();

            // 清空文件夹下的旧文件
            if (Directory.Exists(outputDir))
            {
                string[] fileList = Directory.GetFiles(outputDir);

                foreach (var item in fileList)
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            ExcelUtil.TraverseDirectory(inputDir, outputDir, null);

            MessageBox.Show("Json生成完成！");
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            string f = FileUtil.BrowseFolder("选择输出目录");
            if (string.IsNullOrEmpty(f) == false)
            {
                jsonOutputTextBox.Text = f;
                dirRadioButton.Checked = true;
            }
        }
    }
}
