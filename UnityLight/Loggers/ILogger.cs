using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Loggers
{
    public interface ILogger
    {
        //void Print(LogLevel oLogLevel, string msg);

        void Log(LogLevel oLogLevel, string msg);

        void Clear();
    }
}
