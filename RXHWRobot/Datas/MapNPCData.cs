using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapNPCData
    {
        /**
         * NPC ID
         */
        public uint npcID;
        /**
         * NPC像素坐标位置x
         */
        public uint gridX;
        /**
         * NPC像素坐标位置y
         */
        public uint gridY;

        /**
         * 克隆一个新对象
         */
        public MapNPCData clone() {
            MapNPCData npc = new MapNPCData();
            npc.npcID = npcID;
            npc.gridX = gridX;
            npc.gridY = gridY;
            return npc;
        }
    }
}
