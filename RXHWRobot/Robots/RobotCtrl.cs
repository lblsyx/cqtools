using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;
using System.Threading;
using UnityLight.Internets;
using UnityLight.Events;
using RXHWRobot.Datas;

namespace RXHWRobot.Robots
{
    public partial class RobotCtrl : ZEventDispatcher<int, ZEvent>
    {
        /// <summary>
        /// 机器人控制
        /// </summary>
        public Random random;
        public bool LoginCompleted;
        public MapAsset CurrentMapAsset;
        public MapDeploy CurrentMapDeploy;
        public int SleepMillionSeconds = 10;
        public CRobotData Data = new CRobotData();
        public Callback<RobotCtrl> OnStoppedCallback;
        public Timer mTimer;
        public DateTimeOffset mOffset = new DateTimeOffset(DateTime.UtcNow);

        public string RobotKey
        {
            get { return string.Format("[{0}|{1}|{2}|{3}|{4}]", Data.NickName, Data.Account, Data.PlatformID, Data.ServerID, Data.PlayerID); }
        }

        public SortedDictionary<UInt64, MapObject> MapObjectDict = new SortedDictionary<UInt64, MapObject>();

        private bool mPlaying;
        private bool mUpdating;
        private Thread mThread;

        public RobotCtrl()
        {
            mPlaying = false;
            Data.LoginClient.Closed += new ClientHandler(LoginClient_Closed);
            Data.GatewayClient.Closed += new ClientHandler(GatewayClient_Closed);

            Initialize();
        }

        private void GatewayClient_Closed(TCPClient client)
        {
            Console.WriteLine("GatewayClient_Closed");
            mUpdating = false;
        }

        private void LoginClient_Closed(TCPClient client)
        {
            RobotClient oRobotClient = client as RobotClient;

            if (mPlaying == false)
            {
                mUpdating = false;
            }
        }

        public void Start()
        {
            if (mThread != null) return;
            mUpdating = true;
            mThread = new Thread(new ParameterizedThreadStart(Update));
            mThread.IsBackground = true;
            mThread.Start(this);
        }

        public void CloseSocket()
        {
            long Count = 0;
            foreach (var item in Data.GatewayClient.mPkgCountDic)
            {
                Count = Count + (long)item.Value;
                Console.WriteLine("[{0},{1}]", item.Key, item.Value);
            }
            Console.WriteLine("接收+发送数量{0}", Count);

            Data.LoginClient.Close();
            Data.GatewayClient.Close();
        }

        public void OnceTimer(TimerCallback callback, object state, int dueTime)
        {
            mTimer = new Timer(callback, state, dueTime, -1);
        }

        public void SettleRobotData()
        {
            if(Data.Level <= 10)
            {
                SendReqGMCommand("cs 5");
                SendReqGMCommand("lv 300");
                SendReqGMCommand("money 3000000");
                SettleSkill();
            }
        }

        public void SettleSkill()
        {
            switch (Data.Career)
            {
                case 2:
                    for (int i = 0; i < FaShiSkill.Length; i++)
                    {
                        SendReqGMCommand(string.Format("skill {0}", FaShiSkill[i]));
                    }
                    break;
                case 3:
                    for (int i = 0; i < DaoShiSkill.Length; i++)
                    {
                        SendReqGMCommand(string.Format("skill {0}", DaoShiSkill[i]));
                    }
                    break;
            }
        }

        public void MinPkgCount(object state)
        {
            Data.GatewayClient.mIsPkgCount = true;
            mTimer.Dispose();
        }

        public void TransferToMap(object state)
        {
            SendReqNPCTransferToMap(Data.TransferMap);
            mTimer.Dispose();

            OnceTimer(MinPkgCount, null, 60000);
        }

        public void OnLoginComplete()
        {
            LogoutTime = Environment.TickCount + 60000;
            LoginCompleted = true;
            SettleRobotData();
            OnceTimer(TransferToMap, null, 800);
        }

        public void OnThreadStop()
        {
            if (Global.HeartBeatRobot == this && Global.Started)
            {
                for (int i = 0; i < Global.RobotCtrlList.Count; i++)
                {
                    if (Global.RobotCtrlList[i].LoginCompleted)
                    {
                        Global.RobotCtrlList[i].EnableHeartBeat = true;
                        Global.Main.Log("机器人连接断开，切换发送心跳包机器人！{0}", RobotKey);
                        break;
                    }
                }
            }

            if (OnStoppedCallback != null)
            {
                OnStoppedCallback(this);
            }
        }

        public static void Update(object data)
        {
            RobotCtrl oRobotCtrl = data as RobotCtrl;
            CRobotData oRobotData = oRobotCtrl.Data;

            oRobotData.LoginClient.TargetID1 = (byte)ServerType.LoginServerType;
            oRobotData.LoginClient.TargetID2 = (byte)ServerType.PlayerClientType;
            oRobotData.GatewayClient.TargetID2 = (byte)ServerType.PlayerClientType;

            while (oRobotData.LoginClient.Connect(oRobotData.LoginIP, oRobotData.LoginPort) == false)
            {
                Thread.Sleep(oRobotCtrl.SleepMillionSeconds);
                //lock (Global.ErrorRobotCtrlListLocker)
                //{
                //    Global.ErrorRobotCtrlList.Add(oRobotCtrl);
                //}
                //return;
            }

            //发送登陆包
            oRobotCtrl.SendReqPlayerLogin0100();

            Packet[] packets = null;

            ZEvent oZEvent = new ZEvent();

            while (oRobotCtrl.mUpdating)
            {
                if (oRobotData.LoginClient != null)
                {
                    lock (oRobotData.LoginClient.PacketSyncRoot)
                    {
                        packets = oRobotData.LoginClient.PacketQueue.ToArray();
                        oRobotData.LoginClient.PacketQueue.Clear();
                    }

                    foreach (var item in packets)
                    {
                        oZEvent.Target = item;
                        oRobotCtrl.DispatchEvent(item.PacketID, oZEvent);
                        //Global.RobotCtrlProcessorFactory.Process(oRobotCtrl, item);
                    }
                }

                if (oRobotData.GatewayClient != null)
                {
                    lock (oRobotData.GatewayClient.PacketSyncRoot)
                    {
                        packets = oRobotData.GatewayClient.PacketQueue.ToArray();
                        oRobotData.GatewayClient.PacketQueue.Clear();
                    }

                    foreach (var item in packets)
                    {
                        oZEvent.Target = item;
                        oRobotCtrl.DispatchEvent(item.PacketID, oZEvent);
                        //Global.RobotCtrlProcessorFactory.Process(oRobotCtrl, item);
                    }
                }

                oRobotCtrl.CalcRobotAction();

                Thread.Sleep(oRobotCtrl.SleepMillionSeconds);
            }

            oRobotCtrl.LoginCompleted = false;

            oRobotCtrl.OnThreadStop();
        }
    }
}
