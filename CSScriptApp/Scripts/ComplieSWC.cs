#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;

namespace CSScriptApp.Scripts
{
    public class ComplieSWC : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string swcName = args[0] as string;
                string projRoot = args[1] as string;
                string swcOutput = args[2] as string;
                string[] swcs = args[3] as string[];

                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Global.CurrentDirectory, "compc-config/flex-config.xml"));

                XmlNode node = null;
                XmlElement xe = null;

                node = doc.SelectSingleNode("flex-config/compiler/external-library-path");
                node.RemoveAll();

                xe = doc.CreateElement("path-element");
                xe.InnerText = Path.Combine(PublishAS3.FLEX_SDK, "frameworks/libs/player/11.1/playerglobal.swc").Replace("\\", "/");
                node.AppendChild(xe);

                xe = doc.CreateElement("path-element");
                xe.InnerText = Path.Combine(PublishAS3.FLEX_SDK, "frameworks/libs").Replace("\\", "/");
                node.AppendChild(xe);

                node = doc.SelectSingleNode("flex-config/compiler/library-path");
                node.RemoveAll();

                xe = doc.CreateElement("path-element");
                xe.InnerText = Path.Combine(PublishAS3.FLEX_SDK, "frameworks/locale/{locale}").Replace("\\", "/");
                node.AppendChild(xe);

                xe = doc.CreateElement("path-element");
                xe.InnerText = swcOutput.Replace("\\", "/");
                node.AppendChild(xe);

                foreach (var item in swcs)
                {
                    xe = doc.CreateElement("path-element");
                    xe.InnerText = item.Replace("\\", "/");
                    node.AppendChild(xe);
                }

                node = doc.SelectSingleNode("flex-config/compiler/fonts");
                xe = doc.CreateElement("local-fonts-snapshot");
                xe.InnerText = Path.Combine(PublishAS3.FLEX_SDK, "frameworks/localFonts.ser").Replace("\\", "/");
                node.AppendChild(xe);

                string swcFile = Path.Combine(swcOutput, swcName + ".swc").Replace("\\", "/");
                node = doc.SelectSingleNode("flex-config/output");
                node.InnerText = swcFile;
                if (File.Exists(swcFile))
                {
                    File.Delete(swcFile);
                }

                string srcDir = Path.Combine(projRoot, swcName + "/src").Replace("\\", "/");
                node = doc.SelectSingleNode("flex-config/compiler/source-path");
                xe = doc.CreateElement("path-element");
                xe.InnerText = srcDir;
                node.AppendChild(xe);

                node = doc.SelectSingleNode("flex-config/include-classes");
                //遍历所有类路径
                List<string> list = new List<string>();
                List<string> classes = new List<string>();
                ScriptMethod.FindChildren(srcDir, list, "*.as");
                foreach (var item in list)
                {
                    string cls = item.Replace("\\", "/").Replace(".as", string.Empty);
                    cls = cls.Replace(srcDir, string.Empty).Substring(1);
                    cls = cls.Replace("/", ".");
                    classes.Add(cls);
                }
                foreach (var item in classes)
                {
                    xe = doc.CreateElement("class");
                    xe.InnerText = item;
                    node.AppendChild(xe);
                }

                string xmlpath = Path.Combine(swcOutput, swcName + "-Config.xml");
                using (XmlTextWriter writer = new XmlTextWriter(xmlpath, Encoding.Default))
                {
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);
                }
                while (File.Exists(xmlpath) == false)
                {
                    Thread.Sleep(100);
                }
                return ScriptMethod.ExecCommand(Path.Combine(PublishAS3.FLEX_SDK, "bin\\compc.exe"), " -load-config " + xmlpath);
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