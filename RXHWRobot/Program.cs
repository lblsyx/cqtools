using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RXHWRobot
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.random = new Random((int)DateTime.Now.TotalSeconds());
            Global.Main = new MainForm();
            Application.Run(Global.Main);
        }
    }
}
