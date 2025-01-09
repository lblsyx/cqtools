using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Chunks.MapChunks
{
    public class MapChunkType
    {
        /**
         * 地图属性块
         */
        public static readonly int MAP_PROP = 0xB0;
        /**
         * 地图阻挡块
         */
        public static readonly int MAP_BLOCK = 0xB1;
        /**
         * 地图事件
         */
        public static readonly int MAP_EVENT = 0xB6;
    }
}
