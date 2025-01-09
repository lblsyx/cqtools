using DatabaseTool.DatabaseCore;
using DatabaseTool.DatabaseCore.Generates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using UnityLight;

namespace DatabaseTool
{
    public class DatabaseUtil
    {
        public static void CompilerScripts()
        {
#if USE_SCRIPT
            List<string> reflections = new List<string>();
            reflections.AddRange(Global.AppConfig.ScriptReflections);
            string name = System.IO.Path.GetFileName(Application.ExecutablePath);

            if (reflections.IndexOf(name) == -1)
            {
                reflections.Add(name);
            }

            //移动到配置文件内
            //string[] reflections = new string[] {
            //    "System.dll",
            //    "System.Core.dll",
            //    "System.Data.dll",
            //    "System.Data.DataSetExtensions.dll",
            //    "System.Deployment.dll",
            //    "System.Drawing.dll",
            //    "System.Windows.Forms.dll",
            //    "System.Xml.dll",
            //    "System.Xml.Linq.dll",
            //    "MySql.Data.dll",
            //    "UnityLight.dll",
            //    name
            //};

            System.Reflection.Assembly ass = ScriptCompiler.CompileCS("./Scripts", "./Scripts.dll", reflections.ToArray());
#else
            System.Reflection.Assembly ass = typeof(DatabaseUtil).Assembly;
#endif

            StructGeneratorMgr.SearchAssembly(ass);
        }

        public static bool HasListenPort(string ip, int port)
        {
            try
            {
                Ping ping = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "test";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 500;
                PingReply reply = ping.Send(ip, timeout, buffer);
                if (reply.Status == IPStatus.Success)
                {
                    Socket oSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    oSocket.Connect(ip, port);
                    oSocket.Shutdown(SocketShutdown.Both);
                    oSocket.Close();
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public static void ReloadDatabaseList()
        {
            List<DBInfo> list = new List<DBInfo>();
            MySqlUtil.GetDatabaseList(list);
            Global.DBInfoList.Clear();
            foreach (var item in list)
            {
                Global.DBInfoList.Add(item);
            }
        }

        public static void ReloadTableList(DBInfo oDBInfo)
        {
            if (oDBInfo == null) return;

            List<TBInfo> list = new List<TBInfo>();
            MySqlUtil.GetTableList(oDBInfo, list);
            Global.TBInfoList.Clear();
            foreach (var item in list)
            {
                Global.TBInfoList.Add(item);
            }
        }

        public static string GetPath(PathType ePathType, string sFilter)
        {
            string path = string.Empty;

            switch (ePathType)
            {
                case PathType.FilePath:
                    {
                        OpenFileDialog oOpenFileDialog = new OpenFileDialog();
                        oOpenFileDialog.Title = "选择文件:";
                        oOpenFileDialog.Filter = sFilter;
                        if (oOpenFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            path = oOpenFileDialog.FileName;
                        }
                    }
                    break;
                case PathType.FolderPath:
                    {
                        FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();
                        oFolderBrowserDialog.ShowNewFolderButton = true;
                        //oFolderBrowserDialog.SelectedPath = path;
                        oFolderBrowserDialog.Description = "选择文件夹:";
                        if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            path = oFolderBrowserDialog.SelectedPath;
                        }
                    }
                    break;
            }

            return path;
        }

        public static void DeleteTable(DBInfo oDBInfo, TBInfo oTBInfo)
        {
            MySqlUtil.DeleteTable(oDBInfo, oTBInfo);
        }

        public static void DeleteTable(DBInfo oDBInfo, TBInfo[] oTBInfos)
        {
            MySqlUtil.DeleteTable(oDBInfo, oTBInfos);
        }

        public static void TruncateTable(DBInfo oDBInfo, TBInfo oTBInfo)
        {
            MySqlUtil.TruncateTable(oDBInfo, oTBInfo);
        }

        public static void TruncateTable(DBInfo oDBInfo, TBInfo[] oTBInfos)
        {
            MySqlUtil.TruncateTable(oDBInfo, oTBInfos);
        }
    }
}
