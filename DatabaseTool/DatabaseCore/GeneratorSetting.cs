using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    public class GeneratorSetting
    {
        public string DBName { get; set; }

        public string GeneratorType { get; set; }

        public string CodeContentFormat1 { get; set; }

        public string CodeContentFormat2 { get; set; }
    }
}
