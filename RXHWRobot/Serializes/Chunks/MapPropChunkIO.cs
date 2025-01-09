using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapPropChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_PROP; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            mapData.assetID = chunkBytes.ReadInt();
            mapData.assetName = chunkBytes.ReadString();
            mapData.assetWidth = chunkBytes.ReadUInt();
            mapData.assetHeight = chunkBytes.ReadUInt();
            mapData.assetHCount = chunkBytes.ReadUInt();
            mapData.assetVCount = chunkBytes.ReadUInt();
            mapData.assetMiniMode = chunkBytes.ReadByte();
            mapData.assetMiniWidth = chunkBytes.ReadUInt();
            mapData.assetMiniHeight = chunkBytes.ReadUInt();
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteInt(mapData.assetID);
            chunkBytes.WriteString(mapData.assetName);
            chunkBytes.WriteUInt(mapData.assetWidth);
            chunkBytes.WriteUInt(mapData.assetHeight);
            chunkBytes.WriteUInt(mapData.assetHCount);
            chunkBytes.WriteUInt(mapData.assetVCount);
            chunkBytes.WriteByte((byte)mapData.assetMiniMode);
            chunkBytes.WriteUInt(mapData.assetMiniWidth);
            chunkBytes.WriteUInt(mapData.assetMiniHeight);
        }

        #endregion
    }
}
