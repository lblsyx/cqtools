using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapNPCChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_NPC; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint length = chunkBytes.ReadUInt();
            if (length > 0)
            {
                MapNPCData npc = null;
                mapData.mapNPCList.Clear();
                for (int m = 0; m < length; m++)
                {
                    npc = new MapNPCData();
                    mapData.mapNPCList.Add(npc);
                    npc.npcID = chunkBytes.ReadUInt();
                    npc.gridX = chunkBytes.ReadUShort();
                    npc.gridY = chunkBytes.ReadUShort();
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteInt(mapData.mapNPCList.Count);
            if (mapData.mapNPCList.Count > 0)
            {
                foreach (MapNPCData npc in mapData.mapNPCList)
                {
                    chunkBytes.WriteUInt(npc.npcID);
                    chunkBytes.WriteShort((short)npc.gridX);
                    chunkBytes.WriteShort((short)npc.gridY);
                }
            }
        }

        #endregion
    }
}
