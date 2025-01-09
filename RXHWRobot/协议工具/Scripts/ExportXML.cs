using ProtocolCore;
using ProtocolCore.Exports;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolClient.Scripts
{
    [Export(true)]
    public class ExportXML : IExport
    {
        public string Name
        {
            get { return "XML数据"; }
        }

        public string OutputFilter
        {
            get { return "导出文件(*.xml)|*.xml"; }
        }

        public PathType OutputPathType
        {
            get { return PathType.FilePath; }
        }

        public void Export(ProjectInfo oProjectInfo, string sPath)
        {
            string content = XMLUtil.Serialize(oProjectInfo);

            using (FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                fs.Write(bytes, 0, bytes.Length);
            }

            //MessageBox.Show("导出成功!文件位于：\r\n" + saveFile);
        }
    }
}
