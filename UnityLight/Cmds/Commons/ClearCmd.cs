using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Cmds.Commons
{
    [Command("Clear", "清除控制台文字信息", "")]
    public class ClearCmd : ICommand
    {
        public bool Execute(string[] paramsList)
        {
            XLogger.ClearLog();

            return true;
        }
    }
}
