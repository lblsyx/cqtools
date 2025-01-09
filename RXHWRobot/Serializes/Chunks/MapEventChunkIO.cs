using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;
using UnityLight.Exts;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapEventChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_EVENT; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint length = chunkBytes.ReadUInt();
            if (length > 0)
            {
                mapData.mapEventList.Clear();
                for (int i = 0; i < length; i++)
                {
                    MapEventData mapEventData = new MapEventData();
                    mapEventData.id = chunkBytes.ReadInt();
                    mapEventData.eventID = chunkBytes.ReadInt();
                    mapEventData.eventName = chunkBytes.ReadString();
                    mapEventData.paramStr = chunkBytes.ReadString();
                    mapEventData.summary = chunkBytes.ReadString();
                    uint length2 = chunkBytes.ReadUInt();
                    for (int j = 0; j < length2; j++)
                    {
                        Point point = new Point();
                        point.X = chunkBytes.ReadUShort();
                        point.Y = chunkBytes.ReadUShort();
                        mapEventData.gridList.Add(point);
                    }
                    mapData.mapEventList.Add(mapEventData);
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteUInt((uint)mapData.mapEventList.Count);
            if (mapData.mapEventList.Count > 0)
            {
                foreach (MapEventData mapEventData in mapData.mapEventList)
                {
                    chunkBytes.WriteUInt((uint)mapEventData.id);
                    chunkBytes.WriteUInt((uint)mapEventData.eventID);
                    chunkBytes.WriteString(mapEventData.eventName);
                    chunkBytes.WriteString(mapEventData.paramStr);
                    chunkBytes.WriteString(mapEventData.summary);
                    chunkBytes.WriteUInt((uint)mapEventData.gridList.Count);
                    foreach (Point point in mapEventData.gridList)
                    {
                        chunkBytes.WriteShort((short)point.X);
                        chunkBytes.WriteShort((short)point.Y);
                    }
                }
            }
        }

        #endregion
    }
}
