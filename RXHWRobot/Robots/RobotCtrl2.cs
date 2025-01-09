using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;
using UnityLight.Internets;
using System.Threading;

namespace RXHWRobot.Robots
{
    public enum RobotAction
    {
        None,
        Move,
        Attack,
        TotalAction
    }

    public enum ObjectType
    {
        NPC_PSID = 511,
        MON_PSID = 510,
        ITEM_PSID = 509,
        SKILL_PSID = 508,
        PET_PSID = 507,
        MARS_PSID = 506,
        HUMANMON_PSID = 505,
        ROBOT_PSID = 504,
        MIRROR_PSID = 503
    }

    public partial class RobotCtrl
    {
        /// <summary>
        /// 挂机(移动攻击)
        /// </summary>
        private const int ActionSpan = 1000;

        public bool EnableHeartBeat = false;
        private int NextSendHeartBeatTime = 0;

        private RobotDirect NextForwardMoveDir;

        public bool RecalcAction;
        public int NextActionTime;
        public RobotAction NextAction;
        public int LogoutTime;
        public void CalcRobotAction()
        {
            if(LoginCompleted == false) return;
            if(Data.NPCTransfered == false) return;
            if(LogoutTime > Environment.TickCount) return;

            CheckBeat();

            //RandomMake();

            //NextAction = (RobotAction)random.Next(1, (int)RobotAction.TotalAction);
            if (Environment.TickCount >= NextActionTime)
            {
                if (RobotConfig.Instance.EnableRandomFight)
                {
                    MapObject oMapObject = FindTargetAttack();
                    if(oMapObject == null)
                    {
                        RandomMove();
                    }
                    else
                    {
                        if ((Math.Abs(oMapObject.MapX - Data.MapX) < 2) && (Math.Abs(oMapObject.MapY - Data.MapY) < 2))
                        {
                            AttackMon(oMapObject);
                            NextActionTime = Environment.TickCount + ActionSpan;
                        }
                        else
                        {
                            ForwarMove(oMapObject.MapX, oMapObject.MapY);
                        }                             
                    }
                }
                else
                {
                    RandomMove();
                }
            }
        }

        public void CheckBeat()
        {
            if (EnableHeartBeat == false) return;

            if (Environment.TickCount >= NextSendHeartBeatTime)
            {
                SendReqHeartBeat();
                NextSendHeartBeatTime = Environment.TickCount + 1000;
            }
        }

        //随机移动
        private int NextMoveDir = 0;
        private uint TargetX = 0;
        private uint TargetY = 0;
        //public uint SendMovePacketCount = 0;
        private const int HVMoveInterval = 1400;
        private const int HMoveInterval = 1350;
        private const int VMoveInterval = 1180;
        private bool InSpecialRange(uint x, uint y)
        {
            return x >= RobotConfig.Instance.MinMapX && x <= RobotConfig.Instance.MaxMapX && y >= RobotConfig.Instance.MinMapY && y <= RobotConfig.Instance.MaxMapY;
        }
        public void RandomMove()
        {
            do
            {
                NextMoveDir = Global.RobotDirects[random.Next(Global.RobotDirects.Length)];
                RobotDirect dir = (RobotDirect)NextMoveDir;
                switch (dir)
                {
                    case RobotDirect.Left:
                        NextActionTime = Environment.TickCount + HMoveInterval;
                        TargetX = Data.MapX - 1;
                        TargetY = Data.MapY;
                        break;
                    case RobotDirect.Right:
                        NextActionTime = Environment.TickCount + HMoveInterval;
                        TargetX = Data.MapX + 1;
                        TargetY = Data.MapY;
                        break;
                    case RobotDirect.Up:
                        NextActionTime = Environment.TickCount + VMoveInterval;
                        TargetX = Data.MapX;
                        TargetY = Data.MapY - 1;
                        break;
                    case RobotDirect.Down:
                        NextActionTime = Environment.TickCount + VMoveInterval;
                        TargetX = Data.MapX;
                        TargetY = Data.MapY + 1;
                        break;
                    case RobotDirect.LeftUp:
                        NextActionTime = Environment.TickCount + HVMoveInterval;
                        TargetX = Data.MapX - 1;
                        TargetY = Data.MapY - 1;
                        break;
                    case RobotDirect.LeftDown:
                        NextActionTime = Environment.TickCount + HVMoveInterval;
                        TargetX = Data.MapX - 1;
                        TargetY = Data.MapY + 1;
                        break;
                    case RobotDirect.RightUp:
                        NextActionTime = Environment.TickCount + HVMoveInterval;
                        TargetX = Data.MapX + 1;
                        TargetY = Data.MapY - 1;
                        break;
                    case RobotDirect.RightDown:
                        NextActionTime = Environment.TickCount + HVMoveInterval;
                        TargetX = Data.MapX + 1;
                        TargetY = Data.MapY + 1;
                        break;
                }
            }
            while (CurrentMapAsset.hasCellFlag((int)TargetX, (int)TargetY, (int)GridFlag.BLOCK) || (InSpecialRange((uint)Data.MapX, (uint)Data.MapY) && InSpecialRange((uint)TargetX, (uint)TargetY) == false));

            Data.MapX = TargetX;
            Data.MapY = TargetY;
            SendReqPlayerMove();
        }


        //向前移动
        public void ForwarMove(uint targetX, uint targetY)
        {
            NextForwardMoveDir = RobotUtil.getDirect((int)Data.MapX, (int)Data.MapY, (int)targetX, (int)targetY);
            switch (NextForwardMoveDir)
            {
                case RobotDirect.Left:
                    NextActionTime = Environment.TickCount + HMoveInterval;
                    TargetX = Data.MapX - 1;
                    TargetY = Data.MapY;
                    break;
                case RobotDirect.Right:
                    NextActionTime = Environment.TickCount + HMoveInterval;
                    TargetX = Data.MapX + 1;
                    TargetY = Data.MapY;
                    break;
                case RobotDirect.Up:
                    NextActionTime = Environment.TickCount + VMoveInterval;
                    TargetX = Data.MapX;
                    TargetY = Data.MapY - 1;
                    break;
                case RobotDirect.Down:
                    NextActionTime = Environment.TickCount + VMoveInterval;
                    TargetX = Data.MapX;
                    TargetY = Data.MapY + 1;
                    break;
                case RobotDirect.LeftUp:
                    NextActionTime = Environment.TickCount + HVMoveInterval;
                    TargetX = Data.MapX - 1;
                    TargetY = Data.MapY - 1;
                    break;
                case RobotDirect.LeftDown:
                    NextActionTime = Environment.TickCount + HVMoveInterval;
                    TargetX = Data.MapX - 1;
                    TargetY = Data.MapY + 1;
                    break;
                case RobotDirect.RightUp:
                    NextActionTime = Environment.TickCount + HVMoveInterval;
                    TargetX = Data.MapX + 1;
                    TargetY = Data.MapY - 1;
                    break;
                case RobotDirect.RightDown:
                    NextActionTime = Environment.TickCount + HVMoveInterval;
                    TargetX = Data.MapX + 1;
                    TargetY = Data.MapY + 1;
                    break;
            }

            Data.MapX = TargetX;
            Data.MapY = TargetY;
            SendReqPlayerMove();
        }

        //找目标攻击
        public MapObject FindTargetAttack()
        {
            MapObject oMapObject = null;
            int Distance = 20;
            int CurDistance = 0;
            foreach (var obj in MapObjectDict)
            {
                CurDistance = Math.Max(Math.Abs((int)obj.Value.MapX - (int)Data.MapX), Math.Abs((int)obj.Value.MapY - (int)Data.MapY));
                if (CurDistance < Distance && RobotUtil.CheckObjectIsMon(obj.Value))
                {
                    Distance = CurDistance;
                    oMapObject = obj.Value;
                }
            }

            return oMapObject;
        }

        //玩家攻击
        private bool HasPet = false;
        private ushort[] FaShiSkill = new ushort[] { 20003, 21203, 21501, 20801, 21801 };
        private ushort[] FaShiSkillCD = new ushort[] { 1100, 1300, 10200, 1200, 18100 };
        private ushort[] DaoShiSkill = new ushort[] { 31303, 30511, 34803, 30603 };
        private ushort[] DaoShiSkillCD = new ushort[] { 1100, 1200, 1500, 500, 13000 };
        private Dictionary<uint, int> NextUseSkillTime = new Dictionary<uint, int>();
        public void AttackMon(MapObject oMapObject)
        {
            ReqPlayerAttack oReqPlayerAttack = new ReqPlayerAttack();
            oReqPlayerAttack.Direct = (byte)RobotUtil.getDirect((int)Data.MapX, (int)Data.MapY, (int)oMapObject.MapX, (int)oMapObject.MapY);
            oReqPlayerAttack.TargetX = (ushort)oMapObject.MapX;
            oReqPlayerAttack.TargetY = (ushort)oMapObject.MapY;
            oReqPlayerAttack.TargetObjectID = oMapObject.ObjectID;
            oReqPlayerAttack.AppendAttackSkillID = 0;

            int idx = 0;
            switch (Data.Career)
            {
                case 1://战士
                    oReqPlayerAttack.SkillID = 0;
                    NextActionTime = Environment.TickCount + 1100;
                    break;
                case 2://法师
                    int i = 0;
                    do
                    {
                        i += 1;
                        idx = random.Next(FaShiSkill.Length);
                        oReqPlayerAttack.SkillID = FaShiSkill[idx];
                        if (i >= 10)
                        {
                            oReqPlayerAttack.SkillID = 0;
                            break;
                        }
                    }
                    while (Environment.TickCount < NextUseSkillTime[oReqPlayerAttack.SkillID]);
                    NextUseSkillTime[oReqPlayerAttack.SkillID] = Environment.TickCount + FaShiSkillCD[idx];
                    NextActionTime = Environment.TickCount + 1500;
                    break;
                case 3://道士
                    int j = 0;
                    do
                    {
                        idx = random.Next(DaoShiSkill.Length);
                        oReqPlayerAttack.SkillID = DaoShiSkill[idx];
                        if (j >= 10)
                        {
                            oReqPlayerAttack.SkillID = 0;
                            break;
                        }
                    }
                    while (Environment.TickCount < NextUseSkillTime[oReqPlayerAttack.SkillID] || (HasPet && oReqPlayerAttack.SkillID == 24003));

                    if (oReqPlayerAttack.SkillID == 24003) HasPet = true;

                    NextUseSkillTime[oReqPlayerAttack.SkillID] = Environment.TickCount + DaoShiSkillCD[idx];
                    NextActionTime = Environment.TickCount + 1500;
                    break;
                default:
                    NextActionTime = Environment.TickCount + ActionSpan;
                    return;
            }

            Data.SendToMap(oReqPlayerAttack);
        }

        //IList<Itemtemplate> oItemtemplateList = Templates.Itemtemplates.FindAll();

        //Make道具
        private int NextMakeTime = 0;
        private int HasItemCount = 0;
        private int[] MakeItems = new int[] { 218, 103, 1203, 2084, 2099, 3074, 3301, 3264, 3401, 3800, 10327, 10328, 420, 286, 147, 122 };
        public void RandomMake()
        {
            if (RobotConfig.Instance.EnableRandomMake == false) return;

            if (Environment.TickCount > NextMakeTime)
            {
                NextMakeTime = Environment.TickCount + random.Next(RobotConfig.Instance.MinMakeInterval, RobotConfig.Instance.MaxMakeInterval);

                if (HasItemCount >= 40)
                {
                    HasItemCount = 0;
                    SendReqGMCommand("clear");
                }
                else
                {
                    int makeCount = random.Next(RobotConfig.Instance.MinMakeNum, RobotConfig.Instance.MaxMakeNum);
                    HasItemCount += makeCount;
                    for (int i = 0; i < makeCount; i++)
                    {
                        int itemTID = MakeItems[random.Next(MakeItems.Length)];
                        string str = string.Format("make {0} 1", itemTID);
                        SendReqGMCommand(str);
                    }
                }
            }
        }


    }
}
