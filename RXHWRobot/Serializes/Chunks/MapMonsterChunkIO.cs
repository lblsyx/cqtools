using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class MapMonsterChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.MAP_MONSTER; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            uint length = chunkBytes.ReadUInt();
            if (length > 0)
            {
                MapMonData monster = null;
                mapData.mapMonList.Clear();
                for (int n = 0; n < length; n++)
                {
                    monster = new MapMonData();
                    mapData.mapMonList.Add(monster);
                    monster.monsterID = chunkBytes.ReadUInt();
                    monster.monsterNum = chunkBytes.ReadUShort();
                    monster.gridX = chunkBytes.ReadUShort();
                    monster.gridY = chunkBytes.ReadUShort();
                    monster.offsetX = chunkBytes.ReadUShort();
                    monster.offsetY = chunkBytes.ReadUShort();
                    monster.reviveTime = chunkBytes.ReadUInt();
                }
            }
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            MapData mapData = data as MapData;
            chunkBytes.WriteInt(mapData.mapMonList.Count);
            if (mapData.mapMonList.Count > 0)
            {
                foreach (MapMonData monsterData in mapData.mapMonList)
                {
                    chunkBytes.WriteUInt(monsterData.monsterID);
                    chunkBytes.WriteUShort((ushort)monsterData.monsterNum);
                    chunkBytes.WriteUShort((ushort)monsterData.gridX);
                    chunkBytes.WriteUShort((ushort)monsterData.gridY);
                    chunkBytes.WriteUShort((ushort)monsterData.offsetX);
                    chunkBytes.WriteUShort((ushort)monsterData.offsetY);
                    chunkBytes.WriteUInt(monsterData.reviveTime);
                }
            }
        }

        #endregion
    }
}
