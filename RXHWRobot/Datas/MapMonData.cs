using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapMonData
    {
        /**
         * 怪物ID
         */
        public uint monsterID;
        /**
         * 怪物数量
         */
        public uint monsterNum = 1;
        /**
         * 怪物像素坐标位置x
         */
        public uint gridX;
        /**
         * 怪物像素坐标位置y
         */
        public uint gridY;
        /**
         * 怪物随机刷新x轴偏移量
         */
        public uint offsetX;
        /**
         * 怪物随机刷新y轴偏移量
         */
        public uint offsetY;
        /**
         * 复活时间
         */
        public uint reviveTime;

        /**
         * 克隆一个新对象
         */
        public MapMonData clone() {
            MapMonData monster = new MapMonData();
            monster.monsterID = monsterID;
            monster.monsterNum = monsterNum;
            monster.gridX = gridX;
            monster.gridY = gridY;
            monster.offsetX = offsetX;
            monster.offsetY = offsetY;
            monster.reviveTime = reviveTime;
            return monster;
        }
    }
}
