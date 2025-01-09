using System;
using CommandLine;
using ProtocolCore.Generates;

namespace ProtocolClient
{
    // 错误码新增后，也要在 模板工具命令行支持.md 中更新
    enum EErrorCode
    {
        Success = 0,
        InvalidGenType = 105,       // 非法的gen_type
    }

    public class Options
    {
        [Option('t', "gen_type", Required = true, HelpText = "Generate type such as: as3, cpp")]
        public string GenType { get; set; }

        [Option('o', "output_path", Required = true, HelpText = "Output file/dir path.")]
        public string OutputPath { get; set; }

        [Option('u', "output_path2", Required = false, HelpText = "Output file/dir path2.")]
        public string OutputPath2 { get; set; }

        [Option('p', "proto_ver", Required = false, HelpText = "Proto version.")]
        public int ProtoVer { get; set; }

        [Option('s', "struct_ver", Required = false, HelpText = "Struct version.")]
        public int StructVer { get; set; }

        [Option('j', "project_id", Required = false, HelpText = "Project ID")]
        public int ProjectID { get; set; }
        public static void Parse(Options options)
        {
            switch (options.GenType)
            {
                case "as3":
                    OnGenCode("AS3Code", options.OutputPath, options.OutputPath2, options.ProtoVer, options.StructVer, options.ProjectID);
                    break;
                case "cpp":
                    OnGenCode("C++Code", options.OutputPath, options.OutputPath2, options.ProtoVer, options.StructVer, options.ProjectID);
                    break;
                default:
                    Environment.Exit((int)EErrorCode.InvalidGenType);
                    break;

            }

            Environment.Exit((int)EErrorCode.Success);
        }

        private static void InitData(int iProjectID)
        {
            Global.AppConfig.Load();

            if (ProtocolUtil.CheckSQLConnectionString())
            {
                ProtocolUtil.ReloadProjectList();
                ProtocolUtil.ReloadCurStructOperateVer();
                ProtocolUtil.ReloadCurProtocolOperateVer();
            }

            if(iProjectID != 0)
            {
                Global.AppConfig.ProjectID = iProjectID;
            }

            if (Global.AppConfig.ProjectID != 0)
            {
                foreach (var item in Global.ProjectList)
                {
                    if (item.ProjectID == Global.AppConfig.ProjectID)
                    {
                        Global.SelectedProject = item;
                        break;
                    }
                }
            }

            ProtocolUtil.CompilerScripts();
        }

        public static int OnGenCode(string strType, string strPath, string strPath2, int iProtoVer, int iStructVer, int iProjectID)
        {
            InitData(iProjectID);

            IGenerator iIGenerator = GeneratorMgr.GetGenerator(strType);

            if (iIGenerator != null)
            {
                try
                {
                    if (iStructVer > 0)
                    {
                        Global.StructOperateVer = iStructVer;
                    }
                    if (iProtoVer > 0)
                    {
                        Global.ProtocolOperateVer = iProtoVer;
                    }
                    ProtocolUtil.ReloadStructList(Global.SelectedProject, Global.StructOperateVer);
                    ProtocolUtil.ReloadProtocolList(Global.SelectedProject, Global.ProtocolOperateVer);

                    GeneratorSetting oGeneratorSetting = ProtocolUtil.LoadGeneratorSetting(strType);
                    iIGenerator.Generate(Global.SelectedProject, oGeneratorSetting, strPath, strPath2);
                }
                catch (Exception)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}
