using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public class PacketMgr
    {
        private static ulong mPacketCount = 0;
        private static ulong mPacketTotalSize = 0;
        private static object mSyncObject = new object();
        private static Queue<Packet> mPacketQueue = new Queue<Packet>();

        public ulong RecvPacketCount { get { return mPacketCount; } }

        public ulong RecvPacketTotalSize { get { return mPacketTotalSize; } }

        public static Packet[] DequeueAll()
        {
            lock (mSyncObject)
            {
                Packet[] array = mPacketQueue.ToArray();
                mPacketQueue.Clear();
                return array;
            }
        }

        public static void Enqueue(Packet pkg)
        {
            lock (mSyncObject)
            {
                mPacketCount += 1;
                mPacketQueue.Enqueue(pkg);
                mPacketTotalSize += pkg.PacketSize;
            }
        }
    }
}