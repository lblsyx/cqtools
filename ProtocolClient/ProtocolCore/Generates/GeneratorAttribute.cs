using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolCore.Generates
{
    public class GeneratorAttribute : Attribute
    {
        public string Type { get; set; }

        public bool Visible { get; set; }

        public GeneratorAttribute(string type, bool visible)
        {
            Type = type;
            Visible = visible;
        }
    }
}
