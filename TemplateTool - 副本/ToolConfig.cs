using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using TemplateTool.Gens;

namespace TemplateTool
{
    [Serializable]
    public class GenerateParameter
    {
        public bool AutoCommit = false;
        public int GeneratorType = 0;
        public string CodeFileOutputPath = string.Empty;
        public string CodeFileOutputPath2 = string.Empty;
        public string CodeFileContentFormat = string.Empty;
        public string CodeFileContentFormat2 = string.Empty;

    }

    public class PackParameter
    {
        public int PackerType = 0;
        public string PackDataFileOutputPath = string.Empty;
        public bool PackDataAutoCommit = false;

    }

    [Serializable]
    public class ToolConfig
    {
        public const string ConfigFileName = "Config.xml";

        private static ToolConfig mInstance = new ToolConfig();

        public static ToolConfig Instance { get { return mInstance; } }

        public bool UseFolder = false;
        public string ExcelFilePath = string.Empty;
        public string ExcelFolderPath = string.Empty;
        public string IgnoreSheetNames = string.Empty;
        public string CodeFileOutputPath = string.Empty;
        public string CodeFileOutputPath2 = string.Empty;
        public string CodeFileContentFormat = string.Empty;
        public string CodeFileContentFormat2 = string.Empty;

        public string PackDataFileOutputPath = string.Empty;
        public string ExcelFileSplitOutputPath = string.Empty;
        public string ExcelToJsonOutputPath = string.Empty;

        public List<PackParameter> PackParameterList = new List<PackParameter>();
        public List<GenerateParameter> CodeParameterList = new List<GenerateParameter>();
        public List<int> OneKeySelectedIdx = new List<int>();

        public string MySQLHost = string.Empty;
        public string MySQLPort = string.Empty;
        public string MySQLUser = string.Empty;
        public string MySQLPass = string.Empty;
        public string MySQLDB = string.Empty;

        public int GenSelectedIndex = 0;
        public int PackSelectedIndex = 0;
        public static string ConfigPath
        {
            get
            {
                return Path.Combine(Application.StartupPath, ConfigFileName);
            }
        }

        public static bool Exists
        {
            get { return File.Exists(ConfigPath); }
        }

        public PackParameter GetPackParameter(int packerType)
        {
            foreach (PackParameter item in PackParameterList)
            {
                if (item.PackerType == packerType)
                {
                    return item;
                }
            }
            return null;
        }

        public GenerateParameter GetGenerateParameter(int generatorType)
        {
            foreach (GenerateParameter item in CodeParameterList)
            {
                if (item.GeneratorType == generatorType)
                {
                    return item;
                }
            }
            return null;
        }
        //这里应该是目录，不带文件名
        public string getAS3OutPath()
        {
            var item = GetGenerateParameter((int)GeneratorType.AS3Client);
            if (item!=null)
            {
                return item.CodeFileOutputPath;
            }
            return "";

        }
        //这个带文件名
        public string getPackOutPath()
        {
            return PackDataFileOutputPath;
        }
        public bool isCheckBoxListItemSelected(int idx)
        {
            return OneKeySelectedIdx.IndexOf(idx)>-1;
        }
        public void setCheckBoxListSelected(int idx,bool isSelect)
        {
            if (isSelect)
            {
                if (!isCheckBoxListItemSelected(idx))
                {
                    OneKeySelectedIdx.Add(idx);
                }

            }
            else
            {
                OneKeySelectedIdx.Remove(idx);
            }

        }

        public static void Load()
        {
            Type type = ToolConfig.Instance.GetType();

            using (StreamReader reader = new StreamReader(ConfigPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                
                mInstance = xmlSerializer.Deserialize(reader) as ToolConfig;

                mInstance.CodeFileContentFormat = mInstance.CodeFileContentFormat.Replace("\r\n", "\n");
                mInstance.CodeFileContentFormat = mInstance.CodeFileContentFormat.Replace("\r", "\n");
                mInstance.CodeFileContentFormat = mInstance.CodeFileContentFormat.Replace("\n", "\r\n");

                mInstance.CodeFileContentFormat2 = mInstance.CodeFileContentFormat2.Replace("\r\n", "\n");
                mInstance.CodeFileContentFormat2 = mInstance.CodeFileContentFormat2.Replace("\r", "\n");
                mInstance.CodeFileContentFormat2 = mInstance.CodeFileContentFormat2.Replace("\n", "\r\n");

                foreach (GenerateParameter item in mInstance.CodeParameterList)
                {
                    item.CodeFileContentFormat = item.CodeFileContentFormat.Replace("\r\n", "\n");
                    item.CodeFileContentFormat = item.CodeFileContentFormat.Replace("\r", "\n");
                    item.CodeFileContentFormat = item.CodeFileContentFormat.Replace("\n", "\r\n");

                    item.CodeFileContentFormat2 = item.CodeFileContentFormat2.Replace("\r\n", "\n");
                    item.CodeFileContentFormat2 = item.CodeFileContentFormat2.Replace("\r", "\n");
                    item.CodeFileContentFormat2 = item.CodeFileContentFormat2.Replace("\n", "\r\n");
                }
            }
        }

        public static void Save()
        {
            Type type = ToolConfig.Instance.GetType();
            using (StreamWriter writer = new StreamWriter(ConfigPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);

                xmlSerializer.Serialize(writer, ToolConfig.Instance);
            }
        }
    }
}
