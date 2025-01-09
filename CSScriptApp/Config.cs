using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSScriptApp
{
    public class Config
    {
        /// <summary>
        /// 动态编译引用列表
        /// </summary>
        public List<string> ScriptReflections = new List<string>();

        #region 配置文件持久化

        public virtual string ConfigFile
        {
            get { return "CSScriptConfig.xml"; }
        }

        public void Load(string dir)
        {
            string file = Path.Combine(dir, ConfigFile);

            if (File.Exists(file) == false) Save(dir);

            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                XmlSerializer xmldes = new XmlSerializer(GetType());
                Config o = xmldes.Deserialize(fs) as Config;
                if (o == null) return;

                XMLUtil.CopyObject(o, this);
            }

            if (ScriptReflections.Count == 0)
            {
                ScriptReflections.AddRange(new string[]{
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll",
                    "System.Data.DataSetExtensions.dll",
                    "System.Deployment.dll",
                    "System.Drawing.dll",
                    "System.Windows.Forms.dll",
                    "System.Xml.dll",
                    "System.Xml.Linq.dll",
                    "NPOI.dll",
                    "MySql.Data.dll",
                    "UnityLight.dll"
                });
            }
        }

        public void Save(string dir)
        {
            if (ScriptReflections.Count == 0)
            {
                ScriptReflections.AddRange(new string[]{
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll",
                    "System.Data.DataSetExtensions.dll",
                    "System.Deployment.dll",
                    "System.Drawing.dll",
                    "System.Windows.Forms.dll",
                    "System.Xml.dll",
                    "System.Xml.Linq.dll",
                    "NPOI.dll",
                    "MySql.Data.dll",
                    "UnityLight.dll"
                });
            }

            string file = Path.Combine(dir, ConfigFile);

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.NewLineOnAttributes = false;
                settings.Indent = true;

                using (XmlWriter myWriter = XmlWriter.Create(file, settings))
                {//序列化对象
                    XmlSerializer xml = new XmlSerializer(GetType());
                    xml.Serialize(myWriter, this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("配置文件保存失败!ErrMsg:{0}", ex.Message));
            }
        }

        #endregion
    }
}
