using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapAssetMgr
    {
        private static Dictionary<int, MapAsset> mMapAssetDict = new Dictionary<int, MapAsset>();

        public static void Clear()
        {
            mMapAssetDict.Clear();
        }

        public static void AddMapAsset(MapAsset oMapAsset)
        {
            if(mMapAssetDict.ContainsKey(oMapAsset.assetID))
            {
                Console.WriteLine("重复地图资源{0}", oMapAsset.assetID);
                return;
            }

            mMapAssetDict.Add(oMapAsset.assetID, oMapAsset);
        }

        public static MapAsset GetMapAsset(int assetID)
        {
            if (mMapAssetDict.ContainsKey(assetID))
            {
                return mMapAssetDict[assetID];
            }
            return null;
        }
    }
}
