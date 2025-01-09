using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Loggers
{
    public class XLogger
    {
        public const string ExceptionFormat = "{0}，错误如下：\r\n{1}";

        public static bool Enabled { get; set; }

        public static bool NeedFileInfo { get; set; }

        public static LogLevel GetLevel(LogLevel eLogLevel)
        {
            return LogManager.Level;
        }

        public static void SetLevel(LogLevel eLogLevel)
        {
            LogManager.Level = eLogLevel;
        }

        public static void ClearLoggers()
        {
            LogManager.ClearLoggers();
        }

        public static void AddLogger(ILogger iILogger)
        {
            LogManager.AddLogger(iILogger);
        }

        public static void ClearLog()
        {
            LogManager.ClearLog();
        }

        public static void Debug(string msg)
        {
            LogManager.Log(LogLevel.DEBUG, msg);
        }

        public static void Debug(Exception ex)
        {
            string str = string.Format(ExceptionFormat, "抛出异常", ex.ToString());
            LogManager.Log(LogLevel.DEBUG, str);
        }

        public static void Debug(string msg, Exception ex)
        {
            string str = string.Format(ExceptionFormat, msg, ex.ToString());
            LogManager.Log(LogLevel.DEBUG, str);
        }

        public static void DebugFormat(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            LogManager.Log(LogLevel.DEBUG, str);
        }

        public static void Info(string msg)
        {
            LogManager.Log(LogLevel.INFO, msg);
        }

        public static void Info(Exception ex)
        {
            string str = string.Format(ExceptionFormat, "抛出异常", ex.ToString());
            LogManager.Log(LogLevel.INFO, str);
        }

        public static void Info(string msg, Exception ex)
        {
            string str = string.Format(ExceptionFormat, msg, ex.ToString());
            LogManager.Log(LogLevel.INFO, str);
        }

        public static void InfoFormat(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            LogManager.Log(LogLevel.INFO, str);
        }

        public static void Warn(string msg)
        {
            LogManager.Log(LogLevel.WARN, msg);
        }

        public static void Warn(Exception ex)
        {
            string str = string.Format(ExceptionFormat, "抛出异常", ex.ToString());
            LogManager.Log(LogLevel.WARN, str);
        }

        public static void Warn(string msg, Exception ex)
        {
            string str = string.Format(ExceptionFormat, msg, ex.ToString());
            LogManager.Log(LogLevel.WARN, str);
        }

        public static void WarnFormat(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            LogManager.Log(LogLevel.WARN, str);
        }

        public static void Error(string msg)
        {
            LogManager.Log(LogLevel.ERROR, msg);
        }

        public static void Error(Exception ex)
        {
            string str = string.Format(ExceptionFormat, "抛出异常", ex.ToString());
            LogManager.Log(LogLevel.ERROR, str);
        }

        public static void Error(string msg, Exception ex)
        {
            string str = string.Format(ExceptionFormat, msg, ex.ToString());
            LogManager.Log(LogLevel.ERROR, str);
        }

        public static void ErrorFormat(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            LogManager.Log(LogLevel.ERROR, str);
        }

        public static void Fatal(string msg)
        {
            LogManager.Log(LogLevel.FATAL, msg);
        }

        public static void Fatal(Exception ex)
        {
            string str = string.Format(ExceptionFormat, "抛出异常", ex.ToString());
            LogManager.Log(LogLevel.FATAL, str);
        }

        public static void Fatal(string msg, Exception ex)
        {
            string str = string.Format(ExceptionFormat, msg, ex.ToString());
            LogManager.Log(LogLevel.FATAL, str);
        }

        public static void FatalFormat(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            LogManager.Log(LogLevel.FATAL, str);
        }
    }
}
