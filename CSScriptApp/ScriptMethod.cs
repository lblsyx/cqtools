using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CSScriptApp
{
    public class ScriptMethod
    {
        public static object ExecMethod(string name, params object[] args)
        {
            name = name.ToLower();
            if (Global.MethodDict.ContainsKey(name))
            {
                return Global.MethodDict[name].Do(args);
            }
            return null;
        }

        public static bool ExecCommand(string cmd, string arguments, bool showWindow = false)
        {
            try
            {
                Process oProcess = new Process();
                oProcess.StartInfo.FileName = cmd;
                oProcess.StartInfo.Arguments = arguments;
                oProcess.StartInfo.CreateNoWindow = !showWindow;
                oProcess.StartInfo.UseShellExecute = false;
                oProcess.StartInfo.RedirectStandardOutput = true;
                oProcess.Start();
                string rlt = oProcess.StandardOutput.ReadToEnd();
                oProcess.WaitForExit();
                oProcess.Close();
                if (string.IsNullOrEmpty(rlt) == false) Program.WriteToConsole(rlt);
                //Process.Start(cmd, string.Format("up -q {0}", upFolder));
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(ex.Message);
                Program.WriteToConsole(ex.StackTrace);
                return false;
            }
        }

        public static void FindChildren(string folder, List<string> list, string searchPattern)
        {
            if (Directory.Exists(folder) == false) return;

            string[] files = Directory.GetFiles(folder, searchPattern);
            foreach (var item in files)
            {
                list.Add(item);
            }

            string[] dirs = Directory.GetDirectories(folder);
            foreach (var item in dirs)
            {
                FindChildren(item, list, searchPattern);
            }
        }

        public static byte[] ReadFileBytes(string path)
        {
            try
            {
                byte[] buffer = new byte[1024];

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = File.OpenRead(path))
                    {
                        while (true)
                        {
                            int len = fs.Read(buffer, 0, buffer.Length);
                            if (len > 0) ms.Write(buffer, 0, len);
                            else break;
                        }

                        return ms.ToArray();
                    }
                }
            }
            catch// (Exception ex)
            {
                return new byte[0];
            }
        }

        public static bool WriteFileBytes(string path, byte[] bytes, int offset = 0)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(bytes, offset, bytes.Length - offset);
                    fs.Flush();
                }

                return true;
            }
            catch// (Exception ex)
            {
                return false;
            }
        }

        public static long GetFileLength(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            return fileInfo.Length;
        }
    }
}
