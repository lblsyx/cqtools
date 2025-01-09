using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapInfoChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_INFO; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            mapData.mapID = chunkBytes.ReadInt();
            mapData.assetID = chunkBytes.ReadInt();
            mapData.mapName = chunkBytes.ReadString();
            mapData.reviveOffsetX = chunkBytes.ReadInt();
            mapData.reviveOffsetY = chunkBytes.ReadInt();
            mapData.mapReviveX = chunkBytes.ReadInt();
            mapData.mapReviveY = chunkBytes.ReadInt();
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteInt(mapData.mapID);
            chunkBytes.WriteInt(mapData.assetID);
            chunkBytes.WriteString(mapData.mapName);
            chunkBytes.WriteInt(mapData.reviveOffsetX);
            chunkBytes.WriteInt(mapData.reviveOffsetY);
            chunkBytes.WriteInt(mapData.mapReviveX);
            chunkBytes.WriteInt(mapData.mapReviveY);
        }

        #endregion
    }
}
