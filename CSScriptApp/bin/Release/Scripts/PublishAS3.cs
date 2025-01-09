#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using CSScriptApp.Scripts.GenProtocols;
using CSScriptApp.ProtocolCore.GenProtocols;
using System.Threading;

namespace CSScriptApp.Scripts
{
    public class PublishAS3 : BaseScript
    {
        public const string PUBLISH_DIR = @"D:\RXHWPublish";//发布目录
        public const string COMPLIE_OUT = PUBLISH_DIR + @"\SWCs";//编译目录
        public const string OUT_PATH = PUBLISH_DIR + @"\RPGGame.swf";//主游戏库
        public const string MORN_OUT = PUBLISH_DIR + @"\MornAssets";//UI资源压缩输出目录
        public const string MAP_LIST_TARGET = PUBLISH_DIR + @"\MapList.dat";//地图数据输出路径
        public const string TEMP_LIST_TARGET = PUBLISH_DIR + @"\TempList.dat";//模板数据输出路径
        public const string FLASH_EXE = @"C:\Program Files (x86)\Adobe\Adobe Flash CS6\Flash.exe";//Flash程序

        public const string TEMPLATE_FOLDER = @"D:\AllSVN\RXHW\RXHW_PLAN";//模板SVN目录
        public const string TEMPLATE_FILE = @"D:\AllSVN\RXHW\RXHW_PLAN\Template.xls";//模板配置文件
        public const string TEMPLATE_IGNORE_NAMES = @"导出";//生成模板代码忽略名称
        public const string TEMPLATE_CODE_DIR = @"D:\AllSVN\RXHW\RXHW_CLIENT\RPGGame\src\game\tpls";//模板代码输出目录
        public const string TEMPLATE_SVN_URL = @"https://192.168.0.9/svn/Rxhw_Plan";//模板SVN更新URL
        public const string TEMPLATE_SVN_USER = @"sync";//SVN帐户
        public const string TEMPLATE_SVN_PASS = @"###sync###";//SVN密码

        public const string JVM_DIR = @"F:\Software\AS3\FlexSDK\jre";//JVM目录
        public const string FLEX_SDK = @"F:\Software\AS3\FlexSDK\4.6.0";//FlexSDK目录
        public const string PROJECT_ROOT = @"D:\AllSVN\RXHW\RXHW_CLIENT";//项目根目录
        public const string PROJECT_SVN_URL = @"https://192.168.0.9/svn/Rxhw_Client/trunk";//项目SVN更新URL
        public const string PROJECT_SVN_USER = @"sync";//SVN帐户
        public const string PROJECT_SVN_PASS = @"###sync###";//SVN密码

        public const string MAP_LIST_SOURCE = @"Z:\MapList.dat";//地图数据源路径
        public const string TEMP_LIST_SOURCE = @"Z:\TempList.dat";//模板数据源路径
        public const string SWC_PATH = COMPLIE_OUT + @"\RPGGame.swc";//主游戏库源路径
        public const string MORN_DIR = @"D:\AllSVN\RXHW\RXHW_CLIENT\morn\assets";//UI资源源路径

#region IScript 成员

        public override bool EnableRun
        {
            get { return false; }
        }

        public override bool RunInNewThread
        {
            get { return false; }
        }

        public override void Run(string exeDir)
        {
            bool rlt = false;

#region SVN更新

            Log("检出/更新模板SVN目录：{0}", TEMPLATE_FOLDER);
            ScriptMethod.ExecMethod("SVNCOUP", TEMPLATE_FOLDER, TEMPLATE_SVN_URL, TEMPLATE_SVN_USER, TEMPLATE_SVN_PASS, false);
            
            Log("检出/更新模板SVN目录：{0}", PROJECT_ROOT);
            ScriptMethod.ExecMethod("SVNCOUP", PROJECT_ROOT, PROJECT_SVN_URL, PROJECT_SVN_USER, PROJECT_SVN_PASS, true);

#endregion
            
#region 生成模板代码/数据

            //TODO: 还差个生成模板配置数据文件,目前以从远程复制到发布目录代替。
            //生成AS3模板代码
            ScriptMethod.ExecMethod("GenAS3TplCode", TEMPLATE_FILE, TEMPLATE_IGNORE_NAMES, TEMPLATE_CODE_DIR);
            //生成AS3模板数据
            ScriptMethod.ExecMethod("GenAS3TplData", TEMPLATE_FILE, TEMPLATE_IGNORE_NAMES, TEMP_LIST_TARGET);

#endregion
            
#region 生成协议代码

            DBHelper.SetConnectionInfo("192.168.0.10", 3306, "root", "gongxifacai", "protocols");
            ScriptMethod.ExecMethod("GenAS3ProtocolCode", "热血虎卫", @"D:\AllSVN\RXHW\RXHW_CLIENT\RPGGame\src\game\pkts");

#endregion
            
#region AS3编译

            //设置FlexSDK的JVM路径
            string[] lines = File.ReadAllLines(Path.Combine(FLEX_SDK, @"bin\jvm.config"));
            lines[26] = string.Format("java.home={0}", JVM_DIR.Replace("\\", "/"));
            File.WriteAllLines(Path.Combine(FLEX_SDK, @"bin\jvm.config"), lines);

            if (Directory.Exists(COMPLIE_OUT))
            {
                Directory.Delete(COMPLIE_OUT, true);
            }
            Directory.CreateDirectory(COMPLIE_OUT);

            rlt = (bool)ScriptMethod.ExecMethod("ComplieSWC", "ClientLight", PROJECT_ROOT, COMPLIE_OUT, new string[] { });
            if (rlt)
            {
                Log("编译 ClientLight.swc 成功!!");
            }
            else
            {
                Log("编译 ClientLight.swc 失败!!");
                return;
            }

            rlt = (bool)ScriptMethod.ExecMethod("ComplieSWC", "MornUICore", PROJECT_ROOT, COMPLIE_OUT, new string[] { });
            if (rlt)
            {
                Log("编译 MornUICore.swc 成功!!");
            }
            else
            {
                Log("编译 MornUICore.swc 失败!!");
                return;
            }

            rlt = (bool)ScriptMethod.ExecMethod("ComplieSWC", "RPGCore", PROJECT_ROOT, COMPLIE_OUT,
                new string[] {
                    Path.Combine(PROJECT_ROOT, "RPGCore/swc").Replace("\\", "/")
                });
            if (rlt)
            {
                Log("编译 RPGCore.swc 成功!!");
            }
            else
            {
                Log("编译 RPGCore.swc 失败!!");
                return;
            }

            rlt = (bool)ScriptMethod.ExecMethod("ComplieSWC", "RPGGame", PROJECT_ROOT, COMPLIE_OUT,
                new string[] {
                    Path.Combine(PROJECT_ROOT, "RPGCore/swc").Replace("\\", "/"),
                    Path.Combine(PROJECT_ROOT, "RPGGame/swc").Replace("\\", "/")
                });
            if (rlt)
            {
                Log("编译 RPGGame.swc 成功!!");
            }
            else
            {
                Log("编译 RPGGame.swc 失败!!");
                return;
            }

#endregion

#region 解压游戏库

            Thread.Sleep(2000);

            rlt = (bool)ScriptMethod.ExecMethod("UnCompressSWF", SWC_PATH, OUT_PATH);

            if (rlt)
            {
                Log("解压成功!File：{0}", SWC_PATH);

                if (Directory.Exists(COMPLIE_OUT))
                {
                    Directory.Delete(COMPLIE_OUT, true);
                }
            }
            else
            {
                Log("解压失败!File：{0}", SWC_PATH);
            }

#endregion

#region 复制数据文件

            try
            {
                File.Copy(MAP_LIST_SOURCE, MAP_LIST_TARGET, true);
                Log("Copy file：{0}", MAP_LIST_SOURCE);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
            }

            //try
            //{
            //    File.Copy(TEMP_LIST_SOURCE, TEMP_LIST_TARGET, true);
            //    Log("Copy file：{0}", TEMP_LIST_SOURCE);
            //}
            //catch (Exception ex)
            //{
            //    Log(ex.Message);
            //    Log(ex.StackTrace);
            //}

#endregion

#region 压缩UI资源并打包到swf文件

            if (Directory.Exists(MORN_OUT))
            {
                Directory.Delete(MORN_OUT, true);
            }
            rlt = (bool)ScriptMethod.ExecMethod("CompressMornPNG", MORN_DIR, MORN_OUT);

            if (rlt)
            {
                Log("压缩PNG成功!Folder：{0}", MORN_DIR);
            }

            if (ScriptMethod.ExecCommand(FLASH_EXE, Path.Combine(Global.CurrentDirectory, "jsflscripts/packMorn.jsfl")))
            {
                Log("打包SWF文件成功!");

                if (Directory.Exists(MORN_OUT))
                {
                    Directory.Delete(MORN_OUT, true);
                }
            }

#endregion

#region 打开发布目录

            ScriptMethod.ExecCommand("Explorer.exe", PUBLISH_DIR);

#endregion
        }

#endregion
    }
}
#endif