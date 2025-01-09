using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityLight.Cmds;
using UnityLight.Configs;
using UnityLight.Loggers;

namespace UnityLight
{
    public class ServerMain
    {
        public static IServerMain Main { get; private set; }

        private static AutoResetEvent oAutoResetEvent;

        public static void AddLogger(ILogger iILogger)
        {
            XLogger.AddLogger(iILogger);
        }

        public static void ClearLoggers()
        {
            XLogger.ClearLoggers();
        }

        public static string Run(ConfigAbstract config, IServerMain iIServerMain, params Assembly[] assemblies)
        {
            string errMsg = string.Empty;
            if (iIServerMain == null)
            {
                XLogger.Error("运行错误!ErrMsg:参数对象不能为空!");
                return "参数对象不能为空!";
            }

            oAutoResetEvent = new AutoResetEvent(false);

            Main = iIServerMain;

            //初始化添加控制台日志对象
            AddLogger(new ConsoleLogger());

            //加载配置
            config.Load();

            LogLevel eLogLevel = (LogLevel)config.LogLevel;
            //设置日志显示级别
            XLogger.SetLevel(eLogLevel);

            //设置控制台标题栏
            Console.Title = string.Format("{0}[日志级别：{1}]", config.ServerName, eLogLevel);

            //初始化命令列表
            CmdMgr.Instance.Initialize();

            //合并执行方法的当前程序的命令
            Assembly ass = iIServerMain.GetType().Assembly;
            
            CmdMgr.Instance.CombinCommand(ass, false);

            //合并其他程序集列表中的所有命令
            if (assemblies.Length > 0)
            {
                foreach (Assembly assembly in assemblies)
                {
                    CmdMgr.Instance.CombinCommand(assembly, false);
                }
            }

            XLogger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>服务器正在启动<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            ////////////////////////    执行启动命令    ////////////////////////
            string[] startupCmds = config.StartupCmds.Split(new string[] { ",", "|", "，" }, StringSplitOptions.RemoveEmptyEntries);
            if (startupCmds.Length > 0)
            {
                foreach (string cmd in startupCmds)
                {
                    XLogger.DebugFormat("正在执行启动命令：{0}...", cmd.Trim());

                    System.Threading.Thread.Sleep(200);

                    //执行命令
                    if (CmdMgr.Instance.StartupCommand(cmd.Trim()) == false)
                    {
                        errMsg = string.Format("{0}启动失败!命令：{1}", config.ServerName, cmd.Trim());
                        XLogger.ErrorFormat("运行错误!ErrMsg:{0}", errMsg);
                        return errMsg;
                    }
                }
            }
            //else
            //{
            //    XLogger.Warn("无启动命令，未配置!");
            //}
            ////////////////////////    执行启动命令    ////////////////////////

            errMsg = iIServerMain.OnStart();
            if (string.IsNullOrEmpty(errMsg) == false)
            {
                XLogger.ErrorFormat("运行错误!ErrMsg:{0}", errMsg);
                return errMsg;
            }

            XLogger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>服务器启动完成<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\r\n");

            Thread oThread = new Thread(WaitCmdProc);
            oThread.Start();
            XLogger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>命令行启动成功<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\r\n");

            XLogger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>启动成功进入主循环<<<<<<<<<<<<<<<<<<<<<<<<<<<<\r\n");
            errMsg = iIServerMain.Update();
            if (string.IsNullOrEmpty(errMsg) == false)
            {
                XLogger.ErrorFormat("运行错误!ErrMsg:{0}", errMsg);
                return errMsg;
            }

            XLogger.Info("===============================正在关闭服务端===============================");
            errMsg = iIServerMain.OnFinally();
            if (string.IsNullOrEmpty(errMsg) == false)
            {
                XLogger.ErrorFormat("运行错误!ErrMsg:{0}", errMsg);
            }
            XLogger.Info("===============================服务端关闭完成===============================");

            return errMsg;
        }

        public static void WaitCmdProc()
        {
            while (true)
            {
                string cmd = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(cmd)) continue;

                CmdMgr.Instance.AsyncExecCmd(cmd);

                if (cmd == "exit") break;
            }
        }
    }
}
