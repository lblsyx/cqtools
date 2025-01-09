using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProtocolClient.Scripts
{
    [Generator("ProtoCode", true)]
    public class ProtoCodeGenerator : IGenerator
    {
        public const int ProtocolStart = 100;

        public string Name
        {
            get { return "Proto代码"; }
        }

        public string ReplaceStr
        {
            get { return Global.CODE_REPLACE_STR; }
        }

        public PathType Path1Type
        {
            get { return PathType.FolderPath; }
        }

        public string Path1Filter
        {
            get { return string.Empty; }
        }

        public PathType Path2Type
        {
            get { return PathType.None; }
        }

        public string Path2Filter
        {
            get { return string.Empty; }
        }

        public void Generate(ProjectInfo oProjectInfo, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2)
        {
            string root = sPath1;
            if (string.IsNullOrEmpty(root))
            {
                MessageBox.Show("请选择Proto代码文件夹");
                return;
            }

            string path;

            StringBuilder sbStructFile = new StringBuilder();

            sbStructFile.Append("message PbPacketHeader\r\n");
            sbStructFile.Append("{\r\n");
            sbStructFile.Append("\tuint32 PacketID = 1;\r\n");
            sbStructFile.Append("\tuint32 OwnerID1 = 2;\r\n");
            sbStructFile.Append("\tuint32 OwnerID2 = 3;\r\n");
            sbStructFile.Append("\tuint32 SourceID1 = 4;\r\n");
            sbStructFile.Append("\tuint32 SourceID2 = 5;\r\n");
            sbStructFile.Append("\tuint32 TargetID1 = 6;\r\n");
            sbStructFile.Append("\tuint32 TargetID2 = 7;\r\n");
            sbStructFile.Append("\tuint32 PacketSize = 8;\r\n");
            sbStructFile.Append("}\r\n\r\n");

            foreach (var stitem in oProjectInfo.Structs)
            {
                sbStructFile.Append(string.Format("message Pb{0}\r\n", stitem.StructName));
                sbStructFile.Append("{\r\n");
                int index = 1;
                foreach (var fielditem in stitem.Fields)
                {
                    // proto格式的字段
                    string protofield = CodeUtil.GetProtobufField(fielditem, index++);
                    sbStructFile.Append(string.Format("{0}\r\n", protofield));
                }                                                                                      
                sbStructFile.Append("}\r\n");
                sbStructFile.Append("\r\n");
            }

            sbStructFile.Append("\r\n");

            foreach (var ptitem in oProjectInfo.Protocols)
            {
                // c2s
                sbStructFile.Append(string.Format("message Pb{0}C2S\r\n", ptitem.ProtocolName));
                sbStructFile.Append("{\r\n");
                sbStructFile.Append("\tPbPacketHeader Header = 1;\r\n");
                if (ptitem.ReqFields.Count > 0)
                {
                    sbStructFile.Append("\r\n");
                }
                int cIndex = 2;
                foreach (var fielditem in ptitem.ReqFields)
                {
                    // proto格式的字段
                    string cppfield = CodeUtil.GetProtobufField(fielditem, cIndex++);
                    sbStructFile.Append(string.Format("{0}\r\n", cppfield));
                }
                sbStructFile.Append("}\r\n");
                sbStructFile.Append("\r\n");

                // s2c
                sbStructFile.Append(string.Format("message Pb{0}S2C\r\n", ptitem.ProtocolName));
                sbStructFile.Append("{\r\n");
                sbStructFile.Append("\tPbPacketHeader Header = 1;\r\n");
                if (ptitem.ResFields.Count > 0)
                {
                    sbStructFile.Append("\r\n");
                }
                int sIndex = 2;
                foreach (var fielditem in ptitem.ResFields)
                {
                    // proto格式的字段
                    string cppfield = CodeUtil.GetProtobufField(fielditem, sIndex++);
                    sbStructFile.Append(string.Format("{0}\r\n", cppfield));
                }
                sbStructFile.Append("}\r\n");
                sbStructFile.Append("\r\n");
            }

            #region 写入Proto协议文件

            path = Path.Combine(root, "protocol.proto");
            using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbStructFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 写入Proto协议文件

            return;
        }

    }
}