using System;
using System.IO;
using System.Windows.Forms;
using UnityLight.Loggers;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;

namespace TemplateTool.Utils
{
    class SvnUtil
    {
        /**
         * 
         * 
         * 
         * 
         *  svn cleanup  /path/to/your/project

            svn add --force /path/to/your/project/*

            svn cleanup  /path/to/your/project

            svn commit /path/to/your/project  -m 'Adding a file'
         * 
         * 
         ***/

        private static string _svnPath = "";
        /// <summary>
        /// 先把目录下文件删光，再调用svn update
        /// </summary>
        /// <param name="path"></param>
        public static void DelFileAndSvnUpdate(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Update(path);
        }

        public static bool checkPath(string path)
        {

            if (path == String.Empty || path == null)
            {
                MessageBox.Show("文件路径为空！");
                return false;
            }
            var pathDirName = String.Empty;
            if (Path.GetExtension(path).Length > 0)
            {
                pathDirName = Path.GetDirectoryName(path);
            }
            else
            {
                pathDirName = path;
            }
            if (pathDirName != String.Empty && Directory.Exists(pathDirName))
            {
                return true;
            }
            else
            {
                MessageBox.Show("文件路径不存在！请检查输出文件输入框:", pathDirName);
                return false;

            }
        }
        public static void Revert(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                ExecuteSvnCmd(string.Format("revert -R {0}", path));
            }
        }
        //更新目录
        public static void Update(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                ExecuteSvnCmd(string.Format("up -q {0}", path));
            }
        }
        //svn cleanup  /path/to/your/project
        public static void Cleanup(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                ExecuteSvnCmd(string.Format("cleanup {0}", path));
            }
        }
        //svn commit /path/to/your/project  -m '日志内容'
        public static void Commit(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                ExecuteSvnCmd(string.Format("ci {0} -m {1}", path, "工具自动提交"));
            }
        }
        /// <summary>
        /// add 本地新增的文件 命令：svn add --force /path/to/your/project/*
        /// </summary>
        /// <param name="path"></param>
        /// <param name="needCheck"></param>
        public static void Add(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                ExecuteSvnCmd(string.Format("add --force {0}", path));
            }

        }

        //svn resolve --accept working 1.txt  //使用1.txt 冲突符号保留
        //svn resolve --accept theirs-full 1.txt 使用1.txt.rNew作为最后提交的版本
        //svn resolve --accept mine-full 1.txt 使用1.txt.mine作为最后提交的版本(使用完全的mine文本)
        //svn resolve --accept mine-conflict 1.txt 使用1.txt.mine的冲突部分作为最后提交的版本
        //svn resolve --accept theirs-conflict 1.txt 使用1.txt.rNew的冲突部分作为最后提交的版本(选项会保留其它人引起冲突的部分)
        /// <summary>
        /// 同时解决内容冲突和树冲突
        /// </summary>
        /// <param name="path"></param>
        /// <param name="needCheck"></param>
        public static void resolveUseMy(string path, bool needCheck = true)
        {
            var isPathValid = true;
            if (needCheck)
            {
                isPathValid = checkPath(path);
            }
            if (isPathValid)
            {
                //svn resolve --accept mine-conflict -R E:\svn\CQ_BUILD\test | svn resolve --accept working -R E:\svn\CQ_BUILD\test
                //step1：解决内容冲突 step2：解决树冲突
                ExecuteSvnCmd(string.Format("resolve --accept mine-conflict -R {0} resolve --accept working -R {1}", path, path));

            }
        }
        public static void resolveUseMy(string []files, bool needCheck = true)
        {
            for (int i = 0; i < files.Length; ++i)
            {
                if (needCheck &&!checkPath(files[i]))
                {
                    return;
                }
                files[i] = files[i] + ' ';
            }
            var filesStr = string.Concat(files);
            resolveUseMy(filesStr, false);
        }
        
        /// <summary>
        /// 删除本地丢失的文件 
        /// powershell命令：
        /// 1.带路径：svn status E:\svn\CQ_BUILD\test | ? { $_ -match '^!\s+(.*)' } | % { svn rm $Matches[1] } 
        /// 2.不带路径（已经cd到工作目录下） svn status | ? { $_ -match '^!\s+(.*)' } | % { svn rm $Matches[1] } 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="needCheck"></param>
        public static void DelteLocalLost(string path, bool needCheck = true)
        {

        }
        //private static List<string> drives = new List<string>() { "c:", "d:", "e:", "f:" };

        //private static string svnPath = @"\Program Files\TortoiseSVN\bin\";
        //private static string svnProc = @"TortoiseProc.exe";
        //private static string svnProcPath = "";
        private static string GetSvnProcPath()
        {

            string disk = Application.StartupPath.Substring(0, Application.StartupPath.IndexOf(":"));
            if (_svnPath != string.Empty)
            {
                return _svnPath;
            }
            //foreach (string item in drives)
            //{
            //    string path = string.Concat(item, svnPath, svnProc);
            //    if (File.Exists(path))
            //    {
            //        _svnPath = path;
            //        break;
            //    }
            //}
            string svnFolder = Path.GetFullPath("svn/svn.exe");
            string cmd = string.Empty;
            if (File.Exists(svnFolder))
            {
                _svnPath = svnFolder;
            }
            if (_svnPath == string.Empty)
            {
                _svnPath = FileUtil.SelectOneFile("Select TortoiseProc.exe", disk + ":\\", "exe文件|*.exe"); //"C++头文件(*.h;*.hpp)|*.h;*.hpp";
            }
            //可以配置一下
            return _svnPath;
        }

        public static void ExecuteSvnCmd(string command, int seconds = 0)
        {
            var filePath = GetSvnProcPath();
            CommonUtil.ExecuteProcess(filePath, command, "", seconds);
        }
        /// <summary>
        /// 用本地版本完全覆盖服务器版本，本地缺少的要delete，本地新增的add,其他的用本地覆盖服务器
        /// 具体执行步骤：1.解决冲突,2.add和delete, 3.提交
        /// ps命令：svn add * --force | svn status | ? { $_ -match '^!\s+(.*)' } | % { svn rm $Matches[1] } | svn ci -m '脚本自动提交'
        /// </summary>
        /// <param name="command"></param>
        /// <param name="seconds"></param>
        public static void CoverCommit(string commitPath, bool needDeleteLostFile, int seconds = 0)
        {

            //判断是目录还是单个文件
            var ext = Path.GetExtension(commitPath);
            var command = "";
            var cdCmd = "";
            if (ext != String.Empty)//单个文件提交不能删除丢失的文件
            {
                if (!File.Exists(commitPath))
                {
                    MessageBox.Show("文件不存在！");
                    return;
                }
                cdCmd = "cd " + Path.GetDirectoryName(commitPath);
                var fileName = Path.GetFileName(commitPath);
                command = String.Format(" | svn resolve {0} --accept working -R | svn add {1} --force | svn ci {2} -m '工具自动提交'", fileName, fileName, fileName);
            }
            else
            {
                if (!Directory.Exists(commitPath))
                {
                    MessageBox.Show("路径不存在！");
                    return;
                }
                // cd E:\\svn\\CQ_BUILD\\test | svn add * --force | svn status
                cdCmd = "cd " + commitPath;
                if (needDeleteLostFile)
                {
                    command = @" | svn status | ? { $_ -match '^!\s+(.*)' } | % { svn rm $Matches[1] } | svn up | svn resolve * --accept mine-conflict -R | svn resolve * --accept working -R | svn add * --force | svn ci -m '工具自动提交'";
                }
                else
                {
                    command = @" | svn up | svn resolve * --accept mine-conflict -R | svn resolve * --accept working -R | svn add * --force | svn ci -m '工具自动提交'";
                }
            }
            string svnFolder = Path.GetFullPath("svn/svn.exe");
            if (svnFolder != String.Empty && File.Exists(svnFolder))
            {
                command = command.Replace("svn", svnFolder);
            }
            else
            {
                XLogger.Error("svn.exe 不存在");
            }
            ExecutePowerShell(cdCmd+command, false);
            //Collection<PSObject> Results = new Collection<PSObject>();
            //PowerShell ps = PowerShell.Create();
            //if (PS.Basic.RunPS(ps, command, out Results))
            //{//执行成功
            //    XLogger.InfoFormat("CoverCommit Succeed! CMD:{0}", command);
            //}
            //else
            //{//执行报错了
            //    XLogger.ErrorFormat("CoverCommit ERROR! CMD:{0}", command);
            //}

        }
        public static void ExecutePowerShell(string command,bool autoClose = true, int seconds = 0)
        {
            string[] psPaths = {
                    "C:\\WINDOWS\\system32\\WindowsPowerShell\\v1.0\\powershell.exe",
                    "C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0\\powershell.exe"
                    };
            var psPath = String.Empty;
            foreach (string item in psPaths)
            {
                if (File.Exists(item))
                {
                    psPath = item;
                    break;
                }
            }
            if (psPath == String.Empty)
            {
                XLogger.Error("未找到pwoershell");
                return;
            }
            if (!autoClose)
            {
                command = "-noexit " + command;
            }
            XLogger.Info("PowerShell:"+ command);
            CommonUtil.ExecuteProcess(psPath, command, "", seconds);
        }
        public static void CoverCommit(string[] files, int seconds = 0)
        {

            for(int i=0;i< files.Length;++i)
            {
                if (!checkPath(files[i]))
                {
                    return;
                }
                files[i] = files[i] + ' ';
            }
            var str_paths = string.Concat(files);
            //var command = String.Format("svn resolve {0} --accept mine-conflict -R | svn resolve {1} --accept working -R | svn add {2} --force | svn ci {3} -m '工具自动提交'", str_paths, str_paths, str_paths, str_paths);
            //生成代码后文件被覆盖了 解决冲突时候 只需使用覆盖的文件： 
            var command = String.Format("svn resolve {0} --accept working -R | svn add {1} --force | svn ci {2} -m '工具自动提交'", str_paths, str_paths, str_paths);
            string svnFolder = Path.GetFullPath("svn/svn.exe");
            if (svnFolder != String.Empty && File.Exists(svnFolder))
            {
                command = command.Replace("svn", svnFolder);
            }
            else
            {
                XLogger.Error("svn.exe 不存在");
            }
            ExecutePowerShell(command, false, seconds);
        }
    }

}
