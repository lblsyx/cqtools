using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateTool.Gens
{
    public class GeneratorInfo
    {
        /// <summary>
        /// 生成器类型
        /// </summary>
        public int GeneratorType = 0;
        /// <summary>
        /// 生成器名称
        /// </summary>
        public string GeneratorName = string.Empty;

        public override string ToString()
        {
            return GeneratorName;
        }
    }
}
