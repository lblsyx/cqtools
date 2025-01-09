using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    class MapShadeChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_SHADE; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint heightCount = chunkBytes.ReadUInt();
            if (heightCount > 0)
            {
                mapData.assetMaskList.Clear();
                uint widthCount = chunkBytes.ReadUInt();
                for (int k = 0; k < heightCount; k++)
                {
                    mapData.assetMaskList.Add(new List<byte>());
                    for (int l = 0; l < widthCount; l++)
                    {
                        mapData.assetMaskList[k].Add(chunkBytes.ReadByte());
                    }
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteInt(mapData.assetMaskList.Count);
            if (mapData.assetMaskList.Count > 0)
            {
                chunkBytes.WriteInt(mapData.assetMaskList[0].Count);
                foreach (var array1 in mapData.assetMaskList)
                {
                    foreach (var flag1 in array1)
                    {
                        chunkBytes.WriteByte(flag1);
                    }
                }
            }
        }

        #endregion
    }
}
