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
    [Generator("C++Code", true)]
    public class CPPCodeGenerator : IGenerator
    {
        public const int ProtocolStart = 100;

        public string Name
        {
            get { return "C++服务端代码"; }
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
            get { return "头文件(*.h;*.hpp)|*.h;*.hpp"; }
        }

        public PathType Path2Type
        {
            get { return PathType.FilePath; }
        }

        public string Path2Filter
        {
            get { return "代码文件(*.c;*.cpp)|*.c;*.cpp"; }
        }

        public void Generate(ProjectInfo oProjectInfo, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2)
        {

            if (string.IsNullOrEmpty(sPath1))
            {
                MessageBox.Show("请选择C++协议/结构头文件");
                return;
            }

            if (string.IsNullOrEmpty(sPath2))
            {
                MessageBox.Show("请选择C++协议/结构代码文件");
                return;
            }

            string[] splitter = new string[] { "\r\n" };

            //结构头文件
            StringBuilder sbSHFile = new StringBuilder();
            //结构头文件(序列化/反序列化)
            StringBuilder sbSSHFile = new StringBuilder();
            //结构实现文件(序列化/反序列化)
            StringBuilder sbSCFile = new StringBuilder();
            //结构体重置
            StringBuilder sbReset = new StringBuilder();
            //MsgPack序列化
            StringBuilder sbMsgPack = new StringBuilder();

            //添加包含头文件
            //string includes = Global.Config.ProtocolIncludes.Replace(",", "\r\n");
            //sbSCFile.Append(includes + "\r\n\r\n");

            foreach (var stitem in oProjectInfo.Structs)
            {
                //结构体重置
                sbReset.Clear();
                sbReset.Append("    void reset()\r\n");
                sbReset.Append("    {\r\n");
                //结构体声明
                sbSHFile.Append(string.Format("typedef struct ST{0}\r\n", stitem.StructName));
                sbSHFile.Append("{\r\n");
                sbSHFile.Append(string.Format("    ST{0}()\r\n", stitem.StructName));
                sbSHFile.Append("    {\r\n");
                sbSHFile.Append("        reset();\r\n");
                sbSHFile.Append("    }\r\n");

                sbMsgPack.Clear();
                sbMsgPack.Append("    template<class T>\r\n    void pack(T& pack) {\r\n        pack(");
                foreach (var fielditem in stitem.Fields)
                {
                    //声明代码
                    string cppfield = CodeUtil.FieldData2CPPField(fielditem, 4);
                    sbSHFile.Append(string.Format("{0}\r\n", cppfield));
                    //重置代码
                    string ctorfield = CodeUtil.FieldData2CtorField(fielditem, 8);
                    sbReset.Append(string.Format("{0}\r\n", ctorfield));
                    //
                    sbMsgPack.Append(string.Format("{0}, ", fielditem.FieldName));
                }

                sbReset.Append("    }\r\n");

                sbMsgPack.Remove(sbMsgPack.Length - 2, 2);
                sbMsgPack.Append(");\r\n    };\r\n");

                sbSHFile.Append(sbReset.ToString());

                sbSHFile.Append(sbMsgPack.ToString());

                sbSHFile.Append("}");
                sbSHFile.Append(string.Format(" {0}, *LP{0};\r\n\r\n", stitem.StructName));

                //结构体序列化/反序列化运算符重载
                sbSSHFile.Append(string.Format("ServerLight::ByteArray& operator >> (ServerLight::ByteArray& stream, ST{0}& oST{0});\r\n", stitem.StructName));
                sbSSHFile.Append(string.Format("ServerLight::ByteArray& operator << (ServerLight::ByteArray& stream, ST{0}& oST{0});\r\n\r\n", stitem.StructName));
                sbSSHFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, ST{0}& oST{0});\r\n", stitem.StructName));
                sbSSHFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, ST{0}& oST{0});\r\n\r\n", stitem.StructName));

                //结构体序反列化运算符重载
                sbSCFile.Append(string.Format("ServerLight::ByteArray& operator >> (ServerLight::ByteArray& stream, ST{0}& oST{0})\r\n", stitem.StructName));
                sbSCFile.Append("{\r\n");
                foreach (var fielditem in stitem.Fields)
                {
                    //反序列化代码
                    string serfield = CodeUtil.FieldData2CPPDeserializeBin(fielditem, string.Format("oST{0}.", stitem.StructName), 4);
                    //                     string[] lst = serfield.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    //                     serfield = string.Join("    \r\n", lst);
                    //                     sbSCFile.Append(string.Format("    {0}\r\n", serfield));
                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return stream;\r\n}\r\n\r\n");

                //结构体序列化运算符重载
                sbSCFile.Append(string.Format("ServerLight::ByteArray& operator << (ServerLight::ByteArray& stream, ST{0}& oST{0})\r\n", stitem.StructName));
                sbSCFile.Append("{\r\n");
                foreach (var fielditem in stitem.Fields)
                {
                    //序列化代码
                    string serfield = CodeUtil.FieldData2CPPSerializeBin(fielditem, string.Format("oST{0}.", stitem.StructName), 4);
                    //                     string[] lst = serfield.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    //                     serfield = string.Join("    \r\n", lst);
                    //                     sbSCFile.Append(string.Format("    {0}\r\n", serfield));
                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return stream;\r\n}\r\n\r\n");

                //结构体反序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator >> (Json::Value& jsonvalue, ST{0}& oST{0})", stitem.StructName));
                sbSCFile.Append("{\r\n");
                foreach (var fielditem in stitem.Fields)
                {
                    //反序列化代码
                    string serfield = CodeUtil.FieldData2CPPDeserializeJson(fielditem, string.Format("oST{0}.", stitem.StructName), 4);
                    //                     string[] lst = serfield.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    //                     serfield = string.Join("    \r\n", lst);
                    //                     sbSCFile.Append(string.Format("    {0}\r\n", serfield));
                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");

                //结构体序列化JSON运算符重载
                sbSCFile.Append(string.Format("Json::Value& operator << (Json::Value& jsonvalue, ST{0}& oST{0})", stitem.StructName));
                sbSCFile.Append("{\r\n");
                foreach (var fielditem in stitem.Fields)
                {
                    //序列化代码
                    string serfield = CodeUtil.FieldData2CPPSerializeJson(fielditem, string.Format("oST{0}.", stitem.StructName), 4);
                    //                     string[] lst = serfield.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    //                     serfield = string.Join("    \r\n", lst);
                    //                     sbSCFile.Append(string.Format("    {0}\r\n", serfield));
                    sbSCFile.Append(serfield);
                    sbSCFile.Append("\r\n");
                }
                sbSCFile.Append("\r\n    return jsonvalue;\r\n}\r\n\r\n");
            }

            //协议头文件
            StringBuilder sbPHFile = new StringBuilder();
            //协议头文件(宏协议号)
            StringBuilder sbPDHFile = new StringBuilder();
            //协议实现文件
            StringBuilder sbPCFile = new StringBuilder();

            StringBuilder sbCtor = new StringBuilder();
            StringBuilder sbClone = new StringBuilder();
            StringBuilder sbBinRead = new StringBuilder();
            StringBuilder sbBinWrite = new StringBuilder();

            StringBuilder sbJsonRead = new StringBuilder();
            StringBuilder sbJsonWrite = new StringBuilder();

            sbPDHFile.Append(string.Format("/* 协议版本号[{0}]; */\r\n", oProjectInfo.PacketVer));
            sbPDHFile.Append(string.Format("#define PacketVer {0}\r\n", oProjectInfo.PacketVer));
            sbPDHFile.Append(string.Format("#define StructVer {0}\r\n", Global.StructOperateVer));
            sbPDHFile.Append(string.Format("#define ProtocolVer {0}\r\n", Global.ProtocolOperateVer));
            int protocolStartCode = ProtocolStart + 1;
            foreach (var ptitem in oProjectInfo.Protocols)
            {
                sbPDHFile.Append(string.Format("/* [请求]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPDHFile.Append(string.Format("#define Req{0}Code {1}\r\n", ptitem.ProtocolName, protocolStartCode++));
                sbPDHFile.Append(string.Format("/* [响应]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPDHFile.Append(string.Format("#define Res{0}Code {1}\r\n\r\n", ptitem.ProtocolName, protocolStartCode++));

                //请求包代码
                sbCtor.Clear();
                sbClone.Clear();
                sbBinRead.Clear();
                sbBinWrite.Clear();
                sbJsonRead.Clear();
                sbJsonWrite.Clear();
                sbMsgPack.Clear();

                sbMsgPack.Append("    template<class T>\r\n    void pack(T& pack) {\r\n        pack(");
                sbCtor.Append(string.Format("    usPacketID = Req{0}Code;\r\n", ptitem.ProtocolName));
                sbPHFile.Append(string.Format("/* [请求]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPHFile.Append(string.Format("BEGIN_PACKET_CLASS(Req{0}, Req{0}Code)\r\n", ptitem.ProtocolName));
                foreach (var fielditem in ptitem.ReqFields)
                {
                    //声明代码
                    string cppfield = CodeUtil.FieldData2CPPField(fielditem, 4);
                    sbPHFile.Append(string.Format("{0}\r\n", cppfield));

                    string serfield = CodeUtil.FieldData2CPPDeserializeBin(fielditem, "", 8);
                    sbBinRead.Append(string.Format("{0}\r\n", serfield));

                    string ctorfield = CodeUtil.FieldData2CtorField(fielditem, 4);
                    sbCtor.Append(string.Format("{0}\r\n", ctorfield));

                    string clonefield = CodeUtil.FieldData2CloneField(fielditem, 4);
                    sbClone.Append(string.Format("{0}\r\n", clonefield));

                    serfield = CodeUtil.FieldData2CPPDeserializeJson(fielditem, "", 8);
                    sbJsonRead.Append(string.Format("{0}\r\n", serfield));

                    serfield = CodeUtil.FieldData2CPPSerializeBin(fielditem, "", 8);
                    sbBinWrite.Append(string.Format("{0}\r\n", serfield));

                    serfield = CodeUtil.FieldData2CPPSerializeJson(fielditem, "", 8);
                    sbJsonWrite.Append(string.Format("{0}\r\n", serfield));
                }

                //协议来源限定
                if (ptitem.FromClient == 0)
                {
                    sbPHFile.Append("    bool ClientRequest() { return false; }\r\n");
                }
                else
                {
                    sbPHFile.Append("    bool ClientRequest() { return true; }\r\n");
                }
                sbPHFile.Append(string.Format("END_PACKET_CLASS(Req{0})\r\n\r\n", ptitem.ProtocolName));

                //构造函数代码
                sbPCFile.Append(string.Format("PACKET_CONSTRUCTOR(Req{0}, Req{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                sbPCFile.Append(sbCtor.ToString());
                sbPCFile.Append("}\r\n\r\n");
                //sbPCFile.Append(string.Format("END_PACKET_CONSTRUCTOR(Req{0}, Req{0}Code)\r\n\r\n", ptitem.ProtocolName));

                //克隆函数代码
                sbPCFile.Append(string.Format("PACKET_CLONE(Req{0}, Req{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                sbPCFile.Append(string.Format("    Req{0}* pkg = (Req{0}*)ServerLight::PacketCreator::GetInstance().CreatePacket(Req{0}Code);\r\n", ptitem.ProtocolName));
                sbPCFile.Append(sbClone.ToString());
                sbPCFile.Append("    return pkg;\r\n");
                sbPCFile.Append("}\r\n\r\n");

                //序列化/反序列化代码
                sbPCFile.Append(string.Format("PACKET_SERIALIZE_BIN(Req{0}, Req{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                //sbPCFile.Append("{\r\n    /*ServerLight::Packet::SerializeBin(stream, serialize);*/\r\n\r\n    if (serialize)\r\n    {\r\n");
                if (string.IsNullOrEmpty(sbBinWrite.ToString()) == false || string.IsNullOrEmpty(sbBinRead.ToString()) == false)
                {
                    sbPCFile.Append("    if (serialize)\r\n    {\r\n");
                    sbPCFile.Append(sbBinWrite);
                    sbPCFile.Append("    }\r\n    else\r\n    {\r\n");
                    sbPCFile.Append(sbBinRead);
                    sbPCFile.Append("    }\r\n");
                }
                sbPCFile.Append("}\r\n\r\n");
                sbPCFile.Append(string.Format("PACKET_SERIALIZE_JSON(Req{0}, Req{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                //sbPCFile.Append("{\r\n    /*ServerLight::Packet::SerializeBin(stream, serialize);*/\r\n\r\n    if (serialize)\r\n    {\r\n");
                if (string.IsNullOrEmpty(sbJsonWrite.ToString()) == false || string.IsNullOrEmpty(sbJsonRead.ToString()) == false)
                {
                    sbPCFile.Append("    if (serialize)\r\n    {\r\n");
                    sbPCFile.Append(sbJsonWrite);
                    sbPCFile.Append("    }\r\n    else\r\n    {\r\n");
                    sbPCFile.Append(sbJsonRead);
                    sbPCFile.Append("    }\r\n");
                }
                sbPCFile.Append("}\r\n\r\n");

                //响应包代码
                sbCtor.Clear();
                sbClone.Clear();
                sbBinRead.Clear();
                sbBinWrite.Clear();
                sbJsonRead.Clear();
                sbJsonWrite.Clear();
                sbCtor.Append(string.Format("    usPacketID = Res{0}Code;\r\n", ptitem.ProtocolName));
                sbPHFile.Append(string.Format("/* [响应]{0}; */\r\n", ptitem.ProtocolSummary));
                sbPHFile.Append(string.Format("BEGIN_PACKET_CLASS(Res{0}, Res{0}Code)\r\n", ptitem.ProtocolName));
                foreach (var fielditem in ptitem.ResFields)
                {
                    //声明代码
                    string cppfield = CodeUtil.FieldData2CPPField(fielditem, 4);
                    sbPHFile.Append(string.Format("{0}\r\n", cppfield));

                    string serfield = CodeUtil.FieldData2CPPDeserializeBin(fielditem, "", 8);
                    sbBinRead.Append(string.Format("{0}\r\n", serfield));

                    string ctorfield = CodeUtil.FieldData2CtorField(fielditem, 4);
                    sbCtor.Append(string.Format("{0}\r\n", ctorfield));

                    string clonefield = CodeUtil.FieldData2CloneField(fielditem, 4);
                    sbClone.Append(string.Format("{0}\r\n", clonefield));

                    serfield = CodeUtil.FieldData2CPPDeserializeJson(fielditem, "", 8);
                    sbJsonRead.Append(string.Format("{0}\r\n", serfield));

                    serfield = CodeUtil.FieldData2CPPSerializeBin(fielditem, "", 8);
                    sbBinWrite.Append(string.Format("{0}\r\n", serfield));

                    serfield = CodeUtil.FieldData2CPPSerializeJson(fielditem, "", 8);
                    sbJsonWrite.Append(string.Format("{0}\r\n", serfield));
                    //
                    sbMsgPack.Append(string.Format("{0}, ", fielditem.FieldName));
                }

                if (ptitem.ResFields.Count() > 0)
                {
                    sbMsgPack.Remove(sbMsgPack.Length - 2, 2);
                    sbMsgPack.Append(");\r\n    };\r\n");
                }
                //协议来源限定
                sbPHFile.Append("    bool ClientRequest() { return false; }\r\n");
                sbPHFile.Append(string.Format("END_PACKET_CLASS(Res{0})\r\n\r\n", ptitem.ProtocolName));

                sbPHFile.Append(sbMsgPack.ToString());

                //构造函数代码
                sbPCFile.Append(string.Format("PACKET_CONSTRUCTOR(Res{0}, Res{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                sbPCFile.Append(sbCtor.ToString());
                sbPCFile.Append("}\r\n\r\n");
                //sbPCFile.Append(string.Format("END_PACKET_CONSTRUCTOR(Res{0}, Res{0}Code)\r\n\r\n", ptitem.ProtocolName));

                //克隆函数代码
                sbPCFile.Append(string.Format("PACKET_CLONE(Res{0}, Res{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                sbPCFile.Append(string.Format("    Res{0}* pkg = (Res{0}*)ServerLight::PacketCreator::GetInstance().CreatePacket(Res{0}Code);\r\n", ptitem.ProtocolName));
                sbPCFile.Append(sbClone.ToString());
                sbPCFile.Append("    return pkg;\r\n");
                sbPCFile.Append("}\r\n\r\n");

                //序列化/反序列化代码
                sbPCFile.Append(string.Format("PACKET_SERIALIZE_BIN(Res{0}, Res{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                //sbPCFile.Append("{\r\n    ServerLight::Packet::Serialization(stream, serialize);\r\n\r\n    if (serialize)\r\n    {\r\n");
                if (string.IsNullOrEmpty(sbBinWrite.ToString()) == false || string.IsNullOrEmpty(sbBinRead.ToString()) == false)
                {
                    sbPCFile.Append("    if (serialize)\r\n    {\r\n");
                    sbPCFile.Append(sbBinWrite);
                    sbPCFile.Append("    }\r\n    else\r\n    {\r\n");
                    sbPCFile.Append(sbBinRead);
                    sbPCFile.Append("    }\r\n");
                }
                sbPCFile.Append("}\r\n\r\n");

                sbPCFile.Append(string.Format("PACKET_SERIALIZE_JSON(Res{0}, Res{0}Code)\r\n", ptitem.ProtocolName));
                sbPCFile.Append("{\r\n");
                //sbPCFile.Append("{\r\n    ServerLight::Packet::Serialization(stream, serialize);\r\n\r\n    if (serialize)\r\n    {\r\n");
                if (string.IsNullOrEmpty(sbJsonWrite.ToString()) == false || string.IsNullOrEmpty(sbJsonRead.ToString()) == false)
                {
                    sbPCFile.Append("    if (serialize)\r\n    {\r\n");
                    sbPCFile.Append(sbJsonWrite);
                    sbPCFile.Append("    }\r\n    else\r\n    {\r\n");
                    sbPCFile.Append(sbJsonRead);
                    sbPCFile.Append("    }\r\n");
                }
                sbPCFile.Append("}\r\n\r\n");
            }

            using (Stream fStream = new FileStream(sPath1, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);

                StringBuilder sb = new StringBuilder();
                sb.Append(sbPDHFile.ToString());
                sb.Append(sbSHFile.ToString());
                sb.Append(sbSSHFile.ToString());
                sb.Append(sbPHFile.ToString());

                string content = oGeneratorSetting.ContentFormat1.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sb.ToString());

                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }

            using (Stream fStream = new FileStream(sPath2, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);

                StringBuilder sb = new StringBuilder();
                sb.Append(sbSCFile.ToString());
                sb.Append(sbPCFile.ToString());

                string content = oGeneratorSetting.ContentFormat2.Replace("\n", "\r\n");
                content = content.Replace(ReplaceStr, sb.ToString());

                using (StreamWriter sw = new StreamWriter(fStream, Encoding.Default))
                {
                    sw.Write(content);
                }
            }
        }
    }
}
