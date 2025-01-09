using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapDeploy
    {
        /**
     * 地图ID
     */
        public int mapID;
        /**
         * 地图名称
         */
        public string mapName;
        /**
         * 资源ID
         */
        public int assetID;
        /**
         * NPC分布数据
         */
        public List<MapNPCData> mapNPCList = new List<MapNPCData>();
        /**
         * 事件列表
         */
        public List<MapEventData> mapEventList = new List<MapEventData>();
        /**
         * 特效分布数据
         */
        public List<MapEffectData> mapEffectList = new List<MapEffectData>();
        /**
         * 怪物分布数据
         */
        public List<MapMonData> mapMonList = new List<MapMonData>();
        /**
         * 出生点
         */
        public int mapBornX;
        public int mapBornY;
        /**
         * 出生范围X、Y
         */
        public int mapScopeOffsetX;
        public int mapScopeOffsetY;

        public void copyFrom(MapData mapData)
        {
            mapID = mapData.mapID;
            mapName = mapData.mapName;
            assetID = mapData.assetID;
            mapBornX = mapData.mapReviveX;
            mapBornY = mapData.mapReviveY;
            mapScopeOffsetX = mapData.reviveOffsetX;
            mapScopeOffsetY = mapData.reviveOffsetY;

            mapNPCList.Clear();
            mapMonList.Clear();
            mapEventList.Clear();
            mapEffectList.Clear();

            foreach (MapNPCData mapNPCData in mapData.mapNPCList)
            {
                mapNPCList.Add(mapNPCData.clone());
            }
            foreach (MapMonData mapMonsterData in mapData.mapMonList)
            {
                mapMonList.Add(mapMonsterData.clone());
            }
            foreach (MapEventData mapEventData in mapData.mapEventList)
            {
                mapEventList.Add(mapEventData.clone());
            }
            foreach (MapEffectData mapEffectData in mapData.mapEffectList)
            {
                mapEffectList.Add(mapEffectData.clone());
            }
        }
    }
}
