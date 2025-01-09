using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Robots
{
    public partial class RobotCtrl
    {
        /// <summary>
        /// 登录创角(请求)
        /// </summary>
        /// 
        public void SendReqPlayerLogin0100()
        {
            ReqPlayerLogin0100 oReqPlayerLogin0100 = new ReqPlayerLogin0100();
            oReqPlayerLogin0100.LoginInf.PlatformID = Data.PlatformID;
            oReqPlayerLogin0100.LoginInf.ServerID = Data.ServerID;
            oReqPlayerLogin0100.LoginInf.ClientType = Data.ClientType;
            oReqPlayerLogin0100.LoginInf.IndulgeFlag = Data.IndulgeFlag;
            oReqPlayerLogin0100.LoginInf.LoginTime = Data.LoginSignTime;
            oReqPlayerLogin0100.LoginInf.SignCode = Data.LoginSignCode;
            oReqPlayerLogin0100.LoginInf.Account = Data.Account;
            Data.LoginClient.SendTCP(oReqPlayerLogin0100);
        }

        public void SendReqCreateRole0100()
        {
            byte career = (byte)random.Next(1, 3);
            byte sex = (byte)random.Next(1, 2);

            char[] charArray = new char[random.Next(7, 12)];
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = Global.letters[random.Next(0, Global.letters.Length)];
            }
            Data.NickName = new String(charArray);

            ReqCreateRole0100 oReqCreateRole0100 = new ReqCreateRole0100();
            oReqCreateRole0100.PlayerIDSignCode = Data.IDSignCode;
            oReqCreateRole0100.PlayerIDSignTime = Data.IDSignTime;
            oReqCreateRole0100.Career = career;
            oReqCreateRole0100.Sex = sex;
            oReqCreateRole0100.NickName = Data.NickName;
            Data.LoginClient.SendTCP(oReqCreateRole0100);
        }

        public void SendReqEnterGame001()
        {
            ReqEnterGame001 oReqEnterGame001 = new ReqEnterGame001();
            oReqEnterGame001.PlayerIDSignTime = Data.IDSignTime;
            oReqEnterGame001.PlayerIDSignCode = Data.IDSignCode;
            oReqEnterGame001.PlayerIsIndulgence = Data.IndulgeFlag;
            Data.SendToGateway(oReqEnterGame001);
        }

        public void SendReqEnterMap008(ServerType type)
        {
            if(type == ServerType.WorldServerType)
            {
                Data.SendToWorld(new ReqEnterMap008());
            }
            else if (type == ServerType.GatewayServerType)
            {
                Data.SendToGateway(new ReqEnterMap008());
            }        
        }

        public void SendReqPlayerMove()
        {
            ReqPlayerMove oReqPlayerMove = new ReqPlayerMove();
            oReqPlayerMove.WalkType = 0;
            oReqPlayerMove.MapID = Data.MapID;
            oReqPlayerMove.TargetX = (ushort)TargetX;
            oReqPlayerMove.TargetY = (ushort)TargetY;
            Data.SendToMap(oReqPlayerMove);
        }

        public void SendReqNoticeStopWalk()
        {
            Data.SendToMap(new ReqNoticeStopWalk());
        }

        public void SendReqHeartBeat()
        {
            ReqHeartBeat oReqHeartBeat = new ReqHeartBeat();
            oReqHeartBeat.SendMsgTime = (double)(mOffset.Hour * 3600 + mOffset.Minute + mOffset.Second);
            Data.SendToGateway(oReqHeartBeat);
        }

        public void SendReqGMCommand(string cmd)
        {
            ReqGMCommand oReqGMCommand = new ReqGMCommand();
            oReqGMCommand.CommandStr = cmd;
            Data.SendToWorld(oReqGMCommand);
        }

        public void SendReqNPCTransferToMap(uint Map)
        {
            ReqNPCTransferToMap oReqNPCTransferToMap = new ReqNPCTransferToMap();
            oReqNPCTransferToMap.MapID = Map;
            Data.SendToWorld(oReqNPCTransferToMap);
            Data.NPCTransfered = false;
        }
    }
}
