using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds
{
    /// <summary>
    /// 命令接口。
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 执行命令过程。
        /// </summary>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        bool Execute(string[] paramsList);
    }
}
