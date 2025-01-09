using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using UnityLight.Loggers;

namespace ProtocolClient
{
    public class Config
    {
        public string Name = string.Empty;
        public int ProjectID = 0;
        public int SQLPort = 3306;
        public string SQLDB = "protocols";
        public string SQLHost = "127.0.0.1";
        public string SQLUser = "root";
        public string SQLPass = string.Empty;
        public List<string> ScriptReflections = new List<string>();

        public List<GeneratorPaths> Paths = new List<GeneratorPaths>();
        //public Dictionary<string, GeneratorPaths> Paths = new Dictionary<string, GeneratorPaths>();

        public string GetPath1(int nProjectID, string sGeneratorType)
        {
            GeneratorPaths oGeneratorPaths = GetGeneratorPaths(nProjectID, sGeneratorType);
            return oGeneratorPaths.OutputPath1;
        }

        public void SetPath1(int nProjectID, string sGeneratorType, string path)
        {
            GeneratorPaths oGeneratorPaths = GetGeneratorPaths(nProjectID, sGeneratorType);
            oGeneratorPaths.OutputPath1 = path;
            Save();
        }

        public string GetPath2(int nProjectID, string sGeneratorType)
        {
            GeneratorPaths oGeneratorPaths = GetGeneratorPaths(nProjectID, sGeneratorType);
            return oGeneratorPaths.OutputPath2;
        }

        public void SetPath2(int nProjectID, string sGeneratorType, string path)
        {
            GeneratorPaths oGeneratorPaths = GetGeneratorPaths(nProjectID, sGeneratorType);
            oGeneratorPaths.OutputPath2 = path;
            Save();
        }

        public GeneratorPaths GetGeneratorPaths(int nProjectID, string sGeneratorType)
        {
            for (int i = 0; i < Paths.Count; i++)
            {
                if (Paths[i].GeneratorType == sGeneratorType && Paths[i].ProjectID == nProjectID)
                {
                    return Paths[i];
                }
            }

            GeneratorPaths temp = new GeneratorPaths();
            temp.GeneratorType = sGeneratorType;
            temp.ProjectID = nProjectID;
            temp.OutputPath1 = string.Empty;
            temp.OutputPath2 = string.Empty;
            Paths.Add(temp);

            return temp;
        }

        private string GetConfigFile()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProtocolConfig.xml");
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
    }
}
