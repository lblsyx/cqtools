using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds.Commons
{
    [Command(@"/?", "查看命令列表 与'Help'命令相同", "")]
    public class HelpSignCmd : ICommand
    {
        public bool Execute(string[] paramsList)
        {
            CmdMgr.Instance.DisplayCommandList();
            return true;
        }
    }
}
