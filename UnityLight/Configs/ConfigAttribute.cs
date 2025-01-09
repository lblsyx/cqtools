﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Configs
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ConfigAttribute : Attribute
    {
        public ConfigAttribute(string key, string description, object defaultValue)
        {
            Key = key;
            Description = description;
            DefaultValue = defaultValue;
        }

        public string Key { get; private set; }

        public string Description { get; private set; }

        public object DefaultValue { get; private set; }
    }
}
