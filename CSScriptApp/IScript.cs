using System;
using System.Collections.Generic;
using System.Text;

namespace CSScriptApp
{
    public interface IScript
    {
        bool EnableRun { get; }
        bool RunInNewThread { get; }
        int ThreadGroup { get; }
        int ThreadGroupIndex { get; }
        void Run(string exeDir);
    }
}
