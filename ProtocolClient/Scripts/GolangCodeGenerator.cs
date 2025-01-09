using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProtocolClient.Scripts
{
    public enum ProtocolType
    {
        none,
        C2S,
        S2C,
    }

    [Generator("GolangCode", true)]
    public class GolangCodeGenerator : IGenerator
    {
        public const int ProtocolStart = 100;

        public string Name
        {
            get { return "GolangCode����"; }
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
                MessageBox.Show("��ѡ��Golang�����ļ���");
                return;
            }

            string path;

            #region ���ɽṹ�����
            // �ṹ��      // ������нṹ��Ĵ���
            StringBuilder sbSTMain = new StringBuilder();
            // ��������     // ���һ���ṹ�壬�����Get��������
            StringBuilder sbSTOperate = new StringBuilder();

            sbSTMain.Append(string.Format("/* Э��汾��[{0}]; */\r\n", oProjectInfo.PacketVer));
            sbSTMain.Append("const (\r\n");
            sbSTMain.Append(string.Format("\tPacketVer = {0}\r\n", oProjectInfo.PacketVer));
            sbSTMain.Append(string.Format("\tStructVer = {0}\r\n", Global.StructOperateVer));
            sbSTMain.Append(string.Format("\tProtocolVer = {0}\r\n", Global.ProtocolOperateVer));
            sbSTMain.Append(")\r\n");
            sbSTMain.Append("\r\n");

            // ����ÿһ���ṹ
            foreach (var stitem in oProjectInfo.Structs)
            {
                // �ֶ�������ĸ��д����Ϊgo�д�дΪ���У�СдΪ˽��
                string structName = stitem.StructName;
                structName = char.ToUpper(structName[0]) + structName.Substring(1);

                sbSTOperate.Clear();
                sbSTMain.Append(string.Format("type {0} struct {{\r\n", structName));
                // ����ÿһ���ֶ�
                foreach (var fielditem in stitem.Fields)
                {
                    // �ֶ�
                    string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                    sbSTMain.Append(string.Format("{0}\r\n", GolangField));

                    // ��������
                    string OperateFields = CodeUtil.GetGolangOPFunc(stitem.StructName, fielditem);
                    sbSTOperate.Append(string.Format("{0}\r\n", OperateFields));

                }
                sbSTMain.Append("}\r\n");
                sbSTMain.Append("\r\n");

                sbSTMain.Append(sbSTOperate.ToString());
                // ת������
                sbSTMain.Append(string.Format("{0}\r\n", CodeUtil.GetGolangSTConvertFunc(stitem)));
            }

            #endregion ���ɽṹ�����

            #region ����Э�����
            // Э��         // ����������Э�����ɵĴ���
            StringBuilder sbPTMain = new StringBuilder();

            // Э��ṹ
            const int fileCount = 3;    // ��Ҫ�ּ����ļ�
            StringBuilder[] sbPTStructArray = new StringBuilder[fileCount];

            for (int i = 0; i < fileCount; i++)
            {
                sbPTStructArray[i] = new StringBuilder();
                // ��import
                sbPTStructArray[i].Append("import (\r\n");
                sbPTStructArray[i].Append("\t\"bytes\"\r\n");
                sbPTStructArray[i].Append("\t\"errors\"\r\n");
                sbPTStructArray[i].Append("\t\"reflect\"\r\n");
                sbPTStructArray[i].Append(")\r\n\r\n");
            }

            // ��������     // ���һ��Э�飬�����ӿ�ʵ�ֺ���
            StringBuilder sbPTOperate = new StringBuilder();
            // Э�����
            StringBuilder sbPTCode = new StringBuilder();
            // Э��ӳ��
            StringBuilder sbPTMap = new StringBuilder();
            sbPTMap.Append("var PacketConstructors = map[uint16]func() IPacket{\r\n");

            int protocolStartCode = ProtocolStart + 1;

            // ������������л���ֵ
            int loopCounter = 0;

            int[] switchThresholds = new int[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                switchThresholds[i] = i * 300;
            }

            StringBuilder sbPTStructTemp = sbPTStructArray[0];

            // ����ÿ��Э��
            foreach (var ptitem in oProjectInfo.Protocols)
            {
                // ����Ƿ�ﵽ�л���ֵ
                for (int i = 0; i < fileCount;i++)
                {
                    if (loopCounter == switchThresholds[i])
                    {
                        sbPTStructTemp = sbPTStructArray[i];
                        break;
                    }
                }

                // �ֶ�������ĸ��д����Ϊgo�д�дΪ���У�СдΪ˽��
                string protocolName = ptitem.ProtocolName;
                protocolName = char.ToUpper(protocolName[0]) + protocolName.Substring(1);

                sbPTCode.Append(string.Format("/* [����]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPTMap.Append(string.Format("\t{0} : func() IPacket {{ return &{1}C2S{{Header: &PacketHeader{{}}}} }},\r\n", protocolStartCode, protocolName));
                sbPTCode.Append(string.Format("const {0}C2SCode = {1}\r\n", protocolName, protocolStartCode++));

                sbPTCode.Append(string.Format("/* [��Ӧ]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPTMap.Append(string.Format("\t{0} : func() IPacket {{ return &{1}S2C{{Header: &PacketHeader{{}}}} }},\r\n", protocolStartCode, protocolName));
                sbPTCode.Append(string.Format("const {0}S2CCode = {1}\r\n\r\n", protocolName, protocolStartCode++));

                // C2S
                sbPTOperate.Clear();

                sbPTStructTemp.Append(string.Format("type {0}C2S struct {{\r\n", protocolName));
                sbPTStructTemp.Append("    Header *PacketHeader\r\n");

                if (ptitem.ReqFields.Count() > 0)
                {
                    sbPTStructTemp.Append("\r\n");
                    foreach (var fielditem in ptitem.ReqFields)
                    {
                        // �ֶ�
                        string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                        sbPTStructTemp.Append(string.Format("{0}\r\n", GolangField));

                        // ��������
                        string OperateFields = CodeUtil.GetGolangOPFunc(string.Format("{0}C2S", protocolName), fielditem);
                        sbPTOperate.Append(string.Format("{0}\r\n", OperateFields));
                    }
                }
                sbPTStructTemp.Append("}\r\n");
                sbPTStructTemp.Append("\r\n");

                // װ�ؽӿں���
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangIFFunc(string.Format("{0}C2S", protocolName))));
                // װ�ز�������
                sbPTStructTemp.Append(sbPTOperate.ToString());
                // װ��ת������
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangPTConvertFunc(ptitem, ProtocolType.C2S)));

                // S2C
                sbPTOperate.Clear();

                sbPTStructTemp.Append(string.Format("type {0}S2C struct {{\r\n", protocolName));
                sbPTStructTemp.Append("    Header *PacketHeader\r\n");

                if (ptitem.ResFields.Count() > 0)
                {
                    sbPTStructTemp.Append("\r\n");
                    foreach (var fielditem in ptitem.ResFields)
                    {
                        // �ֶ�
                        string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                        sbPTStructTemp.Append(string.Format("{0}\r\n", GolangField));

                        // ��������
                        string OperateFields = CodeUtil.GetGolangOPFunc(string.Format("{0}S2C", protocolName), fielditem);
                        sbPTOperate.Append(string.Format("{0}\r\n", OperateFields));
                    }
                }
                sbPTStructTemp.Append("}\r\n");
                sbPTStructTemp.Append("\r\n");

                // װ�ؽӿں���
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangIFFunc(string.Format("{0}S2C", protocolName))));
                // װ�ز�������
                sbPTStructTemp.Append(sbPTOperate.ToString());
                // װ��ת������
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangPTConvertFunc(ptitem, ProtocolType.S2C)));

                loopCounter++;
            }
            sbPTMap.Append("}\r\n\r\n");

            sbPTMain.Append(sbPTCode.ToString());
            sbPTMain.Append(sbPTMap.ToString());

            #endregion ����Э�����

            #region д��ProtoЭ���ļ�main

            StringBuilder sbResFile = new StringBuilder();
            sbResFile.Append(sbSTMain.ToString());  // �ṹ�����
            sbResFile.Append(sbPTMain.ToString());  // Э�����

            path = Path.Combine(root, "protocol.go");
            using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbResFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion д��ProtoЭ���ļ�main

            #region д��PortoЭ���ļ�struct

            StringBuilder sbResSFile = new StringBuilder();
            for (int i = 0; i < fileCount; i++)
            {
                sbResSFile.Clear();
                sbResSFile.Append(sbPTStructArray[i].ToString());
                path = Path.Combine(root, String.Format("protocol{0}.go", i+1));
                using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fStream.Position = 0;
                    fStream.SetLength(0);
                    string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbResSFile.ToString());
                    using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }
            }

            #endregion д��ProtoЭ���ļ�struct

            return;
        }

    }
}