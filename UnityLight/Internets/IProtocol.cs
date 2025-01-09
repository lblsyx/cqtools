using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public interface IProtocol
    {
        int OnSendData(TCPClient oTCPClient, Packet oPacket, ByteArray oByteArray);

        int OnRecvData(TCPClient oTCPClient, ByteArray oByteArray);
    }
}