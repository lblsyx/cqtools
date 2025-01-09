using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateTool.Packs
{
    public class PackerInfo
    {
        /// <summary>
        /// 打包器类型
        /// </summary>
        public int PackerType;
        /// <summary>
        /// 打包器名称
        /// </summary>
        public string PackerName;
        public override string ToString()
        {
            return PackerName;
        }
    }
}
