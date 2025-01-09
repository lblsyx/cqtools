using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapEffectChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_EFFECT; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint length = chunkBytes.ReadUInt();
            if (length > 0)
            {
                mapData.mapEffectList.Clear();
                MapEffectData mapEffectData = null;
                for (int m = 0; m < length; m++)
                {
                    mapEffectData = new MapEffectData();
                    mapData.mapEffectList.Add(mapEffectData);
                    mapEffectData.effectID = chunkBytes.ReadUInt();
                    mapEffectData.gridX = chunkBytes.ReadUShort();
                    mapEffectData.gridY = chunkBytes.ReadUShort();
                    mapEffectData.offectX = chunkBytes.ReadInt();
                    mapEffectData.offectY = chunkBytes.ReadInt();
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteUInt((uint)mapData.mapEffectList.Count);
            if (mapData.mapEffectList.Count > 0)
            {
                foreach (MapEffectData effectData in mapData.mapEffectList)
                {
                    chunkBytes.WriteUInt(effectData.effectID);
                    chunkBytes.WriteShort((short)effectData.gridX);
                    chunkBytes.WriteShort((short)effectData.gridY);
                    chunkBytes.WriteInt(effectData.offectX);
                    chunkBytes.WriteInt(effectData.offectY);
                }
            }
        }

        #endregion
    }
}
