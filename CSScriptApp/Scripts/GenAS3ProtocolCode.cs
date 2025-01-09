#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSScriptApp.Scripts.GenProtocols;
using CSScriptApp.ProtocolCore.GenProtocols;
using CSScriptApp.ProtocolCore.Generates;

namespace CSScriptApp.Scripts
{
    public class GenAS3ProtocolCode : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string projName = args[0] as string;
                string dirPath = args[1] as string;

                List<ProjectInfo> list = new List<ProjectInfo>();
                MySQLUtil.ReloadProjectList(list);
                ProjectInfo oProjectInfo = null;
                foreach (var item in list)
                {
                    if (item.ProjectName == projName)
                    {
                        oProjectInfo = item;
                        break;
                    }
                }

                if (oProjectInfo == null)
                {
                    Program.WriteToConsole("未找到指定项目信息!Name：{0}", projName);
                    return false;
                }

                string sGenType = "AS3Code";

                GeneratorSetting oGeneratorSetting = MySQLUtil.LoadGeneratorSetting(oProjectInfo, sGenType);
                MySQLUtil.ReloadProtocolList(oProjectInfo, oProjectInfo.Protocols);
                MySQLUtil.ReloadStructList(oProjectInfo, oProjectInfo.Structs);
                IGenerator iIGenerator = GeneratorMgr.GetGenerator(sGenType);

                if (iIGenerator != null)
                {
                    iIGenerator.Generate(oProjectInfo, oGeneratorSetting, dirPath, string.Empty);
                    Program.WriteToConsole("生成AS3协议代码成功!!!");
                    return true;
                }
                Program.WriteToConsole("未找到 {0} 代码生成器!!", sGenType);
                return false;
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(ex.Message);
                Program.WriteToConsole(ex.StackTrace);
                return false;
            }
        }

        #endregion
    }
}
#endif