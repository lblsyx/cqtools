using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore.Generates
{
    public class StructGeneratorAttribute : Attribute
    {
        public bool Visible { get; set; }

        public StructGeneratorAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
