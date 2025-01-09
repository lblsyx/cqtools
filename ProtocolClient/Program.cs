using CommandLine;
using Microsoft.Extensions.CommandLineUtils;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProtocolClient
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
            Global.MainForm = new MainForm();
            Global.args = args;
            if (args.Length == 0)
            {
                Application.Run(Global.MainForm);
            }
            else
            {
                var app = new CommandLineApplication();

                app.HelpOption("-?|-h|--help");
                app.Option("-t", "Output file/dir path", CommandOptionType.SingleValue);
                app.Option("-o", "Output file/dir path.", CommandOptionType.SingleValue);
                app.Option("-u", "Output file/dir path2.", CommandOptionType.SingleValue);
                app.Option("-p", "Proto version.", CommandOptionType.SingleValue);
                app.Option("-s", "Struct version.", CommandOptionType.SingleValue);
                app.Option("-j", "Project ID.", CommandOptionType.SingleValue);

                app.OnExecute(() =>
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

                    return 0;
                });

                app.Execute(args);
               
            }

        }
    }
}
