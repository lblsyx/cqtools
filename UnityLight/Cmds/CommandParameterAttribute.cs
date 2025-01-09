using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CommandParameterAttribute : Attribute
    {
        public CommandParameterAttribute(string paramKey, string paramDescription)
        {
            Key = paramKey;
            Description = paramDescription;
        }

        public string Key { get; private set; }

        public string Description { get; private set; }
    }
}
