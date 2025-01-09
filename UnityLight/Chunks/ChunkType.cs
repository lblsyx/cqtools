using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Chunks
{
    public class ChunkType
    {
        /**
         * 文件起始块
         */
        public static readonly int FILE_START = 0xA0;
        /**
         * 文件结束块
         */
        public static readonly int FILE_END = 0xAF;
    }
}
