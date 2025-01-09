using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Cmds
{
    /// <summary>
    /// 命令管理器。
    /// </summary>
    public sealed class CmdMgr
    {
        private Dictionary<string, ICommand> StartupList = new Dictionary<string, ICommand>();
        private Dictionary<string, ICommand> CommandsList = new Dictionary<string, ICommand>();
        private Dictionary<string, string> DescriptionsList = new Dictionary<string, string>();

        private object mLockHelper = new object();
        private List<string> Commands = new List<string>();

        private CmdMgr()
        {
        }

        public void AsyncExecCmd(string cmd)
        {
            lock (mLockHelper)
            {
                Commands.Add(cmd);
            }
        }

        public void Update()
        {
            string[] cmds = null;
            lock (mLockHelper)
            {
                cmds = Commands.ToArray();
                Commands.Clear();
            }

            foreach (var cmd in cmds)
            {
                ExecuteCommand(cmd);
            }
        }

        /// <summary>
        /// 通过命令行执行对应的启动命令。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool StartupCommand(string cmd)
        {
            return ExecuteCommandImp(cmd, StartupList);
        }

        /// <summary>
        /// 通过命令行执行对应的命令。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public bool ExecuteCommand(string cmd)
        {
            return ExecuteCommandImp(cmd, CommandsList);
        }

        /// <summary>
        /// 通过命令行执行对应的命令。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool ExecuteCommandImp(string cmd, Dictionary<string, ICommand> list)
        {
            if (string.IsNullOrEmpty(cmd)) return false;

            string cmdStr = cmd.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim().ToLower();

            if (string.IsNullOrEmpty(cmdStr))
            {
                XLogger.Error("The command can't empty.");
                return false;
            }

            if (list.ContainsKey(cmdStr) == false)
            {
                XLogger.ErrorFormat("The command of '{0}' is not exists.", cmdStr);
                return false;
            }

            ICommand command = list[cmdStr];

            int paramStartIndex = cmd.IndexOf(" ");

            string[] paramsList = null;

            if (paramStartIndex != -1)
            {
                string tmp = cmd.Substring(paramStartIndex).Trim();
                //if (!string.IsNullOrEmpty(tmp)) tmp = " " + tmp;
                paramsList = tmp.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                paramsList = new string[0];
            }

            if (paramsList.Length > 0 && paramsList[0] == "/?")
            {
                DisplayDescription(cmdStr);
                return true;
            }

            for (int i = 0; i < paramsList.Length; i++)
            {
                paramsList[i] = paramsList[i].Trim();
                paramsList[i] = "-" + paramsList[i];
            }

            try
            {
                return command.Execute(paramsList);
            }
            catch (Exception ex)
            {
                XLogger.Error(string.Format("'{0}' command execute failed.", cmdStr), ex);
                return false;
            }
        }

        /// <summary>
        /// 显示支持的命令列表
        /// </summary>
        public void DisplayCommandList()
        {
            WriteLine(0, "支持的命令如下：", 0, "");

            string[] keyList = new string[CommandsList.Keys.Count];

            CommandsList.Keys.CopyTo(keyList, 0);

            foreach (string cmd in keyList)
            {
                ICommand command = CommandsList[cmd];

                object[] attrs = command.GetType().GetCustomAttributes(typeof(CommandAttribute), false);

                if (attrs.Length == 0) continue;

                CommandAttribute ca = attrs[0] as CommandAttribute;

                WriteLine(4, ca.Cmd, 20, ca.Description);
            }
            WriteLine(2, "命令参数支持多个参数，格式为：", 0, "命令 [-参数名 [参数值 [...]]");
            Console.WriteLine();
        }

        /// <summary>
        /// 显示指定命令的帮助信息。
        /// </summary>
        /// <param name="cmd"></param>
        public void DisplayDescription(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) return;

            cmd = cmd.ToLower();

            if (CommandsList.ContainsKey(cmd) == false)
            {
                XLogger.ErrorFormat("The command of '{0}' is not exists.", cmd);
                return;
            }

            ICommand command = CommandsList[cmd];

            object[] attrs = command.GetType().GetCustomAttributes(typeof(CommandAttribute), false);

            if (attrs.Length > 0)
            {
                CommandAttribute ca = attrs[0] as CommandAttribute;

                if (!string.IsNullOrEmpty(ca.Description))
                {
                    WriteLine(0, ca.Cmd + ": ", ca.Cmd.Length + 2, ca.Description);
                }

                if (!string.IsNullOrEmpty(ca.Usage))
                {
                    WriteLine(0, "用法：", 0, ca.Usage + "\r\n");
                }
            }

            attrs = command.GetType().GetCustomAttributes(typeof(CommandParameterAttribute), false);

            WriteLine(4, "/?", 10, "显示帮助信息");

            for (int i = 0; i < attrs.Length; i++)
            {
                CommandParameterAttribute cpa = attrs[i] as CommandParameterAttribute;
                WriteLine(4, cpa.Key, 10, cpa.Description);
            }
            Console.WriteLine();
        }

        private static void WriteLine(int preSpace, string param, int paramPlace, string description)
        {
            for (int i = 0; i < preSpace; i++)
            {
                Console.Write(" ");
            }

            Console.Write(param);

            int rlt = paramPlace - param.Length;
            if (rlt > 0)
            {
                for (int i = 0; i < rlt; i++)
                {
                    Console.Write(" ");
                }
            }
            else
            {
                if (paramPlace > 0) Console.Write(" ");
            }

            Console.WriteLine(description);
        }

        #region CommandList

        public void Initialize()
        {
            CombinCommonCommand();
        }

        public void ClearCommand()
        {
            CommandsList.Clear();
            CombinCommonCommand();
        }

        private int CombinCommonCommand()
        {
            if (!CommandsList.ContainsKey("help"))
            {
                return CombinCommand(Assembly.GetAssembly(typeof(CmdMgr)), true);
            }

            return 0;
        }

        public int CombinCommand(Assembly ass, bool replaceExists)
        {
            int count = 0;

            Type[] tList = ass.GetTypes();

            string interfaceStr = typeof(ICommand).ToString();

            foreach (Type type in tList)
            {
                if (type.IsClass != true) continue;

                if (type.GetInterface(interfaceStr) == null) continue;

                CommandAttribute[] atts = (CommandAttribute[])type.GetCustomAttributes(typeof(CommandAttribute), true);

                if (atts.Length > 0)
                {
                    count++;

                    if (replaceExists == false && CommandsList.ContainsKey(atts[0].Cmd))
                    {
                        XLogger.ErrorFormat("The command of '{0}' is exists, command's description is {1}", atts[0].Cmd, atts[0].Description);
                    }
                    else
                    {
                        if (replaceExists == true && CommandsList.ContainsKey(atts[0].Cmd))
                        {
                            XLogger.WarnFormat("The command of '{0}' has been replaced! Description: {1}", atts[0].Cmd, DescriptionsList[atts[0].Cmd]);
                        }

                        if (atts[0].Startup)
                        {
                            StartupList[atts[0].Cmd.ToLower()] = (ICommand)Activator.CreateInstance(type);
                        }
                        else
                        {
                            CommandsList[atts[0].Cmd.ToLower()] = (ICommand)Activator.CreateInstance(type);
                        }
                        DescriptionsList[atts[0].Cmd.ToLower()] = atts[0].Description;
                    }
                }
            }

            return count;
        }

        #endregion CommandList

        #region 单例模式

        private static CmdMgr mInstance;
        private static readonly object mSyncRoot = new object();

        public static CmdMgr Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSyncRoot)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new CmdMgr();
                        }

                        return mInstance;
                    }
                }

                return mInstance;
            }
        }

        #endregion 单例模式
    }
}
