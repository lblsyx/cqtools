#if !USE_SCRIPT
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.ProtocolCore.Imports;
using CSScriptApp.ProtocolCore.GenProtocols;

namespace CSScriptApp.Scripts.GenProtocols
{
    [Import(true)]
    public class ImportXML : IImport
    {
        public string Name
        {
            get { return "XML数据"; }
        }

        public string InputFilter
        {
            get { return "导入文件(*.xml)|*.xml"; }
        }

        public PathType InputPathType
        {
            get { return PathType.FilePath; }
        }

        public ProjectInfo Import(string sPath)
        {
            string str = string.Empty;
            ProjectInfo oProjectInfo = null;

            using (StreamReader fs = new StreamReader(sPath))
            {
                str = fs.ReadToEnd();
            }

            try
            {
                oProjectInfo = XMLUtil.Deserialize<ProjectInfo>(str);
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(string.Format("反序列化失败!ErrorMsg:{0}", ex.Message));
                return null;
            }

            return oProjectInfo;
        }
    }
}
#endif