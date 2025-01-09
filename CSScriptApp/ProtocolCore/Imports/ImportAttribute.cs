using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSScriptApp.ProtocolCore.Imports
{
    public class ImportAttribute : Attribute
    {
        public bool Visible { get; set; }

        public ImportAttribute(bool visible)
        {
            Visible = visible;
        }
    }
}
