using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using CSScriptApp.TemplateCore;
using CSScriptApp.ProtocolCore.Imports;
using CSScriptApp.ProtocolCore.Exports;
using CSScriptApp.ProtocolCore.Generates;
using System.IO;

namespace CSScriptApp
{
    public class Global
    {
        public static Assembly CurrentAssembly;

        public static Assembly ScriptAssembly;

        public static string CurrentFileName;
        public static string CurrentDirectory;

        public static string CodeReplaceString = "__CodeContent__";

        public static string CODE_REPLACE_STR = "$$CodeContent$$";

        public const string MYSQL_CONNECT_TEST_FORMAT = "server={0};port={1};user={2};pwd={3};database={4};pooling=false;connect timeout=2;charset=utf8";
        public const string MYSQL_CONNECTION_FORMAT = "server={0};port={1};user={2};pwd={3};database={4};pooling=false;charset=utf8";

        /// <summary>
        /// {0}：数据库名
        /// </summary>
        public const string MYSQL_CREATE_DATABASE = "CREATE DATABASE IF NOT EXISTS `{0}`";
        /// <summary>
        /// {0}：数据库名
        /// </summary>
        public const string MYSQL_SELECT_TABLES = "SELECT `TABLE_NAME` FROM `information_schema`.`TABLES` WHERE `TABLE_SCHEMA` = '{0}'";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// </summary>
        public const string MYSQL_DROP_TABLES = "DROP TABLE IF EXISTS `{0}`.`{1}`";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// </summary>
        public const string MYSQL_FORCE_DROP_TABLES = "DROP TABLE `{0}`.`{1}`";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// {2}：数据列列表
        /// </summary>
        public const string MYSQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS `{0}`.`{1}` (\r\n  {2}\r\n) ENGINE=InnoDB DEFAULT CHARSET=utf8;";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// {2}：行数据
        /// </summary>
        public const string MYSQL_INSERT_DATA = "INSERT INTO `{0}`.`{1}` VALUES ({2});";

        public static List<IScript> ScriptList = new List<IScript>();
        public static Dictionary<string, IScriptMethod> MethodDict = new Dictionary<string, IScriptMethod>();

        public static Config Config = new Config();

        public static void CompilerScripts()
        {
#if USE_SCRIPT
            List<string> reflections = new List<string>();
            reflections.AddRange(Global.Config.ScriptReflections);
            string name = Path.GetFileName(CurrentAssembly.Location);

            if (reflections.IndexOf(name) == -1)
            {
                reflections.Add(name);
            }
            
            Console.WriteLine("编译脚本!脚本目录：{0}", Path.GetFullPath("./Scripts"));
            Global.ScriptAssembly = ScriptCompiler.CompileCS("./Scripts", "./Scripts.dll", reflections.ToArray());

            GenMgr.SearchAssembly(Global.ScriptAssembly);
            PackMgr.SearchAssembly(Global.ScriptAssembly);
            ImportMgr.SearchAssembly(Global.ScriptAssembly);
            ExportMgr.SearchAssembly(Global.ScriptAssembly);
            GeneratorMgr.SearchAssembly(Global.ScriptAssembly);
            Type[] types = Global.ScriptAssembly.GetTypes();
#else
            GenMgr.SearchAssembly(Global.CurrentAssembly);
            PackMgr.SearchAssembly(Global.CurrentAssembly);
            ImportMgr.SearchAssembly(Global.CurrentAssembly);
            ExportMgr.SearchAssembly(Global.CurrentAssembly);
            GeneratorMgr.SearchAssembly(Global.CurrentAssembly);
            Type[] types = Global.CurrentAssembly.GetTypes();
#endif

            string sInterfaceStr = typeof(IScript).Name;

            ScriptList.Clear();
            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                IScript iIScript = Activator.CreateInstance(type) as IScript;

                ScriptList.Add(iIScript);
            }

            MethodDict.Clear();
            sInterfaceStr = typeof(IScriptMethod).Name;
            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                IScriptMethod iIScriptMethod = Activator.CreateInstance(type) as IScriptMethod;

                MethodDict.Add(type.Name.ToLower(), iIScriptMethod);
            }
        }
    }
}
