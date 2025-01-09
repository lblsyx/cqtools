using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Exts;
using UnityLight.Internets;

namespace UnityExt.Chunks.MapChunks
{
    public class MapEventChunkIO : IChunkIO
    {
        public int Code
        {
            get { return MapChunkType.MAP_EVENT; }
        }

        public void ReadData(object data, ByteArray bytes)
        {
            MapData oMapData = data as MapData;

            int count = bytes.ReadInt();
            oMapData.Events = new List<EventData>();

            for (int i = 0; i < count; i++)
            {
                EventData eventData = new EventData();
                eventData.EventType = bytes.ReadInt();
                eventData.EventID = bytes.ReadInt();
                int temp = bytes.ReadInt();
                Point[] points = new Point[temp];
                for (int j = 0; j < points.Length; j++)
                {
                    Point point = new Point();
                    point.X = bytes.ReadInt();
                    point.Y = bytes.ReadInt();
                    points[j] = point;
                }
                eventData.EventName = bytes.ReadUTF();
                eventData.EventParams = bytes.ReadUTF();
                eventData.EventSummary = bytes.ReadUTF();
                eventData.GridList = points;
                oMapData.Events.Add(eventData);
            }
        }

        public void WriteData(object data, ByteArray bytes)
        {
            MapData oMapData = data as MapData;

            bytes.WriteInt(oMapData.Events.Count);
            for (int i = 0; i < oMapData.Events.Count; i++)
            {
                EventData eventData = oMapData.Events[i];
                bytes.WriteInt(eventData.EventType);
                bytes.WriteInt(eventData.EventID);
                bytes.WriteInt(eventData.GridList.Length);
                for (int j = 0; j < eventData.GridList.Length; j++)
                {
                    bytes.WriteInt(eventData.GridList[j].X);
                    bytes.WriteInt(eventData.GridList[j].Y);
                }
                bytes.WriteUTF(eventData.EventName);
                bytes.WriteUTF(eventData.EventParams);
                bytes.WriteUTF(eventData.EventSummary);
            }
        }
    }
}
