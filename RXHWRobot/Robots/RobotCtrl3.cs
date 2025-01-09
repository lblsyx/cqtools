using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Events;
using UnityLight;
using RXHWRobot.Datas;
using System.Threading;

namespace RXHWRobot.Robots
{
    public partial class RobotCtrl
    {
        /// <summary>
        /// 登录创角(响应)
        /// </summary>
        /// 
        public void Initialize()
        {
            LoginCompleted = false;
            random = new Random(Global.random.Next(int.MaxValue));
            SleepMillionSeconds = 16;//random.Next(4, 17);

            foreach (var item in FaShiSkill)
            {
                NextUseSkillTime.Add(item, 0);
            }
            foreach (var item in DaoShiSkill)
            {
                NextUseSkillTime.Add(item, 0);
            }

            Data.TransferMap = RobotConfig.Instance.MapID[(uint)random.Next(0, (int)RobotConfig.Instance.MapID.Length)];
            Data.NPCTransfered = false;

            AddEventListener((int)PacketCode.ResPlayerLogin0100Code, OnResPlayerLogin0100Handler);
            AddEventListener((int)PacketCode.ResCreateRole0100Code, OnResCreateRole0100Handler);
            AddEventListener((int)PacketCode.ResPlayerLogin0102Code, OnResPlayerLogin0102Handler);
            AddEventListener((int)PacketCode.ResEnterGame004Code, OnResEnterGame004Handler);
            AddEventListener((int)PacketCode.ResEnterMap007Code, OnResEnterMap007Handler);
            AddEventListener((int)PacketCode.ResEnterMap009Code, OnResEnterMap009Handler);
            AddEventListener((int)PacketCode.ResPlayerMoveCode, OnResPlayerMoveHandler);
            AddEventListener((int)PacketCode.ResHeartBeatCode, OnResHeartBeatHandler);
            AddEventListener((int)PacketCode.ResAddMapObjectCode, OnResAddMapObjectHandler);
            AddEventListener((int)PacketCode.ResDelMapObjectCode, OnResDelMapObjectHandler);
            AddEventListener((int)PacketCode.ResPlayerDisconnectCode, OnResPlayerDisconnectHandler);
        }

        private void OnResPlayerLogin0100Handler(ZEvent e)
        {
            ResPlayerLogin0100 oResPlayerLogin0100 = e.Target as ResPlayerLogin0100;

            Data.PlayerID = oResPlayerLogin0100.PlayerID;
            Data.GatewayIP = oResPlayerLogin0100.GatewayIP;
            Data.GatewayPort = oResPlayerLogin0100.GatewayPort;
            Data.LoginClient.OwnerID2 = oResPlayerLogin0100.PlayerID;
            Data.GatewayClient.OwnerID2 = oResPlayerLogin0100.PlayerID;
            Data.IDSignTime = oResPlayerLogin0100.PlayerIDSignTime;
            Data.IDSignCode = oResPlayerLogin0100.PlayerIDSignCode;

            switch (oResPlayerLogin0100.Result)
            {//0表示成功，1表示校验失败，2表示没有可用的网关，3表示未创建角色，4表示创建账号失败;
                case 0:
                    break;
                case 3:
                    SendReqCreateRole0100();
                    break;
                case 1:
                    CloseSocket();
                    Global.Main.Log("{0}校验失败，断开连接!", RobotKey);
                    lock (Global.ErrorRobotCtrlListLocker)
                    {
                        Global.ErrorRobotCtrlList.Add(this);
                    }
                    break;
                case 2:
                    CloseSocket();
                    Global.Main.Log("{0}没有可用的网关，断开连接!", RobotKey);
                    lock (Global.ErrorRobotCtrlListLocker)
                    {
                        Global.ErrorRobotCtrlList.Add(this);
                    }
                    break;
                case 4:
                    CloseSocket();
                    Global.Main.Log("{0}创建账号失败，断开连接!", RobotKey);
                    lock (Global.ErrorRobotCtrlListLocker)
                    {
                        Global.ErrorRobotCtrlList.Add(this);
                    }
                    break;
                default:
                    CloseSocket();
                    Global.Main.Log("{0}登陆未知错误，断开连接!", RobotKey);
                    lock (Global.ErrorRobotCtrlListLocker)
                    {
                        Global.ErrorRobotCtrlList.Add(this);
                    }
                    break;
            }
        }

        private void OnResCreateRole0100Handler(ZEvent e)
        {
            ResCreateRole0100 oResCreateRole0100 = e.Target as ResCreateRole0100;

            switch (oResCreateRole0100.Result)
            {//0表示成功，1表示校验失败，2表示角色已存在，3表示昵称已存在，4表示数据更新失败，5表示昵称记录失败
                case 0:
                    break;
                case 1:
                    Global.Main.Log("{0}校验失败,断开连接!", RobotKey);
                    CloseSocket();
                    break;
                case 2:
                    Global.Main.Log("{0}角色已存在,断开连接!", RobotKey);
                    CloseSocket();
                    break;
                case 3:
                    Global.Main.Log("{0}昵称已存在,重新创建角色!", RobotKey);
                    SendReqCreateRole0100();
                    break;
                case 4:
                    Global.Main.Log("{0}数据更新失败,断开连接!", RobotKey);
                    CloseSocket();
                    break;
                case 5:
                    Global.Main.Log("{0}昵称记录失败,断开连接!", RobotKey);
                    CloseSocket();
                    break;
                default:
                    Global.Main.Log("{0}创建角色未知错误,断开连接!", RobotKey);
                    CloseSocket();
                    break;
            }
        }

        private void OnResPlayerLogin0102Handler(ZEvent e)
        {
            ResPlayerLogin0102 oResPlayerLogin0102 = e.Target as ResPlayerLogin0102;

            Data.NickName = oResPlayerLogin0102.NickName;
            Data.GuildName = oResPlayerLogin0102.GuildName;
            Data.Career = oResPlayerLogin0102.Career;
            Data.Level = oResPlayerLogin0102.Level;
            Data.Sex = oResPlayerLogin0102.Sex;

            mPlaying = true;
            Data.LoginClient.Close();

            while (Data.GatewayClient.Connect(Data.GatewayIP, Data.GatewayPort) == false)
            {
                Thread.Sleep(SleepMillionSeconds);
            }

            SendReqEnterGame001();
            //else
            //{
            //    Global.Main.Log("{0}网关连接失败,断开连接!", RobotKey);
            //}
        }

        private void OnResEnterGame004Handler(ZEvent e)
        {
            ResEnterGame004 oResEnterGame004 = e.Target as ResEnterGame004;
            Data.Career = oResEnterGame004.Career;
            Data.Level = oResEnterGame004.Lv;
            Data.Sex = oResEnterGame004.Sex;
            Data.MapID = oResEnterGame004.MapID;
            Data.MapX = oResEnterGame004.MapX;
            Data.MapY = oResEnterGame004.MapY;
        }

        private void OnResEnterMap007Handler(ZEvent e)
        {
            ResEnterMap007 oResEnterMap007 = e.Target as ResEnterMap007;
            Data.MapID = oResEnterMap007.MapID;
            Data.MapX = oResEnterMap007.MapX;
            Data.MapY = oResEnterMap007.MapY;
            MapObjectDict.Clear();
            CurrentMapDeploy = MapDeployMgr.getMapDeploy((int)Data.MapID);
            if (CurrentMapDeploy != null)
            {
                CurrentMapAsset = MapAssetMgr.GetMapAsset(CurrentMapDeploy.assetID);
                if (CurrentMapAsset == null)
                {
                    Global.Main.Log("{0}找不到地图:{1}的相关资源，断开连接(MapAsset)!", RobotKey, Data.MapID);
                    CloseSocket();
                    return;
                }
            }
            else
            {
                Global.Main.Log("{0}找不到地图:{1}的相关资源，断开连接(MapDeploy)!", RobotKey, Data.MapID);
                CloseSocket();
                return;
            }

            Data.NPCTransfered = true;
            if (oResEnterMap007.Type == 5)
            {
                SendReqEnterMap008(ServerType.WorldServerType);
                if (CurrentMapAsset != null)
                {
                    Global.Main.Log("{0}登陆成功：{1}({2},{3})!", RobotKey, Data.MapID, Data.MapX, Data.MapY);
                    //Data.GatewayClient.mIsPkgCount = true;
                    OnLoginComplete();
                    return;
                }
            }
            
            Global.Main.Log("当前传送地图ID:" + Data.MapID);
        }

        private void OnResEnterMap009Handler(ZEvent e)
        {
            SendReqEnterMap008(ServerType.GatewayServerType);
        }

        private void OnResPlayerMoveHandler(ZEvent e)
        {
            ResPlayerMove oResPlayerMove = e.Target as ResPlayerMove;

            if(oResPlayerMove.ObjectID.PlatformID == Data.PlatformID 
                && oResPlayerMove.ObjectID.ServerID == Data.ServerID 
                && oResPlayerMove.ObjectID.ObjectID == Data.PlayerID)
            {
                Data.MapX = oResPlayerMove.TargetX;
                Data.MapY = oResPlayerMove.TargetY;
            }
        }

        private void OnResHeartBeatHandler(ZEvent e)
        {
            ResHeartBeat oResHeartBeat = e.Target as ResHeartBeat;
            int span = Environment.TickCount - (int)oResHeartBeat.SendMsgTime;
        }

        private void OnResAddMapObjectHandler(ZEvent e)
        {
            ResAddMapObject oResAddMapObject = e.Target as ResAddMapObject;
            MapObject oMapObject = new MapObject();
            oMapObject.ObjectID.PlatformID = oResAddMapObject.ObjectID.PlatformID;
            oMapObject.ObjectID.ServerID = oResAddMapObject.ObjectID.ServerID;
            oMapObject.ObjectID.ObjectID = oResAddMapObject.ObjectID.ObjectID;

            oMapObject.OwnerID.PlatformID = oResAddMapObject.OwnerID.PlatformID;
            oMapObject.OwnerID.ServerID = oResAddMapObject.OwnerID.ServerID;
            oMapObject.OwnerID.ObjectID = oResAddMapObject.OwnerID.ObjectID;

            oMapObject.Level = oResAddMapObject.Level;
            oMapObject.MapID = oResAddMapObject.MapID;
            oMapObject.MapX = oResAddMapObject.MapX;
            oMapObject.MapY = oResAddMapObject.MapY;
            oMapObject.Camp = oResAddMapObject.Camp;

            UInt64 Guid = RobotUtil.MakeServerObjectKey(oResAddMapObject.ObjectID);
            if (!MapObjectDict.ContainsKey(Guid))
            {
                MapObjectDict.Add(Guid, oMapObject);   
            }
            else
            {
                MapObjectDict[Guid] = oMapObject;
            }
        }

        private void OnResDelMapObjectHandler(ZEvent e)
        {
            ResDelMapObject oResDelMapObject = e.Target as ResDelMapObject;
            UInt64 Guid = RobotUtil.MakeServerObjectKey(oResDelMapObject.ObjectID);
            MapObjectDict.Remove(Guid);
        }      

        private void OnResPlayerDisconnectHandler(ZEvent e)
        {
            Console.WriteLine("PlayerDisconnect");
        }
    }
}
