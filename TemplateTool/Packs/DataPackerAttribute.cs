using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateTool.Packs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class DataPackerAttribute : Attribute
    {
        public int PackerType { get; set; }
        public string PackerName { get; set; }
        public bool EnableBind { get; set; }

        public DataPackerAttribute(int packerType, string packerName, bool bCanBind)
        {
            EnableBind = bCanBind;
            PackerType = packerType;
            PackerName = packerName;
        }
    }
}
