using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProtocolClient.Scripts
{
    [Generator("UnityCode", true)]
    public class UnityCodeGenerator : IGenerator
    {
        public const int ProtocolStart = 100;

        public string Name
        {
            get { return "龙卫客户端代码"; }
        }

        public string ReplaceStr
        {
            get { return Global.CODE_REPLACE_STR; }
        }

        public PathType Path1Type
        {
            get { return PathType.FilePath; }
        }

        public string Path1Filter
        {
            get { return "C#文件(*.cs)|*.cs"; }
        }

        public PathType Path2Type
        {
            get { return PathType.FilePath; }
        }

        public string Path2Filter
        {
            get { return "C#文件(*.cs)|*.cs"; }
        }

        public void Generate(ProjectInfo oProjectInfo, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2)
        {
            #region 生成结构体文件

            StringBuilder sbStructFile = new StringBuilder();

            List<string> lName = new List<string>();

            sbStructFile.Clear();

            foreach (var stitem in oProjectInfo.Structs)
            {
                sbStructFile.Append(string.Format("/// <summary>\r\n    /// {0}\r\n    /// </summary>\r\n", stitem.StructSummary));
                sbStructFile.Append(string.Format("    public class {0}: IStruct \r\n", stitem.StructName));
                sbStructFile.Append("    {\r\n");
                foreach (var fieldItem in stitem.Fields)
                {
                    sbStructFile.Append(string.Format("        /// <summary>\r\n        /// {0}\r\n        /// </summary>\r\n", fieldItem.FieldSummary));
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbStructFile.Append(string.Format("        public {0} {1} = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        sbStructFile.Append(string.Format("        public List<{0}> {1} = new List<{0}>();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else
                    {
                        sbStructFile.Append(string.Format("        public {0} {1};\r\n", CodeUtil.GetU3DTypeStr(fieldItem, false), fieldItem.FieldName));
                    }
                }

                sbStructFile.Append("\r\n        public object Clone()\r\n        {\r\n");
                sbStructFile.Append(string.Format("            {0} st = new {0}();\r\n", stitem.StructName));
                foreach (var fieldItem in stitem.Fields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbStructFile.Append(string.Format("            st.{1} = {1}.Clone() as {0};\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbStructFile.Append(string.Format("            foreach ({0} item in {1})\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                st.{1}.Add(item.Clone() as {0});\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbStructFile.Append(string.Format("            foreach (var item in {0})\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                st.{0}.Add(item);\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbStructFile.Append(string.Format("            st.{0} = {0};\r\n", fieldItem.FieldName));
                    }
                }
                sbStructFile.Append("            return st;\r\n        }\r\n\r\n");


                sbStructFile.Append("        public void Serializtion(ByteArray oByteArray, bool bSerialize)\r\n        {\r\n");
                sbStructFile.Append("            if (bSerialize)\r\n            {\r\n");
                foreach (var fieldItem in stitem.Fields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbStructFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbStructFile.Append(string.Format("                oByteArray.WriteInt({0}.Count);\r\n", fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                {1}[i].Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbStructFile.Append(string.Format("                oByteArray.WriteInt({0}.Count);\r\n", fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                    oByteArray.{0}({1}[i]);\r\n", CodeUtil.GetU3DWriteStr(fieldItem, true), fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbStructFile.Append(string.Format("                oByteArray.{0}({1});\r\n", CodeUtil.GetU3DWriteStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbStructFile.Append("            }\r\n            else\r\n            {\r\n");
                foreach (var fieldItem in stitem.Fields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbStructFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbStructFile.Append(string.Format("                int {0}Count = oByteArray.ReadInt();\r\n", fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                    {0} obj = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                    obj.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                    {0}.Add(obj);\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbStructFile.Append(string.Format("                int {0}Count = oByteArray.ReadInt();\r\n", fieldItem.FieldName));
                            sbStructFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbStructFile.Append("            {\r\n");
                            sbStructFile.Append(string.Format("                    {1}.Add(oByteArray.{0}());\r\n", CodeUtil.GetU3DReadStr(fieldItem, true), fieldItem.FieldName));
                            sbStructFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbStructFile.Append(string.Format("                {1} = oByteArray.{0}();\r\n", CodeUtil.GetU3DReadStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbStructFile.Append("            }\r\n        }\r\n    }");
            }
            
            #endregion 生成结构体文件

            #region 写入结构体文件

            using (FileStream fStream = new FileStream(sPath1, FileMode.OpenOrCreate))
            {
                fStream.Seek(0, SeekOrigin.Begin);
                fStream.SetLength(0);

                string content = oGeneratorSetting.ContentFormat1.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sbStructFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 写入结构体文件

            #region 生成协议文件

            StringBuilder sbProtocolFile = new StringBuilder();
            int protocolStartCode = ProtocolStart + 1;

            sbProtocolFile.Clear();
            sbProtocolFile.Append(string.Format("/// <summary>\r\n    ///  数据包版本号[{0}]\r\n    /// </summary>\r\n", oProjectInfo.PacketVer));
            sbProtocolFile.Append("    public class PacketVer\r\n");
            sbProtocolFile.Append("    {\r\n");
            sbProtocolFile.Append(string.Format("        public const int Ver = {0};", oProjectInfo.PacketVer));
            sbProtocolFile.Append(string.Format("        public const int SVer = {0};", Global.StructOperateVer));
            sbProtocolFile.Append(string.Format("        public const int PVer = {0};", Global.ProtocolOperateVer));
            sbProtocolFile.Append("    }\r\n\r\n");

            foreach (var ptitem in oProjectInfo.Protocols)
            {
                //请求类生成
                string sProName = "Req" + ptitem.ProtocolName;
                sbProtocolFile.Append(string.Format("/// <summary>\r\n    ///  [请求类]{0}\r\n    /// </summary>\r\n", ptitem.ProtocolSummary));
                sbProtocolFile.Append(string.Format("    public class {0}: Packet \r\n", sProName));
                sbProtocolFile.Append("    {\r\n");
                foreach (var fieldItem in ptitem.ReqFields)
                {
                    sbProtocolFile.Append(string.Format("        /// <summary>\r\n        /// {0}\r\n        /// </summary>\r\n", fieldItem.FieldSummary));
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("        public {0} {1} = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        sbProtocolFile.Append(string.Format("        public List<{0}> {1} = new List<{0}>();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("        public {0} {1};\r\n", CodeUtil.GetU3DTypeStr(fieldItem, false), fieldItem.FieldName));
                    }
                }


                sbProtocolFile.Append("\r\n        public override Packet Clone()\r\n        {\r\n");
                sbProtocolFile.Append(string.Format("            {0} pkg = new {0}();\r\n", sProName));
                sbProtocolFile.Append("            pkg.Header = Header;\r\n");
                sbProtocolFile.Append("            pkg.PacketID = PacketID;\r\n");
                sbProtocolFile.Append("            pkg.OwnerID1 = OwnerID1;\r\n");
                sbProtocolFile.Append("            pkg.OwnerID2 = OwnerID2;\r\n");
                sbProtocolFile.Append("            pkg.SourceID1 = SourceID1;\r\n");
                sbProtocolFile.Append("            pkg.SourceID2 = SourceID2;\r\n");
                sbProtocolFile.Append("            pkg.TargetID1 = TargetID1;\r\n");
                sbProtocolFile.Append("            pkg.TargetID2 = TargetID2;\r\n\r\n");

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("            pkg.{1} = {1}.Clone() as {0};\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("            foreach ({0} item in {1})\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                pkg.{1}.Add(item.Clone() as {0});\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("            foreach (var item in {0})\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                pkg.{0}.Add(item);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("            pkg.{0} = {0};\r\n", fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            return pkg;\r\n");
                sbProtocolFile.Append("        }\r\n\r\n");


                sbProtocolFile.Append("        public override void Serializtion(ByteArray oByteArray, bool bSerialize)\r\n        {\r\n");
                sbProtocolFile.Append("            base.Serializtion(oByteArray, bSerialize);\r\n");
                sbProtocolFile.Append("            if (bSerialize)\r\n            {\r\n");
                foreach (var fieldItem in ptitem.ReqFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("                oByteArray.WriteUShort((ushort){0}.Count);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                {1}[i].Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("                oByteArray.WriteUShort((ushort){0}.Count);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    oByteArray.{0}({1}[i]);\r\n", CodeUtil.GetU3DWriteStr(fieldItem, true), fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("                oByteArray.{0}({1});\r\n", CodeUtil.GetU3DWriteStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            }\r\n");
                sbProtocolFile.Append("            else\r\n");
                sbProtocolFile.Append("            {\r\n");
                foreach (var fieldItem in ptitem.ReqFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("                ushort {0}Count = oByteArray.ReadUShort();\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    {0} obj = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                    obj.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                    {0}.Add(obj);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("                ushort {0}Count = oByteArray.ReadUShort();\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    {1}.Add(oByteArray.{0}());\r\n", CodeUtil.GetU3DReadStr(fieldItem, true), fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("                {1} = oByteArray.{0}();\r\n", CodeUtil.GetU3DReadStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            }\r\n");
                sbProtocolFile.Append("        }\r\n");

                sbProtocolFile.Append(string.Format("		public {0}()\r\n", sProName));
                sbProtocolFile.Append("		{\r\n");
                sbProtocolFile.Append(string.Format("			PacketID = {0};\r\n", protocolStartCode++));
                sbProtocolFile.Append("		}\r\n");

                lName.Add(string.Format("        /// <summary>\r\n        ///  [请求类] {0}\r\n        /// </summary>\r\n        {1}Code = {2}", ptitem.ProtocolSummary, sProName, protocolStartCode - 1));

                sbProtocolFile.Append("    }\r\n\r\n");

                sbProtocolFile.Append(string.Format("    /// <summary>\r\n    ///  [请求类] {0} 创建者\r\n    /// </summary>\r\n", ptitem.ProtocolSummary));
                sbProtocolFile.Append(string.Format("    [Package({0})]\r\n", protocolStartCode - 1));
                sbProtocolFile.Append(string.Format("    public class {0}Creator : IPacketCreator\r\n", sProName));
                sbProtocolFile.Append("    {\r\n");
                sbProtocolFile.Append("        public Packet CreatePacket()\r\n");
                sbProtocolFile.Append("        {\r\n");
                sbProtocolFile.Append(string.Format("            return new {0}();\r\n", sProName));
                sbProtocolFile.Append("        }\r\n");
                sbProtocolFile.Append("    }\r\n\r\n\r\n");


                //响应类生成
                sProName = "Res" + ptitem.ProtocolName;
                sbProtocolFile.Append(string.Format("    /// <summary>\r\n    ///  [响应类]{0}\r\n    /// </summary>\r\n", ptitem.ProtocolSummary));
                sbProtocolFile.Append(string.Format("    public class {0}: Packet \r\n", sProName));
                sbProtocolFile.Append("    {\r\n");
                foreach (var fieldItem in ptitem.ResFields)
                {
                    sbProtocolFile.Append(string.Format("        /// <summary>\r\n        /// {0}\r\n        /// </summary>\r\n", fieldItem.FieldSummary));
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("        public {0} {1} = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        sbProtocolFile.Append(string.Format("        public List<{0}> {1} = new List<{0}>();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("        public {0} {1};\r\n", CodeUtil.GetU3DTypeStr(fieldItem, false), fieldItem.FieldName));
                    }
                }


                sbProtocolFile.Append("\r\n        public override Packet Clone()\r\n        {\r\n");
                sbProtocolFile.Append(string.Format("            {0} pkg = new {0}();\r\n", sProName));
                sbProtocolFile.Append("            pkg.Header = Header;\r\n");
                sbProtocolFile.Append("            pkg.PacketID = PacketID;\r\n");
                sbProtocolFile.Append("            pkg.OwnerID1 = OwnerID1;\r\n");
                sbProtocolFile.Append("            pkg.OwnerID2 = OwnerID2;\r\n");
                sbProtocolFile.Append("            pkg.SourceID1 = SourceID1;\r\n");
                sbProtocolFile.Append("            pkg.SourceID2 = SourceID2;\r\n");
                sbProtocolFile.Append("            pkg.TargetID1 = TargetID1;\r\n");
                sbProtocolFile.Append("            pkg.TargetID2 = TargetID2;\r\n\r\n");

                foreach (var fieldItem in ptitem.ResFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("            pkg.{1} = {1}.Clone() as {0};\r\n", fieldItem.FieldType, fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("            foreach ({0} item in {1})\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                pkg.{1}.Add(item.Clone() as {0});\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("            foreach (var item in {0})\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                pkg.{0}.Add(item);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("            pkg.{0} = {0};\r\n", fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            return pkg;\r\n");
                sbProtocolFile.Append("        }\r\n\r\n");


                sbProtocolFile.Append("        public override void Serializtion(ByteArray oByteArray, bool bSerialize)\r\n        {\r\n");
                sbProtocolFile.Append("            base.Serializtion(oByteArray, bSerialize);\r\n");
                sbProtocolFile.Append("            if (bSerialize)\r\n            {\r\n");
                foreach (var fieldItem in ptitem.ResFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("                oByteArray.WriteUShort((ushort){0}.Count);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                {1}[i].Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("                oByteArray.WriteUShort((ushort){0}.Count);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}.Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    oByteArray.{0}({1}[i]);\r\n", CodeUtil.GetU3DWriteStr(fieldItem, true), fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("                oByteArray.{0}({1});\r\n", CodeUtil.GetU3DWriteStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            }\r\n");
                sbProtocolFile.Append("            else\r\n");
                sbProtocolFile.Append("            {\r\n");
                foreach (var fieldItem in ptitem.ResFields)
                {
                    string sTemp = CodeUtil.GetU3DTypeStr(fieldItem, false);
                    if (sTemp == "Pak")
                    {
                        sbProtocolFile.Append(string.Format("                {0}.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldName));
                    }
                    else if (sTemp == "List")
                    {
                        string sTemp2 = CodeUtil.GetU3DTypeStr(fieldItem, true);
                        if (sTemp2 == "Pak")
                        {
                            sbProtocolFile.Append(string.Format("                ushort {0}Count = oByteArray.ReadUShort();\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    {0} obj = new {0}();\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                    obj.Serializtion(oByteArray, bSerialize);\r\n", fieldItem.FieldType, fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                    {0}.Add(obj);\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                        else
                        {
                            sbProtocolFile.Append(string.Format("                ushort {0}Count = oByteArray.ReadUShort();\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append(string.Format("                for (int i = 0; i < {0}Count; i++)\r\n", fieldItem.FieldName));
                            sbProtocolFile.Append("            {\r\n");
                            sbProtocolFile.Append(string.Format("                    {1}.Add(oByteArray.{0}());\r\n", CodeUtil.GetU3DReadStr(fieldItem, true), fieldItem.FieldName));
                            sbProtocolFile.Append("            }\r\n");
                        }
                    }
                    else
                    {
                        sbProtocolFile.Append(string.Format("                {1} = oByteArray.{0}();\r\n", CodeUtil.GetU3DReadStr(fieldItem, false), fieldItem.FieldName));
                    }
                }
                sbProtocolFile.Append("            }\r\n");
                sbProtocolFile.Append("        }\r\n");

                sbProtocolFile.Append(string.Format("		public {0}()\r\n", sProName));
                sbProtocolFile.Append("		{\r\n");
                sbProtocolFile.Append(string.Format("			PacketID = {0};\r\n", protocolStartCode++));
                sbProtocolFile.Append("		}\r\n");
                lName.Add(string.Format("        /// <summary>\r\n        ///  [响应类] {0}\r\n        /// </summary>\r\n        {1}Code = {2}", ptitem.ProtocolSummary, sProName, protocolStartCode - 1));

                sbProtocolFile.Append("    }\r\n\r\n");

                sbProtocolFile.Append(string.Format("    /// <summary>\r\n    ///  [响应类] {0} 创建者\r\n    /// </summary>\r\n", ptitem.ProtocolSummary));
                sbProtocolFile.Append(string.Format("    [Package({0})]\r\n", protocolStartCode - 1));
                sbProtocolFile.Append(string.Format("    public class {0}Creator : IPacketCreator\r\n", sProName));
                sbProtocolFile.Append("    {\r\n");
                sbProtocolFile.Append("        public Packet CreatePacket()\r\n");
                sbProtocolFile.Append("        {\r\n");
                sbProtocolFile.Append(string.Format("            return new {0}();\r\n", sProName));
                sbProtocolFile.Append("        }\r\n");
                sbProtocolFile.Append("    }\r\n\r\n\r\n");
            }

            sbProtocolFile.Append("    /// <summary>\r\n    ///  协议枚举\r\n    /// </summary>\r\n");
            sbProtocolFile.Append("    public enum PacketCode\r\n");
            sbProtocolFile.Append("    {\r\n");
            for (int i = 0; i < lName.Count; i++)
            {
                sbProtocolFile.Append(lName[i]);
                if (i != lName.Count - 1)
                {
                    sbProtocolFile.Append(",");
                }

                sbProtocolFile.Append("\r\n");
            }
            sbProtocolFile.Append("    }\r\n");

            #endregion 生成协议文件

            #region 写入协议文件

            using (FileStream fStream = new FileStream(sPath2, FileMode.OpenOrCreate))
            {
                fStream.Position = 0;
                fStream.SetLength(0);

                string content = oGeneratorSetting.ContentFormat2.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sbProtocolFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 写入协议文件
        }
    }
}
