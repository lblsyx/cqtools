using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace RXHWRobot.Robots
{
    public partial class CRobotData
    {
        public void SendToMap(Packet pkg)
        {
            if (GatewayClient != null && GatewayClient.Socket != null && GatewayClient.Socket.Connected)
            {
                GatewayClient.SendTCP(pkg, (byte)ServerType.MapServerType);
            }
            else
            {
                //网关连接断开
            }
        }

        public void SendToWorld(Packet pkg)
        {
            if (GatewayClient != null && GatewayClient.Socket != null && GatewayClient.Socket.Connected)
            {
                GatewayClient.SendTCP(pkg, (byte)ServerType.WorldServerType);
            }
            else
            {
                //网关连接断开
            }
        }

        public void SendToGateway(Packet pkg)
        {
            if (GatewayClient != null && GatewayClient.Socket != null && GatewayClient.Socket.Connected)
            {
                GatewayClient.SendTCP(pkg, (byte)ServerType.GatewayServerType);
            }
            else
            {
                //网关连接断开
            }
        }
    }
}
