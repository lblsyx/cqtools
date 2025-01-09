using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Loggers
{
    internal class LogManager
    {
        private static IList<ILogger> mLoggerList = new List<ILogger>();

        public static LogLevel Level { get; set; }

        public static void ClearLoggers()
        {
            mLoggerList.Clear();
        }

        public static void AddLogger(ILogger oILogger)
        {
            if (mLoggerList.IndexOf(oILogger) != -1) return;

            mLoggerList.Add(oILogger);
        }

        public static void ClearLog()
        {
            foreach (var logger in mLoggerList)
            {
                logger.Clear();
            }
        }

        public static void Log(LogLevel oLogLevel, string msg)
        {
            if (oLogLevel < Level) return;

            foreach (var logger in mLoggerList)
            {
                logger.Log(oLogLevel, msg);
            }
        }

        //public static void Print(LogLevel oLogLevel, string msg)
        //{
        //    if (oLogLevel < Level) return;

        //    foreach (var logger in mLoggerList)
        //    {
        //        logger.Print(oLogLevel, msg);
        //    }
        //}
    }
}
