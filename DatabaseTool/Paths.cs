using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DatabaseTool
{
    public class Paths
    {
        #region 配置项

        public List<GeneratorPath> GeneratorPaths = new List<GeneratorPath>();

        #endregion

        public GeneratorPath GetGeneratorPath(string sDBName, string sGeneratorType)
        {
            foreach (var item in GeneratorPaths)
            {
                if (item.DBName == sDBName && item.GeneratorType == sGeneratorType)
                {
                    return item;
                }
            }

            GeneratorPath oGeneratorPath = new GeneratorPath();
            oGeneratorPath.DBName = sDBName;
            oGeneratorPath.GeneratorType = sGeneratorType;
            GeneratorPaths.Add(oGeneratorPath);

            return oGeneratorPath;
        }

        #region 加载/保存配置

        private string GetPathsFile()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabasePaths.xml");
        }

        public void Load()
        {
            string file = GetPathsFile();

            if (File.Exists(file) == false) Save();

            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                XmlSerializer xmldes = new XmlSerializer(GetType());
                Paths o = xmldes.Deserialize(fs) as Paths;
                if (o == null) return;

                XMLUtil.CopyObject(o, this);
            }
        }

        public void Save()
        {
            string file = GetPathsFile();

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
                MessageBox.Show(string.Format("路径文件保存失败!ErrMsg:{0}", ex.Message));
            }
        }

        #endregion
    }
}
