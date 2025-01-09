#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.TemplateCore;

namespace CSScriptApp.Scripts
{
    public class GenAS3TplData : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string fileOrDir = args[0] as string;
                string ignoreNames = args[1] as string;
                string output = args[2] as string;

                IList<TableInfo> tables = ExcelUtil.ParseTableList(fileOrDir, ignoreNames, true);

                if (tables.Count == 0)
                {
                    Program.WriteToConsole("未找到模板配置信息!!!");
                    return false;
                }

                PackMgr.PackData((int)PackerType.BinaryPacker, tables, output);

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