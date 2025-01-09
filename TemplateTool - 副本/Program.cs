using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows.Forms;
using CommandLine;

namespace TemplateTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Global.args = args;

            if (args.Length == 0)
            {
                Application.Run(new MainForm());
            }
            else
            {
                // 命令行解析
                Parser.Default.ParseArguments<Options>(args).MapResult(
                    options =>
                    {
                        Options.Parse(options);                        
                        return 0;
                    },

                    errors =>
                    {
                        MessageBox.Show("Failed to parse command line arguments:");
                        foreach (var error in errors)
                        {
                            // 把所有错误都打印出来的话，弹窗太多了，一般不需要
                            //MessageBox.Show(error.ToString());
                        }
                        return 1;
                    });

            }
        }
    }
}
