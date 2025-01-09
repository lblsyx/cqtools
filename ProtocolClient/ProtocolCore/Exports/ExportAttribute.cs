using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolCore.Exports
{
    public class ExportAttribute : Attribute
    {
        public bool Visible { get; set; }

        public ExportAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
