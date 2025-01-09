using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

namespace CSScriptApp
{
    public class Program
    {
        public static uint ThreadNum = 0;
        public static object ThreadNumLocker = new object();
        public static object ConsoleMsgLocker = new object();
        public static List<string> ConsoleMsgList = new List<string>();

        public static void WriteToConsole(string format, params object[] args)
        {
            lock (ConsoleMsgLocker)
            {
                ConsoleMsgList.Add(string.Format(format, args));
            }
        }

        private static List<IScript> MainScripts = new List<IScript>();
        private static Dictionary<int, List<IScript>> ThreadGroups = new Dictionary<int, List<IScript>>();

        static void Main(string[] args)
        {
            Console.Title = "C#脚本工具";

            Global.CurrentAssembly = typeof(Program).Assembly;
            Global.CurrentFileName = Path.GetFileName(Global.CurrentAssembly.Location);
            Global.CurrentDirectory = Path.GetDirectoryName(Global.CurrentAssembly.Location);

            Directory.SetCurrentDirectory(Global.CurrentDirectory);

            Global.Config.Load(Global.CurrentDirectory);

            try
            {
                Global.CompilerScripts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
                return;
            }

            foreach (var item in Global.ScriptList)
            {
                if (item.EnableRun == false)
                {
                    Console.WriteLine("【{0}】脚本配置为不运行!!!", item.GetType().Name);
                    continue;
                }

                if (item.RunInNewThread)
                {
                    if (ThreadGroups.ContainsKey(item.ThreadGroup) == false)
                    {
                        ThreadGroups.Add(item.ThreadGroup, new List<IScript>());
                    }
                    ThreadGroups[item.ThreadGroup].Add(item);
                }
                else
                {
                    MainScripts.Add(item);
                }
            }

            lock (ThreadNumLocker)
            {
                ThreadNum++;
                ThreadPool.QueueUserWorkItem(new WaitCallback(ScriptThreadProcImp), MainScripts);
                foreach (var item in ThreadGroups)
                {
                    if (item.Key <= 0)
                    {
                        foreach (var script in item.Value)
                        {
                            ThreadNum++;
                            List<IScript> list = new List<IScript>();
                            list.Add(script);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ScriptThreadProcImp), list);
                        }
                    }
                    else
                    {
                        ThreadNum++;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ScriptThreadProcImp), item.Value);
                    }
                }
            }

            WriteConsoleLogBlock();

            Console.Write("脚本执行完成!");

            Console.ReadKey();
        }

        static void ScriptThreadProcImp(object state)
        {
            List<IScript> scripts = state as List<IScript>;
            scripts.Sort(new ScriptComparer());
            foreach (var item in scripts)
            {
                try
                {
                    item.Run(Global.CurrentDirectory);
                }
                catch (Exception ex)
                {
                    Program.WriteToConsole(ex.Message);
                    Program.WriteToConsole(ex.StackTrace);
                }
            }

            lock (ThreadNumLocker)
            {
                ThreadNum--;
            }
        }

        static void WriteConsoleLogBlock()
        {
            List<string> list = new List<string>();

            while (true)
            {
                lock (ConsoleMsgLocker)
                {
                    list.Clear();
                    list.AddRange(ConsoleMsgList);
                    ConsoleMsgList.Clear();
                }

                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }

                bool complete = false;
                lock (ThreadNumLocker)
                {
                    complete = ThreadNum == 0;
                }

                if (complete) break;

                Thread.Sleep(33);
            }
        }
    }
}
