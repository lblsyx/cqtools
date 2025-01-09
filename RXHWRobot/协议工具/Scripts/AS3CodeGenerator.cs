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
    [Generator("AS3Code", true)]
    public class AS3CodeGenerator : IGenerator
    {
        public const int ProtocolStart = 100;
        public string Name
        {
            get { return "AS3客户端代码"; }
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
                MessageBox.Show("请选择AS3数据库映射代码文件夹");
                return;
            }

            if (Directory.Exists(root))
            {
                string[] fileList = Directory.GetFiles(root);

                foreach (var item in fileList)
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            string path;

            StringBuilder sbEnumFile = new StringBuilder();
            StringBuilder sbPacketFile = new StringBuilder();

            StringBuilder sbStructFile = new StringBuilder();
            StringBuilder sbPacketUtilFile = new StringBuilder();

            StringBuilder sbRegisterFile = new StringBuilder();

            //string nowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //sbEnumFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", nowStr));
            //sbEnumFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbEnumFile.Append("{\r\npublic class PacketEnum {\r\n");
            sbEnumFile.Append("public class PacketEnum {\r\n");

            //sbPacketUtilFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", nowStr));
            //sbPacketUtilFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbPacketUtilFile.Append("{\r\nimport com.ByteStream;\r\n\r\npublic class PacketUtil {\r\n");
            sbPacketUtilFile.Append("import com.ByteStream;\r\n\r\npublic class PacketUtil {\r\n");

            //sbRegisterFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", nowStr));
            //sbRegisterFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbRegisterFile.Append("{\r\nimport com.mgr.PacketMgr;\r\n\r\npublic class PacketRegister {\r\n");
            sbRegisterFile.Append("import com.mgr.PacketMgr;\r\n\r\npublic class PacketRegister {\r\n");
            sbRegisterFile.Append("    public static function init():void {\r\n");

            string[] ForVars = new string[] { "i", "j", "k", "m", "n" };

            foreach (var stitem in oProjectInfo.Structs)
            {
                #region 生成结构体文件

                sbStructFile.Clear();
                //sbStructFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", stitem.CreateTime));
                //sbStructFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbStructFile.Append("{\r\n\r\n");
                sbStructFile.Append(string.Format("/**\r\n * {1}\r\n */\r\npublic class {0} ", stitem.StructName, stitem.StructSummary));
                sbStructFile.Append("{\r\n");
                foreach (var fieldItem in stitem.Fields)
                {
                    sbStructFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\r\n");
                }
                //sbStructFile.Append("    }\r\n}\r\n\r\n");
                sbStructFile.Append("    }\r\n");
                //写入到结构类文件
                path = Path.Combine(root, stitem.StructName + ".as");
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

                #endregion 生成结构体文件

                #region 工具类读写方法

                sbPacketUtilFile.Append(string.Format("    public static function read{0}(bytes:ByteStream):{0} ", stitem.StructName));
                sbPacketUtilFile.Append("{\r\n");
                sbPacketUtilFile.Append(string.Format("        var item:{0} = new {0}();\r\n", stitem.StructName));

                int ArrayCount = 0;

                foreach (var fieldItem in stitem.Fields)
                {
                    sbPacketUtilFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "item", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketUtilFile.Append("        return item;\r\n    }\r\n\r\n");

                sbPacketUtilFile.Append(string.Format("    public static function write{0}(bytes:ByteStream, item:{0}):void ", stitem.StructName));
                sbPacketUtilFile.Append("{\r\n");
                ArrayCount = 0;
                foreach (var fieldItem in stitem.Fields)
                {
                    sbPacketUtilFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "item", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketUtilFile.Append("    }\r\n\r\n");

                #endregion 工具类读写方法
            }
            //sbPacketUtilFile.Append("}\r\n}\r\n\r\n");
            sbPacketUtilFile.Append("}\r\n");

            #region 将读写方法到工具类文件

            path = Path.Combine(root, "PacketUtil.as");
            using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbPacketUtilFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 将读写方法到工具类文件

            int protocolStartCode = ProtocolStart + 1;

            sbEnumFile.Append(string.Format("    /**\r\n     * 数据包版本号\r\n     */\r\n    public static const Ver:int = {0};\r\n", oProjectInfo.PacketVer));

            foreach (var ptitem in oProjectInfo.Protocols)
            {
                sbEnumFile.Append(string.Format("    /**\r\n     * {1}\r\n     */\r\n    public static const {0}:String = \"{0}\";\r\n", ptitem.ProtocolName, ptitem.ProtocolSummary));

                #region 请求类协议注册代码

                sbRegisterFile.Append(string.Format("        PacketMgr.register({0}, Req{1});\r\n", protocolStartCode, ptitem.ProtocolName));

                #endregion 请求类协议注册代码

                string imports = "import com.ByteStream;\r\nimport com.net.Packet;";//Global.Config.ProtocolAS3Import.Replace(",", "\r\n");

                #region 协议请求类代码

                sbPacketFile.Clear();
                //sbPacketFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", ptitem.CreateTime));
                //sbPacketFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbPacketFile.Append("{\r\n");
                sbPacketFile.Append(string.Format("{0}\r\n\r\n", imports));
                sbPacketFile.Append(string.Format("/**\r\n * [请求类]{0}\r\n */\r\n", ptitem.ProtocolSummary));
                sbPacketFile.Append(string.Format("public class Req{0} extends Packet ", ptitem.ProtocolName));
                sbPacketFile.Append("{\r\n");

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\r\n");
                }

                sbPacketFile.Append(string.Format("\r\n    public function Req{0}() ", ptitem.ProtocolName));
                sbPacketFile.Append("{\r\n");
                sbPacketFile.Append(string.Format("        super({0});\r\n", protocolStartCode++));
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function get eventName():String {\r\n");
                sbPacketFile.Append(string.Format("        return PacketEnum.{0};\r\n", ptitem.ProtocolName));
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function write(bytes:ByteStream):void {\r\n");

                int ArrayCount = 0;

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function read(bytes:ByteStream):void {\r\n");

                ArrayCount = 0;
                foreach (var fieldItem in ptitem.ReqFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\r\n");
                //sbPacketFile.Append("}\r\n}\r\n\r\n");
                sbPacketFile.Append("}\r\n");

                //写入协议请求类文件
                path = Path.Combine(root, string.Format("Req{0}.as", ptitem.ProtocolName));
                using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fStream.Position = 0;
                    fStream.SetLength(0);
                    string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbPacketFile.ToString());
                    using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }

                #endregion 协议请求类代码

                #region 响应类协议注册代码

                sbRegisterFile.Append(string.Format("        PacketMgr.register({0}, Res{1});\r\n", protocolStartCode, ptitem.ProtocolName));

                #endregion 响应类协议注册代码

                #region 协议响应类代码

                sbPacketFile.Clear();
                //sbPacketFile.Append(string.Format("/**\r\n * Created by Tool on {0}.\r\n */\r\n", ptitem.CreateTime));
                //sbPacketFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbPacketFile.Append("{\r\n");
                sbPacketFile.Append(string.Format("{0}\r\n\r\n", imports));
                sbPacketFile.Append(string.Format("/**\r\n * [响应类]{0}\r\n */\r\n", ptitem.ProtocolSummary));
                sbPacketFile.Append(string.Format("public class Res{0} extends Packet ", ptitem.ProtocolName));
                sbPacketFile.Append("{\r\n");

                foreach (var fieldItem in ptitem.ResFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\r\n");
                }

                sbPacketFile.Append(string.Format("\r\n    public function Res{0}() ", ptitem.ProtocolName));
                sbPacketFile.Append("{\r\n");
                sbPacketFile.Append(string.Format("        super({0});\r\n", protocolStartCode++));
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function get eventName():String {\r\n");
                sbPacketFile.Append(string.Format("        return PacketEnum.{0};\r\n", ptitem.ProtocolName));
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function write(bytes:ByteStream):void {\r\n");

                ArrayCount = 0;
                foreach (var fieldItem in ptitem.ResFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\r\n\r\n");
                sbPacketFile.Append("    override public function read(bytes:ByteStream):void {\r\n");

                ArrayCount = 0;
                foreach (var fieldItem in ptitem.ResFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", ForVars[ArrayCount])).Append("\r\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\r\n");
                //sbPacketFile.Append("}\r\n}\r\n\r\n");
                sbPacketFile.Append("}\r\n");

                //写入协议响应类文件
                path = Path.Combine(root, string.Format("Res{0}.as", ptitem.ProtocolName));
                using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fStream.Position = 0;
                    fStream.SetLength(0);
                    string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbPacketFile.ToString());
                    using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }

                #endregion 协议响应类代码
            }
            //sbEnumFile.Append("}\r\n}\r\n\r\n");
            sbEnumFile.Append("}\r\n");
            sbRegisterFile.Append("    }\r\n");
            //sbRegisterFile.Append("}\r\n}\r\n\r\n");
            sbRegisterFile.Append("}\r\n");

            #region 写入协议枚举文件

            path = Path.Combine(root, "PacketEnum.as");
            using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbEnumFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 写入协议枚举文件

            #region 写入协议注册类文件

            path = Path.Combine(root, "PacketRegister.as");
            using (Stream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fStream.Position = 0;
                fStream.SetLength(0);
                string content = oGeneratorSetting.ContentFormat1.Replace(ReplaceStr, sbRegisterFile.ToString());
                using (StreamWriter sw = new StreamWriter(fStream, Encoding.UTF8))
                {
                    sw.Write(content);
                }
            }

            #endregion 写入协议注册类文件
        }
    }
}
