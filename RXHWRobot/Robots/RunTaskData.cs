using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace RXHWRobot.Robots
{
    public class RunTaskData
    {
        public byte Career;
        public int RunTaskStartMapX;
        public int RunTaskStartMapY;
        public List<int> RecordTimerList = new List<int>();
        public List<Packet> RecordPacketList = new List<Packet>();

        public RunTaskData Clone()
        {
            RunTaskData oRunTaskData = new RunTaskData();

            oRunTaskData.RunTaskStartMapX = RunTaskStartMapX;
            oRunTaskData.RunTaskStartMapY = RunTaskStartMapY;
            oRunTaskData.RecordTimerList.AddRange(RecordTimerList);
            foreach (var item in RecordPacketList)
            {
                oRunTaskData.RecordPacketList.Add(item.Clone());
            }

            return oRunTaskData;
        }
    }
}
