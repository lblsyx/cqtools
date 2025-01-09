using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public class ProtocolTCP : IProtocol
    {
        private Packet TempPacket = new Packet();

        private void CalPackCount(TCPClient oTCPClient, Packet oPacket)
        {
            if (oTCPClient.mIsPkgCount == true)
            {
                if (oTCPClient.mPkgCountDic.ContainsKey(oPacket.PacketID))
                {
                    oTCPClient.mPkgCountDic[oPacket.PacketID]++;
                }
                else
                {
                    oTCPClient.mPkgCountDic.Add(oPacket.PacketID, 1);
                }
            }
        }

        public int OnSendData(TCPClient oTCPClient, Packet oPacket, ByteArray oByteArray)
        {
            ByteArray ByteArraySend = new ByteArray(PacketConfig.PackageBufSize * 4);
            ByteArraySend.Reset();
            oPacket.Serializtion(ByteArraySend, true);
            ByteArraySend.Position = 2;
            ByteArraySend.WriteUInt((uint)ByteArraySend.Length);
            ByteArraySend.Position = 0;
            Console.WriteLine("Robot|发送一个数据包!PacketID:{0}:{1}:{2}", oPacket.PacketID, oPacket.TargetID1, oPacket.TargetID2);
            if (oByteArray.RemainedSize >= ByteArraySend.Length)
            {
                //oTCPClient.mSendFSM.XOR(ByteArraySend, ByteArraySend, ByteArraySend.Length);
                //oTCPClient.mSendFSM.UpdateState();
                oByteArray.WriteBytes(ByteArraySend.Buffer, 0, ByteArraySend.Length);
                CalPackCount(oTCPClient, oPacket);
                return ByteArraySend.Length;
            }
            return 0;
        }

        public int OnRecvData(TCPClient oTCPClient, ByteArray oByteArray)
        {
            int nPacketSize = 0;
            int nParseSize = 0;
            ByteArray ByteArrayRecv = new ByteArray(PacketConfig.PackageBufSize * 4);
            while (oByteArray.BytesAvailable >= PacketConfig.PackageHeaderSize)
            {
                ByteArrayRecv.Reset();
                oTCPClient.mRecvFSM.XOR(oByteArray, ByteArrayRecv, oByteArray.Length - oByteArray.Position);
                ByteArrayRecv.Position = 0;
                TempPacket.Serializtion(ByteArrayRecv, false);
                nPacketSize = (int)TempPacket.PacketSize;
                ByteArrayRecv.Position = 0;

                if (TempPacket.Header != PacketConfig.PackageHeader)
                {
                    throw new Exception("非法包头标志数据!");
                }

                if (nPacketSize > ByteArrayRecv.BytesAvailable)
                {
                    Console.WriteLine("Robot|nPacketSize[{0}]BytesAvailable[{1}]",  nPacketSize, ByteArrayRecv.BytesAvailable);
                    break;
                }
                    
                nParseSize += nPacketSize;
                Console.WriteLine("Robot|接收到一个数据包!PacketID:{0}:{1}:{2}", TempPacket.PacketID, nPacketSize, nParseSize);
                oTCPClient.mRecvFSM.UpdateState();

                Packet pkg = PacketFactory.CreatePacket(TempPacket.PacketID);
                if (pkg != null)
                {
                    pkg.Serializtion(ByteArrayRecv, false);
                    pkg.Client = oTCPClient;
                    CalPackCount(oTCPClient, pkg);
                    if (oTCPClient.RecvPacket(pkg) == false)
                    {
                        PacketMgr.Enqueue(pkg);
                    }
                }

                oByteArray.Position += (int)TempPacket.PacketSize;
            }

            return nParseSize;
        }

        /*
        public int OnRecvData1(TCPClient oTCPClient, ByteArray oByteArray)
        {
            int nPacketSize = 0;
            int nParseSize = 0;
            while (oByteArray.BytesAvailable >= PacketConfig.PackageHeaderSize)
            {
                oTCPClient.mRecvFSM.XOR(oByteArray, oByteArray, PacketConfig.PackageHeaderSize);
                oByteArray.Position -= PacketConfig.PackageHeaderSize;
                TempPacket.Serializtion(oByteArray, false);
                Console.WriteLine("Robot|接收到一个数据包!PacketID:{0}", TempPacket.PacketID);

                if (TempPacket.Header != PacketConfig.PackageHeader)
                {
                    throw new Exception("非法包头标志数据!");
                }

                nPacketSize = (int)TempPacket.PacketSize;

                if (nPacketSize > oByteArray.BytesAvailable + PacketConfig.PackageHeaderSize) break;
                nParseSize += nPacketSize;

                oTCPClient.mRecvFSM.XOR(oByteArray, oByteArray, nPacketSize - PacketConfig.PackageHeaderSize);
                oTCPClient.mRecvFSM.UpdateState();

                Packet pkg = PacketFactory.CreatePacket(TempPacket.PacketID);
                if (pkg != null)
                {
                    oByteArray.Position -= nPacketSize;
                    pkg.Serializtion(oByteArray, false);
                    pkg.Client = oTCPClient;

                    if (oTCPClient.RecvPacket(pkg) == false)
                    {
                        PacketMgr.Enqueue(pkg);
                    }
                }
                else
                {
                    oByteArray.Position += (int)TempPacket.PacketSize;
                }
            }

            return nParseSize;
        }*/
    }
}