using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapAsset
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

        public void copyFrom(MapData mapData)
        {
            assetID = mapData.assetID;
            assetName = mapData.assetName;
            assetWidth = mapData.assetWidth;
            assetHeight = mapData.assetHeight;
            assetHCount = mapData.assetHCount;
            assetVCount = mapData.assetVCount;
            assetMiniMode = mapData.assetMiniMode;
            assetMiniWidth = mapData.assetMiniWidth;
            assetMiniHeight = mapData.assetMiniHeight;

            foreach (var gridList in mapData.assetGridList)
            {
                List<byte> temp = new List<byte>();
                assetGridList.Add(temp);
                temp.AddRange(gridList);
            }
            foreach (var maskList in mapData.assetMaskList)
            {
                List<byte> temp = new List<byte>();
                assetMaskList.Add(temp);
                temp.AddRange(maskList);
            }
        }

        /**
         * 判断格子是否具有指定阻挡/安全区标志
         * @param x 要判断的格子x坐标
         * @param y 要判断的格子y坐标
         * @param flag 要判断的阻挡/安全区标志
         * @return true表示有指定阻挡/安全区标志，false表示没有指定阻挡/安全区标志
         */
        public bool hasCellFlag(int x, int y, int flag) {
            if (y >= assetGridList.Count || y < 0) {
                return false;
            }
            List<byte> temp = assetGridList[y];
            if (x >= temp.Count || x < 0) {
                return false;
            }

            if ((temp[x] & flag) == flag)
            {
                return true;
            }

            return false;
        }
    }
}
