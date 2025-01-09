using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds.Commons
{
    [Command("Exit", "退出当前应用程序", "exit [-y]")]
    [CommandParameter("-y", "不提示确认直接退出当前应用程序")]
    public class ExitCmd : ICommand
    {
        public bool Execute(string[] paramsList)
        {
            if (ServerMain.Main != null)
            {
                ServerMain.Main.Exit();
            }

            //if (paramsList.Length == 0)
            //{
            //    Console.Write("是否退出服务器？(y/n)");
            //    ConsoleKeyInfo cki = Console.ReadKey();

            //    if (cki.Key == ConsoleKey.Y)
            //    {
            //        if (ServerMain.Main != null)
            //        {
            //            ServerMain.Main.Exit();
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine();
            //    }
            //}
            //else
            //{
            //    string p = paramsList[0].ToLower();

            //    if (p.ToLower() == "-y")
            //    {
            //        if (ServerMain.Main != null)
            //        {
            //            ServerMain.Main.Exit();
            //        }
            //    }
            //}

            return true;
        }
    }
}
