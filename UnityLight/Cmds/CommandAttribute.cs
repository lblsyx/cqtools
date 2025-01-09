using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Cmds
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string cmd, string description, string usage, bool startup = false)
        {
            Cmd = cmd;
            Usage = usage;
            Startup = startup;
            Description = description;
        }

        public string Cmd { get; private set; }

        public string Usage { get; private set; }

        public bool Startup { get; private set; }

        public string Description { get; private set; }
    }
}
