using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Crypts;
using UnityLight.Internets;
using RXHWRobot.Datas;

namespace RXHWRobot.Robots
{
    public partial class CRobotData
    {
        public byte ClientType = 0;//一般不需要修改
        public byte IndulgeFlag = 0;//一般不需要修改

        public ushort PlatformID;
        public ushort ServerID;
        public uint PlayerID;
        public string Account;
        public uint LoginSignTime;
        public string LoginSignKey;
        public string LoginSignCode;//自己计算签名串
        public string LoginIP;
        public int LoginPort;

        public byte Sex;
        public uint Level;
        public byte Career;
        public string NickName;
        public string GuildName;

        public uint MapID;
        public uint MapX;
        public uint MapY;

        public string GatewayIP;
        public int GatewayPort;
        public uint IDSignTime;
        public string IDSignCode;

        public uint TransferMap;
        public bool NPCTransfered;

        public RobotClient LoginClient = new RobotClient();

        public RobotClient GatewayClient = new RobotClient();

        public CRobotData()
        {
        }

        public void AssignClientOwnerID()
        {
            LoginClient.OwnerID1 = PlatformID;
            LoginClient.OwnerID1 = LoginClient.OwnerID1 << 16;
            LoginClient.OwnerID1 = LoginClient.OwnerID1 | ServerID;

            GatewayClient.OwnerID1 = PlatformID;
            GatewayClient.OwnerID1 = GatewayClient.OwnerID1 << 16;
            GatewayClient.OwnerID1 = GatewayClient.OwnerID1 | ServerID;
        }

        public void MakeLoginSignCode()
        {
            string ming = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                PlatformID,
                ServerID,
                ClientType,
                IndulgeFlag,// 防沉迷不是这里控制的，废弃的。是通过EnterGame001协议控制防沉迷
                LoginSignTime,
                Account,
                LoginSignKey);
            LoginSignCode = MD5.Encode(ming).ToLower();
        }
    }
}
