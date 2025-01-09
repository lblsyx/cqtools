using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapBlockChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_BLOCK; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint heightCount = chunkBytes.ReadUInt();

            if (heightCount > 0)
            {
                mapData.assetGridList.Clear();
                uint widthCount = chunkBytes.ReadUInt();
                for (int i = 0; i < heightCount; i++)
                {
                    mapData.assetGridList.Add(new List<byte>());
                    for (int j = 0; j < widthCount; j++)
                    {
                        mapData.assetGridList[i].Add(chunkBytes.ReadByte());
                    }
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteUInt((uint)mapData.assetGridList.Count);
            if (mapData.assetGridList.Count > 0)
            {
                chunkBytes.WriteUInt((uint)mapData.assetGridList[0].Count);
                foreach (List<byte> array in mapData.assetGridList)
                {
                    foreach (byte flag in array)
                    {
                        chunkBytes.WriteByte(flag);
                    }
                }
            }
        }

        #endregion
    }
}
