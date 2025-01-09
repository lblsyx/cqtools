using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapEffectData
    {
        /**
         * 特效ID
         */
        public uint effectID;
        /**
         * 特效像素坐标位置x
         */
        public uint gridX;
        /**
         * 特效像素坐标位置y
         */
        public uint gridY;

        /**
         * 偏移坐标X
         */
        public int offectX;

        /**
         * 偏移坐标Y
         */
        public int offectY;

        /**
         * 克隆一个新对象
         */
        public MapEffectData clone()
        {
            MapEffectData eff = new MapEffectData();
            eff.effectID = effectID;
            eff.gridX = gridX;
            eff.gridY = gridY;
            eff.offectX = offectX;
            eff.offectY = offectY;
            return eff;
        }
    }
}
