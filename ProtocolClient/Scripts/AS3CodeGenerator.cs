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

            //sbEnumFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", nowStr));
            //sbEnumFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbEnumFile.Append("{\npublic class PacketEnum {\n");
            sbEnumFile.Append("public class PacketEnum {\n");

            //sbPacketUtilFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", nowStr));
            //sbPacketUtilFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbPacketUtilFile.Append("{\nimport com.ByteStream;\n\npublic class PacketUtil {\n");
            sbPacketUtilFile.Append("import com.ByteStream;\n\npublic class PacketUtil {\n");

            //sbRegisterFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", nowStr));
            //sbRegisterFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
            //sbRegisterFile.Append("{\nimport com.mgr.PacketMgr;\n\npublic class PacketRegister {\n");
            sbRegisterFile.Append("import com.mgr.PacketMgr;\n\npublic class PacketRegister {\n");
            sbRegisterFile.Append("    public static function init():void {\n");

            string[] ForVars = new string[] { "i", "j", "k", "m", "n" ,"o" };

            bool tag_IsFirst = true;

            foreach (var stitem in oProjectInfo.Structs)
            {
                #region 生成结构体文件

                sbStructFile.Clear();
                //sbStructFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", stitem.CreateTime));
                //sbStructFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbStructFile.Append("{\n\n");
                sbStructFile.Append(string.Format("/**\n * {1}\n */\npublic class {0} ", stitem.StructName, stitem.StructSummary));
                sbStructFile.Append("{\n");
                foreach (var fieldItem in stitem.Fields)
                {
                    sbStructFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\n");
                }
                //sbStructFile.Append("    }\n}\n\n");
                sbStructFile.Append("    }\n");
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
                sbPacketUtilFile.Append("{\n");
                sbPacketUtilFile.Append(string.Format("        var item:{0} = new {0}();\n", stitem.StructName));

                int ArrayCount = 1;
                tag_IsFirst = true;

                foreach (var fieldItem in stitem.Fields)
                {
                    sbPacketUtilFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "item", string.Concat("i", ArrayCount), ref tag_IsFirst)).Append("\n");

                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketUtilFile.Append("        return item;\n    }\n\n");

                sbPacketUtilFile.Append(string.Format("    public static function write{0}(bytes:ByteStream, item:{0}):void ", stitem.StructName));
                sbPacketUtilFile.Append("{\n");
                ArrayCount = 1;
                foreach (var fieldItem in stitem.Fields)
                {
                    //sbPacketUtilFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "item", ForVars[ArrayCount])).Append("\n");
                    sbPacketUtilFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "item", string.Concat("i", ArrayCount))).Append("\n");                   
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketUtilFile.Append("    }\n\n");

                #endregion 工具类读写方法
            }
            //sbPacketUtilFile.Append("}\n}\n\n");
            sbPacketUtilFile.Append("}\n");

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

            sbEnumFile.Append(string.Format("    /**\n     * 数据包版本号\n     */\n    public static const Ver:int = {0};\n", oProjectInfo.PacketVer));
            sbEnumFile.Append(string.Format("    /**\n     * 数据结构版本号\n     */\n    public static const SVer:int = {0};\n", Global.StructOperateVer));
            sbEnumFile.Append(string.Format("    /**\n     * 数据协议版本号\n     */\n    public static const PVer:int = {0};\n", Global.ProtocolOperateVer));

            foreach (var ptitem in oProjectInfo.Protocols)
            {
                sbEnumFile.Append(string.Format("    /**\n     * {1}\n     */\n    public static const {0}:String = \"{0}\";\n", ptitem.ProtocolName, ptitem.ProtocolSummary));

                #region 请求类协议注册代码

                sbRegisterFile.Append(string.Format("        PacketMgr.register({0}, Req{1});\n", protocolStartCode, ptitem.ProtocolName));

                #endregion 请求类协议注册代码

                string imports = "import com.ByteStream;\nimport com.net.Packet;";//Global.Config.ProtocolAS3Import.Replace(",", "\n");

                #region 协议请求类代码

                sbPacketFile.Clear();
                //sbPacketFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", ptitem.CreateTime));
                //sbPacketFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbPacketFile.Append("{\n");
                sbPacketFile.Append(string.Format("{0}\n\n", imports));
                sbPacketFile.Append(string.Format("/**\n * [请求类]{0}\n */\n", ptitem.ProtocolSummary));
                sbPacketFile.Append(string.Format("public class Req{0} extends Packet ", ptitem.ProtocolName));
                sbPacketFile.Append("{\n");
                //Console.WriteLine("{0}", ptitem.ProtocolName);

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\n");
                }

                sbPacketFile.Append(string.Format("\n    public function Req{0}() ", ptitem.ProtocolName));
                sbPacketFile.Append("{\n");
                sbPacketFile.Append(string.Format("        super({0});\n", protocolStartCode++));
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function get eventName():String {\n");
                sbPacketFile.Append(string.Format("        return PacketEnum.{0};\n", ptitem.ProtocolName));
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function write(bytes:ByteStream):void {\n");

                int ArrayCount = 1;

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    //sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", ForVars[ArrayCount])).Append("\n");
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", string.Concat("i" + ArrayCount))).Append("\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function read(bytes:ByteStream):void {\n");

                ArrayCount = 1;
                tag_IsFirst = true;

                foreach (var fieldItem in ptitem.ReqFields)
                {
                    //sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", ForVars[ArrayCount])).Append("\n");
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", string.Concat("i", ArrayCount), ref tag_IsFirst)).Append("\n");
                    //tag_IsFirst = false;

                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\n");
                //sbPacketFile.Append("}\n}\n\n");
                sbPacketFile.Append("}\n");

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

                sbRegisterFile.Append(string.Format("        PacketMgr.register({0}, Res{1});\n", protocolStartCode, ptitem.ProtocolName));

                #endregion 响应类协议注册代码

                #region 协议响应类代码

                sbPacketFile.Clear();
                //sbPacketFile.Append(string.Format("/**\n * Created by Tool on {0}.\n */\n", ptitem.CreateTime));
                //sbPacketFile.Append(string.Format("package {0} ", Global.Config.ProtocolAS3Package));
                //sbPacketFile.Append("{\n");
                sbPacketFile.Append(string.Format("{0}\n\n", imports));
                sbPacketFile.Append(string.Format("/**\n * [响应类]{0}\n */\n", ptitem.ProtocolSummary));
                sbPacketFile.Append(string.Format("public class Res{0} extends Packet ", ptitem.ProtocolName));
                sbPacketFile.Append("{\n");

                foreach (var fieldItem in ptitem.ResFields)
                {
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Field(fieldItem)).Append("\n");
                }

                sbPacketFile.Append(string.Format("\n    public function Res{0}() ", ptitem.ProtocolName));
                sbPacketFile.Append("{\n");
                sbPacketFile.Append(string.Format("        super({0});\n", protocolStartCode++));
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function get eventName():String {\n");
                sbPacketFile.Append(string.Format("        return PacketEnum.{0};\n", ptitem.ProtocolName));
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function write(bytes:ByteStream):void {\n");

                ArrayCount = 1;
                foreach (var fieldItem in ptitem.ResFields)
                {
                    //sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", ForVars[ArrayCount])).Append("\n");
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Write(fieldItem, "", string.Concat("i", ArrayCount))).Append("\n");
                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\n\n");
                sbPacketFile.Append("    override public function read(bytes:ByteStream):void {\n");

                ArrayCount = 1;
                tag_IsFirst = true;

                foreach (var fieldItem in ptitem.ResFields)
                {
                    //sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", ForVars[ArrayCount])).Append("\n");
                    sbPacketFile.Append(CodeUtil.FieldData2AS3Read(fieldItem, "", string.Concat("i", ArrayCount), ref tag_IsFirst)).Append("\n");
                    //tag_IsFirst = false;

                    if (string.IsNullOrEmpty(fieldItem.FieldLength) == false) ArrayCount += 1;
                }
                sbPacketFile.Append("    }\n");
                //sbPacketFile.Append("}\n}\n\n");
                sbPacketFile.Append("}\n");

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
            //sbEnumFile.Append("}\n}\n\n");
            sbEnumFile.Append("}\n");
            sbRegisterFile.Append("    }\n");
            //sbRegisterFile.Append("}\n}\n\n");
            sbRegisterFile.Append("}\n");

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
