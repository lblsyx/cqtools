using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityLight.Loggers;

namespace TemplateTool.Utils
{
    class CommonUtil
    {
        /// <summary>
        /// 调用可执行文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="command"></param>
        /// <param name="workPath"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool ExecuteProcess(string filePath, string command, string workPath = "", int seconds = 0)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            Process process = new Process();//创建进程对象
            process.StartInfo.WorkingDirectory = workPath;
            process.StartInfo.FileName = filePath;
            process.StartInfo.Arguments = command;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = false;//不重定向输出
            try
            {
                if (process.Start())
                {
                    if (seconds == 0)
                    {
                        process.WaitForExit(); //无限等待进程结束
                    }
                    else
                    {
                        process.WaitForExit(seconds); //等待毫秒
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                XLogger.ErrorFormat(e.Message);
                return false;

            }
            finally
            {
                process.Close();
            }
        }
    }
}
