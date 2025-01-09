using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Robots;
using UnityLight.Internets;
using UnityLight.Loggers;

namespace RXHWRobot
{
    public class RunTaskMgr
    {
        private static object locker = new object();
        private static Dictionary<byte, List<RunTaskData>> mDataDict = new Dictionary<byte, List<RunTaskData>>();

        public static void ParseData(ByteArray bytes)
        {
            bytes = bytes.Uncompress();
            bytes.Position = 0;
            List<RunTaskData> list = null;
            RunTaskData oRunTaskData = new RunTaskData();
            oRunTaskData.Career = bytes.ReadByte();
            oRunTaskData.RunTaskStartMapX = bytes.ReadInt();
            oRunTaskData.RunTaskStartMapY = bytes.ReadInt();
            int len = bytes.ReadInt();
            for (int i = 0; i < len; i++)
            {
                int sendTime = bytes.ReadInt();
                string clsName = bytes.ReadUTF();
                int pkgDataLen = bytes.ReadInt();
                byte[] datas = bytes.ReadBytes(pkgDataLen, false);

                Type type = Global.CurrentAssembly.GetType("RXHWRobot." + clsName);
                Packet pkg = Activator.CreateInstance(type) as Packet;

                if (pkg == null)
                {
                    throw new Exception("找不到对应数据包对象!Class：" + clsName);
                }

                pkg.Serializtion(new ByteArray(datas, datas.Length), false);
                oRunTaskData.RecordTimerList.Add(sendTime);
                oRunTaskData.RecordPacketList.Add(pkg);

                if (mDataDict.ContainsKey(oRunTaskData.Career) == false)
                {
                    list = new List<RunTaskData>();
                    mDataDict.Add(oRunTaskData.Career, list);
                }
                else
                {
                    list = mDataDict[oRunTaskData.Career];
                }

                list.Add(oRunTaskData);
            }
        }

        public static RunTaskData GetRunTask(byte career)
        {
            lock (locker)
            {
                if (mDataDict.ContainsKey(career))
                {
                    List<RunTaskData> list = mDataDict[career];
                    if (list.Count > 0)
                    {
                        RunTaskData rtd = list[0];
                        list.RemoveAt(0);
                        list.Add(rtd);
                        return rtd.Clone();
                    }
                }
            }
            return null;
        }
    }
}
