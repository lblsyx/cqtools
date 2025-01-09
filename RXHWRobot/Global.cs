using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RXHWRobot.Robots;
using UnityLight.Internets;
using UnityLight;
using System.Threading;

namespace RXHWRobot
{
    public class Global
    {
        public static int RobotID = 0;
        public static ushort ServerID = 0;
        public static ushort PlatformID = 0;
        public static string PreAccount = string.Empty;
        public static int LoginPort = 0;
        public static string LoginIP = string.Empty;
        public static Callback<RobotCtrl> RobotCtrlStop;
        public static MainForm Main = null;

        public static Random random = null;

        public static Assembly CurrentAssembly = null;

        public static bool Started = false;
        public static RobotCtrl HeartBeatRobot = null;

        public static object RobotCtrlListLocker = new object();
        public static List<RobotCtrl> RobotCtrlList = new List<RobotCtrl>();

        public static object ErrorRobotCtrlListLocker = new object();
        public static List<RobotCtrl> ErrorRobotCtrlList = new List<RobotCtrl>();

        public static string letterList = "qwertyuiopasdfghjklzxcvbnm1234567890ZXCVBNMASDFGHJKLQWERTYUIOP";
        public static char[] letters = letterList.ToCharArray();
        public static void CreateRobot()
        {
            lock (Global.RobotCtrlListLocker)
            {
                RobotCtrl oRobotCtrl = new RobotCtrl();
                if (Global.RobotID == 0)
                {
                    oRobotCtrl.EnableHeartBeat = true;
                    Global.HeartBeatRobot = oRobotCtrl;
                }
                CRobotData oRobotData = oRobotCtrl.Data;
                oRobotData.PlatformID = PlatformID;
                oRobotData.ServerID = ServerID;
                oRobotData.Account = string.Format("{0}{1}", PreAccount, Global.RobotID);
                oRobotData.LoginSignTime = DateTime.Now.TotalSeconds();
                oRobotData.LoginSignKey = RobotConfig.Instance.RobotSignKey;
                oRobotData.LoginIP = LoginIP;
                oRobotData.LoginPort = LoginPort;
                oRobotData.AssignClientOwnerID();
                oRobotData.MakeLoginSignCode();
                oRobotCtrl.Start();
                oRobotCtrl.OnStoppedCallback = RobotCtrlStop;
                Global.RobotCtrlList.Add(oRobotCtrl);
                Global.RobotID++;
            }
        }

        public static int MaxNameNum = 0;
        public static bool ResourceLoaded = false;
        public static int[] RobotDirects = new int[] { (int)RobotDirect.Left, (int)RobotDirect.Right, (int)RobotDirect.Up, (int)RobotDirect.Down, (int)RobotDirect.LeftUp, (int)RobotDirect.LeftDown, (int)RobotDirect.RightUp, (int)RobotDirect.RightDown };

        private static int mSleepTime = 10;
        private static bool mRunning = false;
        private static Thread mRunTaskThread = null;

        public static void StartRunTask()
        {
            if (mRunning) return;
            mRunning = true;
            mSleepTime = (int)(1000 / RobotConfig.Instance.LoginSpeed);
            mRunTaskThread = new Thread(new ThreadStart(StartRunTaskImp));
            mRunTaskThread.IsBackground = true;
            mRunTaskThread.Start();
        }

        public static bool StopRunTask()
        {
            if (mRunning)
            {
                mRunning = false;

                while (mRunTaskThread.IsAlive)
                {
                    Thread.Sleep(100);
                }

                mRunTaskThread.Abort();
                mRunTaskThread = null;

                return true;
            }

            return false;
        }

        private static void StartRunTaskImp()
        {
            while (mRunning)
            {
                //mRunning = false;
                //CreateRobot();
                //Thread.Sleep(mSleepTime);
            }
        }

        //public static ProcessorFactory<RobotCtrl> RobotCtrlProcessorFactory = new ProcessorFactory<RobotCtrl>();
    }
}
