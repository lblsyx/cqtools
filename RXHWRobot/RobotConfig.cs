using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RXHWRobot
{
    public class RobotConfig
    {
        #region 配置文件操作

        public const string ConfigFileName = "RobotConfig.xml";

        public static bool Exists { get { return File.Exists(ConfigPath); } }

        public static string ConfigPath { get { return Path.Combine(Application.StartupPath, ConfigFileName); } }

        private static RobotConfig mInstance = new RobotConfig();

        public static RobotConfig Instance { get { return mInstance; } }

        public static void Load()
        {
            Type type = RobotConfig.Instance.GetType();

            using (StreamReader reader = new StreamReader(ConfigPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);

                mInstance = xmlSerializer.Deserialize(reader) as RobotConfig;
            }
        }

        public static void Save()
        {
            Type type = RobotConfig.Instance.GetType();
            using (StreamWriter writer = new StreamWriter(ConfigPath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);

                xmlSerializer.Serialize(writer, RobotConfig.Instance);
            }
        }

        #endregion

        #region 配置项

        public int PlatformID = 1;

        public bool AutoLoadResource = true;

        public int LoginServerSelectedIndex = 0;

        public List<string> LoginServers = new List<string>();

        public string AssetRoot = string.Empty;

        public string RobotSignKey = string.Empty;

        public string AccountPre = string.Empty;

        public int RobotNum = 200;

        public uint[] MapID = new uint[5] { 2751, 3001, 3021, 3041, 3051 };

        public uint MinMapX = 20;
        public uint MinMapY = 85;
        public uint MaxMapX = 30;
        public uint MaxMapY = 90;

        public bool EnableRandomFight = true;
        public bool EnableRandomMake = true;
        public int MinMakeNum = 2;
        public int MaxMakeNum = 10;
        public int MinMakeInterval = 2000;
        public int MaxMakeInterval = 6000;

        public int LoginSpeed = 10;

        #endregion
    }
}
