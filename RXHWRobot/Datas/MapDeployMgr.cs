using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;
using RXHWRobot.Serializes.IOs;

namespace RXHWRobot.Datas
{
    public class MapDeployMgr
    {
        private static Dictionary<int, MapDeploy> _mapDeployDic = new Dictionary<int, MapDeploy>();

        public static void Clear()
        {
            _mapDeployDic.Clear();
        }

        public static MapDeploy getMapDeploy(int mapID)
        {
            if (_mapDeployDic.ContainsKey(mapID))
            {
                return _mapDeployDic[mapID] as MapDeploy;
            }

            return null;
        }

        public static MapDeploy[] getMapDeploy()
        {
            return _mapDeployDic.Values.ToArray();
        }

        public static bool decodeMapDeployList(ByteArray bytes)
        {
            if (bytes == null) return false;
            bytes.Position = 0;
            if (bytes.BytesAvailable == 0) return false;

            ByteArray byteArray = new ByteArray();
            bool isCompress = bytes.ReadBoolean();
            uint uncompressLen = bytes.ReadUInt();
            uint size = bytes.ReadUInt();
            byteArray.WrapBuffer(bytes.ReadBytes((int)size, false), (int)size);
            if (isCompress) byteArray = byteArray.Uncompress();
            if (uncompressLen != byteArray.Length) return false;

            MapDeployIO mapDeployIO = new MapDeployIO();
            uint count = byteArray.ReadUInt();
            ByteArray temp = new ByteArray();
            MapData mapData = new MapData();
            uint len; MapDeploy md;
            bool rlt = false;

            for (int i = 0; i < count; i++)
            {
                temp.Reset();
                len = byteArray.ReadUInt();
                temp.WrapBuffer(byteArray.ReadBytes((int)len, false), (int)len);
                temp.Position = 0;
                if (mapDeployIO.Decode(temp, mapData))
                {
                    md = null;
                    rlt = true;
                    if (_mapDeployDic.ContainsKey(mapData.mapID))
                    {
                        md = _mapDeployDic[mapData.mapID];
                        md.copyFrom(mapData);
                    }
                    else
                    {
                        md = new MapDeploy();
                        md.copyFrom(mapData);
                        _mapDeployDic.Add(md.mapID, md);
                    }
                }
            }

            return rlt;
        }
    }
}
