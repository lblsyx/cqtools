using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapData : RpgData
    {
        /**
         * 资源ID
         */
        public int assetID;
        /**
         * 资源名称
         */
        public string assetName = string.Empty;
        /**
         * 地图总宽度
         */
        public uint assetWidth;
        /**
         * 地图总高度
         */
        public uint assetHeight;
        /**
         * 资源地图横向分割数量
         */
        public uint assetHCount;
        /**
         * 资源地图纵向分割数量
         */
        public uint assetVCount;
        /**
         * 缩略模式(1:根据宽度缩放,2:根据高度缩放,3:自动根据宽高填充,4:宽高强制缩放[变形])
         */
        public uint assetMiniMode;
        /**
         * 缩略图的宽度
         */
        public uint assetMiniWidth;
        /**
         * 缩略图的高度
         */
        public uint assetMiniHeight;
        /**
         * 格子标志数据(最高支持8种标志)
         */
        public List<List<byte>> assetGridList = new List<List<byte>>();
        /**
         * 遮挡标志数据
         */
        public List<List<byte>> assetMaskList = new List<List<byte>>();

        /**
         * 地图ID
         */
        public int mapID;
        /**
         * 地图名称
         */
        public string mapName = string.Empty;
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
        public int mapReviveX;
        public int mapReviveY;
        /**
         * 出生范围X、Y
         */
        public int reviveOffsetX;
        public int reviveOffsetY;

        public void copyFromAsset(MapAsset mapAsset)
        {
            assetID = mapAsset.assetID;
            assetName = mapAsset.assetName;
            assetWidth = mapAsset.assetWidth;
            assetHeight = mapAsset.assetHeight;
            assetHCount = mapAsset.assetHCount;
            assetVCount = mapAsset.assetVCount;
            assetMiniMode = mapAsset.assetMiniMode;
            assetMiniWidth = mapAsset.assetMiniWidth;
            assetMiniHeight = mapAsset.assetMiniHeight;
            foreach (var gridList in mapAsset.assetGridList)
            {
                List<byte> temp = new List<byte>();
                assetGridList.Add(temp);
                temp.AddRange(gridList);
            }
            foreach (var maskList in mapAsset.assetMaskList)
            {
                List<byte> temp = new List<byte>();
                assetMaskList.Add(temp);
                temp.AddRange(maskList);
            }
        }

        public void copyFromDeploy(MapDeploy mapDeploy)
        {
            mapID = mapDeploy.mapID;
            mapName = mapDeploy.mapName;
            assetID = mapDeploy.assetID;
            mapReviveX = mapDeploy.mapBornX;
            mapReviveY = mapDeploy.mapBornY;
            reviveOffsetX = mapDeploy.mapScopeOffsetX;
            reviveOffsetY = mapDeploy.mapScopeOffsetY;


            mapNPCList.Clear();
            mapMonList.Clear();
            mapEventList.Clear();
            mapEffectList.Clear();
            foreach (var mapNPCData in mapDeploy.mapNPCList)
            {
                mapNPCList.Add(mapNPCData.clone());
            }

            foreach (MapNPCData mapNPCData in mapDeploy.mapNPCList)
            {
                mapNPCList.Add(mapNPCData.clone());
            }
            foreach (MapMonData mapMonsterData in mapDeploy.mapMonList)
            {
                mapMonList.Add(mapMonsterData.clone());
            }
            foreach (MapEventData mapEventData in mapDeploy.mapEventList)
            {
                mapEventList.Add(mapEventData.clone());
            }
            foreach (MapEffectData mapEffectData in mapDeploy.mapEffectList)
            {
                mapEffectList.Add(mapEffectData.clone());
            }
        }

        /**
         * 判断格子是否具有指定阻挡/安全区标志
         * @param x 要判断的格子x坐标
         * @param y 要判断的格子y坐标
         * @param flag 要判断的阻挡/安全区标志
         * @return true表示有指定阻挡/安全区标志，false表示没有指定阻挡/安全区标志
         */
        public bool hasCellFlag(int x, int y, int flag)
        {
            if (y >= assetGridList.Count || y < 0)
            {
                return false;
            }
            List<byte> temp = assetGridList[y];
            if (x >= temp.Count || x < 0)
            {
                return false;
            }

            if ((temp[x] & flag) == flag)
            {
                return true;
            }

            return false;
        }

        /**
         * 判断格子是否具有指定遮挡标志
         * @param x 要判断的格子x坐标
         * @param y 要判断的格子y坐标
         * @param flag 要判断的遮挡标志
         * @return true表示有指定遮挡标志，false表示没有指定遮挡标志
         */
        public bool hasShadeFlag(int x, int y, int flag)
        {
            if (y >= assetMaskList.Count || y < 0)
            {
                return false;
            }
            List<byte> temp = assetMaskList[y];
            if (x >= temp.Count || x < 0)
            {
                return false;
            }

            if ((temp[x] & flag) == flag)
            {
                return true;
            }

            return false;
        }

        public MapData clone()
        {
            MapData mapData = new MapData();

            mapData.mapID = mapID;
            mapData.assetID = assetID;
            mapData.assetName = assetName;
            mapData.assetWidth = assetWidth;
            mapData.assetHeight = assetHeight;
            mapData.objectType = objectType;
            mapData.assetHCount = assetHCount;
            mapData.assetVCount = assetVCount;
            mapData.assetMiniMode = assetMiniMode;
            mapData.assetMiniWidth = assetMiniWidth;
            mapData.assetMiniHeight = assetMiniHeight;

            foreach (var gridList in assetGridList)
            {
                List<byte> temp = new List<byte>();
                mapData.assetGridList.Add(temp);
                temp.AddRange(gridList);
            }
            foreach (var maskList in assetMaskList)
            {
                List<byte> temp = new List<byte>();
                mapData.assetMaskList.Add(temp);
                temp.AddRange(maskList);
            }

            foreach (MapNPCData mapNPCData in mapNPCList)
            {
                mapData.mapNPCList.Add(mapNPCData.clone());
            }
            foreach (MapEventData mapEventData in mapEventList)
            {
                mapData.mapEventList.Add(mapEventData.clone());
            }
            foreach (MapEffectData mapEffectData in mapEffectList)
            {
                mapData.mapEffectList.Add(mapEffectData.clone());
            }
            foreach (MapMonData mapMonData in mapMonList)
            {
                mapData.mapMonList.Add(mapMonData.clone());
            }

            return mapData;
        }
    }
}
