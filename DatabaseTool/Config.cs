using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DatabaseTool
{
    public class Config
    {
        #region 配置项

        /// <summary>
        /// 生成器代码模板设置
        /// </summary>
        public List<GeneratorSetting> GeneratorSettings = new List<GeneratorSetting>();

        /// <summary>
        /// 数据库连接列表
        /// </summary>
        public BindingList<ConnectionInfo> Connections = new BindingList<ConnectionInfo>();

        /// <summary>
        /// 脚本引用程序集列表
        /// </summary>
        public List<string> ScriptReflections = new List<string>();

        #endregion

        public GeneratorSetting GetGeneratorSetting(string sDBName, string sGeneratorType)
        {
            foreach (var item in GeneratorSettings)
            {
                if (item.DBName == sDBName && item.GeneratorType == sGeneratorType)
                {
                    return item;
                }
            }

            GeneratorSetting oGeneratorSetting = new GeneratorSetting();
            oGeneratorSetting.DBName = sDBName;
            oGeneratorSetting.GeneratorType = sGeneratorType;
            GeneratorSettings.Add(oGeneratorSetting);

            return oGeneratorSetting;
        }

        #region 加载/保存配置

        private string GetConfigFile()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseConfig.xml");
        }

        public void Load()
        {
            string file = GetConfigFile();

            if (File.Exists(file) == false) Save();

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
                    "MySql.Data.dll",
                    "UnityLight.dll"
                });
            }
        }

        public void Save()
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
                    "MySql.Data.dll",
                    "UnityLight.dll"
                });
            }

            string file = GetConfigFile();

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
                MessageBox.Show(string.Format("配置文件保存失败!ErrMsg:{0}", ex.Message));
            }
        }

        #endregion
    }
}
