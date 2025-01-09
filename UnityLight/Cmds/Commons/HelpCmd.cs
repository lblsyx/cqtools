using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds.Commons
{
    [Command("Help", @"查看命令列表 与'/?'命令相同", "")]
    public class HelpCmd : ICommand
    {
        public bool Execute(string[] paramsList)
        {
            CmdMgr.Instance.DisplayCommandList();
            return true;
        }
    }
}
