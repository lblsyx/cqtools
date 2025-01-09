using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSScriptApp.TemplateCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class GeneratorAttribute : Attribute
    {
        public int GeneratorType { get; set; }

        public string GeneratorName { get; set; }

        /// <summary>
        /// 是否可绑定到下拉列表控件进行选择
        /// </summary>
        public bool EnableBind { get; set; }

        public GeneratorAttribute(int generatorType, string generatorName, bool bCanBind)
        {
            EnableBind = bCanBind;
            GeneratorType = generatorType;
            GeneratorName = generatorName;
        }
    }
}
