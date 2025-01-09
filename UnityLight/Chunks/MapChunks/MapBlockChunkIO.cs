using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;
using UnityLight.Loggers;

namespace UnityExt.Chunks.MapChunks
{
    public class MapBlockChunkIO : IChunkIO
    {
        public int Code
        {
            get { return MapChunkType.MAP_BLOCK; }
        }

        public void ReadData(object data, ByteArray bytes)
        {
            MapData oMapData = data as MapData;
            oMapData.HCount = bytes.ReadInt();
            oMapData.VCount = bytes.ReadInt();
            //List<string> list = new List<string>();
            oMapData.GridFlags = new int[oMapData.HCount, oMapData.VCount];
            for (int y = 0; y < oMapData.VCount; y++)
            {
                for (int x = 0; x < oMapData.HCount; x++)
                {
                    oMapData.GridFlags[x, y] = bytes.ReadByte();

                    //list.Add(string.Format("({0},{1}) == {2}", i, j, oMapData.GridFlags[i, j]));
                }
            }
        }

        public void WriteData(object data, UnityLight.Internets.ByteArray bytes)
        {
            MapData oMapData = data as MapData;
            bytes.WriteInt(oMapData.HCount);
            bytes.WriteInt(oMapData.VCount);
            for (int j = 0; j < oMapData.VCount; j++)
            {
                for (int i = 0; i < oMapData.HCount; i++)
                {
                    bytes.WriteByte((byte)oMapData.GridFlags[i, j]);
                }
            }
        }
    }
}
