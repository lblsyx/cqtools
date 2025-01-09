using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnityLight.Configs
{
    public abstract class ConfigAbstract
    {
        [Config("ServerName", "服务器名称", "未配置服务端")]
        public string ServerName;

        [Config("LogLevel", "日志级别", 0)]
        public int LogLevel;

        [Config("StartupCmds", "启动依次执行的命令列表", "")]
        public string StartupCmds;

        public virtual void Load()
        {
            LoadConfig(this);
            ManualConfig();
        }

        public virtual void ManualConfig()
        {
        }

        public virtual object LoadFieldValue(ConfigAttribute attribute)
        {
            return attribute.DefaultValue;
        }

        #region 应用程序配置文件工具

        /// <summary>
        /// 加载应用程序配置文件(不支持属性)。
        /// </summary>
        /// <param name="oConfigAbstract"></param>
        private static void LoadConfig(ConfigAbstract oConfigAbstract)
        {
            Type type = oConfigAbstract.GetType();

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                object[] attributes = field.GetCustomAttributes(typeof(ConfigAttribute), false);

                if (attributes.Length == 0) continue;

                ConfigAttribute attribute = (ConfigAttribute)attributes[0];

                field.SetValue(oConfigAbstract, oConfigAbstract.LoadFieldValue(attribute));
            }
        }

        //private static object LoadConfigValue(ConfigAttribute attribute)
        //{
        //    string key = attribute.Key;

        //    string val = ConfigurationManager.AppSettings[key];

        //    if (val == null)
        //    {
        //        val = attribute.DefaultValue.ToString();
        //        //log.WarnFormat("Loading {0} value is null, using default value:'{1}'", key, val);
        //    }
        //    //else
        //    //{
        //    //    //log.InfoFormat("Loading {0} value is '{1}'", key, val);
        //    //}

        //    return Convert.ChangeType(val, attribute.DefaultValue.GetType());

        //    //try
        //    //{
        //    //    return Convert.ChangeType(val, attribute.DefaultValue.GetType());
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //log.Error("Exception in ServerConfig load: ", ex);
        //    //    return null;
        //    //}
        //}

        #endregion 应用程序配置文件工具
    }
}
