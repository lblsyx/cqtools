using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace UnityExt.Chunks.MapChunks
{
    public class MapPropChunkIO : IChunkIO
    {
        public int Code
        {
            get { return MapChunkType.MAP_PROP; }
        }

        public void ReadData(object data, ByteArray bytes)
        {
            MapData oMapData = data as MapData;

            oMapData.MapID = bytes.ReadInt();
            oMapData.MapName = bytes.ReadUTF();
            oMapData.Width = bytes.ReadInt();
            oMapData.Height = bytes.ReadInt();
        }

        public void WriteData(object data, ByteArray bytes)
        {
            MapData oMapData = data as MapData;

            bytes.WriteInt(oMapData.MapID);
            bytes.WriteUTF(oMapData.MapName);
            bytes.WriteInt(oMapData.Width);
            bytes.WriteInt(oMapData.Height);
        }
    }
}
