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
            get { return "GolangCode代码"; }
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
                MessageBox.Show("请选择Golang代码文件夹");
                return;
            }

            string path;

            #region 生成结构体代码
            // 结构体      // 存放所有结构体的代码
            StringBuilder sbSTMain = new StringBuilder();
            // 操作函数     // 针对一个结构体，存放其Get操作函数
            StringBuilder sbSTOperate = new StringBuilder();

            sbSTMain.Append(string.Format("/* 协议版本号[{0}]; */\r\n", oProjectInfo.PacketVer));
            sbSTMain.Append("const (\r\n");
            sbSTMain.Append(string.Format("\tPacketVer = {0}\r\n", oProjectInfo.PacketVer));
            sbSTMain.Append(string.Format("\tStructVer = {0}\r\n", Global.StructOperateVer));
            sbSTMain.Append(string.Format("\tProtocolVer = {0}\r\n", Global.ProtocolOperateVer));
            sbSTMain.Append(")\r\n");
            sbSTMain.Append("\r\n");

            // 遍历每一个结构
            foreach (var stitem in oProjectInfo.Structs)
            {
                // 字段名首字母大写，因为go中大写为公有，小写为私有
                string structName = stitem.StructName;
                structName = char.ToUpper(structName[0]) + structName.Substring(1);

                sbSTOperate.Clear();
                sbSTMain.Append(string.Format("type {0} struct {{\r\n", structName));
                // 遍历每一个字段
                foreach (var fielditem in stitem.Fields)
                {
                    // 字段
                    string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                    sbSTMain.Append(string.Format("{0}\r\n", GolangField));

                    // 操作函数
                    string OperateFields = CodeUtil.GetGolangOPFunc(stitem.StructName, fielditem);
                    sbSTOperate.Append(string.Format("{0}\r\n", OperateFields));

                }
                sbSTMain.Append("}\r\n");
                sbSTMain.Append("\r\n");

                sbSTMain.Append(sbSTOperate.ToString());
                // 转换函数
                sbSTMain.Append(string.Format("{0}\r\n", CodeUtil.GetGolangSTConvertFunc(stitem)));
            }

            #endregion 生成结构体代码

            #region 生成协议代码
            // 协议         // 这里存放所有协议生成的代码
            StringBuilder sbPTMain = new StringBuilder();

            // 协议结构
            const int fileCount = 3;    // 想要分几个文件
            StringBuilder[] sbPTStructArray = new StringBuilder[fileCount];

            for (int i = 0; i < fileCount; i++)
            {
                sbPTStructArray[i] = new StringBuilder();
                // 加import
                sbPTStructArray[i].Append("import (\r\n");
                sbPTStructArray[i].Append("\t\"bytes\"\r\n");
                sbPTStructArray[i].Append("\t\"errors\"\r\n");
                sbPTStructArray[i].Append("\t\"reflect\"\r\n");
                sbPTStructArray[i].Append(")\r\n\r\n");
            }

            // 操作函数     // 针对一个协议，存放其接口实现函数
            StringBuilder sbPTOperate = new StringBuilder();
            // 协议序号
            StringBuilder sbPTCode = new StringBuilder();
            // 协议映射
            StringBuilder sbPTMap = new StringBuilder();
            sbPTMap.Append("var PacketConstructors = map[uint16]func() IPacket{\r\n");

            int protocolStartCode = ProtocolStart + 1;

            // 定义计数器和切换阈值
            int loopCounter = 0;

            int[] switchThresholds = new int[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                switchThresholds[i] = i * 300;
            }

            StringBuilder sbPTStructTemp = sbPTStructArray[0];

            // 遍历每条协议
            foreach (var ptitem in oProjectInfo.Protocols)
            {
                // 检查是否达到切换阈值
                for (int i = 0; i < fileCount;i++)
                {
                    if (loopCounter == switchThresholds[i])
                    {
                        sbPTStructTemp = sbPTStructArray[i];
                        break;
                    }
                }

                // 字段名首字母大写，因为go中大写为公有，小写为私有
                string protocolName = ptitem.ProtocolName;
                protocolName = char.ToUpper(protocolName[0]) + protocolName.Substring(1);

                sbPTCode.Append(string.Format("/* [请求]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPTMap.Append(string.Format("\t{0} : func() IPacket {{ return &{1}C2S{{Header: &PacketHeader{{}}}} }},\r\n", protocolStartCode, protocolName));
                sbPTCode.Append(string.Format("const {0}C2SCode = {1}\r\n", protocolName, protocolStartCode++));

                sbPTCode.Append(string.Format("/* [响应]{0}; */\r\n", ptitem.ProtocolSummary));
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
                        // 字段
                        string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                        sbPTStructTemp.Append(string.Format("{0}\r\n", GolangField));

                        // 操作函数
                        string OperateFields = CodeUtil.GetGolangOPFunc(string.Format("{0}C2S", protocolName), fielditem);
                        sbPTOperate.Append(string.Format("{0}\r\n", OperateFields));
                    }
                }
                sbPTStructTemp.Append("}\r\n");
                sbPTStructTemp.Append("\r\n");

                // 装载接口函数
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangIFFunc(string.Format("{0}C2S", protocolName))));
                // 装载操作函数
                sbPTStructTemp.Append(sbPTOperate.ToString());
                // 装载转换函数
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
                        // 字段
                        string GolangField = CodeUtil.GetGolangSTField(fielditem, 4);
                        sbPTStructTemp.Append(string.Format("{0}\r\n", GolangField));

                        // 操作函数
                        string OperateFields = CodeUtil.GetGolangOPFunc(string.Format("{0}S2C", protocolName), fielditem);
                        sbPTOperate.Append(string.Format("{0}\r\n", OperateFields));
                    }
                }
                sbPTStructTemp.Append("}\r\n");
                sbPTStructTemp.Append("\r\n");

                // 装载接口函数
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangIFFunc(string.Format("{0}S2C", protocolName))));
                // 装载操作函数
                sbPTStructTemp.Append(sbPTOperate.ToString());
                // 装载转换函数
                sbPTStructTemp.Append(string.Format("{0}\r\n", CodeUtil.GetGolangPTConvertFunc(ptitem, ProtocolType.S2C)));

                loopCounter++;
            }
            sbPTMap.Append("}\r\n\r\n");

            sbPTMain.Append(sbPTCode.ToString());
            sbPTMain.Append(sbPTMap.ToString());

            #endregion 生成协议代码

            #region 写入Proto协议文件main

            StringBuilder sbResFile = new StringBuilder();
            sbResFile.Append(sbSTMain.ToString());  // 结构体代码
            sbResFile.Append(sbPTMain.ToString());  // 协议代码

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

            #endregion 写入Proto协议文件main

            #region 写入Porto协议文件struct

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

            #endregion 写入Proto协议文件struct

            return;
        }

    }
}