using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Chunks.MapChunks
{
    public class MapData : ChunkData
    {
        // 地图ID
        public int MapID = 0;

        // 地图名字
        public string MapName = string.Empty;

        // 格子缩放单位
        public float Unit = 1;

        // 地图数据的宽度(也就是缩放值)
        public int Width = 0;

        // 地图数据的高度(也就是缩放值)
        public int Height = 0;

        // 地图横向格子数
        public int HCount = 0;

        // 地图纵向格子数
        public int VCount = 0;

        //格子标志数据
        public int[,] GridFlags;

        public List<EventData> Events;
    }
}
