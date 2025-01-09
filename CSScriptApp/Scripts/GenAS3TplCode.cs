#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.Scripts.GenTemplates;
using CSScriptApp.TemplateCore;

namespace CSScriptApp.Scripts
{
    public class GenAS3TplCode : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string fileOrDir = args[0] as string;
                string ignoreNames = args[1] as string;
                string codePath = args[2] as string;
                
                IList<TableInfo> tables = ExcelUtil.ParseTableList(fileOrDir, ignoreNames, false);

                if (tables.Count == 0)
                {
                    Program.WriteToConsole("未找到模板配置信息!!!");
                    return false;
                }

                GenMgr.Generate((int)GeneratorType.AS3Client, tables, codePath, string.Empty);

                return true;
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