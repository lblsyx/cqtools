using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;
using System.Collections;

namespace RXHWRobot.Robots
{
    public class RobotClient : TCPClient
    {
        public object PacketSyncRoot = new object();
        public Queue<Packet> PacketQueue = new Queue<Packet>(100);
        public uint RecvPacketCount = 0;

        public RobotClient()
            : base(new ProtocolTCP())
        {

        }

        public override bool RecvPacket(Packet pkg)
        {
            lock (PacketSyncRoot)
            {
                RecvPacketCount++;
                PacketQueue.Enqueue(pkg);
            }
            return true;
        }
    }
}
