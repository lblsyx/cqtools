using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UnityLight.Loggers
{
    public class ConsoleLogger : ILogger
    {
        //public void Print(LogLevel oLogLevel, string msg)
        //{
        //    ConsoleColor cc = Console.ForegroundColor;

        //    switch (oLogLevel)
        //    {
        //        case LogLevel.eDebug:
        //            break;

        //        case LogLevel.eInfo:
        //            Console.ForegroundColor = ConsoleColor.Green;
        //            break;

        //        case LogLevel.eWarn:
        //            Console.ForegroundColor = ConsoleColor.Yellow;
        //            break;

        //        case LogLevel.eError:
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            break;

        //        case LogLevel.eFatal:
        //            Console.ForegroundColor = ConsoleColor.Magenta;
        //            break;
        //    }

        //    Console.WriteLine(msg);

        //    Console.ForegroundColor = cc;
        //}

        public void Log(LogLevel oLogLevel, string msg)
        {
            ConsoleColor cc = Console.ForegroundColor;

            string pre = string.Empty;
            switch (oLogLevel)
            {
                case LogLevel.DEBUG:
                    pre = "[D]";
                    break;

                case LogLevel.INFO:
                    pre = "[I]";
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case LogLevel.WARN:
                    pre = "[W]";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.ERROR:
                    pre = "[E]";
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.FATAL:
                    pre = "[F]";
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }

            Console.WriteLine(msg);
            
            Console.ForegroundColor = cc;

            Debug.WriteLine(pre + msg);
        }


        public void Clear()
        {
            Console.Clear();
        }
    }
}
