/**
 * Created by Tool.
 */
using System;
using System.Collections.Generic;
using System.Text;
using UnityLight.Internets;
namespace RXHWRobot
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtocolPair : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key;
        /// <summary>
        /// 
        /// </summary>
        public int Value;

        public object Clone()
        {
            ProtocolPair st = new ProtocolPair();
            st.Key = Key;
            st.Value = Value;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(Key);
                oByteArray.WriteInt(Value);
            }
            else
            {
                Key = oByteArray.ReadInt();
                Value = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            Key = 0;
            Value = 0;

        }
    }

    /// <summary>
    /// 对象ID
    /// </summary>
    public class ObjectGuidInfo : IStruct
    {
        /// <summary>
        /// 平台ID
        /// </summary>
        public uint PlatformID;
        /// <summary>
        /// 区服ID
        /// </summary>
        public uint ServerID;
        /// <summary>
        /// 对象ID
        /// </summary>
        public uint ObjectID;

        public object Clone()
        {
            ObjectGuidInfo st = new ObjectGuidInfo();
            st.PlatformID = PlatformID;
            st.ServerID = ServerID;
            st.ObjectID = ObjectID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(PlatformID);
                oByteArray.WriteUInt(ServerID);
                oByteArray.WriteUInt(ObjectID);
            }
            else
            {
                PlatformID = oByteArray.ReadUInt();
                ServerID = oByteArray.ReadUInt();
                ObjectID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlatformID = 0;
            ServerID = 0;
            ObjectID = 0;

        }
    }

    /// <summary>
    /// 沙巴克胜利行会主要成员的信息
    /// </summary>
    public class ShabakGuildInfo : IStruct
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 衣服ID
        /// </summary>
        public uint ClothesID;
        /// <summary>
        /// 武器ID
        /// </summary>
        public uint WeaponID;
        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;
        /// <summary>
        /// 行会职位
        /// </summary>
        public uint GuildPosition;
        /// <summary>
        /// 
        /// </summary>
        public uint FashionClothesID;
        /// <summary>
        /// 
        /// </summary>
        public uint FashionWeaponID;

        public object Clone()
        {
            ShabakGuildInfo st = new ShabakGuildInfo();
            st.Name = Name;
            st.ClothesID = ClothesID;
            st.WeaponID = WeaponID;
            st.Sex = Sex;
            st.GuildPosition = GuildPosition;
            st.FashionClothesID = FashionClothesID;
            st.FashionWeaponID = FashionWeaponID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(Name);
                oByteArray.WriteUInt(ClothesID);
                oByteArray.WriteUInt(WeaponID);
                oByteArray.WriteByte(Sex);
                oByteArray.WriteUInt(GuildPosition);
                oByteArray.WriteUInt(FashionClothesID);
                oByteArray.WriteUInt(FashionWeaponID);
            }
            else
            {
                Name = oByteArray.ReadUTF();
                ClothesID = oByteArray.ReadUInt();
                WeaponID = oByteArray.ReadUInt();
                Sex = oByteArray.ReadByte();
                GuildPosition = oByteArray.ReadUInt();
                FashionClothesID = oByteArray.ReadUInt();
                FashionWeaponID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Name = string.Empty;
            ClothesID = 0;
            WeaponID = 0;
            Sex = 0;
            GuildPosition = 0;
            FashionClothesID = 0;
            FashionWeaponID = 0;

        }
    }

    /// <summary>
    /// 队伍信息
    /// </summary>
    public class GroupInfo : IStruct
    {
        /// <summary>
        /// 队长ID
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 队长名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 等级
        /// </summary>
        public uint Lv;
        /// <summary>
        /// 职业
        /// </summary>
        public byte Career;
        /// <summary>
        /// 队伍ID
        /// </summary>
        public uint GroupID;
        /// <summary>
        /// 队伍平台ID
        /// </summary>
        public uint GroupPlatformID;
        /// <summary>
        /// 队伍区服ID
        /// </summary>
        public uint GroupServerID;
        /// <summary>
        /// 当前人数
        /// </summary>
        public sbyte TeamNum;
        /// <summary>
        /// 队长公会名字
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 是否同意自动入队1是同意0不同意
        /// </summary>
        public bool IsAutoAgreeTeam;

        public object Clone()
        {
            GroupInfo st = new GroupInfo();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.NickName = NickName;
            st.Lv = Lv;
            st.Career = Career;
            st.GroupID = GroupID;
            st.GroupPlatformID = GroupPlatformID;
            st.GroupServerID = GroupServerID;
            st.TeamNum = TeamNum;
            st.GuildName = GuildName;
            st.IsAutoAgreeTeam = IsAutoAgreeTeam;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteByte(Career);
                oByteArray.WriteUInt(GroupID);
                oByteArray.WriteUInt(GroupPlatformID);
                oByteArray.WriteUInt(GroupServerID);
                oByteArray.WriteSByte(TeamNum);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteBoolean(IsAutoAgreeTeam);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                NickName = oByteArray.ReadUTF();
                Lv = oByteArray.ReadUInt();
                Career = oByteArray.ReadByte();
                GroupID = oByteArray.ReadUInt();
                GroupPlatformID = oByteArray.ReadUInt();
                GroupServerID = oByteArray.ReadUInt();
                TeamNum = oByteArray.ReadSByte();
                GuildName = oByteArray.ReadUTF();
                IsAutoAgreeTeam = oByteArray.ReadBoolean();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            NickName = string.Empty;
            Lv = 0;
            Career = 0;
            GroupID = 0;
            GroupPlatformID = 0;
            GroupServerID = 0;
            TeamNum = 0;
            GuildName = string.Empty;
            IsAutoAgreeTeam = false;

        }
    }

    /// <summary>
    /// 附近玩家信息
    /// </summary>
    public class NearPlayerInfo : IStruct
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 职业
        /// </summary>
        public byte Career;
        /// <summary>
        /// 等级
        /// </summary>
        public uint Lv;
        /// <summary>
        /// 帮会名字
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 是否在队伍中
        /// </summary>
        public bool TeamState;

        public object Clone()
        {
            NearPlayerInfo st = new NearPlayerInfo();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.NickName = NickName;
            st.Career = Career;
            st.Lv = Lv;
            st.GuildName = GuildName;
            st.TeamState = TeamState;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteByte(Career);
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteBoolean(TeamState);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                NickName = oByteArray.ReadUTF();
                Career = oByteArray.ReadByte();
                Lv = oByteArray.ReadUInt();
                GuildName = oByteArray.ReadUTF();
                TeamState = oByteArray.ReadBoolean();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            NickName = string.Empty;
            Career = 0;
            Lv = 0;
            GuildName = string.Empty;
            TeamState = false;

        }
    }

    /// <summary>
    /// 开服活动奖励数结构体
    /// </summary>
    public class OpenAwardState : IStruct
    {
        /// <summary>
        /// 活动TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 剩余奖励数 如果是-1 表示不限次数
        /// </summary>
        public int SurplusTimes;
        /// <summary>
        /// 活动类型
        /// </summary>
        public uint Type;
        /// <summary>
        /// 是否可以领取,0可以，1不可以，2已经领过
        /// </summary>
        public uint State;
        /// <summary>
        /// 字符串数据
        /// </summary>
        public string Str;

        public object Clone()
        {
            OpenAwardState st = new OpenAwardState();
            st.TID = TID;
            st.SurplusTimes = SurplusTimes;
            st.Type = Type;
            st.State = State;
            st.Str = Str;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteInt(SurplusTimes);
                oByteArray.WriteUInt(Type);
                oByteArray.WriteUInt(State);
                oByteArray.WriteUTF(Str);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                SurplusTimes = oByteArray.ReadInt();
                Type = oByteArray.ReadUInt();
                State = oByteArray.ReadUInt();
                Str = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            TID = 0;
            SurplusTimes = 0;
            Type = 0;
            State = 0;
            Str = string.Empty;

        }
    }

    /// <summary>
    /// 排行榜信息
    /// </summary>
    public class RankInfo : IStruct
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 性别
        /// </summary>
        public uint Sex;
        /// <summary>
        /// 职业
        /// </summary>
        public uint Career;
        /// <summary>
        /// 等级
        /// </summary>
        public uint Level;
        /// <summary>
        /// 公会名称
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;
        /// <summary>
        /// 当前排名
        /// </summary>
        public int Pos;
        /// <summary>
        /// 是否在线 true-在线，false -离线
        /// </summary>
        public bool Online;
        /// <summary>
        /// 排行榜Type,0人物等级，1物理攻击，2魔法攻击，3道术攻击，4翅膀等级，5品阶等级，6英雄等级，7斗笠等级，8传奇之路累计星星数
        /// </summary>
        public uint RankType;
        /// <summary>
        /// 排行属性数值
        /// </summary>
        public uint RankValue;
        /// <summary>
        /// 平台VIP特权TID
        /// </summary>
        public uint PlatfromVipTID;
        /// <summary>
        /// 排行第二属性值
        /// </summary>
        public uint SecondRankValue;
        /// <summary>
        /// 阵营
        /// </summary>
        public uint Camp;

        public object Clone()
        {
            RankInfo st = new RankInfo();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.Sex = Sex;
            st.Career = Career;
            st.Level = Level;
            st.GuildName = GuildName;
            st.NickName = NickName;
            st.Pos = Pos;
            st.Online = Online;
            st.RankType = RankType;
            st.RankValue = RankValue;
            st.PlatfromVipTID = PlatfromVipTID;
            st.SecondRankValue = SecondRankValue;
            st.Camp = Camp;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(Sex);
                oByteArray.WriteUInt(Career);
                oByteArray.WriteUInt(Level);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteInt(Pos);
                oByteArray.WriteBoolean(Online);
                oByteArray.WriteUInt(RankType);
                oByteArray.WriteUInt(RankValue);
                oByteArray.WriteUInt(PlatfromVipTID);
                oByteArray.WriteUInt(SecondRankValue);
                oByteArray.WriteUInt(Camp);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                Sex = oByteArray.ReadUInt();
                Career = oByteArray.ReadUInt();
                Level = oByteArray.ReadUInt();
                GuildName = oByteArray.ReadUTF();
                NickName = oByteArray.ReadUTF();
                Pos = oByteArray.ReadInt();
                Online = oByteArray.ReadBoolean();
                RankType = oByteArray.ReadUInt();
                RankValue = oByteArray.ReadUInt();
                PlatfromVipTID = oByteArray.ReadUInt();
                SecondRankValue = oByteArray.ReadUInt();
                Camp = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            Sex = 0;
            Career = 0;
            Level = 0;
            GuildName = string.Empty;
            NickName = string.Empty;
            Pos = 0;
            Online = false;
            RankType = 0;
            RankValue = 0;
            PlatfromVipTID = 0;
            SecondRankValue = 0;
            Camp = 0;

        }
    }

    /// <summary>
    /// 属性结构
    /// </summary>
    public class Property : IStruct
    {
        /// <summary>
        /// 属性枚举
        /// </summary>
        public uint PropertyType;
        /// <summary>
        /// 属性数值
        /// </summary>
        public long Value;

        public object Clone()
        {
            Property st = new Property();
            st.PropertyType = PropertyType;
            st.Value = Value;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(PropertyType);
                oByteArray.WriteInt64(Value);
            }
            else
            {
                PropertyType = oByteArray.ReadUInt();
                Value = oByteArray.ReadInt64();
            }
        }


        public void Reset()
        {
            PropertyType = 0;
            Value = 0;

        }
    }

    /// <summary>
    /// 物品TID和数量的结构，用于奖励
    /// </summary>
    public class ItemInfo : IStruct
    {
        /// <summary>
        /// 物品TID
        /// </summary>
        public int TmpID;
        /// <summary>
        /// 物品数量
        /// </summary>
        public int Num;

        public object Clone()
        {
            ItemInfo st = new ItemInfo();
            st.TmpID = TmpID;
            st.Num = Num;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(TmpID);
                oByteArray.WriteInt(Num);
            }
            else
            {
                TmpID = oByteArray.ReadInt();
                Num = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            TmpID = 0;
            Num = 0;

        }
    }

    /// <summary>
    /// 传奇之路信息更新
    /// </summary>
    public class LegendRoadInfo : IStruct
    {
        /// <summary>
        /// 传奇之路TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 剩余次数
        /// </summary>
        public uint CurTimes;
        /// <summary>
        /// 第一次完成奖励是否已经领取
        /// </summary>
        public uint FirstFinishAwardSign;
        /// <summary>
        /// 完成星级
        /// </summary>
        public uint FinishLevel;
        /// <summary>
        /// AddTimes
        /// </summary>
        public uint AddTimes;

        public object Clone()
        {
            LegendRoadInfo st = new LegendRoadInfo();
            st.TID = TID;
            st.CurTimes = CurTimes;
            st.FirstFinishAwardSign = FirstFinishAwardSign;
            st.FinishLevel = FinishLevel;
            st.AddTimes = AddTimes;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(CurTimes);
                oByteArray.WriteUInt(FirstFinishAwardSign);
                oByteArray.WriteUInt(FinishLevel);
                oByteArray.WriteUInt(AddTimes);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                CurTimes = oByteArray.ReadUInt();
                FirstFinishAwardSign = oByteArray.ReadUInt();
                FinishLevel = oByteArray.ReadUInt();
                AddTimes = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            CurTimes = 0;
            FirstFinishAwardSign = 0;
            FinishLevel = 0;
            AddTimes = 0;

        }
    }

    /// <summary>
    /// 传奇之路奖励信息
    /// </summary>
    public class STLegendRoadAwardInfo : IStruct
    {
        /// <summary>
        /// 传奇之路TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 物品列表
        /// </summary>
        public List<ItemInfo> ItemInfoVec = new List<ItemInfo>();
        /// <summary>
        /// 增加经验值
        /// </summary>
        public uint AddExp;
        /// <summary>
        /// 增加传奇之魂
        /// </summary>
        public uint AddSoulValue;

        public object Clone()
        {
            STLegendRoadAwardInfo st = new STLegendRoadAwardInfo();
            st.TID = TID;
            foreach (ItemInfo item in ItemInfoVec)
            {
                st.ItemInfoVec.Add(item.Clone() as ItemInfo);
            }
            st.AddExp = AddExp;
            st.AddSoulValue = AddSoulValue;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUShort((ushort)ItemInfoVec.Count);
                for (int i = 0; i < ItemInfoVec.Count; i++)
                {
                    ItemInfoVec[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteUInt(AddExp);
                oByteArray.WriteUInt(AddSoulValue);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                int ItemInfoVecCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < ItemInfoVecCount; i++)
                {
                    ItemInfo obj = new ItemInfo();
                    obj.Serializtion(oByteArray, bSerialize);
                    ItemInfoVec.Add(obj);
                }
                AddExp = oByteArray.ReadUInt();
                AddSoulValue = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            ItemInfoVec.Clear();
            AddExp = 0;
            AddSoulValue = 0;

        }
    }

    /// <summary>
    /// 运营活动
    /// </summary>
    public class YYRankInfo : IStruct
    {
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 玩家排行数值
        /// </summary>
        public long Value;

        public object Clone()
        {
            YYRankInfo st = new YYRankInfo();
            st.NickName = NickName;
            st.Value = Value;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteInt64(Value);
            }
            else
            {
                NickName = oByteArray.ReadUTF();
                Value = oByteArray.ReadInt64();
            }
        }


        public void Reset()
        {
            NickName = string.Empty;
            Value = 0;

        }
    }

    /// <summary>
    /// 跨服战排行榜信息带uint64
    /// </summary>
    public class ServiceWarRankInfo : IStruct
    {
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;
        /// <summary>
        /// 玩家排行数值
        /// </summary>
        public uint Value;
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerGuid = new ObjectGuidInfo();
        /// <summary>
        /// 所属阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 数值改变时间
        /// </summary>
        public long ValueChangeTime;

        public object Clone()
        {
            ServiceWarRankInfo st = new ServiceWarRankInfo();
            st.NickName = NickName;
            st.Value = Value;
            st.PlayerGuid = PlayerGuid.Clone() as ObjectGuidInfo;
            st.Camp = Camp;
            st.ValueChangeTime = ValueChangeTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(Value);
                PlayerGuid.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteInt64(ValueChangeTime);
            }
            else
            {
                NickName = oByteArray.ReadUTF();
                Value = oByteArray.ReadUInt();
                PlayerGuid.Serializtion(oByteArray, bSerialize);
                Camp = oByteArray.ReadUInt();
                ValueChangeTime = oByteArray.ReadInt64();
            }
        }


        public void Reset()
        {
            NickName = string.Empty;
            Value = 0;
            PlayerGuid = default(ObjectGuidInfo);
            Camp = 0;
            ValueChangeTime = 0;

        }
    }

    /// <summary>
    /// 服务器IP和端口信息
    /// </summary>
    public class ServiceWarServerInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ServerIP;
        /// <summary>
        /// 
        /// </summary>
        public uint ServerPort;
        /// <summary>
        /// 区服id
        /// </summary>
        public uint ServerID;

        public object Clone()
        {
            ServiceWarServerInfo st = new ServiceWarServerInfo();
            st.ServerIP = ServerIP;
            st.ServerPort = ServerPort;
            st.ServerID = ServerID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(ServerIP);
                oByteArray.WriteUInt(ServerPort);
                oByteArray.WriteUInt(ServerID);
            }
            else
            {
                ServerIP = oByteArray.ReadUTF();
                ServerPort = oByteArray.ReadUInt();
                ServerID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            ServerIP = string.Empty;
            ServerPort = 0;
            ServerID = 0;

        }
    }

    /// <summary>
    /// 社交关系结构体
    /// </summary>
    public class SocietyRelation : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 关系
        /// </summary>
        public ushort Relation;
        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;
        /// <summary>
        /// 职业
        /// </summary>
        public byte Career;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 等级
        /// </summary>
        public uint Lv;
        /// <summary>
        /// vip等级
        /// </summary>
        public byte VIPLv;
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature;
        /// <summary>
        /// 关系值（结义就是结义值，情侣就是亲密值，仇人就是仇恨值）
        /// </summary>
        public uint RelationValue;
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline;
        /// <summary>
        /// 平台VIP特权
        /// </summary>
        public uint PlatformVIP;
        /// <summary>
        /// 帮会的名称
        /// </summary>
        public string GuildName;

        public object Clone()
        {
            SocietyRelation st = new SocietyRelation();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.Relation = Relation;
            st.Sex = Sex;
            st.Career = Career;
            st.Name = Name;
            st.Lv = Lv;
            st.VIPLv = VIPLv;
            st.Signature = Signature;
            st.RelationValue = RelationValue;
            st.IsOnline = IsOnline;
            st.PlatformVIP = PlatformVIP;
            st.GuildName = GuildName;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUShort(Relation);
                oByteArray.WriteByte(Sex);
                oByteArray.WriteByte(Career);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteByte(VIPLv);
                oByteArray.WriteUTF(Signature);
                oByteArray.WriteUInt(RelationValue);
                oByteArray.WriteBoolean(IsOnline);
                oByteArray.WriteUInt(PlatformVIP);
                oByteArray.WriteUTF(GuildName);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                Relation = oByteArray.ReadUShort();
                Sex = oByteArray.ReadByte();
                Career = oByteArray.ReadByte();
                Name = oByteArray.ReadUTF();
                Lv = oByteArray.ReadUInt();
                VIPLv = oByteArray.ReadByte();
                Signature = oByteArray.ReadUTF();
                RelationValue = oByteArray.ReadUInt();
                IsOnline = oByteArray.ReadBoolean();
                PlatformVIP = oByteArray.ReadUInt();
                GuildName = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            Relation = 0;
            Sex = 0;
            Career = 0;
            Name = string.Empty;
            Lv = 0;
            VIPLv = 0;
            Signature = string.Empty;
            RelationValue = 0;
            IsOnline = false;
            PlatformVIP = 0;
            GuildName = string.Empty;

        }
    }

    /// <summary>
    /// boss状态
    /// </summary>
    public class MonsterState : IStruct
    {
        /// <summary>
        /// 复活时间戳
        /// </summary>
        public uint RefreshTime;
        /// <summary>
        /// 是否活着
        /// </summary>
        public bool Live;
        /// <summary>
        /// 怪物ID
        /// </summary>
        public uint MonsterID;

        public object Clone()
        {
            MonsterState st = new MonsterState();
            st.RefreshTime = RefreshTime;
            st.Live = Live;
            st.MonsterID = MonsterID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(RefreshTime);
                oByteArray.WriteBoolean(Live);
                oByteArray.WriteUInt(MonsterID);
            }
            else
            {
                RefreshTime = oByteArray.ReadUInt();
                Live = oByteArray.ReadBoolean();
                MonsterID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            RefreshTime = 0;
            Live = false;
            MonsterID = 0;

        }
    }

    /// <summary>
    /// 队伍成员信息
    /// </summary>
    public class TeamMemberInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 
        /// </summary>
        public uint Lv;
        /// <summary>
        /// 
        /// </summary>
        public string NickName;
        /// <summary>
        /// 
        /// </summary>
        public uint PlatformVIP;

        public object Clone()
        {
            TeamMemberInfo st = new TeamMemberInfo();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.Lv = Lv;
            st.NickName = NickName;
            st.PlatformVIP = PlatformVIP;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(PlatformVIP);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                Lv = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
                PlatformVIP = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            Lv = 0;
            NickName = string.Empty;
            PlatformVIP = 0;

        }
    }

    /// <summary>
    /// 登陆信息
    /// </summary>
    public class LoginInfo : IStruct
    {
        /// <summary>
        /// 平台ID
        /// </summary>
        public uint PlatformID;
        /// <summary>
        /// 区服ID
        /// </summary>
        public uint ServerID;
        /// <summary>
        /// 客户端类型;0表示网页，1表示微端
        /// </summary>
        public byte ClientType;
        /// <summary>
        /// 登陆时间(即签名时间)
        /// </summary>
        public uint LoginTime;
        /// <summary>
        /// 玩家帐号
        /// </summary>
        public string Account;
        /// <summary>
        /// 签名校验串
        /// </summary>
        public string SignCode;
        /// <summary>
        /// 防沉迷0是未验证1是验证过的
        /// </summary>
        public byte IndulgeFlag;
        /// <summary>
        /// 后台登录校验串
        /// </summary>
        public string BackstageLogin;
        /// <summary>
        /// 平台vip等级
        /// </summary>
        public uint[] PlatformVipLv = new uint[4];
        /// <summary>
        /// YY平台信息(弃用)
        /// </summary>
        public uint[] YYPlatformInfo = new uint[4];
        /// <summary>
        /// 平台登陆入口id,用于区分不同域名指向相同平台
        /// </summary>
        public uint PlatformEntrance;
        /// <summary>
        /// 玩家IP
        /// </summary>
        public string PlayerIP;
        /// <summary>
        /// 登录额外信息
        /// </summary>
        public string LoginExt;

        public object Clone()
        {
            LoginInfo st = new LoginInfo();
            st.PlatformID = PlatformID;
            st.ServerID = ServerID;
            st.ClientType = ClientType;
            st.LoginTime = LoginTime;
            st.Account = Account;
            st.SignCode = SignCode;
            st.IndulgeFlag = IndulgeFlag;
            st.BackstageLogin = BackstageLogin;
            for (int i = 0; i < 4; i++)
            {
                st.PlatformVipLv[i] = PlatformVipLv[i];
            }
            for (int i = 0; i < 4; i++)
            {
                st.YYPlatformInfo[i] = YYPlatformInfo[i];
            }
            st.PlatformEntrance = PlatformEntrance;
            st.PlayerIP = PlayerIP;
            st.LoginExt = LoginExt;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(PlatformID);
                oByteArray.WriteUInt(ServerID);
                oByteArray.WriteByte(ClientType);
                oByteArray.WriteUInt(LoginTime);
                oByteArray.WriteUTF(Account);
                oByteArray.WriteUTF(SignCode);
                oByteArray.WriteByte(IndulgeFlag);
                oByteArray.WriteUTF(BackstageLogin);
                for (int i = 0; i < 4; i++)
                {
                    oByteArray.WriteUInt(PlatformVipLv[i]);
                }
                for (int i = 0; i < 4; i++)
                {
                    oByteArray.WriteUInt(YYPlatformInfo[i]);
                }
                oByteArray.WriteUInt(PlatformEntrance);
                oByteArray.WriteUTF(PlayerIP);
                oByteArray.WriteUTF(LoginExt);
            }
            else
            {
                PlatformID = oByteArray.ReadUInt();
                ServerID = oByteArray.ReadUInt();
                ClientType = oByteArray.ReadByte();
                LoginTime = oByteArray.ReadUInt();
                Account = oByteArray.ReadUTF();
                SignCode = oByteArray.ReadUTF();
                IndulgeFlag = oByteArray.ReadByte();
                BackstageLogin = oByteArray.ReadUTF();
                for (int i = 0; i < 4; i++)
                {
                    PlatformVipLv[i] = oByteArray.ReadUInt();
                }
                for (int i = 0; i < 4; i++)
                {
                    YYPlatformInfo[i] = oByteArray.ReadUInt();
                }
                PlatformEntrance = oByteArray.ReadUInt();
                PlayerIP = oByteArray.ReadUTF();
                LoginExt = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            PlatformID = 0;
            ServerID = 0;
            ClientType = 0;
            LoginTime = 0;
            Account = string.Empty;
            SignCode = string.Empty;
            IndulgeFlag = 0;
            BackstageLogin = string.Empty;
            Array.Clear(PlatformVipLv, 0, 4);
            Array.Clear(YYPlatformInfo, 0, 4);
            PlatformEntrance = 0;
            PlayerIP = string.Empty;
            LoginExt = string.Empty;

        }
    }

    /// <summary>
    /// 特殊效果信息
    /// </summary>
    public class EffectsInfo : IStruct
    {
        /// <summary>
        /// 特效ID
        /// </summary>
        public uint EffectID;
        /// <summary>
        /// 触发概率
        /// </summary>
        public uint Probability;
        /// <summary>
        /// 时间戳
        /// </summary>
        public uint NextCanUseTime;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public uint CDTime;
        /// <summary>
        /// 模板合成数据1
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 数据2
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 数据3
        /// </summary>
        public uint Data3;
        /// <summary>
        /// 数据4
        /// </summary>
        public uint Data4;
        /// <summary>
        /// 效果参数(施法者攻击力,累积值等)
        /// </summary>
        public int[] BuffValue = new int[4];
        /// <summary>
        /// 过期时间戳
        /// </summary>
        public int ExpireTime;
        /// <summary>
        /// 已叠加次数
        /// </summary>
        public uint StackNum;

        public object Clone()
        {
            EffectsInfo st = new EffectsInfo();
            st.EffectID = EffectID;
            st.Probability = Probability;
            st.NextCanUseTime = NextCanUseTime;
            st.CDTime = CDTime;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.Data3 = Data3;
            st.Data4 = Data4;
            for (int i = 0; i < 4; i++)
            {
                st.BuffValue[i] = BuffValue[i];
            }
            st.ExpireTime = ExpireTime;
            st.StackNum = StackNum;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(EffectID);
                oByteArray.WriteUInt(Probability);
                oByteArray.WriteUInt(NextCanUseTime);
                oByteArray.WriteUInt(CDTime);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(Data3);
                oByteArray.WriteUInt(Data4);
                for (int i = 0; i < 4; i++)
                {
                    oByteArray.WriteInt(BuffValue[i]);
                }
                oByteArray.WriteInt(ExpireTime);
                oByteArray.WriteUInt(StackNum);
            }
            else
            {
                EffectID = oByteArray.ReadUInt();
                Probability = oByteArray.ReadUInt();
                NextCanUseTime = oByteArray.ReadUInt();
                CDTime = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                Data3 = oByteArray.ReadUInt();
                Data4 = oByteArray.ReadUInt();
                for (int i = 0; i < 4; i++)
                {
                    BuffValue[i] = oByteArray.ReadInt();
                }
                ExpireTime = oByteArray.ReadInt();
                StackNum = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            EffectID = 0;
            Probability = 0;
            NextCanUseTime = 0;
            CDTime = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            Data4 = 0;
            Array.Clear(BuffValue, 0, 4);
            ExpireTime = 0;
            StackNum = 0;

        }
    }

    /// <summary>
    /// 行会成员信息
    /// </summary>
    public class GuildmemberInfo : IStruct
    {
        /// <summary>
        /// 等级
        /// </summary>
        public uint Lv;
        /// <summary>
        /// vip等级
        /// </summary>
        public uint VipLv;
        /// <summary>
        /// 名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 职业
        /// </summary>
        public uint Career;
        /// <summary>
        /// 下线时间0表示在线
        /// </summary>
        public uint OfflineTime;
        /// <summary>
        /// 3个ID
        /// </summary>
        public ObjectGuidInfo ObjectID = new ObjectGuidInfo();
        /// <summary>
        /// 贡献值
        /// </summary>
        public uint DonateValue;
        /// <summary>
        /// 行会职位
        /// </summary>
        public byte Position;
        /// <summary>
        /// 最大攻击力
        /// </summary>
        public uint MaxAtk;
        /// <summary>
        /// 平台特权
        /// </summary>
        public uint PlatformVIP;
        /// <summary>
        /// 性别
        /// </summary>
        public uint Sex;
        /// <summary>
        /// 周领奖信息
        /// </summary>
        public string AwardInfo;
        /// <summary>
        /// 境界等级
        /// </summary>
        public uint StateLv;
        /// <summary>
        /// 战斗力
        /// </summary>
        public uint ComatEffectiveness;

        public object Clone()
        {
            GuildmemberInfo st = new GuildmemberInfo();
            st.Lv = Lv;
            st.VipLv = VipLv;
            st.NickName = NickName;
            st.Career = Career;
            st.OfflineTime = OfflineTime;
            st.ObjectID = ObjectID.Clone() as ObjectGuidInfo;
            st.DonateValue = DonateValue;
            st.Position = Position;
            st.MaxAtk = MaxAtk;
            st.PlatformVIP = PlatformVIP;
            st.Sex = Sex;
            st.AwardInfo = AwardInfo;
            st.StateLv = StateLv;
            st.ComatEffectiveness = ComatEffectiveness;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteUInt(VipLv);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(Career);
                oByteArray.WriteUInt(OfflineTime);
                ObjectID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(DonateValue);
                oByteArray.WriteByte(Position);
                oByteArray.WriteUInt(MaxAtk);
                oByteArray.WriteUInt(PlatformVIP);
                oByteArray.WriteUInt(Sex);
                oByteArray.WriteUTF(AwardInfo);
                oByteArray.WriteUInt(StateLv);
                oByteArray.WriteUInt(ComatEffectiveness);
            }
            else
            {
                Lv = oByteArray.ReadUInt();
                VipLv = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
                Career = oByteArray.ReadUInt();
                OfflineTime = oByteArray.ReadUInt();
                ObjectID.Serializtion(oByteArray, bSerialize);
                DonateValue = oByteArray.ReadUInt();
                Position = oByteArray.ReadByte();
                MaxAtk = oByteArray.ReadUInt();
                PlatformVIP = oByteArray.ReadUInt();
                Sex = oByteArray.ReadUInt();
                AwardInfo = oByteArray.ReadUTF();
                StateLv = oByteArray.ReadUInt();
                ComatEffectiveness = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Lv = 0;
            VipLv = 0;
            NickName = string.Empty;
            Career = 0;
            OfflineTime = 0;
            ObjectID = default(ObjectGuidInfo);
            DonateValue = 0;
            Position = 0;
            MaxAtk = 0;
            PlatformVIP = 0;
            Sex = 0;
            AwardInfo = string.Empty;
            StateLv = 0;
            ComatEffectiveness = 0;

        }
    }

    /// <summary>
    /// 行会列表元素信息
    /// </summary>
    public class GuildBaseInfo : IStruct
    {
        /// <summary>
        /// 三个ID
        /// </summary>
        public ObjectGuidInfo GuildGuid = new ObjectGuidInfo();
        /// <summary>
        /// 聚义堂等级
        /// </summary>
        public byte ButylGitoLv;
        /// <summary>
        /// 行会人数
        /// </summary>
        public int GuildMemberNum;
        /// <summary>
        /// 行会名字
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 会长名字
        /// </summary>
        public string LeaderName;
        /// <summary>
        /// 是否已申请
        /// </summary>
        public bool IsApply;
        /// <summary>
        /// 1-允许随时加入 2-需要批准加入 3-暂不收人
        /// </summary>
        public byte GuildJoinType;
        /// <summary>
        /// 行会在线人数
        /// </summary>
        public byte OnLineNumber;
        /// <summary>
        /// 行会GuildIDStr
        /// </summary>
        public string GuildIDStr;
        /// <summary>
        /// 物品兑换境界限制是否开启
        /// </summary>
        public byte StateSet;
        /// <summary>
        /// 物品兑换职业限制是否开启
        /// </summary>
        public byte CareerSet;
        /// <summary>
        /// 行会祭坛等级
        /// </summary>
        public int AltarLv;

        public object Clone()
        {
            GuildBaseInfo st = new GuildBaseInfo();
            st.GuildGuid = GuildGuid.Clone() as ObjectGuidInfo;
            st.ButylGitoLv = ButylGitoLv;
            st.GuildMemberNum = GuildMemberNum;
            st.GuildName = GuildName;
            st.LeaderName = LeaderName;
            st.IsApply = IsApply;
            st.GuildJoinType = GuildJoinType;
            st.OnLineNumber = OnLineNumber;
            st.GuildIDStr = GuildIDStr;
            st.StateSet = StateSet;
            st.CareerSet = CareerSet;
            st.AltarLv = AltarLv;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                GuildGuid.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteByte(ButylGitoLv);
                oByteArray.WriteInt(GuildMemberNum);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteUTF(LeaderName);
                oByteArray.WriteBoolean(IsApply);
                oByteArray.WriteByte(GuildJoinType);
                oByteArray.WriteByte(OnLineNumber);
                oByteArray.WriteUTF(GuildIDStr);
                oByteArray.WriteByte(StateSet);
                oByteArray.WriteByte(CareerSet);
                oByteArray.WriteInt(AltarLv);
            }
            else
            {
                GuildGuid.Serializtion(oByteArray, bSerialize);
                ButylGitoLv = oByteArray.ReadByte();
                GuildMemberNum = oByteArray.ReadInt();
                GuildName = oByteArray.ReadUTF();
                LeaderName = oByteArray.ReadUTF();
                IsApply = oByteArray.ReadBoolean();
                GuildJoinType = oByteArray.ReadByte();
                OnLineNumber = oByteArray.ReadByte();
                GuildIDStr = oByteArray.ReadUTF();
                StateSet = oByteArray.ReadByte();
                CareerSet = oByteArray.ReadByte();
                AltarLv = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            GuildGuid = default(ObjectGuidInfo);
            ButylGitoLv = 0;
            GuildMemberNum = 0;
            GuildName = string.Empty;
            LeaderName = string.Empty;
            IsApply = false;
            GuildJoinType = 0;
            OnLineNumber = 0;
            GuildIDStr = string.Empty;
            StateSet = 0;
            CareerSet = 0;
            AltarLv = 0;

        }
    }

    /// <summary>
    /// 行会日志信息
    /// </summary>
    public class GuildLogInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint CreateTime;
        /// <summary>
        /// 
        /// </summary>
        public byte LogType;
        /// <summary>
        /// 
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 
        /// </summary>
        public uint Data3;
        /// <summary>
        /// 
        /// </summary>
        public string String1;
        /// <summary>
        /// 
        /// </summary>
        public string String2;
        /// <summary>
        /// 
        /// </summary>
        public string String3;

        public object Clone()
        {
            GuildLogInfo st = new GuildLogInfo();
            st.CreateTime = CreateTime;
            st.LogType = LogType;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.Data3 = Data3;
            st.String1 = String1;
            st.String2 = String2;
            st.String3 = String3;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(CreateTime);
                oByteArray.WriteByte(LogType);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(Data3);
                oByteArray.WriteUTF(String1);
                oByteArray.WriteUTF(String2);
                oByteArray.WriteUTF(String3);
            }
            else
            {
                CreateTime = oByteArray.ReadUInt();
                LogType = oByteArray.ReadByte();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                Data3 = oByteArray.ReadUInt();
                String1 = oByteArray.ReadUTF();
                String2 = oByteArray.ReadUTF();
                String3 = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            CreateTime = 0;
            LogType = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            String1 = string.Empty;
            String2 = string.Empty;
            String3 = string.Empty;

        }
    }

    /// <summary>
    /// 红包结构
    /// </summary>
    public class GuildRedBagInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint RedBagID;
        /// <summary>
        /// 
        /// </summary>
        public string OwnerName;
        /// <summary>
        /// 元宝数量（元宝红包有效）
        /// </summary>
        public uint IngotNum;
        /// <summary>
        /// 
        /// </summary>
        public byte RedBagNum;
        /// <summary>
        /// 
        /// </summary>
        public byte IsGetAll;
        /// <summary>
        /// 红包类型 0:元宝红包 1:道具红包
        /// </summary>
        public uint RedBagType;
        /// <summary>
        /// 道具红包配置id
        /// </summary>
        public uint RedBagTmplID;

        public object Clone()
        {
            GuildRedBagInfo st = new GuildRedBagInfo();
            st.RedBagID = RedBagID;
            st.OwnerName = OwnerName;
            st.IngotNum = IngotNum;
            st.RedBagNum = RedBagNum;
            st.IsGetAll = IsGetAll;
            st.RedBagType = RedBagType;
            st.RedBagTmplID = RedBagTmplID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(RedBagID);
                oByteArray.WriteUTF(OwnerName);
                oByteArray.WriteUInt(IngotNum);
                oByteArray.WriteByte(RedBagNum);
                oByteArray.WriteByte(IsGetAll);
                oByteArray.WriteUInt(RedBagType);
                oByteArray.WriteUInt(RedBagTmplID);
            }
            else
            {
                RedBagID = oByteArray.ReadUInt();
                OwnerName = oByteArray.ReadUTF();
                IngotNum = oByteArray.ReadUInt();
                RedBagNum = oByteArray.ReadByte();
                IsGetAll = oByteArray.ReadByte();
                RedBagType = oByteArray.ReadUInt();
                RedBagTmplID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            RedBagID = 0;
            OwnerName = string.Empty;
            IngotNum = 0;
            RedBagNum = 0;
            IsGetAll = 0;
            RedBagType = 0;
            RedBagTmplID = 0;

        }
    }

    /// <summary>
    /// 组装活动子活动信息
    /// </summary>
    public class AssembleActivityInfo : IStruct
    {
        /// <summary>
        /// 活动TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 已经领奖的信息
        /// </summary>
        public string Info;
        /// <summary>
        /// 其他信息
        /// </summary>
        public string Info2;
        /// <summary>
        /// 活动数据1
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 活动数据2
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 活动数据3
        /// </summary>
        public uint Data3;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 玩家Guid
        /// </summary>
        public ulong Guid;
        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;
        /// <summary>
        /// 职业
        /// </summary>
        public byte Career;
        /// <summary>
        /// 活动数据4
        /// </summary>
        public string Data4;
        /// <summary>
        /// 活动数据5
        /// </summary>
        public string Data5;

        public object Clone()
        {
            AssembleActivityInfo st = new AssembleActivityInfo();
            st.TID = TID;
            st.Info = Info;
            st.Info2 = Info2;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.Data3 = Data3;
            st.NickName = NickName;
            st.Guid = Guid;
            st.Sex = Sex;
            st.Career = Career;
            st.Data4 = Data4;
            st.Data5 = Data5;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUTF(Info);
                oByteArray.WriteUTF(Info2);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(Data3);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt64(Guid);
                oByteArray.WriteByte(Sex);
                oByteArray.WriteByte(Career);
                oByteArray.WriteUTF(Data4);
                oByteArray.WriteUTF(Data5);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                Info = oByteArray.ReadUTF();
                Info2 = oByteArray.ReadUTF();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                Data3 = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
                Guid = oByteArray.ReadUInt64();
                Sex = oByteArray.ReadByte();
                Career = oByteArray.ReadByte();
                Data4 = oByteArray.ReadUTF();
                Data5 = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            TID = 0;
            Info = string.Empty;
            Info2 = string.Empty;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            NickName = string.Empty;
            Guid = 0;
            Sex = 0;
            Career = 0;
            Data4 = string.Empty;
            Data5 = string.Empty;

        }
    }

    /// <summary>
    /// 活动日志结构
    /// </summary>
    public class AssembleActivityLog : IStruct
    {
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 物品信息
        /// </summary>
        public string ItemInfo;
        /// <summary>
        /// 日志时间
        /// </summary>
        public uint Time;
        /// <summary>
        /// 数据1
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 0全服1个人
        /// </summary>
        public byte Type;

        public object Clone()
        {
            AssembleActivityLog st = new AssembleActivityLog();
            st.PlayerName = PlayerName;
            st.ItemInfo = ItemInfo;
            st.Time = Time;
            st.Data1 = Data1;
            st.Type = Type;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(PlayerName);
                oByteArray.WriteUTF(ItemInfo);
                oByteArray.WriteUInt(Time);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteByte(Type);
            }
            else
            {
                PlayerName = oByteArray.ReadUTF();
                ItemInfo = oByteArray.ReadUTF();
                Time = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadUInt();
                Type = oByteArray.ReadByte();
            }
        }


        public void Reset()
        {
            PlayerName = string.Empty;
            ItemInfo = string.Empty;
            Time = 0;
            Data1 = 0;
            Type = 0;

        }
    }

    /// <summary>
    /// 战斗属性
    /// </summary>
    public class CombatAttribute : IStruct
    {
        /// <summary>
        /// 生命
        /// </summary>
        public long MaxHP;
        /// <summary>
        /// 魔法
        /// </summary>
        public long MaxMP;
        /// <summary>
        /// 神盾
        /// </summary>
        public long Energy;
        /// <summary>
        /// 物攻下
        /// </summary>
        public long MinPhysicAtk;
        /// <summary>
        /// 物攻上
        /// </summary>
        public long MaxPhysicAtk;
        /// <summary>
        /// 道攻下
        /// </summary>
        public long MinMagicAtk;
        /// <summary>
        /// 道攻上
        /// </summary>
        public long MaxMagicAtk;
        /// <summary>
        /// 法攻下
        /// </summary>
        public long MinTaoistAtk;
        /// <summary>
        /// 法攻上
        /// </summary>
        public long MaxTaoistAtk;
        /// <summary>
        /// 物防下
        /// </summary>
        public long MinPhysicDef;
        /// <summary>
        /// 物防上
        /// </summary>
        public long MaxPhysicDef;
        /// <summary>
        /// 法防下
        /// </summary>
        public long MinMagicDef;
        /// <summary>
        /// 法防上
        /// </summary>
        public long MaxMagicDef;
        /// <summary>
        /// 准确
        /// </summary>
        public long Hits;
        /// <summary>
        /// 闪避
        /// </summary>
        public long Dodge;
        /// <summary>
        /// 暴击
        /// </summary>
        public long CritRate;
        /// <summary>
        /// 暴击力
        /// </summary>
        public long CritForce;
        /// <summary>
        /// 抗暴
        /// </summary>
        public long DisCrit;
        /// <summary>
        /// 幸运
        /// </summary>
        public long Lucky;
        /// <summary>
        /// 神圣伤害
        /// </summary>
        public long HolyDamage;
        /// <summary>
        /// 真实伤害抵抗
        /// </summary>
        public long RealDiscount;
        /// <summary>
        /// 伤害加成
        /// </summary>
        public long AddHurt;
        /// <summary>
        /// 伤害减免
        /// </summary>
        public long ReduceHurt;
        /// <summary>
        /// 对怪增伤
        /// </summary>
        public long AddMonHurt;
        /// <summary>
        /// 对怪减免
        /// </summary>
        public long DisMonHurt;
        /// <summary>
        /// 对战士增伤
        /// </summary>
        public long DamageToSoldier;
        /// <summary>
        /// 对战士减伤
        /// </summary>
        public long ResistSoldier;
        /// <summary>
        /// 对法师增伤
        /// </summary>
        public long DamageToMage;
        /// <summary>
        /// 对法师减伤
        /// </summary>
        public long ResistMage;
        /// <summary>
        /// 对道士增伤
        /// </summary>
        public long DamageToTaoist;
        /// <summary>
        /// 对道士减免
        /// </summary>
        public long ResistTaoist;
        /// <summary>
        /// 生命回复
        /// </summary>
        public long LifeRecove;
        /// <summary>
        /// 魔法回复
        /// </summary>
        public long MagicRecove;
        /// <summary>
        /// 战斗力
        /// </summary>
        public long ComatEffectiveness;
        /// <summary>
        /// 当前血量
        /// </summary>
        public long CurHP;
        /// <summary>
        /// 当前蓝量
        /// </summary>
        public long CurMP;
        /// <summary>
        /// 当前内力
        /// </summary>
        public long CurSP;
        /// <summary>
        /// 内功回复
        /// </summary>
        public long EnergyRecove;
        /// <summary>
        /// 神盾承伤万分比
        /// </summary>
        public long EnergyPercent;
        /// <summary>
        /// 反击伤害
        /// </summary>
        public long ReturnHurt;
        /// <summary>
        /// 万分比反伤
        /// </summary>
        public long ThornsNum;
        /// <summary>
        /// 诅咒值
        /// </summary>
        public long Curse;
        /// <summary>
        /// 威名
        /// </summary>
        public long Fame;
        /// <summary>
        /// 威名增加伤害
        /// </summary>
        public long FameAddHurt;
        /// <summary>
        /// 暴击力抵抗
        /// </summary>
        public long DisCritForce;
        /// <summary>
        /// 移动速度
        /// </summary>
        public long MoveSpeed;
        /// <summary>
        /// 经验加成万分比
        /// </summary>
        public long ExtraAddExp;
        /// <summary>
        /// 增加对玩家伤害万分比
        /// </summary>
        public long AddPlayerHurt;
        /// <summary>
        /// 减少受到玩家伤害万分比
        /// </summary>
        public long ReducePlayerHurt;
        /// <summary>
        /// 吸血概率万分比
        /// </summary>
        public long VampireRate;
        /// <summary>
        /// 吸血值
        /// </summary>
        public long Vampire;
        /// <summary>
        /// 异常状态抗性
        /// </summary>
        public long Resist;
        /// <summary>
        /// 心魔攻击
        /// </summary>
        public long InnerDemonsAtk;
        /// <summary>
        /// 心魔防御
        /// </summary>
        public long InnerDemonsDef;
        /// <summary>
        /// 心魔万分比攻击
        /// </summary>
        public long InnerDemonsAtkPer;
        /// <summary>
        /// 心魔万分比防御
        /// </summary>
        public long InnerDemonsDefPer;
        /// <summary>
        /// 真实伤害
        /// </summary>
        public long RealDamage;
        /// <summary>
        /// 攻速
        /// </summary>
        public long AttackSpeed;
        /// <summary>
        /// 诱惑等级
        /// </summary>
        public long ConfuseLv;
        /// <summary>
        /// 反伤
        /// </summary>
        public long DmgCounter;
        /// <summary>
        /// 反伤抵抗
        /// </summary>
        public long DmgCounterReduce;
        /// <summary>
        /// 药品回复效率（万分比）
        /// </summary>
        public long DrugsEffect;
        /// <summary>
        /// 官印等级
        /// </summary>
        public long OfficialSealLv;
        /// <summary>
        /// 官印压制，减伤万分比
        /// </summary>
        public long OfficialSealReduceHurt;
        /// <summary>
        /// 掉落加成
        /// </summary>
        public long BoosDiaoLuo;
        /// <summary>
        /// 基础攻击伤害万分比加成
        /// </summary>
        public long AddBaseHurt;
        /// <summary>
        /// 基础攻击伤害万分比减免
        /// </summary>
        public long ReduceBaseHurt;
        /// <summary>
        /// 技能附加伤害万分比加成
        /// </summary>
        public long AddSkillHurt;
        /// <summary>
        /// 技能附加伤害万分比减免
        /// </summary>
        public long ReduceSkillHurt;
        /// <summary>
        /// 暴击伤害万分比加成
        /// </summary>
        public long AddCritHurt;
        /// <summary>
        /// 暴击伤害万分比减免
        /// </summary>
        public long ReduceCritHurt;
        /// <summary>
        /// 神圣伤害万分比加成
        /// </summary>
        public long AddHolyHurt;
        /// <summary>
        /// 基础暴击伤害加成系数
        /// </summary>
        public long CritHurtPct;
        /// <summary>
        /// 初始极品加成
        /// </summary>
        public long OriginalBaptize;
        /// <summary>
        /// 熔炼收益加成
        /// </summary>
        public long SmelterIncome;
        /// <summary>
        /// 致命伤害触发概率
        /// </summary>
        public long AccurateTriggerRate;
        /// <summary>
        /// 致命伤害附加伤害万分比
        /// </summary>
        public long AccurateHurtPct;
        /// <summary>
        /// 致命伤害万分比加成
        /// </summary>
        public long AddAccurateHurtRate;
        /// <summary>
        /// 致命伤害万分比减免
        /// </summary>
        public long ReduceAccurateHurtRate;
        /// <summary>
        /// buff触发概率加成
        /// </summary>
        public List<ProtocolPair> AddBuffTriggerRate = new List<ProtocolPair>();
        /// <summary>
        /// 对boss增伤
        /// </summary>
        public long AddBossHurtValue;
        /// <summary>
        /// 对怪致命触发概率
        /// </summary>
        public long DeadlyToMonProb;
        /// <summary>
        /// 对怪致命倍数
        /// </summary>
        public long DeadlyToMonMultiple;
        /// <summary>
        /// 无视防御万分比
        /// </summary>
        public long IgnoreDefend;
        /// <summary>
        /// 吸血抗性
        /// </summary>
        public long DisVampire;
        /// <summary>
        /// 控制状态抗性
        /// </summary>
        public long DisControl;
        /// <summary>
        /// 吸血效果减免
        /// </summary>
        public long ReduceVampirePct;
        /// <summary>
        /// 角色附加等级
        /// </summary>
        public long RoleAddLv ;
        /// <summary>
        /// 冰元素伤害
        /// </summary>
        public long IceHurt;
        /// <summary>
        /// 火元素伤害
        /// </summary>
        public long FireHurt;
        /// <summary>
        /// 雷元素伤害
        /// </summary>
        public long ThunderHurt;
        /// <summary>
        /// 冰元素伤害抵抗
        /// </summary>
        public long IceHurtReduce;
        /// <summary>
        /// 火元素伤害抵抗
        /// </summary>
        public long FireHurtReduce;
        /// <summary>
        /// 雷元素伤害抵抗
        /// </summary>
        public long ThunderHurtReduce;
        /// <summary>
        /// 足迹等级
        /// </summary>
        public long FootMarkLv;
        /// <summary>
        /// 复活封印概率
        /// </summary>
        public long DisRebirth;

        public object Clone()
        {
            CombatAttribute st = new CombatAttribute();
            st.MaxHP = MaxHP;
            st.MaxMP = MaxMP;
            st.Energy = Energy;
            st.MinPhysicAtk = MinPhysicAtk;
            st.MaxPhysicAtk = MaxPhysicAtk;
            st.MinMagicAtk = MinMagicAtk;
            st.MaxMagicAtk = MaxMagicAtk;
            st.MinTaoistAtk = MinTaoistAtk;
            st.MaxTaoistAtk = MaxTaoistAtk;
            st.MinPhysicDef = MinPhysicDef;
            st.MaxPhysicDef = MaxPhysicDef;
            st.MinMagicDef = MinMagicDef;
            st.MaxMagicDef = MaxMagicDef;
            st.Hits = Hits;
            st.Dodge = Dodge;
            st.CritRate = CritRate;
            st.CritForce = CritForce;
            st.DisCrit = DisCrit;
            st.Lucky = Lucky;
            st.HolyDamage = HolyDamage;
            st.RealDiscount = RealDiscount;
            st.AddHurt = AddHurt;
            st.ReduceHurt = ReduceHurt;
            st.AddMonHurt = AddMonHurt;
            st.DisMonHurt = DisMonHurt;
            st.DamageToSoldier = DamageToSoldier;
            st.ResistSoldier = ResistSoldier;
            st.DamageToMage = DamageToMage;
            st.ResistMage = ResistMage;
            st.DamageToTaoist = DamageToTaoist;
            st.ResistTaoist = ResistTaoist;
            st.LifeRecove = LifeRecove;
            st.MagicRecove = MagicRecove;
            st.ComatEffectiveness = ComatEffectiveness;
            st.CurHP = CurHP;
            st.CurMP = CurMP;
            st.CurSP = CurSP;
            st.EnergyRecove = EnergyRecove;
            st.EnergyPercent = EnergyPercent;
            st.ReturnHurt = ReturnHurt;
            st.ThornsNum = ThornsNum;
            st.Curse = Curse;
            st.Fame = Fame;
            st.FameAddHurt = FameAddHurt;
            st.DisCritForce = DisCritForce;
            st.MoveSpeed = MoveSpeed;
            st.ExtraAddExp = ExtraAddExp;
            st.AddPlayerHurt = AddPlayerHurt;
            st.ReducePlayerHurt = ReducePlayerHurt;
            st.VampireRate = VampireRate;
            st.Vampire = Vampire;
            st.Resist = Resist;
            st.InnerDemonsAtk = InnerDemonsAtk;
            st.InnerDemonsDef = InnerDemonsDef;
            st.InnerDemonsAtkPer = InnerDemonsAtkPer;
            st.InnerDemonsDefPer = InnerDemonsDefPer;
            st.RealDamage = RealDamage;
            st.AttackSpeed = AttackSpeed;
            st.ConfuseLv = ConfuseLv;
            st.DmgCounter = DmgCounter;
            st.DmgCounterReduce = DmgCounterReduce;
            st.DrugsEffect = DrugsEffect;
            st.OfficialSealLv = OfficialSealLv;
            st.OfficialSealReduceHurt = OfficialSealReduceHurt;
            st.BoosDiaoLuo = BoosDiaoLuo;
            st.AddBaseHurt = AddBaseHurt;
            st.ReduceBaseHurt = ReduceBaseHurt;
            st.AddSkillHurt = AddSkillHurt;
            st.ReduceSkillHurt = ReduceSkillHurt;
            st.AddCritHurt = AddCritHurt;
            st.ReduceCritHurt = ReduceCritHurt;
            st.AddHolyHurt = AddHolyHurt;
            st.CritHurtPct = CritHurtPct;
            st.OriginalBaptize = OriginalBaptize;
            st.SmelterIncome = SmelterIncome;
            st.AccurateTriggerRate = AccurateTriggerRate;
            st.AccurateHurtPct = AccurateHurtPct;
            st.AddAccurateHurtRate = AddAccurateHurtRate;
            st.ReduceAccurateHurtRate = ReduceAccurateHurtRate;
            foreach (ProtocolPair item in AddBuffTriggerRate)
            {
                st.AddBuffTriggerRate.Add(item.Clone() as ProtocolPair);
            }
            st.AddBossHurtValue = AddBossHurtValue;
            st.DeadlyToMonProb = DeadlyToMonProb;
            st.DeadlyToMonMultiple = DeadlyToMonMultiple;
            st.IgnoreDefend = IgnoreDefend;
            st.DisVampire = DisVampire;
            st.DisControl = DisControl;
            st.ReduceVampirePct = ReduceVampirePct;
            st.RoleAddLv  = RoleAddLv ;
            st.IceHurt = IceHurt;
            st.FireHurt = FireHurt;
            st.ThunderHurt = ThunderHurt;
            st.IceHurtReduce = IceHurtReduce;
            st.FireHurtReduce = FireHurtReduce;
            st.ThunderHurtReduce = ThunderHurtReduce;
            st.FootMarkLv = FootMarkLv;
            st.DisRebirth = DisRebirth;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt64(MaxHP);
                oByteArray.WriteInt64(MaxMP);
                oByteArray.WriteInt64(Energy);
                oByteArray.WriteInt64(MinPhysicAtk);
                oByteArray.WriteInt64(MaxPhysicAtk);
                oByteArray.WriteInt64(MinMagicAtk);
                oByteArray.WriteInt64(MaxMagicAtk);
                oByteArray.WriteInt64(MinTaoistAtk);
                oByteArray.WriteInt64(MaxTaoistAtk);
                oByteArray.WriteInt64(MinPhysicDef);
                oByteArray.WriteInt64(MaxPhysicDef);
                oByteArray.WriteInt64(MinMagicDef);
                oByteArray.WriteInt64(MaxMagicDef);
                oByteArray.WriteInt64(Hits);
                oByteArray.WriteInt64(Dodge);
                oByteArray.WriteInt64(CritRate);
                oByteArray.WriteInt64(CritForce);
                oByteArray.WriteInt64(DisCrit);
                oByteArray.WriteInt64(Lucky);
                oByteArray.WriteInt64(HolyDamage);
                oByteArray.WriteInt64(RealDiscount);
                oByteArray.WriteInt64(AddHurt);
                oByteArray.WriteInt64(ReduceHurt);
                oByteArray.WriteInt64(AddMonHurt);
                oByteArray.WriteInt64(DisMonHurt);
                oByteArray.WriteInt64(DamageToSoldier);
                oByteArray.WriteInt64(ResistSoldier);
                oByteArray.WriteInt64(DamageToMage);
                oByteArray.WriteInt64(ResistMage);
                oByteArray.WriteInt64(DamageToTaoist);
                oByteArray.WriteInt64(ResistTaoist);
                oByteArray.WriteInt64(LifeRecove);
                oByteArray.WriteInt64(MagicRecove);
                oByteArray.WriteInt64(ComatEffectiveness);
                oByteArray.WriteInt64(CurHP);
                oByteArray.WriteInt64(CurMP);
                oByteArray.WriteInt64(CurSP);
                oByteArray.WriteInt64(EnergyRecove);
                oByteArray.WriteInt64(EnergyPercent);
                oByteArray.WriteInt64(ReturnHurt);
                oByteArray.WriteInt64(ThornsNum);
                oByteArray.WriteInt64(Curse);
                oByteArray.WriteInt64(Fame);
                oByteArray.WriteInt64(FameAddHurt);
                oByteArray.WriteInt64(DisCritForce);
                oByteArray.WriteInt64(MoveSpeed);
                oByteArray.WriteInt64(ExtraAddExp);
                oByteArray.WriteInt64(AddPlayerHurt);
                oByteArray.WriteInt64(ReducePlayerHurt);
                oByteArray.WriteInt64(VampireRate);
                oByteArray.WriteInt64(Vampire);
                oByteArray.WriteInt64(Resist);
                oByteArray.WriteInt64(InnerDemonsAtk);
                oByteArray.WriteInt64(InnerDemonsDef);
                oByteArray.WriteInt64(InnerDemonsAtkPer);
                oByteArray.WriteInt64(InnerDemonsDefPer);
                oByteArray.WriteInt64(RealDamage);
                oByteArray.WriteInt64(AttackSpeed);
                oByteArray.WriteInt64(ConfuseLv);
                oByteArray.WriteInt64(DmgCounter);
                oByteArray.WriteInt64(DmgCounterReduce);
                oByteArray.WriteInt64(DrugsEffect);
                oByteArray.WriteInt64(OfficialSealLv);
                oByteArray.WriteInt64(OfficialSealReduceHurt);
                oByteArray.WriteInt64(BoosDiaoLuo);
                oByteArray.WriteInt64(AddBaseHurt);
                oByteArray.WriteInt64(ReduceBaseHurt);
                oByteArray.WriteInt64(AddSkillHurt);
                oByteArray.WriteInt64(ReduceSkillHurt);
                oByteArray.WriteInt64(AddCritHurt);
                oByteArray.WriteInt64(ReduceCritHurt);
                oByteArray.WriteInt64(AddHolyHurt);
                oByteArray.WriteInt64(CritHurtPct);
                oByteArray.WriteInt64(OriginalBaptize);
                oByteArray.WriteInt64(SmelterIncome);
                oByteArray.WriteInt64(AccurateTriggerRate);
                oByteArray.WriteInt64(AccurateHurtPct);
                oByteArray.WriteInt64(AddAccurateHurtRate);
                oByteArray.WriteInt64(ReduceAccurateHurtRate);
                oByteArray.WriteUShort((ushort)AddBuffTriggerRate.Count);
                for (int i = 0; i < AddBuffTriggerRate.Count; i++)
                {
                    AddBuffTriggerRate[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteInt64(AddBossHurtValue);
                oByteArray.WriteInt64(DeadlyToMonProb);
                oByteArray.WriteInt64(DeadlyToMonMultiple);
                oByteArray.WriteInt64(IgnoreDefend);
                oByteArray.WriteInt64(DisVampire);
                oByteArray.WriteInt64(DisControl);
                oByteArray.WriteInt64(ReduceVampirePct);
                oByteArray.WriteInt64(RoleAddLv );
                oByteArray.WriteInt64(IceHurt);
                oByteArray.WriteInt64(FireHurt);
                oByteArray.WriteInt64(ThunderHurt);
                oByteArray.WriteInt64(IceHurtReduce);
                oByteArray.WriteInt64(FireHurtReduce);
                oByteArray.WriteInt64(ThunderHurtReduce);
                oByteArray.WriteInt64(FootMarkLv);
                oByteArray.WriteInt64(DisRebirth);
            }
            else
            {
                MaxHP = oByteArray.ReadInt64();
                MaxMP = oByteArray.ReadInt64();
                Energy = oByteArray.ReadInt64();
                MinPhysicAtk = oByteArray.ReadInt64();
                MaxPhysicAtk = oByteArray.ReadInt64();
                MinMagicAtk = oByteArray.ReadInt64();
                MaxMagicAtk = oByteArray.ReadInt64();
                MinTaoistAtk = oByteArray.ReadInt64();
                MaxTaoistAtk = oByteArray.ReadInt64();
                MinPhysicDef = oByteArray.ReadInt64();
                MaxPhysicDef = oByteArray.ReadInt64();
                MinMagicDef = oByteArray.ReadInt64();
                MaxMagicDef = oByteArray.ReadInt64();
                Hits = oByteArray.ReadInt64();
                Dodge = oByteArray.ReadInt64();
                CritRate = oByteArray.ReadInt64();
                CritForce = oByteArray.ReadInt64();
                DisCrit = oByteArray.ReadInt64();
                Lucky = oByteArray.ReadInt64();
                HolyDamage = oByteArray.ReadInt64();
                RealDiscount = oByteArray.ReadInt64();
                AddHurt = oByteArray.ReadInt64();
                ReduceHurt = oByteArray.ReadInt64();
                AddMonHurt = oByteArray.ReadInt64();
                DisMonHurt = oByteArray.ReadInt64();
                DamageToSoldier = oByteArray.ReadInt64();
                ResistSoldier = oByteArray.ReadInt64();
                DamageToMage = oByteArray.ReadInt64();
                ResistMage = oByteArray.ReadInt64();
                DamageToTaoist = oByteArray.ReadInt64();
                ResistTaoist = oByteArray.ReadInt64();
                LifeRecove = oByteArray.ReadInt64();
                MagicRecove = oByteArray.ReadInt64();
                ComatEffectiveness = oByteArray.ReadInt64();
                CurHP = oByteArray.ReadInt64();
                CurMP = oByteArray.ReadInt64();
                CurSP = oByteArray.ReadInt64();
                EnergyRecove = oByteArray.ReadInt64();
                EnergyPercent = oByteArray.ReadInt64();
                ReturnHurt = oByteArray.ReadInt64();
                ThornsNum = oByteArray.ReadInt64();
                Curse = oByteArray.ReadInt64();
                Fame = oByteArray.ReadInt64();
                FameAddHurt = oByteArray.ReadInt64();
                DisCritForce = oByteArray.ReadInt64();
                MoveSpeed = oByteArray.ReadInt64();
                ExtraAddExp = oByteArray.ReadInt64();
                AddPlayerHurt = oByteArray.ReadInt64();
                ReducePlayerHurt = oByteArray.ReadInt64();
                VampireRate = oByteArray.ReadInt64();
                Vampire = oByteArray.ReadInt64();
                Resist = oByteArray.ReadInt64();
                InnerDemonsAtk = oByteArray.ReadInt64();
                InnerDemonsDef = oByteArray.ReadInt64();
                InnerDemonsAtkPer = oByteArray.ReadInt64();
                InnerDemonsDefPer = oByteArray.ReadInt64();
                RealDamage = oByteArray.ReadInt64();
                AttackSpeed = oByteArray.ReadInt64();
                ConfuseLv = oByteArray.ReadInt64();
                DmgCounter = oByteArray.ReadInt64();
                DmgCounterReduce = oByteArray.ReadInt64();
                DrugsEffect = oByteArray.ReadInt64();
                OfficialSealLv = oByteArray.ReadInt64();
                OfficialSealReduceHurt = oByteArray.ReadInt64();
                BoosDiaoLuo = oByteArray.ReadInt64();
                AddBaseHurt = oByteArray.ReadInt64();
                ReduceBaseHurt = oByteArray.ReadInt64();
                AddSkillHurt = oByteArray.ReadInt64();
                ReduceSkillHurt = oByteArray.ReadInt64();
                AddCritHurt = oByteArray.ReadInt64();
                ReduceCritHurt = oByteArray.ReadInt64();
                AddHolyHurt = oByteArray.ReadInt64();
                CritHurtPct = oByteArray.ReadInt64();
                OriginalBaptize = oByteArray.ReadInt64();
                SmelterIncome = oByteArray.ReadInt64();
                AccurateTriggerRate = oByteArray.ReadInt64();
                AccurateHurtPct = oByteArray.ReadInt64();
                AddAccurateHurtRate = oByteArray.ReadInt64();
                ReduceAccurateHurtRate = oByteArray.ReadInt64();
                int AddBuffTriggerRateCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AddBuffTriggerRateCount; i++)
                {
                    ProtocolPair obj = new ProtocolPair();
                    obj.Serializtion(oByteArray, bSerialize);
                    AddBuffTriggerRate.Add(obj);
                }
                AddBossHurtValue = oByteArray.ReadInt64();
                DeadlyToMonProb = oByteArray.ReadInt64();
                DeadlyToMonMultiple = oByteArray.ReadInt64();
                IgnoreDefend = oByteArray.ReadInt64();
                DisVampire = oByteArray.ReadInt64();
                DisControl = oByteArray.ReadInt64();
                ReduceVampirePct = oByteArray.ReadInt64();
                RoleAddLv  = oByteArray.ReadInt64();
                IceHurt = oByteArray.ReadInt64();
                FireHurt = oByteArray.ReadInt64();
                ThunderHurt = oByteArray.ReadInt64();
                IceHurtReduce = oByteArray.ReadInt64();
                FireHurtReduce = oByteArray.ReadInt64();
                ThunderHurtReduce = oByteArray.ReadInt64();
                FootMarkLv = oByteArray.ReadInt64();
                DisRebirth = oByteArray.ReadInt64();
            }
        }


        public void Reset()
        {
            MaxHP = 0;
            MaxMP = 0;
            Energy = 0;
            MinPhysicAtk = 0;
            MaxPhysicAtk = 0;
            MinMagicAtk = 0;
            MaxMagicAtk = 0;
            MinTaoistAtk = 0;
            MaxTaoistAtk = 0;
            MinPhysicDef = 0;
            MaxPhysicDef = 0;
            MinMagicDef = 0;
            MaxMagicDef = 0;
            Hits = 0;
            Dodge = 0;
            CritRate = 0;
            CritForce = 0;
            DisCrit = 0;
            Lucky = 0;
            HolyDamage = 0;
            RealDiscount = 0;
            AddHurt = 0;
            ReduceHurt = 0;
            AddMonHurt = 0;
            DisMonHurt = 0;
            DamageToSoldier = 0;
            ResistSoldier = 0;
            DamageToMage = 0;
            ResistMage = 0;
            DamageToTaoist = 0;
            ResistTaoist = 0;
            LifeRecove = 0;
            MagicRecove = 0;
            ComatEffectiveness = 0;
            CurHP = 0;
            CurMP = 0;
            CurSP = 0;
            EnergyRecove = 0;
            EnergyPercent = 0;
            ReturnHurt = 0;
            ThornsNum = 0;
            Curse = 0;
            Fame = 0;
            FameAddHurt = 0;
            DisCritForce = 0;
            MoveSpeed = 0;
            ExtraAddExp = 0;
            AddPlayerHurt = 0;
            ReducePlayerHurt = 0;
            VampireRate = 0;
            Vampire = 0;
            Resist = 0;
            InnerDemonsAtk = 0;
            InnerDemonsDef = 0;
            InnerDemonsAtkPer = 0;
            InnerDemonsDefPer = 0;
            RealDamage = 0;
            AttackSpeed = 0;
            ConfuseLv = 0;
            DmgCounter = 0;
            DmgCounterReduce = 0;
            DrugsEffect = 0;
            OfficialSealLv = 0;
            OfficialSealReduceHurt = 0;
            BoosDiaoLuo = 0;
            AddBaseHurt = 0;
            ReduceBaseHurt = 0;
            AddSkillHurt = 0;
            ReduceSkillHurt = 0;
            AddCritHurt = 0;
            ReduceCritHurt = 0;
            AddHolyHurt = 0;
            CritHurtPct = 0;
            OriginalBaptize = 0;
            SmelterIncome = 0;
            AccurateTriggerRate = 0;
            AccurateHurtPct = 0;
            AddAccurateHurtRate = 0;
            ReduceAccurateHurtRate = 0;
            AddBuffTriggerRate.Clear();
            AddBossHurtValue = 0;
            DeadlyToMonProb = 0;
            DeadlyToMonMultiple = 0;
            IgnoreDefend = 0;
            DisVampire = 0;
            DisControl = 0;
            ReduceVampirePct = 0;
            RoleAddLv  = 0;
            IceHurt = 0;
            FireHurt = 0;
            ThunderHurt = 0;
            IceHurtReduce = 0;
            FireHurtReduce = 0;
            ThunderHurtReduce = 0;
            FootMarkLv = 0;
            DisRebirth = 0;

        }
    }

    /// <summary>
    /// 收购信息
    /// </summary>
    public class PurchaseInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo ShopID = new ObjectGuidInfo();
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 单价
        /// </summary>
        public uint Univalent;
        /// <summary>
        /// 以收购数量
        /// </summary>
        public uint Count;
        /// <summary>
        /// 总数量
        /// </summary>
        public uint MaxCount;
        /// <summary>
        /// 
        /// </summary>
        public string NickName;
        /// <summary>
        /// 
        /// </summary>
        public uint EndTime;

        public object Clone()
        {
            PurchaseInfo st = new PurchaseInfo();
            st.ShopID = ShopID.Clone() as ObjectGuidInfo;
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.TID = TID;
            st.Univalent = Univalent;
            st.Count = Count;
            st.MaxCount = MaxCount;
            st.NickName = NickName;
            st.EndTime = EndTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                ShopID.Serializtion(oByteArray, bSerialize);
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(Univalent);
                oByteArray.WriteUInt(Count);
                oByteArray.WriteUInt(MaxCount);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(EndTime);
            }
            else
            {
                ShopID.Serializtion(oByteArray, bSerialize);
                PlayerID.Serializtion(oByteArray, bSerialize);
                TID = oByteArray.ReadUInt();
                Univalent = oByteArray.ReadUInt();
                Count = oByteArray.ReadUInt();
                MaxCount = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
                EndTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            ShopID = default(ObjectGuidInfo);
            PlayerID = default(ObjectGuidInfo);
            TID = 0;
            Univalent = 0;
            Count = 0;
            MaxCount = 0;
            NickName = string.Empty;
            EndTime = 0;

        }
    }

    /// <summary>
    /// 移动信息
    /// </summary>
    public class PlayerMoveInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ushort MapX;
        /// <summary>
        /// 
        /// </summary>
        public ushort MapY;
        /// <summary>
        /// 
        /// </summary>
        public byte Type;

        public object Clone()
        {
            PlayerMoveInfo st = new PlayerMoveInfo();
            st.MapX = MapX;
            st.MapY = MapY;
            st.Type = Type;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUShort(MapX);
                oByteArray.WriteUShort(MapY);
                oByteArray.WriteByte(Type);
            }
            else
            {
                MapX = oByteArray.ReadUShort();
                MapY = oByteArray.ReadUShort();
                Type = oByteArray.ReadByte();
            }
        }


        public void Reset()
        {
            MapX = 0;
            MapY = 0;
            Type = 0;

        }
    }

    /// <summary>
    /// 挑战Boss信息
    /// </summary>
    public class ChallengeBossInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 
        /// </summary>
        public uint MapID;
        /// <summary>
        /// 
        /// </summary>
        public uint ReviveTime;

        public object Clone()
        {
            ChallengeBossInfo st = new ChallengeBossInfo();
            st.TID = TID;
            st.MapID = MapID;
            st.ReviveTime = ReviveTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(MapID);
                oByteArray.WriteUInt(ReviveTime);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                MapID = oByteArray.ReadUInt();
                ReviveTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            MapID = 0;
            ReviveTime = 0;

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AssembleActivityDropInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Probability;
        /// <summary>
        /// 
        /// </summary>
        public uint DropID;
        /// <summary>
        /// 
        /// </summary>
        public uint BossLv;

        public object Clone()
        {
            AssembleActivityDropInfo st = new AssembleActivityDropInfo();
            st.Probability = Probability;
            st.DropID = DropID;
            st.BossLv = BossLv;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Probability);
                oByteArray.WriteUInt(DropID);
                oByteArray.WriteUInt(BossLv);
            }
            else
            {
                Probability = oByteArray.ReadUInt();
                DropID = oByteArray.ReadUInt();
                BossLv = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Probability = 0;
            DropID = 0;
            BossLv = 0;

        }
    }

    /// <summary>
    /// 猜拳增加的钱
    /// </summary>
    public class GuessMoneyInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerID;
        /// <summary>
        /// 
        /// </summary>
        public uint Money;

        public object Clone()
        {
            GuessMoneyInfo st = new GuessMoneyInfo();
            st.PlayerID = PlayerID;
            st.Money = Money;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerID);
                oByteArray.WriteUInt(Money);
            }
            else
            {
                PlayerID = oByteArray.ReadUInt64();
                Money = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerID = 0;
            Money = 0;

        }
    }

    /// <summary>
    /// 行会buff信息
    /// </summary>
    public class GuildBuffInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint BuffID;
        /// <summary>
        /// 层数
        /// </summary>
        public uint Count;
        /// <summary>
        /// 生效的活动（0为随时有效）
        /// </summary>
        public uint ActivityType;
        /// <summary>
        /// 1.无尽地狱呐喊 2无尽地狱击鼓
        /// </summary>
        public uint BuffType;

        public object Clone()
        {
            GuildBuffInfo st = new GuildBuffInfo();
            st.BuffID = BuffID;
            st.Count = Count;
            st.ActivityType = ActivityType;
            st.BuffType = BuffType;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(BuffID);
                oByteArray.WriteUInt(Count);
                oByteArray.WriteUInt(ActivityType);
                oByteArray.WriteUInt(BuffType);
            }
            else
            {
                BuffID = oByteArray.ReadUInt();
                Count = oByteArray.ReadUInt();
                ActivityType = oByteArray.ReadUInt();
                BuffType = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            BuffID = 0;
            Count = 0;
            ActivityType = 0;
            BuffType = 0;

        }
    }

    /// <summary>
    /// 矿工日志
    /// </summary>
    public class MinerLogInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint CreateTime;
        /// <summary>
        /// 
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 
        /// </summary>
        public uint Data3;
        /// <summary>
        /// 
        /// </summary>
        public string String1;
        /// <summary>
        /// 
        /// </summary>
        public string String2;
        /// <summary>
        /// 
        /// </summary>
        public string String3;
        /// <summary>
        /// 
        /// </summary>
        public uint LogType ;

        public object Clone()
        {
            MinerLogInfo st = new MinerLogInfo();
            st.CreateTime = CreateTime;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.Data3 = Data3;
            st.String1 = String1;
            st.String2 = String2;
            st.String3 = String3;
            st.LogType  = LogType ;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(CreateTime);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(Data3);
                oByteArray.WriteUTF(String1);
                oByteArray.WriteUTF(String2);
                oByteArray.WriteUTF(String3);
                oByteArray.WriteUInt(LogType );
            }
            else
            {
                CreateTime = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                Data3 = oByteArray.ReadUInt();
                String1 = oByteArray.ReadUTF();
                String2 = oByteArray.ReadUTF();
                String3 = oByteArray.ReadUTF();
                LogType  = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            CreateTime = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            String1 = string.Empty;
            String2 = string.Empty;
            String3 = string.Empty;
            LogType  = 0;

        }
    }

    /// <summary>
    /// 道具来源信息
    /// </summary>
    public class SourceInfo : IStruct
    {
        /// <summary>
        /// 途径类型
        /// </summary>
        public uint SourceType;
        /// <summary>
        /// 地图配置id
        /// </summary>
        public uint MapTmplID;
        /// <summary>
        /// 怪物配置id
        /// </summary>
        public uint MonTmplID;
        /// <summary>
        /// 道具配置id
        /// </summary>
        public uint ItemTmplID;
        /// <summary>
        /// 
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 获取时间
        /// </summary>
        public uint GainTime;

        public object Clone()
        {
            SourceInfo st = new SourceInfo();
            st.SourceType = SourceType;
            st.MapTmplID = MapTmplID;
            st.MonTmplID = MonTmplID;
            st.ItemTmplID = ItemTmplID;
            st.PlayerName = PlayerName;
            st.GainTime = GainTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(SourceType);
                oByteArray.WriteUInt(MapTmplID);
                oByteArray.WriteUInt(MonTmplID);
                oByteArray.WriteUInt(ItemTmplID);
                oByteArray.WriteUTF(PlayerName);
                oByteArray.WriteUInt(GainTime);
            }
            else
            {
                SourceType = oByteArray.ReadUInt();
                MapTmplID = oByteArray.ReadUInt();
                MonTmplID = oByteArray.ReadUInt();
                ItemTmplID = oByteArray.ReadUInt();
                PlayerName = oByteArray.ReadUTF();
                GainTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            SourceType = 0;
            MapTmplID = 0;
            MonTmplID = 0;
            ItemTmplID = 0;
            PlayerName = string.Empty;
            GainTime = 0;

        }
    }

    /// <summary>
    /// 物品信息
    /// </summary>
    public class GameItemInfo : IStruct
    {
        /// <summary>
        /// 物品唯一ID
        /// </summary>
        public ObjectGuidInfo ItemID = new ObjectGuidInfo();
        /// <summary>
        /// 是否绑定
        /// </summary>
        public byte Binding;
        /// <summary>
        /// 是否可鉴定
        /// </summary>
        public byte CanAppraise;
        /// <summary>
        /// 模板ID
        /// </summary>
        public uint ItemTMPLID;
        /// <summary>
        /// 模板数量
        /// </summary>
        public uint ItemNum;
        /// <summary>
        /// 幸运值
        /// </summary>
        public uint Lucky;
        /// <summary>
        /// 最大强化等级
        /// </summary>
        public uint MaxStrength;
        /// <summary>
        /// 强化等级
        /// </summary>
        public uint StrengthLv;
        /// <summary>
        /// 注灵值
        /// </summary>
        public uint FixSoulLv;
        /// <summary>
        /// 过期时间
        /// </summary>
        public uint ExpireTime;
        /// <summary>
        /// 
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 物品位置(目前只在Game006协议有效)
        /// </summary>
        public uint ItemPos;
        /// <summary>
        /// 额外属性(轮回装备:强化附加属性)
        /// </summary>
        public string AdditionalAttributes;
        /// <summary>
        /// 
        /// </summary>
        public string Data3;
        /// <summary>
        /// 道具来源信息
        /// </summary>
        public SourceInfo ItemSourceInfo = new SourceInfo();

        public object Clone()
        {
            GameItemInfo st = new GameItemInfo();
            st.ItemID = ItemID.Clone() as ObjectGuidInfo;
            st.Binding = Binding;
            st.CanAppraise = CanAppraise;
            st.ItemTMPLID = ItemTMPLID;
            st.ItemNum = ItemNum;
            st.Lucky = Lucky;
            st.MaxStrength = MaxStrength;
            st.StrengthLv = StrengthLv;
            st.FixSoulLv = FixSoulLv;
            st.ExpireTime = ExpireTime;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.ItemPos = ItemPos;
            st.AdditionalAttributes = AdditionalAttributes;
            st.Data3 = Data3;
            st.ItemSourceInfo = ItemSourceInfo.Clone() as SourceInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                ItemID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteByte(Binding);
                oByteArray.WriteByte(CanAppraise);
                oByteArray.WriteUInt(ItemTMPLID);
                oByteArray.WriteUInt(ItemNum);
                oByteArray.WriteUInt(Lucky);
                oByteArray.WriteUInt(MaxStrength);
                oByteArray.WriteUInt(StrengthLv);
                oByteArray.WriteUInt(FixSoulLv);
                oByteArray.WriteUInt(ExpireTime);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(ItemPos);
                oByteArray.WriteUTF(AdditionalAttributes);
                oByteArray.WriteUTF(Data3);
                ItemSourceInfo.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                ItemID.Serializtion(oByteArray, bSerialize);
                Binding = oByteArray.ReadByte();
                CanAppraise = oByteArray.ReadByte();
                ItemTMPLID = oByteArray.ReadUInt();
                ItemNum = oByteArray.ReadUInt();
                Lucky = oByteArray.ReadUInt();
                MaxStrength = oByteArray.ReadUInt();
                StrengthLv = oByteArray.ReadUInt();
                FixSoulLv = oByteArray.ReadUInt();
                ExpireTime = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                ItemPos = oByteArray.ReadUInt();
                AdditionalAttributes = oByteArray.ReadUTF();
                Data3 = oByteArray.ReadUTF();
                ItemSourceInfo.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            ItemID = default(ObjectGuidInfo);
            Binding = 0;
            CanAppraise = 0;
            ItemTMPLID = 0;
            ItemNum = 0;
            Lucky = 0;
            MaxStrength = 0;
            StrengthLv = 0;
            FixSoulLv = 0;
            ExpireTime = 0;
            Data1 = 0;
            Data2 = 0;
            ItemPos = 0;
            AdditionalAttributes = string.Empty;
            Data3 = string.Empty;
            ItemSourceInfo = default(SourceInfo);

        }
    }

    /// <summary>
    /// 拍卖行物品
    /// </summary>
    public class AuctionItem : IStruct
    {
        /// <summary>
        /// 物品信息
        /// </summary>
        public GameItemInfo ItemInfo = new GameItemInfo();
        /// <summary>
        /// 一口价
        /// </summary>
        public uint Price;
        /// <summary>
        /// 竞拍价
        /// </summary>
        public uint AuctionPrice;
        /// <summary>
        /// 货币类型（1元宝3金币）
        /// </summary>
        public uint PriceType;
        /// <summary>
        /// 拍卖失效时间
        /// </summary>
        public uint AuctionExpireTime;
        /// <summary>
        /// 所有者ID
        /// </summary>
        public ObjectGuidInfo OwnerID = new ObjectGuidInfo();
        /// <summary>
        /// 竞拍者ID
        /// </summary>
        public ObjectGuidInfo BidderID = new ObjectGuidInfo();
        /// <summary>
        /// 竞拍加价最低万分比
        /// </summary>
        public uint AddMoneyRate;
        /// <summary>
        /// 拍卖类型1.元宝拍卖，2.金币拍卖，3.行会拍卖，4.跨服拍卖
        /// </summary>
        public uint AuctionType;

        public object Clone()
        {
            AuctionItem st = new AuctionItem();
            st.ItemInfo = ItemInfo.Clone() as GameItemInfo;
            st.Price = Price;
            st.AuctionPrice = AuctionPrice;
            st.PriceType = PriceType;
            st.AuctionExpireTime = AuctionExpireTime;
            st.OwnerID = OwnerID.Clone() as ObjectGuidInfo;
            st.BidderID = BidderID.Clone() as ObjectGuidInfo;
            st.AddMoneyRate = AddMoneyRate;
            st.AuctionType = AuctionType;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                ItemInfo.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(Price);
                oByteArray.WriteUInt(AuctionPrice);
                oByteArray.WriteUInt(PriceType);
                oByteArray.WriteUInt(AuctionExpireTime);
                OwnerID.Serializtion(oByteArray, bSerialize);
                BidderID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(AddMoneyRate);
                oByteArray.WriteUInt(AuctionType);
            }
            else
            {
                ItemInfo.Serializtion(oByteArray, bSerialize);
                Price = oByteArray.ReadUInt();
                AuctionPrice = oByteArray.ReadUInt();
                PriceType = oByteArray.ReadUInt();
                AuctionExpireTime = oByteArray.ReadUInt();
                OwnerID.Serializtion(oByteArray, bSerialize);
                BidderID.Serializtion(oByteArray, bSerialize);
                AddMoneyRate = oByteArray.ReadUInt();
                AuctionType = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            ItemInfo = default(GameItemInfo);
            Price = 0;
            AuctionPrice = 0;
            PriceType = 0;
            AuctionExpireTime = 0;
            OwnerID = default(ObjectGuidInfo);
            BidderID = default(ObjectGuidInfo);
            AddMoneyRate = 0;
            AuctionType = 0;

        }
    }

    /// <summary>
    /// 摆摊物品信息
    /// </summary>
    public class RetailItemInfo : IStruct
    {
        /// <summary>
        /// 1-元宝，4-金币;
        /// </summary>
        public uint MoneyType;
        /// <summary>
        /// 货币数量
        /// </summary>
        public uint MoneyNum;
        /// <summary>
        /// 装备信息
        /// </summary>
        public GameItemInfo ItemInfo = new GameItemInfo();

        public object Clone()
        {
            RetailItemInfo st = new RetailItemInfo();
            st.MoneyType = MoneyType;
            st.MoneyNum = MoneyNum;
            st.ItemInfo = ItemInfo.Clone() as GameItemInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MoneyType);
                oByteArray.WriteUInt(MoneyNum);
                ItemInfo.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                MoneyType = oByteArray.ReadUInt();
                MoneyNum = oByteArray.ReadUInt();
                ItemInfo.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            MoneyType = 0;
            MoneyNum = 0;
            ItemInfo = default(GameItemInfo);

        }
    }

    /// <summary>
    /// 摆摊日志信息
    /// </summary>
    public class RetailLogInfo : IStruct
    {
        /// <summary>
        /// 货币类型
        /// </summary>
        public uint MoneyType;
        /// <summary>
        /// 购买者花费货币数量
        /// </summary>
        public uint CostMoney;
        /// <summary>
        /// 摆摊者实际收入货币数量
        /// </summary>
        public uint AddMoney;
        /// <summary>
        /// 购买者名字
        /// </summary>
        public string BuyerName;
        /// <summary>
        /// 购买时间
        /// </summary>
        public uint CreateTime;
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 物品信息
        /// </summary>
        public GameItemInfo ItemInfo = new GameItemInfo();

        public object Clone()
        {
            RetailLogInfo st = new RetailLogInfo();
            st.MoneyType = MoneyType;
            st.CostMoney = CostMoney;
            st.AddMoney = AddMoney;
            st.BuyerName = BuyerName;
            st.CreateTime = CreateTime;
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.ItemInfo = ItemInfo.Clone() as GameItemInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MoneyType);
                oByteArray.WriteUInt(CostMoney);
                oByteArray.WriteUInt(AddMoney);
                oByteArray.WriteUTF(BuyerName);
                oByteArray.WriteUInt(CreateTime);
                PlayerID.Serializtion(oByteArray, bSerialize);
                ItemInfo.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                MoneyType = oByteArray.ReadUInt();
                CostMoney = oByteArray.ReadUInt();
                AddMoney = oByteArray.ReadUInt();
                BuyerName = oByteArray.ReadUTF();
                CreateTime = oByteArray.ReadUInt();
                PlayerID.Serializtion(oByteArray, bSerialize);
                ItemInfo.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            MoneyType = 0;
            CostMoney = 0;
            AddMoney = 0;
            BuyerName = string.Empty;
            CreateTime = 0;
            PlayerID = default(ObjectGuidInfo);
            ItemInfo = default(GameItemInfo);

        }
    }

    /// <summary>
    /// 悬赏任务信息
    /// </summary>
    public class OfferTaskInfo : IStruct
    {
        /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher;
        /// <summary>
        /// 发布人唯一id
        /// </summary>
        public ObjectGuidInfo PublisherGuid = new ObjectGuidInfo();
        /// <summary>
        /// 需求击杀怪物组id
        /// </summary>
        public uint MonsterGroupID;
        /// <summary>
        /// 完成任务数量
        /// </summary>
        public uint FinishNum;
        /// <summary>
        /// 领取任务玩家数量
        /// </summary>
        public uint ExecutePlayerNum;
        /// <summary>
        /// 任务唯一id
        /// </summary>
        public ObjectGuidInfo TaskGuid = new ObjectGuidInfo();
        /// <summary>
        /// RewardOrderTemplate->TID
        /// </summary>
        public uint TaskTID;
        /// <summary>
        /// 发布时间
        /// </summary>
        public uint PublishTime;
        /// <summary>
        /// 下次可接取任务时间
        /// </summary>
        public uint NextReceiveTime;
        /// <summary>
        /// 奖励数量
        /// </summary>
        public uint RewardNum;
        /// <summary>
        /// 已领取奖励任务数量
        /// </summary>
        public uint TakeRewardNum;
        /// <summary>
        /// 发布任务数量
        /// </summary>
        public uint PublishNum;

        public object Clone()
        {
            OfferTaskInfo st = new OfferTaskInfo();
            st.Publisher = Publisher;
            st.PublisherGuid = PublisherGuid.Clone() as ObjectGuidInfo;
            st.MonsterGroupID = MonsterGroupID;
            st.FinishNum = FinishNum;
            st.ExecutePlayerNum = ExecutePlayerNum;
            st.TaskGuid = TaskGuid.Clone() as ObjectGuidInfo;
            st.TaskTID = TaskTID;
            st.PublishTime = PublishTime;
            st.NextReceiveTime = NextReceiveTime;
            st.RewardNum = RewardNum;
            st.TakeRewardNum = TakeRewardNum;
            st.PublishNum = PublishNum;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(Publisher);
                PublisherGuid.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(MonsterGroupID);
                oByteArray.WriteUInt(FinishNum);
                oByteArray.WriteUInt(ExecutePlayerNum);
                TaskGuid.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(TaskTID);
                oByteArray.WriteUInt(PublishTime);
                oByteArray.WriteUInt(NextReceiveTime);
                oByteArray.WriteUInt(RewardNum);
                oByteArray.WriteUInt(TakeRewardNum);
                oByteArray.WriteUInt(PublishNum);
            }
            else
            {
                Publisher = oByteArray.ReadUTF();
                PublisherGuid.Serializtion(oByteArray, bSerialize);
                MonsterGroupID = oByteArray.ReadUInt();
                FinishNum = oByteArray.ReadUInt();
                ExecutePlayerNum = oByteArray.ReadUInt();
                TaskGuid.Serializtion(oByteArray, bSerialize);
                TaskTID = oByteArray.ReadUInt();
                PublishTime = oByteArray.ReadUInt();
                NextReceiveTime = oByteArray.ReadUInt();
                RewardNum = oByteArray.ReadUInt();
                TakeRewardNum = oByteArray.ReadUInt();
                PublishNum = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Publisher = string.Empty;
            PublisherGuid = default(ObjectGuidInfo);
            MonsterGroupID = 0;
            FinishNum = 0;
            ExecutePlayerNum = 0;
            TaskGuid = default(ObjectGuidInfo);
            TaskTID = 0;
            PublishTime = 0;
            NextReceiveTime = 0;
            RewardNum = 0;
            TakeRewardNum = 0;
            PublishNum = 0;

        }
    }

    /// <summary>
    /// 限时商城道具信息
    /// </summary>
    public class TimeLimitShopItem : IStruct
    {
        /// <summary>
        /// MallTemplate[TID]
        /// </summary>
        public uint TID;
        /// <summary>
        /// 已购数量
        /// </summary>
        public uint Num;
        /// <summary>
        /// 截止时间戳
        /// </summary>
        public uint Time;

        public object Clone()
        {
            TimeLimitShopItem st = new TimeLimitShopItem();
            st.TID = TID;
            st.Num = Num;
            st.Time = Time;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(Num);
                oByteArray.WriteUInt(Time);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                Num = oByteArray.ReadUInt();
                Time = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            Num = 0;
            Time = 0;

        }
    }

    /// <summary>
    /// 市场道具信息(黑市、集市)
    /// </summary>
    public class MarketItemInfo : IStruct
    {
        /// <summary>
        /// MarketTemplate[TID]
        /// </summary>
        public uint TID;
        /// <summary>
        /// 已购数量
        /// </summary>
        public uint YNum;

        public object Clone()
        {
            MarketItemInfo st = new MarketItemInfo();
            st.TID = TID;
            st.YNum = YNum;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(YNum);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                YNum = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            YNum = 0;

        }
    }

    /// <summary>
    /// 日常活动信息
    /// </summary>
    public class DailyActivityInfo : IStruct
    {
        /// <summary>
        /// DailyActivityTemplate[TID]
        /// </summary>
        public uint TID;
        /// <summary>
        /// 0:该活动不能补签;1:不能补签(活动未结束);2:参与了活动;3:可补签;4:已补签
        /// </summary>
        public uint SignState;

        public object Clone()
        {
            DailyActivityInfo st = new DailyActivityInfo();
            st.TID = TID;
            st.SignState = SignState;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(SignState);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                SignState = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            SignState = 0;

        }
    }

    /// <summary>
    /// 跨服战阵营信息
    /// </summary>
    public class ServiceWarCampInfo : IStruct
    {
        /// <summary>
        /// 积分
        /// </summary>
        public uint Point;
        /// <summary>
        /// 排名
        /// </summary>
        public uint Rank;
        /// <summary>
        /// 阵营编号
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 当天积分
        /// </summary>
        public uint CurDayPoint;

        public object Clone()
        {
            ServiceWarCampInfo st = new ServiceWarCampInfo();
            st.Point = Point;
            st.Rank = Rank;
            st.Camp = Camp;
            st.CurDayPoint = CurDayPoint;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Point);
                oByteArray.WriteUInt(Rank);
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteUInt(CurDayPoint);
            }
            else
            {
                Point = oByteArray.ReadUInt();
                Rank = oByteArray.ReadUInt();
                Camp = oByteArray.ReadUInt();
                CurDayPoint = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Point = 0;
            Rank = 0;
            Camp = 0;
            CurDayPoint = 0;

        }
    }

    /// <summary>
    /// 神石共鸣信息
    /// </summary>
    public class JewelResonanceData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint EquipPoint;
        /// <summary>
        /// 当前共鸣类型
        /// </summary>
        public uint CurType;
        /// <summary>
        /// 已激活共鸣类型列表
        /// </summary>
        public List<uint> ActivatedTypeList = new List<uint>();

        public object Clone()
        {
            JewelResonanceData st = new JewelResonanceData();
            st.EquipPoint = EquipPoint;
            st.CurType = CurType;
            foreach (var item in ActivatedTypeList)
            {
                st.ActivatedTypeList.Add(item);
            }
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(EquipPoint);
                oByteArray.WriteUInt(CurType);
                oByteArray.WriteUShort((ushort)ActivatedTypeList.Count);
                for (int i = 0; i < ActivatedTypeList.Count; i++)
                {
                    oByteArray.WriteUInt(ActivatedTypeList[i]);
                }
            }
            else
            {
                EquipPoint = oByteArray.ReadUInt();
                CurType = oByteArray.ReadUInt();
                int ActivatedTypeListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < ActivatedTypeListCount; i++)
                {
                    ActivatedTypeList.Add(oByteArray.ReadUInt());
                }
            }
        }


        public void Reset()
        {
            EquipPoint = 0;
            CurType = 0;
            ActivatedTypeList.Clear();

        }
    }

    /// <summary>
    /// 活动事件
    /// </summary>
    public class DailyActivityEvent : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint EventID;
        /// <summary>
        /// 
        /// </summary>
        public long Data1;
        /// <summary>
        /// 
        /// </summary>
        public List<ulong> Data2 = new List<ulong>();
        /// <summary>
        /// 
        /// </summary>
        public string Data3;
        /// <summary>
        /// 
        /// </summary>
        public long Data4;

        public object Clone()
        {
            DailyActivityEvent st = new DailyActivityEvent();
            st.EventID = EventID;
            st.Data1 = Data1;
            foreach (var item in Data2)
            {
                st.Data2.Add(item);
            }
            st.Data3 = Data3;
            st.Data4 = Data4;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(EventID);
                oByteArray.WriteInt64(Data1);
                oByteArray.WriteUShort((ushort)Data2.Count);
                for (int i = 0; i < Data2.Count; i++)
                {
                    oByteArray.WriteUInt64(Data2[i]);
                }
                oByteArray.WriteUTF(Data3);
                oByteArray.WriteInt64(Data4);
            }
            else
            {
                EventID = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadInt64();
                int Data2Count = (int)oByteArray.ReadUShort();
                for (int i = 0; i < Data2Count; i++)
                {
                    Data2.Add(oByteArray.ReadUInt64());
                }
                Data3 = oByteArray.ReadUTF();
                Data4 = oByteArray.ReadInt64();
            }
        }


        public void Reset()
        {
            EventID = 0;
            Data1 = 0;
            Data2.Clear();
            Data3 = string.Empty;
            Data4 = 0;

        }
    }

    /// <summary>
    /// 装备位强化信息
    /// </summary>
    public class EquipPosStrengthenInfo : IStruct
    {
        /// <summary>
        /// 强化等级信息
        /// </summary>
        public List<ProtocolPair> StrengthenLvList = new List<ProtocolPair>();
        /// <summary>
        /// 熟练度
        /// </summary>
        public uint Mastery;

        public object Clone()
        {
            EquipPosStrengthenInfo st = new EquipPosStrengthenInfo();
            foreach (ProtocolPair item in StrengthenLvList)
            {
                st.StrengthenLvList.Add(item.Clone() as ProtocolPair);
            }
            st.Mastery = Mastery;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUShort((ushort)StrengthenLvList.Count);
                for (int i = 0; i < StrengthenLvList.Count; i++)
                {
                    StrengthenLvList[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteUInt(Mastery);
            }
            else
            {
                int StrengthenLvListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < StrengthenLvListCount; i++)
                {
                    ProtocolPair obj = new ProtocolPair();
                    obj.Serializtion(oByteArray, bSerialize);
                    StrengthenLvList.Add(obj);
                }
                Mastery = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            StrengthenLvList.Clear();
            Mastery = 0;

        }
    }

    /// <summary>
    /// 玩家宠物信息(道士)
    /// </summary>
    public class PlayerPetInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint PetGroup;
        /// <summary>
        /// 
        /// </summary>
        public uint Level;
        /// <summary>
        /// 
        /// </summary>
        public uint Exp;
        /// <summary>
        /// 
        /// </summary>
        public uint MinLv;
        /// <summary>
        /// 
        /// </summary>
        public uint MaxLv;

        public object Clone()
        {
            PlayerPetInfo st = new PlayerPetInfo();
            st.PetGroup = PetGroup;
            st.Level = Level;
            st.Exp = Exp;
            st.MinLv = MinLv;
            st.MaxLv = MaxLv;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(PetGroup);
                oByteArray.WriteUInt(Level);
                oByteArray.WriteUInt(Exp);
                oByteArray.WriteUInt(MinLv);
                oByteArray.WriteUInt(MaxLv);
            }
            else
            {
                PetGroup = oByteArray.ReadUInt();
                Level = oByteArray.ReadUInt();
                Exp = oByteArray.ReadUInt();
                MinLv = oByteArray.ReadUInt();
                MaxLv = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PetGroup = 0;
            Level = 0;
            Exp = 0;
            MinLv = 0;
            MaxLv = 0;

        }
    }

    /// <summary>
    /// 成就限额信息
    /// </summary>
    public class AchievementQuotaInfo : IStruct
    {
        /// <summary>
        /// 成就TID
        /// </summary>
        public int AchievementTID;
        /// <summary>
        /// 平台ID
        /// </summary>
        public uint PlatformID;
        /// <summary>
        /// 区服ID
        /// </summary>
        public uint ServerID;
        /// <summary>
        /// 已完成次数(无限制的成就会是0)
        /// </summary>
        public int AchieveTimes;
        /// <summary>
        /// 首次完成玩家ID
        /// </summary>
        public uint PlayerID;
        /// <summary>
        /// 首次完成玩家昵称
        /// </summary>
        public string FirstNickName;
        /// <summary>
        /// 首次完成时间
        /// </summary>
        public int FirstTime;
        /// <summary>
        /// 存储模式(0:不,1:update)
        /// </summary>
        public int SaveMode;

        public object Clone()
        {
            AchievementQuotaInfo st = new AchievementQuotaInfo();
            st.AchievementTID = AchievementTID;
            st.PlatformID = PlatformID;
            st.ServerID = ServerID;
            st.AchieveTimes = AchieveTimes;
            st.PlayerID = PlayerID;
            st.FirstNickName = FirstNickName;
            st.FirstTime = FirstTime;
            st.SaveMode = SaveMode;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(AchievementTID);
                oByteArray.WriteUInt(PlatformID);
                oByteArray.WriteUInt(ServerID);
                oByteArray.WriteInt(AchieveTimes);
                oByteArray.WriteUInt(PlayerID);
                oByteArray.WriteUTF(FirstNickName);
                oByteArray.WriteInt(FirstTime);
                oByteArray.WriteInt(SaveMode);
            }
            else
            {
                AchievementTID = oByteArray.ReadInt();
                PlatformID = oByteArray.ReadUInt();
                ServerID = oByteArray.ReadUInt();
                AchieveTimes = oByteArray.ReadInt();
                PlayerID = oByteArray.ReadUInt();
                FirstNickName = oByteArray.ReadUTF();
                FirstTime = oByteArray.ReadInt();
                SaveMode = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            AchievementTID = 0;
            PlatformID = 0;
            ServerID = 0;
            AchieveTimes = 0;
            PlayerID = 0;
            FirstNickName = string.Empty;
            FirstTime = 0;
            SaveMode = 0;

        }
    }

    /// <summary>
    /// 玩家成就信息
    /// </summary>
    public class PlayerAchievementInfo : IStruct
    {
        /// <summary>
        /// 成就TID
        /// </summary>
        public int TID;
        /// <summary>
        /// 条件(进度)
        /// </summary>
        public int Time;
        /// <summary>
        /// 状态(0:未完成;1:已完成;2:可领取;3:已领取)
        /// </summary>
        public int State;
        /// <summary>
        /// 完成时间
        /// </summary>
        public int FinishTime;
        /// <summary>
        /// 重复完成次数进度
        /// </summary>
        public int Repeat;
        /// <summary>
        /// 存储模式(0:不,1update)
        /// </summary>
        public int SaveMode;

        public object Clone()
        {
            PlayerAchievementInfo st = new PlayerAchievementInfo();
            st.TID = TID;
            st.Time = Time;
            st.State = State;
            st.FinishTime = FinishTime;
            st.Repeat = Repeat;
            st.SaveMode = SaveMode;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(TID);
                oByteArray.WriteInt(Time);
                oByteArray.WriteInt(State);
                oByteArray.WriteInt(FinishTime);
                oByteArray.WriteInt(Repeat);
                oByteArray.WriteInt(SaveMode);
            }
            else
            {
                TID = oByteArray.ReadInt();
                Time = oByteArray.ReadInt();
                State = oByteArray.ReadInt();
                FinishTime = oByteArray.ReadInt();
                Repeat = oByteArray.ReadInt();
                SaveMode = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            Time = 0;
            State = 0;
            FinishTime = 0;
            Repeat = 0;
            SaveMode = 0;

        }
    }

    /// <summary>
    /// 成就-进度共用信息
    /// </summary>
    public class AchievementMergeInfo : IStruct
    {
        /// <summary>
        /// 成就TID
        /// </summary>
        public int AchievementTID;
        /// <summary>
        /// 条件(进度)
        /// </summary>
        public int Time;

        public object Clone()
        {
            AchievementMergeInfo st = new AchievementMergeInfo();
            st.AchievementTID = AchievementTID;
            st.Time = Time;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(AchievementTID);
                oByteArray.WriteInt(Time);
            }
            else
            {
                AchievementTID = oByteArray.ReadInt();
                Time = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            AchievementTID = 0;
            Time = 0;

        }
    }

    /// <summary>
    /// 成就达成排名信息
    /// </summary>
    public class AchievementRank : IStruct
    {
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;
        /// <summary>
        /// 完成时间
        /// </summary>
        public uint FinishTime;
        /// <summary>
        /// 成就TID
        /// </summary>
        public int AchievementTID;

        public object Clone()
        {
            AchievementRank st = new AchievementRank();
            st.NickName = NickName;
            st.FinishTime = FinishTime;
            st.AchievementTID = AchievementTID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(FinishTime);
                oByteArray.WriteInt(AchievementTID);
            }
            else
            {
                NickName = oByteArray.ReadUTF();
                FinishTime = oByteArray.ReadUInt();
                AchievementTID = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            NickName = string.Empty;
            FinishTime = 0;
            AchievementTID = 0;

        }
    }

    /// <summary>
    /// 玩家活跃信息
    /// </summary>
    public class LivenessInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int TID;
        /// <summary>
        /// 完成次数
        /// </summary>
        public int Data;
        /// <summary>
        /// 领取状况
        /// </summary>
        public int Draw;
        /// <summary>
        /// 存储模式(0:不,1:update)
        /// </summary>
        public int SaveMode;

        public object Clone()
        {
            LivenessInfo st = new LivenessInfo();
            st.TID = TID;
            st.Data = Data;
            st.Draw = Draw;
            st.SaveMode = SaveMode;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(TID);
                oByteArray.WriteInt(Data);
                oByteArray.WriteInt(Draw);
                oByteArray.WriteInt(SaveMode);
            }
            else
            {
                TID = oByteArray.ReadInt();
                Data = oByteArray.ReadInt();
                Draw = oByteArray.ReadInt();
                SaveMode = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            Data = 0;
            Draw = 0;
            SaveMode = 0;

        }
    }

    /// <summary>
    /// 挑战Boss信息
    /// </summary>
    public class BossChallengeInfo : IStruct
    {
        /// <summary>
        /// 地图ID
        /// </summary>
        public uint MapID;
        /// <summary>
        /// 是否活着
        /// </summary>
        public bool Alive;
        /// <summary>
        /// 数量(仅初始时用)
        /// </summary>
        public uint Num;

        public object Clone()
        {
            BossChallengeInfo st = new BossChallengeInfo();
            st.MapID = MapID;
            st.Alive = Alive;
            st.Num = Num;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MapID);
                oByteArray.WriteBoolean(Alive);
                oByteArray.WriteUInt(Num);
            }
            else
            {
                MapID = oByteArray.ReadUInt();
                Alive = oByteArray.ReadBoolean();
                Num = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MapID = 0;
            Alive = false;
            Num = 0;

        }
    }

    /// <summary>
    /// 转盘信息
    /// </summary>
    public class TurntableInformation : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Group;
        /// <summary>
        /// 
        /// </summary>
        public uint Level;
        /// <summary>
        /// 剩余次数
        /// </summary>
        public uint RemainingTimes;
        /// <summary>
        /// 已获取奖励列表
        /// </summary>
        public List<uint> AcquiredRewardList = new List<uint>();
        /// <summary>
        /// 奖池
        /// </summary>
        public List<uint> RewardPool = new List<uint>();
        /// <summary>
        /// 已使用道具次数
        /// </summary>
        public uint ItemCostTimes;

        public object Clone()
        {
            TurntableInformation st = new TurntableInformation();
            st.Group = Group;
            st.Level = Level;
            st.RemainingTimes = RemainingTimes;
            foreach (var item in AcquiredRewardList)
            {
                st.AcquiredRewardList.Add(item);
            }
            foreach (var item in RewardPool)
            {
                st.RewardPool.Add(item);
            }
            st.ItemCostTimes = ItemCostTimes;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Group);
                oByteArray.WriteUInt(Level);
                oByteArray.WriteUInt(RemainingTimes);
                oByteArray.WriteUShort((ushort)AcquiredRewardList.Count);
                for (int i = 0; i < AcquiredRewardList.Count; i++)
                {
                    oByteArray.WriteUInt(AcquiredRewardList[i]);
                }
                oByteArray.WriteUShort((ushort)RewardPool.Count);
                for (int i = 0; i < RewardPool.Count; i++)
                {
                    oByteArray.WriteUInt(RewardPool[i]);
                }
                oByteArray.WriteUInt(ItemCostTimes);
            }
            else
            {
                Group = oByteArray.ReadUInt();
                Level = oByteArray.ReadUInt();
                RemainingTimes = oByteArray.ReadUInt();
                int AcquiredRewardListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AcquiredRewardListCount; i++)
                {
                    AcquiredRewardList.Add(oByteArray.ReadUInt());
                }
                int RewardPoolCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < RewardPoolCount; i++)
                {
                    RewardPool.Add(oByteArray.ReadUInt());
                }
                ItemCostTimes = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Group = 0;
            Level = 0;
            RemainingTimes = 0;
            AcquiredRewardList.Clear();
            RewardPool.Clear();
            ItemCostTimes = 0;

        }
    }

    /// <summary>
    /// 玩家事件信息
    /// </summary>
    public class PlayerEventData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 已触发次数
        /// </summary>
        public uint TriggeredTimes;

        public object Clone()
        {
            PlayerEventData st = new PlayerEventData();
            st.TID = TID;
            st.TriggeredTimes = TriggeredTimes;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(TriggeredTimes);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                TriggeredTimes = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            TriggeredTimes = 0;

        }
    }

    /// <summary>
    /// 玩家境界信息
    /// </summary>
    public class PlayerStateData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint StateLevel;
        /// <summary>
        /// 境界修为值
        /// </summary>
        public uint AcquireValue;

        public object Clone()
        {
            PlayerStateData st = new PlayerStateData();
            st.StateLevel = StateLevel;
            st.AcquireValue = AcquireValue;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(StateLevel);
                oByteArray.WriteUInt(AcquireValue);
            }
            else
            {
                StateLevel = oByteArray.ReadUInt();
                AcquireValue = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            StateLevel = 0;
            AcquireValue = 0;

        }
    }

    /// <summary>
    /// BOSS挑战UI页签信息
    /// </summary>
    public class BossChallengeTabInfo : IStruct
    {
        /// <summary>
        /// NewBossChallengeTemplate中的TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 指定BOSS总数量
        /// </summary>
        public uint TotalCount;
        /// <summary>
        /// 各地图中指定Boss数量(活着)
        /// </summary>
        public List<ProtocolPair> LiveCount = new List<ProtocolPair>();

        public object Clone()
        {
            BossChallengeTabInfo st = new BossChallengeTabInfo();
            st.TID = TID;
            st.TotalCount = TotalCount;
            foreach (ProtocolPair item in LiveCount)
            {
                st.LiveCount.Add(item.Clone() as ProtocolPair);
            }
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(TotalCount);
                oByteArray.WriteUShort((ushort)LiveCount.Count);
                for (int i = 0; i < LiveCount.Count; i++)
                {
                    LiveCount[i].Serializtion(oByteArray, bSerialize);
                }
            }
            else
            {
                TID = oByteArray.ReadUInt();
                TotalCount = oByteArray.ReadUInt();
                int LiveCountCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < LiveCountCount; i++)
                {
                    ProtocolPair obj = new ProtocolPair();
                    obj.Serializtion(oByteArray, bSerialize);
                    LiveCount.Add(obj);
                }
            }
        }


        public void Reset()
        {
            TID = 0;
            TotalCount = 0;
            LiveCount.Clear();

        }
    }

    /// <summary>
    /// 机器人信息
    /// </summary>
    public class RobotData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong DataGuid;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 1.野外挂机机器人 2.安全区机器人
        /// </summary>
        public int RobotType;
        /// <summary>
        /// 生成时间
        /// </summary>
        public int CreateTime;
        /// <summary>
        /// 
        /// </summary>
        public int MapID;
        /// <summary>
        /// 
        /// </summary>
        public int Job;
        /// <summary>
        /// 
        /// </summary>
        public int Sex;
        /// <summary>
        /// 
        /// </summary>
        public int Level;
        /// <summary>
        /// 
        /// </summary>
        public uint AITmplID;
        /// <summary>
        /// 
        /// </summary>
        public int WeaponID;
        /// <summary>
        /// 
        /// </summary>
        public int ClothesID;
        /// <summary>
        /// 
        /// </summary>
        public long HP;
        /// <summary>
        /// 
        /// </summary>
        public long Energy;
        /// <summary>
        /// 
        /// </summary>
        public long MinPhysicAtk;
        /// <summary>
        /// 
        /// </summary>
        public long MaxPhysicAtk;
        /// <summary>
        /// 
        /// </summary>
        public long MinPhysicDef;
        /// <summary>
        /// 
        /// </summary>
        public long MaxPhysicDef;
        /// <summary>
        /// 
        /// </summary>
        public long MinMagicAtk;
        /// <summary>
        /// 
        /// </summary>
        public long RestoreHP;
        /// <summary>
        /// 
        /// </summary>
        public long MaxMagicAtk;
        /// <summary>
        /// 
        /// </summary>
        public long MinMagicDef;
        /// <summary>
        /// 
        /// </summary>
        public long MaxMagicDef;
        /// <summary>
        /// 
        /// </summary>
        public long MinTaoistAtk;
        /// <summary>
        /// 
        /// </summary>
        public long MaxTaoistAtk;
        /// <summary>
        /// 
        /// </summary>
        public int FirstStoveLv;
        /// <summary>
        /// 
        /// </summary>
        public int SecondStoveLv;
        /// <summary>
        /// 
        /// </summary>
        public int ThirdStoveLv;
        /// <summary>
        /// 
        /// </summary>
        public int FourthStoveLv;
        /// <summary>
        /// 
        /// </summary>
        public int FifthStoveLv;
        /// <summary>
        /// 竞技场积分
        /// </summary>
        public int ArenaScore;
        /// <summary>
        /// 
        /// </summary>
        public List<int> Equips = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        public int OfficialLv;
        /// <summary>
        /// 战斗力
        /// </summary>
        public long ComatEffectiveness;
        /// <summary>
        /// 境界等级
        /// </summary>
        public int StateLv;

        public object Clone()
        {
            RobotData st = new RobotData();
            st.DataGuid = DataGuid;
            st.Name = Name;
            st.RobotType = RobotType;
            st.CreateTime = CreateTime;
            st.MapID = MapID;
            st.Job = Job;
            st.Sex = Sex;
            st.Level = Level;
            st.AITmplID = AITmplID;
            st.WeaponID = WeaponID;
            st.ClothesID = ClothesID;
            st.HP = HP;
            st.Energy = Energy;
            st.MinPhysicAtk = MinPhysicAtk;
            st.MaxPhysicAtk = MaxPhysicAtk;
            st.MinPhysicDef = MinPhysicDef;
            st.MaxPhysicDef = MaxPhysicDef;
            st.MinMagicAtk = MinMagicAtk;
            st.RestoreHP = RestoreHP;
            st.MaxMagicAtk = MaxMagicAtk;
            st.MinMagicDef = MinMagicDef;
            st.MaxMagicDef = MaxMagicDef;
            st.MinTaoistAtk = MinTaoistAtk;
            st.MaxTaoistAtk = MaxTaoistAtk;
            st.FirstStoveLv = FirstStoveLv;
            st.SecondStoveLv = SecondStoveLv;
            st.ThirdStoveLv = ThirdStoveLv;
            st.FourthStoveLv = FourthStoveLv;
            st.FifthStoveLv = FifthStoveLv;
            st.ArenaScore = ArenaScore;
            foreach (var item in Equips)
            {
                st.Equips.Add(item);
            }
            st.OfficialLv = OfficialLv;
            st.ComatEffectiveness = ComatEffectiveness;
            st.StateLv = StateLv;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(DataGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(RobotType);
                oByteArray.WriteInt(CreateTime);
                oByteArray.WriteInt(MapID);
                oByteArray.WriteInt(Job);
                oByteArray.WriteInt(Sex);
                oByteArray.WriteInt(Level);
                oByteArray.WriteUInt(AITmplID);
                oByteArray.WriteInt(WeaponID);
                oByteArray.WriteInt(ClothesID);
                oByteArray.WriteInt64(HP);
                oByteArray.WriteInt64(Energy);
                oByteArray.WriteInt64(MinPhysicAtk);
                oByteArray.WriteInt64(MaxPhysicAtk);
                oByteArray.WriteInt64(MinPhysicDef);
                oByteArray.WriteInt64(MaxPhysicDef);
                oByteArray.WriteInt64(MinMagicAtk);
                oByteArray.WriteInt64(RestoreHP);
                oByteArray.WriteInt64(MaxMagicAtk);
                oByteArray.WriteInt64(MinMagicDef);
                oByteArray.WriteInt64(MaxMagicDef);
                oByteArray.WriteInt64(MinTaoistAtk);
                oByteArray.WriteInt64(MaxTaoistAtk);
                oByteArray.WriteInt(FirstStoveLv);
                oByteArray.WriteInt(SecondStoveLv);
                oByteArray.WriteInt(ThirdStoveLv);
                oByteArray.WriteInt(FourthStoveLv);
                oByteArray.WriteInt(FifthStoveLv);
                oByteArray.WriteInt(ArenaScore);
                oByteArray.WriteUShort((ushort)Equips.Count);
                for (int i = 0; i < Equips.Count; i++)
                {
                    oByteArray.WriteInt(Equips[i]);
                }
                oByteArray.WriteInt(OfficialLv);
                oByteArray.WriteInt64(ComatEffectiveness);
                oByteArray.WriteInt(StateLv);
            }
            else
            {
                DataGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                RobotType = oByteArray.ReadInt();
                CreateTime = oByteArray.ReadInt();
                MapID = oByteArray.ReadInt();
                Job = oByteArray.ReadInt();
                Sex = oByteArray.ReadInt();
                Level = oByteArray.ReadInt();
                AITmplID = oByteArray.ReadUInt();
                WeaponID = oByteArray.ReadInt();
                ClothesID = oByteArray.ReadInt();
                HP = oByteArray.ReadInt64();
                Energy = oByteArray.ReadInt64();
                MinPhysicAtk = oByteArray.ReadInt64();
                MaxPhysicAtk = oByteArray.ReadInt64();
                MinPhysicDef = oByteArray.ReadInt64();
                MaxPhysicDef = oByteArray.ReadInt64();
                MinMagicAtk = oByteArray.ReadInt64();
                RestoreHP = oByteArray.ReadInt64();
                MaxMagicAtk = oByteArray.ReadInt64();
                MinMagicDef = oByteArray.ReadInt64();
                MaxMagicDef = oByteArray.ReadInt64();
                MinTaoistAtk = oByteArray.ReadInt64();
                MaxTaoistAtk = oByteArray.ReadInt64();
                FirstStoveLv = oByteArray.ReadInt();
                SecondStoveLv = oByteArray.ReadInt();
                ThirdStoveLv = oByteArray.ReadInt();
                FourthStoveLv = oByteArray.ReadInt();
                FifthStoveLv = oByteArray.ReadInt();
                ArenaScore = oByteArray.ReadInt();
                int EquipsCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < EquipsCount; i++)
                {
                    Equips.Add(oByteArray.ReadInt());
                }
                OfficialLv = oByteArray.ReadInt();
                ComatEffectiveness = oByteArray.ReadInt64();
                StateLv = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            DataGuid = 0;
            Name = string.Empty;
            RobotType = 0;
            CreateTime = 0;
            MapID = 0;
            Job = 0;
            Sex = 0;
            Level = 0;
            AITmplID = 0;
            WeaponID = 0;
            ClothesID = 0;
            HP = 0;
            Energy = 0;
            MinPhysicAtk = 0;
            MaxPhysicAtk = 0;
            MinPhysicDef = 0;
            MaxPhysicDef = 0;
            MinMagicAtk = 0;
            RestoreHP = 0;
            MaxMagicAtk = 0;
            MinMagicDef = 0;
            MaxMagicDef = 0;
            MinTaoistAtk = 0;
            MaxTaoistAtk = 0;
            FirstStoveLv = 0;
            SecondStoveLv = 0;
            ThirdStoveLv = 0;
            FourthStoveLv = 0;
            FifthStoveLv = 0;
            ArenaScore = 0;
            Equips.Clear();
            OfficialLv = 0;
            ComatEffectiveness = 0;
            StateLv = 0;

        }
    }

    /// <summary>
    /// 竞技场排行榜信息
    /// </summary>
    public class ArenaRankData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 
        /// </summary>
        public string NickName;
        /// <summary>
        /// 
        /// </summary>
        public uint Score;
        /// <summary>
        /// 
        /// </summary>
        public uint Rank;

        public object Clone()
        {
            ArenaRankData st = new ArenaRankData();
            st.PlayerGuid = PlayerGuid;
            st.NickName = NickName;
            st.Score = Score;
            st.Rank = Rank;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteUInt(Score);
                oByteArray.WriteUInt(Rank);
            }
            else
            {
                PlayerGuid = oByteArray.ReadUInt64();
                NickName = oByteArray.ReadUTF();
                Score = oByteArray.ReadUInt();
                Rank = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerGuid = 0;
            NickName = string.Empty;
            Score = 0;
            Rank = 0;

        }
    }

    /// <summary>
    /// 玩家竞技场信息
    /// </summary>
    public class PlayerArenaProtocolData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 排名
        /// </summary>
        public uint Rank;
        /// <summary>
        /// 当前分数
        /// </summary>
        public int Score;
        /// <summary>
        /// 历史最高分数
        /// </summary>
        public int HistoryMaxScore;
        /// <summary>
        /// 是否购买尊享奖励
        /// </summary>
        public bool HasBuy;
        /// <summary>
        /// 是否已领取结算奖励
        /// </summary>
        public bool HasTakeSettlementAward;
        /// <summary>
        /// 最近5场历史战绩 0负 1胜
        /// </summary>
        public List<uint> HistoryRecord = new List<uint>();
        /// <summary>
        /// 已获取奖励列表
        /// </summary>
        public List<uint> AcquiredAwardList = new List<uint>();
        /// <summary>
        /// 已获取尊享奖励列表
        /// </summary>
        public List<uint> AcquiredExAwardList = new List<uint>();
        /// <summary>
        /// 赛季总胜场
        /// </summary>
        public uint TotalWinCount;
        /// <summary>
        /// 赛季总负场
        /// </summary>
        public uint TotalLoseCount;

        public object Clone()
        {
            PlayerArenaProtocolData st = new PlayerArenaProtocolData();
            st.PlayerGuid = PlayerGuid;
            st.Name = Name;
            st.Rank = Rank;
            st.Score = Score;
            st.HistoryMaxScore = HistoryMaxScore;
            st.HasBuy = HasBuy;
            st.HasTakeSettlementAward = HasTakeSettlementAward;
            foreach (var item in HistoryRecord)
            {
                st.HistoryRecord.Add(item);
            }
            foreach (var item in AcquiredAwardList)
            {
                st.AcquiredAwardList.Add(item);
            }
            foreach (var item in AcquiredExAwardList)
            {
                st.AcquiredExAwardList.Add(item);
            }
            st.TotalWinCount = TotalWinCount;
            st.TotalLoseCount = TotalLoseCount;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteUInt(Rank);
                oByteArray.WriteInt(Score);
                oByteArray.WriteInt(HistoryMaxScore);
                oByteArray.WriteBoolean(HasBuy);
                oByteArray.WriteBoolean(HasTakeSettlementAward);
                oByteArray.WriteUShort((ushort)HistoryRecord.Count);
                for (int i = 0; i < HistoryRecord.Count; i++)
                {
                    oByteArray.WriteUInt(HistoryRecord[i]);
                }
                oByteArray.WriteUShort((ushort)AcquiredAwardList.Count);
                for (int i = 0; i < AcquiredAwardList.Count; i++)
                {
                    oByteArray.WriteUInt(AcquiredAwardList[i]);
                }
                oByteArray.WriteUShort((ushort)AcquiredExAwardList.Count);
                for (int i = 0; i < AcquiredExAwardList.Count; i++)
                {
                    oByteArray.WriteUInt(AcquiredExAwardList[i]);
                }
                oByteArray.WriteUInt(TotalWinCount);
                oByteArray.WriteUInt(TotalLoseCount);
            }
            else
            {
                PlayerGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                Rank = oByteArray.ReadUInt();
                Score = oByteArray.ReadInt();
                HistoryMaxScore = oByteArray.ReadInt();
                HasBuy = oByteArray.ReadBoolean();
                HasTakeSettlementAward = oByteArray.ReadBoolean();
                int HistoryRecordCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < HistoryRecordCount; i++)
                {
                    HistoryRecord.Add(oByteArray.ReadUInt());
                }
                int AcquiredAwardListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AcquiredAwardListCount; i++)
                {
                    AcquiredAwardList.Add(oByteArray.ReadUInt());
                }
                int AcquiredExAwardListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AcquiredExAwardListCount; i++)
                {
                    AcquiredExAwardList.Add(oByteArray.ReadUInt());
                }
                TotalWinCount = oByteArray.ReadUInt();
                TotalLoseCount = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerGuid = 0;
            Name = string.Empty;
            Rank = 0;
            Score = 0;
            HistoryMaxScore = 0;
            HasBuy = false;
            HasTakeSettlementAward = false;
            HistoryRecord.Clear();
            AcquiredAwardList.Clear();
            AcquiredExAwardList.Clear();
            TotalWinCount = 0;
            TotalLoseCount = 0;

        }
    }

    /// <summary>
    /// 帮派跨服活动小地图信息
    /// </summary>
    public class TURBMiniMapInfo : IStruct
    {
        /// <summary>
        /// 0:BOSS;1:玩家自己;2:敌人;3:会长;4:堂主;5：同伴
        /// </summary>
        public uint Type;
        /// <summary>
        /// 
        /// </summary>
        public uint MapX;
        /// <summary>
        /// 
        /// </summary>
        public uint MapY;
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo ObjectID = new ObjectGuidInfo();

        public object Clone()
        {
            TURBMiniMapInfo st = new TURBMiniMapInfo();
            st.Type = Type;
            st.MapX = MapX;
            st.MapY = MapY;
            st.ObjectID = ObjectID.Clone() as ObjectGuidInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Type);
                oByteArray.WriteUInt(MapX);
                oByteArray.WriteUInt(MapY);
                ObjectID.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                Type = oByteArray.ReadUInt();
                MapX = oByteArray.ReadUInt();
                MapY = oByteArray.ReadUInt();
                ObjectID.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            Type = 0;
            MapX = 0;
            MapY = 0;
            ObjectID = default(ObjectGuidInfo);

        }
    }

    /// <summary>
    /// 直充数据
    /// </summary>
    public class RechargeBuyData : IStruct
    {
        /// <summary>
        /// 直充类型  1为组装活动直充
        /// </summary>
        public uint BuyType;
        /// <summary>
        /// 
        /// </summary>
        public uint Data1;
        /// <summary>
        /// 
        /// </summary>
        public uint Data2;
        /// <summary>
        /// 
        /// </summary>
        public uint Data3;
        /// <summary>
        /// 
        /// </summary>
        public uint Data4;
        /// <summary>
        /// 
        /// </summary>
        public uint Data5;
        /// <summary>
        /// 
        /// </summary>
        public uint Data6;

        public object Clone()
        {
            RechargeBuyData st = new RechargeBuyData();
            st.BuyType = BuyType;
            st.Data1 = Data1;
            st.Data2 = Data2;
            st.Data3 = Data3;
            st.Data4 = Data4;
            st.Data5 = Data5;
            st.Data6 = Data6;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(BuyType);
                oByteArray.WriteUInt(Data1);
                oByteArray.WriteUInt(Data2);
                oByteArray.WriteUInt(Data3);
                oByteArray.WriteUInt(Data4);
                oByteArray.WriteUInt(Data5);
                oByteArray.WriteUInt(Data6);
            }
            else
            {
                BuyType = oByteArray.ReadUInt();
                Data1 = oByteArray.ReadUInt();
                Data2 = oByteArray.ReadUInt();
                Data3 = oByteArray.ReadUInt();
                Data4 = oByteArray.ReadUInt();
                Data5 = oByteArray.ReadUInt();
                Data6 = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            BuyType = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
            Data4 = 0;
            Data5 = 0;
            Data6 = 0;

        }
    }

    /// <summary>
    /// 玩家累充信息
    /// </summary>
    public class PlayerRechargeData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Cycle;
        /// <summary>
        /// 
        /// </summary>
        public uint State;
        /// <summary>
        /// 
        /// </summary>
        public uint HonourValue;
        /// <summary>
        /// 
        /// </summary>
        public uint LastCumulRechargeTime;
        /// <summary>
        /// 
        /// </summary>
        public uint RechargeDays;
        /// <summary>
        /// 
        /// </summary>
        public string CumulRechargeInfo;

        public object Clone()
        {
            PlayerRechargeData st = new PlayerRechargeData();
            st.Cycle = Cycle;
            st.State = State;
            st.HonourValue = HonourValue;
            st.LastCumulRechargeTime = LastCumulRechargeTime;
            st.RechargeDays = RechargeDays;
            st.CumulRechargeInfo = CumulRechargeInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Cycle);
                oByteArray.WriteUInt(State);
                oByteArray.WriteUInt(HonourValue);
                oByteArray.WriteUInt(LastCumulRechargeTime);
                oByteArray.WriteUInt(RechargeDays);
                oByteArray.WriteUTF(CumulRechargeInfo);
            }
            else
            {
                Cycle = oByteArray.ReadUInt();
                State = oByteArray.ReadUInt();
                HonourValue = oByteArray.ReadUInt();
                LastCumulRechargeTime = oByteArray.ReadUInt();
                RechargeDays = oByteArray.ReadUInt();
                CumulRechargeInfo = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            Cycle = 0;
            State = 0;
            HonourValue = 0;
            LastCumulRechargeTime = 0;
            RechargeDays = 0;
            CumulRechargeInfo = string.Empty;

        }
    }

    /// <summary>
    /// 五行系统信息
    /// </summary>
    public class FiveElementsData : IStruct
    {
        /// <summary>
        /// 金灵珠等级
        /// </summary>
        public uint GoldLv;
        /// <summary>
        /// 金灵珠经验
        /// </summary>
        public uint GoldExp;
        /// <summary>
        /// 木灵珠等级
        /// </summary>
        public uint WoodLv;
        /// <summary>
        /// 木灵珠经验
        /// </summary>
        public uint WoodExp;
        /// <summary>
        /// 水灵珠等级
        /// </summary>
        public uint WaterLv;
        /// <summary>
        /// 水灵珠经验
        /// </summary>
        public uint WaterExp;
        /// <summary>
        /// 火灵珠等级
        /// </summary>
        public uint FireLv;
        /// <summary>
        /// 火灵珠经验
        /// </summary>
        public uint FireExp;
        /// <summary>
        /// 土灵珠等级
        /// </summary>
        public uint EarthLv;
        /// <summary>
        /// 土灵珠经验
        /// </summary>
        public uint EarthExp;

        public object Clone()
        {
            FiveElementsData st = new FiveElementsData();
            st.GoldLv = GoldLv;
            st.GoldExp = GoldExp;
            st.WoodLv = WoodLv;
            st.WoodExp = WoodExp;
            st.WaterLv = WaterLv;
            st.WaterExp = WaterExp;
            st.FireLv = FireLv;
            st.FireExp = FireExp;
            st.EarthLv = EarthLv;
            st.EarthExp = EarthExp;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(GoldLv);
                oByteArray.WriteUInt(GoldExp);
                oByteArray.WriteUInt(WoodLv);
                oByteArray.WriteUInt(WoodExp);
                oByteArray.WriteUInt(WaterLv);
                oByteArray.WriteUInt(WaterExp);
                oByteArray.WriteUInt(FireLv);
                oByteArray.WriteUInt(FireExp);
                oByteArray.WriteUInt(EarthLv);
                oByteArray.WriteUInt(EarthExp);
            }
            else
            {
                GoldLv = oByteArray.ReadUInt();
                GoldExp = oByteArray.ReadUInt();
                WoodLv = oByteArray.ReadUInt();
                WoodExp = oByteArray.ReadUInt();
                WaterLv = oByteArray.ReadUInt();
                WaterExp = oByteArray.ReadUInt();
                FireLv = oByteArray.ReadUInt();
                FireExp = oByteArray.ReadUInt();
                EarthLv = oByteArray.ReadUInt();
                EarthExp = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            GoldLv = 0;
            GoldExp = 0;
            WoodLv = 0;
            WoodExp = 0;
            WaterLv = 0;
            WaterExp = 0;
            FireLv = 0;
            FireExp = 0;
            EarthLv = 0;
            EarthExp = 0;

        }
    }

    /// <summary>
    /// 藏宝图地图怪物信息
    /// </summary>
    public class TreasureMapMonData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 0:存活 1:死亡未进入复活倒计时 2:复活中
        /// </summary>
        public uint MonStatus;
        /// <summary>
        /// 复活时间
        /// </summary>
        public uint ReviveTime;
        /// <summary>
        /// 
        /// </summary>
        public uint MapX;
        /// <summary>
        /// 
        /// </summary>
        public uint MapY;
        /// <summary>
        /// 
        /// </summary>
        public ulong MonGuid;

        public object Clone()
        {
            TreasureMapMonData st = new TreasureMapMonData();
            st.TID = TID;
            st.MonStatus = MonStatus;
            st.ReviveTime = ReviveTime;
            st.MapX = MapX;
            st.MapY = MapY;
            st.MonGuid = MonGuid;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(MonStatus);
                oByteArray.WriteUInt(ReviveTime);
                oByteArray.WriteUInt(MapX);
                oByteArray.WriteUInt(MapY);
                oByteArray.WriteUInt64(MonGuid);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                MonStatus = oByteArray.ReadUInt();
                ReviveTime = oByteArray.ReadUInt();
                MapX = oByteArray.ReadUInt();
                MapY = oByteArray.ReadUInt();
                MonGuid = oByteArray.ReadUInt64();
            }
        }


        public void Reset()
        {
            TID = 0;
            MonStatus = 0;
            ReviveTime = 0;
            MapX = 0;
            MapY = 0;
            MonGuid = 0;

        }
    }

    /// <summary>
    /// 客户端数据
    /// </summary>
    public class ClientData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Key;
        /// <summary>
        /// 
        /// </summary>
        public string Value;

        public object Clone()
        {
            ClientData st = new ClientData();
            st.Key = Key;
            st.Value = Value;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Key);
                oByteArray.WriteUTF(Value);
            }
            else
            {
                Key = oByteArray.ReadUInt();
                Value = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            Key = 0;
            Value = string.Empty;

        }
    }

    /// <summary>
    /// 熔炉抽奖日志
    /// </summary>
    public class SmelterdrawLog : IStruct
    {
        /// <summary>
        /// SmelterdrawTemplate[TID]
        /// </summary>
        public uint TID;
        /// <summary>
        /// 物品ID
        /// </summary>
        public uint ItemID;
        /// <summary>
        /// 物品数量
        /// </summary>
        public uint ItemNum;
        /// <summary>
        /// 0:积分兑换;1:天工抽奖;2:荒古抽奖
        /// </summary>
        public uint LogType;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;
        /// <summary>
        /// 是否稀有
        /// </summary>
        public int RareLog;

        public object Clone()
        {
            SmelterdrawLog st = new SmelterdrawLog();
            st.TID = TID;
            st.ItemID = ItemID;
            st.ItemNum = ItemNum;
            st.LogType = LogType;
            st.NickName = NickName;
            st.RareLog = RareLog;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(ItemID);
                oByteArray.WriteUInt(ItemNum);
                oByteArray.WriteUInt(LogType);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteInt(RareLog);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                ItemID = oByteArray.ReadUInt();
                ItemNum = oByteArray.ReadUInt();
                LogType = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
                RareLog = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            ItemID = 0;
            ItemNum = 0;
            LogType = 0;
            NickName = string.Empty;
            RareLog = 0;

        }
    }

    /// <summary>
    /// 精灵
    /// </summary>
    public class Elves : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 等级
        /// </summary>
        public uint Level;
        /// <summary>
        /// 特效激活状态
        /// </summary>
        public uint EffectStatus;

        public object Clone()
        {
            Elves st = new Elves();
            st.TID = TID;
            st.Level = Level;
            st.EffectStatus = EffectStatus;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(Level);
                oByteArray.WriteUInt(EffectStatus);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                Level = oByteArray.ReadUInt();
                EffectStatus = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            TID = 0;
            Level = 0;
            EffectStatus = 0;

        }
    }

    /// <summary>
    /// 精灵列表
    /// </summary>
    public class PlayerElvesData : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Elves> Data = new List<Elves>();
        /// <summary>
        /// 跟随精灵tid
        /// </summary>
        public uint FollowElvs;

        public object Clone()
        {
            PlayerElvesData st = new PlayerElvesData();
            foreach (Elves item in Data)
            {
                st.Data.Add(item.Clone() as Elves);
            }
            st.FollowElvs = FollowElvs;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUShort((ushort)Data.Count);
                for (int i = 0; i < Data.Count; i++)
                {
                    Data[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteUInt(FollowElvs);
            }
            else
            {
                int DataCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < DataCount; i++)
                {
                    Elves obj = new Elves();
                    obj.Serializtion(oByteArray, bSerialize);
                    Data.Add(obj);
                }
                FollowElvs = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Data.Clear();
            FollowElvs = 0;

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PPoint : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int X;
        /// <summary>
        /// 
        /// </summary>
        public int Y;

        public object Clone()
        {
            PPoint st = new PPoint();
            st.X = X;
            st.Y = Y;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(X);
                oByteArray.WriteInt(Y);
            }
            else
            {
                X = oByteArray.ReadInt();
                Y = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            X = 0;
            Y = 0;

        }
    }

    /// <summary>
    /// 行会排名信息
    /// </summary>
    public class GuildRankInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo Guid = new ObjectGuidInfo();
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public int Value;
        /// <summary>
        /// 
        /// </summary>
        public int Rank;

        public object Clone()
        {
            GuildRankInfo st = new GuildRankInfo();
            st.Guid = Guid.Clone() as ObjectGuidInfo;
            st.Name = Name;
            st.Value = Value;
            st.Rank = Rank;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                Guid.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(Value);
                oByteArray.WriteInt(Rank);
            }
            else
            {
                Guid.Serializtion(oByteArray, bSerialize);
                Name = oByteArray.ReadUTF();
                Value = oByteArray.ReadInt();
                Rank = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            Guid = default(ObjectGuidInfo);
            Name = string.Empty;
            Value = 0;
            Rank = 0;

        }
    }

    /// <summary>
    /// 武道会押注信息
    /// </summary>
    public class BudokaiBetInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint RoomIdx;
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 
        /// </summary>
        public uint Count;

        public object Clone()
        {
            BudokaiBetInfo st = new BudokaiBetInfo();
            st.RoomIdx = RoomIdx;
            st.PlayerGuid = PlayerGuid;
            st.Count = Count;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(RoomIdx);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUInt(Count);
            }
            else
            {
                RoomIdx = oByteArray.ReadUInt();
                PlayerGuid = oByteArray.ReadUInt64();
                Count = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            RoomIdx = 0;
            PlayerGuid = 0;
            Count = 0;

        }
    }

    /// <summary>
    /// 跨服竞技场玩家匹配信息
    /// </summary>
    public class CSArenaPlayerMatchingData : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string Name;
        /// <summary>
        /// 当前分数
        /// </summary>
        public int Score;
        /// <summary>
        /// 职业
        /// </summary>
        public uint Career;
        /// <summary>
        /// 性别
        /// </summary>
        public uint Sex;
        /// <summary>
        /// 玩家境界等级
        /// </summary>
        public uint StateLv;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public uint Lv;
        /// <summary>
        /// 玩家战力
        /// </summary>
        public long ComatEffectiveness;
        /// <summary>
        /// 玩家拍卖
        /// </summary>
        public uint Rank;

        public object Clone()
        {
            CSArenaPlayerMatchingData st = new CSArenaPlayerMatchingData();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.Name = Name;
            st.Score = Score;
            st.Career = Career;
            st.Sex = Sex;
            st.StateLv = StateLv;
            st.Lv = Lv;
            st.ComatEffectiveness = ComatEffectiveness;
            st.Rank = Rank;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(Score);
                oByteArray.WriteUInt(Career);
                oByteArray.WriteUInt(Sex);
                oByteArray.WriteUInt(StateLv);
                oByteArray.WriteUInt(Lv);
                oByteArray.WriteInt64(ComatEffectiveness);
                oByteArray.WriteUInt(Rank);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                Score = oByteArray.ReadInt();
                Career = oByteArray.ReadUInt();
                Sex = oByteArray.ReadUInt();
                StateLv = oByteArray.ReadUInt();
                Lv = oByteArray.ReadUInt();
                ComatEffectiveness = oByteArray.ReadInt64();
                Rank = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            Name = string.Empty;
            Score = 0;
            Career = 0;
            Sex = 0;
            StateLv = 0;
            Lv = 0;
            ComatEffectiveness = 0;
            Rank = 0;

        }
    }

    /// <summary>
    /// 跨服竞技场战斗信息
    /// </summary>
    public class CSArenaFightData : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 击杀数
        /// </summary>
        public int KillCount;
        /// <summary>
        /// 已复活次数
        /// </summary>
        public int ReviveCount;
        /// <summary>
        /// 阵营 1红队 2蓝队
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 是否已离开战场
        /// </summary>
        public bool IsLeave;

        public object Clone()
        {
            CSArenaFightData st = new CSArenaFightData();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.Name = Name;
            st.KillCount = KillCount;
            st.ReviveCount = ReviveCount;
            st.Camp = Camp;
            st.IsLeave = IsLeave;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(KillCount);
                oByteArray.WriteInt(ReviveCount);
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteBoolean(IsLeave);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                KillCount = oByteArray.ReadInt();
                ReviveCount = oByteArray.ReadInt();
                Camp = oByteArray.ReadUInt();
                IsLeave = oByteArray.ReadBoolean();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            Name = string.Empty;
            KillCount = 0;
            ReviveCount = 0;
            Camp = 0;
            IsLeave = false;

        }
    }

    /// <summary>
    /// 跨服竞技场结算信息
    /// </summary>
    public class CSArenaSettlementData : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string Name;
        /// <summary>
        /// 击杀数
        /// </summary>
        public int KillCount;
        /// <summary>
        /// 结算分
        /// </summary>
        public int Score;
        /// <summary>
        /// 是否提前离开
        /// </summary>
        public bool LeaveEarly;
        /// <summary>
        /// 总分
        /// </summary>
        public int TotalScore;

        public object Clone()
        {
            CSArenaSettlementData st = new CSArenaSettlementData();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.Name = Name;
            st.KillCount = KillCount;
            st.Score = Score;
            st.LeaveEarly = LeaveEarly;
            st.TotalScore = TotalScore;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(KillCount);
                oByteArray.WriteInt(Score);
                oByteArray.WriteBoolean(LeaveEarly);
                oByteArray.WriteInt(TotalScore);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                KillCount = oByteArray.ReadInt();
                Score = oByteArray.ReadInt();
                LeaveEarly = oByteArray.ReadBoolean();
                TotalScore = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            Name = string.Empty;
            KillCount = 0;
            Score = 0;
            LeaveEarly = false;
            TotalScore = 0;

        }
    }

    /// <summary>
    /// 跨服竞技场排行信息
    /// </summary>
    public class CSArenaRankInfo : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string Name;
        /// <summary>
        /// 玩家当前分数
        /// </summary>
        public int Score;
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank;

        public object Clone()
        {
            CSArenaRankInfo st = new CSArenaRankInfo();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.Name = Name;
            st.Score = Score;
            st.Rank = Rank;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(Score);
                oByteArray.WriteInt(Rank);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                Score = oByteArray.ReadInt();
                Rank = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            Name = string.Empty;
            Score = 0;
            Rank = 0;

        }
    }

    /// <summary>
    /// 时装信息
    /// </summary>
    public class PlayerFashionData : IStruct
    {
        /// <summary>
        /// 称号星级信息
        /// </summary>
        public string TitleStarInfo;
        /// <summary>
        /// 金装星级信息
        /// </summary>
        public string FashionStarInfo;

        public object Clone()
        {
            PlayerFashionData st = new PlayerFashionData();
            st.TitleStarInfo = TitleStarInfo;
            st.FashionStarInfo = FashionStarInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(TitleStarInfo);
                oByteArray.WriteUTF(FashionStarInfo);
            }
            else
            {
                TitleStarInfo = oByteArray.ReadUTF();
                FashionStarInfo = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            TitleStarInfo = string.Empty;
            FashionStarInfo = string.Empty;

        }
    }

    /// <summary>
    /// 龙族宝藏物品日志
    /// </summary>
    public class DragonTreasureDrawLog : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint TID;
        /// <summary>
        /// 物品ID
        /// </summary>
        public uint ItemID;
        /// <summary>
        /// 物品数量
        /// </summary>
        public uint ItemNum;
        /// <summary>
        /// 0:龙鳞兑换 1:宝藏探宝
        /// </summary>
        public uint LogType;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;

        public object Clone()
        {
            DragonTreasureDrawLog st = new DragonTreasureDrawLog();
            st.TID = TID;
            st.ItemID = ItemID;
            st.ItemNum = ItemNum;
            st.LogType = LogType;
            st.NickName = NickName;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUInt(ItemID);
                oByteArray.WriteUInt(ItemNum);
                oByteArray.WriteUInt(LogType);
                oByteArray.WriteUTF(NickName);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                ItemID = oByteArray.ReadUInt();
                ItemNum = oByteArray.ReadUInt();
                LogType = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            TID = 0;
            ItemID = 0;
            ItemNum = 0;
            LogType = 0;
            NickName = string.Empty;

        }
    }

    /// <summary>
    /// 怪物刷新时间调整
    /// </summary>
    public class MonsterReflashInfo : IStruct
    {
        /// <summary>
        /// 地图类型（废弃）
        /// </summary>
        public uint MapType;
        /// <summary>
        /// 怪物类型
        /// </summary>
        public uint MonsterType;
        /// <summary>
        /// 怪物等级
        /// </summary>
        public uint MonsterLevel;
        /// <summary>
        /// 刷新比率(万分比)
        /// </summary>
        public uint RefreshTimeRation;
        /// <summary>
        /// 地图类型
        /// </summary>
        public List<uint> MapTypes = new List<uint>();

        public object Clone()
        {
            MonsterReflashInfo st = new MonsterReflashInfo();
            st.MapType = MapType;
            st.MonsterType = MonsterType;
            st.MonsterLevel = MonsterLevel;
            st.RefreshTimeRation = RefreshTimeRation;
            foreach (var item in MapTypes)
            {
                st.MapTypes.Add(item);
            }
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MapType);
                oByteArray.WriteUInt(MonsterType);
                oByteArray.WriteUInt(MonsterLevel);
                oByteArray.WriteUInt(RefreshTimeRation);
                oByteArray.WriteUShort((ushort)MapTypes.Count);
                for (int i = 0; i < MapTypes.Count; i++)
                {
                    oByteArray.WriteUInt(MapTypes[i]);
                }
            }
            else
            {
                MapType = oByteArray.ReadUInt();
                MonsterType = oByteArray.ReadUInt();
                MonsterLevel = oByteArray.ReadUInt();
                RefreshTimeRation = oByteArray.ReadUInt();
                int MapTypesCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < MapTypesCount; i++)
                {
                    MapTypes.Add(oByteArray.ReadUInt());
                }
            }
        }


        public void Reset()
        {
            MapType = 0;
            MonsterType = 0;
            MonsterLevel = 0;
            RefreshTimeRation = 0;
            MapTypes.Clear();

        }
    }

    /// <summary>
    /// 平台下每个服务器的最新版本号
    /// </summary>
    public class PlatformVersion : IStruct
    {
        /// <summary>
        /// 平台ID
        /// </summary>
        public string PlatformID;
        /// <summary>
        /// 缓存服版本
        /// </summary>
        public string CacheVersion;
        /// <summary>
        /// 世界服版本
        /// </summary>
        public string WorldVersion;
        /// <summary>
        /// 地图服版本
        /// </summary>
        public string MapVersion;
        /// <summary>
        /// 网关服版本
        /// </summary>
        public string GatewayVersion;
        /// <summary>
        /// 登录服版本
        /// </summary>
        public string LoginVersion;

        public object Clone()
        {
            PlatformVersion st = new PlatformVersion();
            st.PlatformID = PlatformID;
            st.CacheVersion = CacheVersion;
            st.WorldVersion = WorldVersion;
            st.MapVersion = MapVersion;
            st.GatewayVersion = GatewayVersion;
            st.LoginVersion = LoginVersion;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(PlatformID);
                oByteArray.WriteUTF(CacheVersion);
                oByteArray.WriteUTF(WorldVersion);
                oByteArray.WriteUTF(MapVersion);
                oByteArray.WriteUTF(GatewayVersion);
                oByteArray.WriteUTF(LoginVersion);
            }
            else
            {
                PlatformID = oByteArray.ReadUTF();
                CacheVersion = oByteArray.ReadUTF();
                WorldVersion = oByteArray.ReadUTF();
                MapVersion = oByteArray.ReadUTF();
                GatewayVersion = oByteArray.ReadUTF();
                LoginVersion = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            PlatformID = string.Empty;
            CacheVersion = string.Empty;
            WorldVersion = string.Empty;
            MapVersion = string.Empty;
            GatewayVersion = string.Empty;
            LoginVersion = string.Empty;

        }
    }

    /// <summary>
    /// 盛大平台信息
    /// </summary>
    public class SODPlatformData : IStruct
    {
        /// <summary>
        /// 盛大关注微信公众号礼包领取标识
        /// </summary>
        public uint WeChatAward;
        /// <summary>
        /// 盛大微端登录领取标识
        /// </summary>
        public uint MicroFirst;

        public object Clone()
        {
            SODPlatformData st = new SODPlatformData();
            st.WeChatAward = WeChatAward;
            st.MicroFirst = MicroFirst;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(WeChatAward);
                oByteArray.WriteUInt(MicroFirst);
            }
            else
            {
                WeChatAward = oByteArray.ReadUInt();
                MicroFirst = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            WeChatAward = 0;
            MicroFirst = 0;

        }
    }

    /// <summary>
    /// YY平台信息
    /// </summary>
    public class YYPlatformData : IStruct
    {
        /// <summary>
        /// 登录天数
        /// </summary>
        public uint loginDays;
        /// <summary>
        /// 登录奖励领取标识
        /// </summary>
        public uint loginAward;
        /// <summary>
        /// 玩家等级奖励
        /// </summary>
        public uint playerLvAward;
        /// <summary>
        /// yy等级奖励
        /// </summary>
        public uint yyLvAward;
        /// <summary>
        /// 平台等级
        /// </summary>
        public uint platformLv;
        /// <summary>
        /// 今日是否登录标识
        /// </summary>
        public byte isLoginToday;
        /// <summary>
        /// yy会员等级
        /// </summary>
        public uint yyVipLv;
        /// <summary>
        /// yy会员奖励领取标识
        /// </summary>
        public uint yyVipAward;
        /// <summary>
        /// yy会员奖励2领取标识
        /// </summary>
        public uint yyVipAward2;
        /// <summary>
        /// yy会员每日购买标识
        /// </summary>
        public uint yyVipDailyBuy;
        /// <summary>
        /// yy会员每周购买标识
        /// </summary>
        public uint yyVIPWeekBuy;
        /// <summary>
        /// 超玩等级
        /// </summary>
        public uint superVipLv;
        /// <summary>
        /// 超玩等级奖励领取标识
        /// </summary>
        public uint superVipAward;
        /// <summary>
        /// 超玩每日奖励领取标识
        /// </summary>
        public uint superVipDailyAward;
        /// <summary>
        /// 新手礼包奖励标识
        /// </summary>
        public uint FirstAward;
        /// <summary>
        /// YY关注微信礼包领取标识
        /// </summary>
        public uint WeChatAward;

        public object Clone()
        {
            YYPlatformData st = new YYPlatformData();
            st.loginDays = loginDays;
            st.loginAward = loginAward;
            st.playerLvAward = playerLvAward;
            st.yyLvAward = yyLvAward;
            st.platformLv = platformLv;
            st.isLoginToday = isLoginToday;
            st.yyVipLv = yyVipLv;
            st.yyVipAward = yyVipAward;
            st.yyVipAward2 = yyVipAward2;
            st.yyVipDailyBuy = yyVipDailyBuy;
            st.yyVIPWeekBuy = yyVIPWeekBuy;
            st.superVipLv = superVipLv;
            st.superVipAward = superVipAward;
            st.superVipDailyAward = superVipDailyAward;
            st.FirstAward = FirstAward;
            st.WeChatAward = WeChatAward;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(loginDays);
                oByteArray.WriteUInt(loginAward);
                oByteArray.WriteUInt(playerLvAward);
                oByteArray.WriteUInt(yyLvAward);
                oByteArray.WriteUInt(platformLv);
                oByteArray.WriteByte(isLoginToday);
                oByteArray.WriteUInt(yyVipLv);
                oByteArray.WriteUInt(yyVipAward);
                oByteArray.WriteUInt(yyVipAward2);
                oByteArray.WriteUInt(yyVipDailyBuy);
                oByteArray.WriteUInt(yyVIPWeekBuy);
                oByteArray.WriteUInt(superVipLv);
                oByteArray.WriteUInt(superVipAward);
                oByteArray.WriteUInt(superVipDailyAward);
                oByteArray.WriteUInt(FirstAward);
                oByteArray.WriteUInt(WeChatAward);
            }
            else
            {
                loginDays = oByteArray.ReadUInt();
                loginAward = oByteArray.ReadUInt();
                playerLvAward = oByteArray.ReadUInt();
                yyLvAward = oByteArray.ReadUInt();
                platformLv = oByteArray.ReadUInt();
                isLoginToday = oByteArray.ReadByte();
                yyVipLv = oByteArray.ReadUInt();
                yyVipAward = oByteArray.ReadUInt();
                yyVipAward2 = oByteArray.ReadUInt();
                yyVipDailyBuy = oByteArray.ReadUInt();
                yyVIPWeekBuy = oByteArray.ReadUInt();
                superVipLv = oByteArray.ReadUInt();
                superVipAward = oByteArray.ReadUInt();
                superVipDailyAward = oByteArray.ReadUInt();
                FirstAward = oByteArray.ReadUInt();
                WeChatAward = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            loginDays = 0;
            loginAward = 0;
            playerLvAward = 0;
            yyLvAward = 0;
            platformLv = 0;
            isLoginToday = 0;
            yyVipLv = 0;
            yyVipAward = 0;
            yyVipAward2 = 0;
            yyVipDailyBuy = 0;
            yyVIPWeekBuy = 0;
            superVipLv = 0;
            superVipAward = 0;
            superVipDailyAward = 0;
            FirstAward = 0;
            WeChatAward = 0;

        }
    }

    /// <summary>
    /// 360平台信息
    /// </summary>
    public class TSZPlatformData : IStruct
    {
        /// <summary>
        /// 新手奖励领取标识(微端)
        /// </summary>
        public uint MicroFirst;
        /// <summary>
        /// 本月已签到天数(微端)
        /// </summary>
        public uint SignDays;
        /// <summary>
        /// 今天是否已签到(微端)
        /// </summary>
        public uint SignToday;
        /// <summary>
        /// 大玩家等级
        /// </summary>
        public uint VipLv;
        /// <summary>
        /// 大玩家礼包领取标识
        /// </summary>
        public uint VipGift;

        public object Clone()
        {
            TSZPlatformData st = new TSZPlatformData();
            st.MicroFirst = MicroFirst;
            st.SignDays = SignDays;
            st.SignToday = SignToday;
            st.VipLv = VipLv;
            st.VipGift = VipGift;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
                oByteArray.WriteUInt(SignDays);
                oByteArray.WriteUInt(SignToday);
                oByteArray.WriteUInt(VipLv);
                oByteArray.WriteUInt(VipGift);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
                SignDays = oByteArray.ReadUInt();
                SignToday = oByteArray.ReadUInt();
                VipLv = oByteArray.ReadUInt();
                VipGift = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;
            SignDays = 0;
            SignToday = 0;
            VipLv = 0;
            VipGift = 0;

        }
    }

    /// <summary>
    /// 37平台信息
    /// </summary>
    public class TSPlatformData : IStruct
    {
        /// <summary>
        /// 新手奖励领取标识(微端)
        /// </summary>
        public uint MicroFirst;
        /// <summary>
        /// VIP会员等级
        /// </summary>
        public uint VipLv;
        /// <summary>
        /// 会员等级奖励领取标识
        /// </summary>
        public uint VipLvAward;
        /// <summary>
        /// 会员游戏等级奖励领取标识
        /// </summary>
        public uint GameLvAward;

        public object Clone()
        {
            TSPlatformData st = new TSPlatformData();
            st.MicroFirst = MicroFirst;
            st.VipLv = VipLv;
            st.VipLvAward = VipLvAward;
            st.GameLvAward = GameLvAward;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
                oByteArray.WriteUInt(VipLv);
                oByteArray.WriteUInt(VipLvAward);
                oByteArray.WriteUInt(GameLvAward);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
                VipLv = oByteArray.ReadUInt();
                VipLvAward = oByteArray.ReadUInt();
                GameLvAward = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;
            VipLv = 0;
            VipLvAward = 0;
            GameLvAward = 0;

        }
    }

    /// <summary>
    /// 2144平台信息
    /// </summary>
    public class TOFFPlatformData : IStruct
    {
        /// <summary>
        /// 2144微端登录礼包领取表示
        /// </summary>
        public uint MicroFirst;

        public object Clone()
        {
            TOFFPlatformData st = new TOFFPlatformData();
            st.MicroFirst = MicroFirst;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;

        }
    }

    /// <summary>
    /// 龙旗结构
    /// </summary>
    public class BannerProtoInfo : IStruct
    {
        /// <summary>
        /// 龙旗TID
        /// </summary>
        public uint TID;
        /// <summary>
        /// 龙旗归属帮派名称
        /// </summary>
        public string BannerName;

        public object Clone()
        {
            BannerProtoInfo st = new BannerProtoInfo();
            st.TID = TID;
            st.BannerName = BannerName;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteUTF(BannerName);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                BannerName = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            TID = 0;
            BannerName = string.Empty;

        }
    }

    /// <summary>
    /// 城门信息
    /// </summary>
    public class CellarInfo : IStruct
    {
        /// <summary>
        /// 城门id
        /// </summary>
        public uint TID;
        /// <summary>
        /// 城门是否开启
        /// </summary>
        public bool IsOpen;

        public object Clone()
        {
            CellarInfo st = new CellarInfo();
            st.TID = TID;
            st.IsOpen = IsOpen;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(TID);
                oByteArray.WriteBoolean(IsOpen);
            }
            else
            {
                TID = oByteArray.ReadUInt();
                IsOpen = oByteArray.ReadBoolean();
            }
        }


        public void Reset()
        {
            TID = 0;
            IsOpen = false;

        }
    }

    /// <summary>
    /// 贪玩平台信息
    /// </summary>
    public class TWPlatformData : IStruct
    {
        /// <summary>
        /// 贪玩微端登录礼包领取表示
        /// </summary>
        public uint MicroFirst;

        public object Clone()
        {
            TWPlatformData st = new TWPlatformData();
            st.MicroFirst = MicroFirst;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;

        }
    }

    /// <summary>
    /// 9377平台信息
    /// </summary>
    public class NTSSPlatformData : IStruct
    {
        /// <summary>
        /// 会员等级信息
        /// </summary>
        public uint VipLv;
        /// <summary>
        /// 会员等级奖励领取标识
        /// </summary>
        public uint VipLvAward;

        public object Clone()
        {
            NTSSPlatformData st = new NTSSPlatformData();
            st.VipLv = VipLv;
            st.VipLvAward = VipLvAward;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(VipLv);
                oByteArray.WriteUInt(VipLvAward);
            }
            else
            {
                VipLv = oByteArray.ReadUInt();
                VipLvAward = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            VipLv = 0;
            VipLvAward = 0;

        }
    }

    /// <summary>
    /// 跨服国战玩家数据
    /// </summary>
    public class CSNationalPlayerData : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank;
        /// <summary>
        /// 当前贡献积分
        /// </summary>
        public int Score;
        /// <summary>
        /// 当日已购粮草记录
        /// </summary>
        public List<ProtocolPair> ProvisionBuy = new List<ProtocolPair>();
        /// <summary>
        /// 国币商店购买记录
        /// </summary>
        public string BuyLimitInfo;
        /// <summary>
        /// 玩家当前所在地图的城池ID
        /// </summary>
        public uint CityID;

        public object Clone()
        {
            CSNationalPlayerData st = new CSNationalPlayerData();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.NickName = NickName;
            st.Rank = Rank;
            st.Score = Score;
            foreach (ProtocolPair item in ProvisionBuy)
            {
                st.ProvisionBuy.Add(item.Clone() as ProtocolPair);
            }
            st.BuyLimitInfo = BuyLimitInfo;
            st.CityID = CityID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteInt(Rank);
                oByteArray.WriteInt(Score);
                oByteArray.WriteUShort((ushort)ProvisionBuy.Count);
                for (int i = 0; i < ProvisionBuy.Count; i++)
                {
                    ProvisionBuy[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteUTF(BuyLimitInfo);
                oByteArray.WriteUInt(CityID);
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                NickName = oByteArray.ReadUTF();
                Rank = oByteArray.ReadInt();
                Score = oByteArray.ReadInt();
                int ProvisionBuyCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < ProvisionBuyCount; i++)
                {
                    ProtocolPair obj = new ProtocolPair();
                    obj.Serializtion(oByteArray, bSerialize);
                    ProvisionBuy.Add(obj);
                }
                BuyLimitInfo = oByteArray.ReadUTF();
                CityID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            NickName = string.Empty;
            Rank = 0;
            Score = 0;
            ProvisionBuy.Clear();
            BuyLimitInfo = string.Empty;
            CityID = 0;

        }
    }

    /// <summary>
    /// 帮派行军信息
    /// </summary>
    public class MarchGuild : IStruct
    {
        /// <summary>
        /// 帮派唯一ID
        /// </summary>
        public ulong GuildGuid;
        /// <summary>
        /// 该帮派名字
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 该帮派所属阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 帮派ID
        /// </summary>
        public ObjectGuidInfo GuildID = new ObjectGuidInfo();
        /// <summary>
        /// 行军类型
        /// </summary>
        public uint MarchType;
        /// <summary>
        /// 行军时间
        /// </summary>
        public uint MarchTime;

        public object Clone()
        {
            MarchGuild st = new MarchGuild();
            st.GuildGuid = GuildGuid;
            st.GuildName = GuildName;
            st.Camp = Camp;
            st.GuildID = GuildID.Clone() as ObjectGuidInfo;
            st.MarchType = MarchType;
            st.MarchTime = MarchTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(GuildGuid);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteUInt(Camp);
                GuildID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(MarchType);
                oByteArray.WriteUInt(MarchTime);
            }
            else
            {
                GuildGuid = oByteArray.ReadUInt64();
                GuildName = oByteArray.ReadUTF();
                Camp = oByteArray.ReadUInt();
                GuildID.Serializtion(oByteArray, bSerialize);
                MarchType = oByteArray.ReadUInt();
                MarchTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            GuildGuid = 0;
            GuildName = string.Empty;
            Camp = 0;
            GuildID = default(ObjectGuidInfo);
            MarchType = 0;
            MarchTime = 0;

        }
    }

    /// <summary>
    /// 国战城池信息
    /// </summary>
    public class CSNationalCityData : IStruct
    {
        /// <summary>
        /// 城池ID
        /// </summary>
        public uint CityID;
        /// <summary>
        /// 阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 所属帮派唯一ID
        /// </summary>
        public ulong GuildGuid;
        /// <summary>
        /// 所属帮派名
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 每小时粮草产出(弃用)
        /// </summary>
        public uint ProvisionHour;
        /// <summary>
        /// 每小时功绩(积分)产出(弃用)
        /// </summary>
        public uint SorceHour;
        /// <summary>
        /// 行军至该城池的帮派信息
        /// </summary>
        public List<MarchGuild> MarchGuilds = new List<MarchGuild>();
        /// <summary>
        /// 是否主城
        /// </summary>
        public bool MainCity;
        /// <summary>
        /// 该城池是否被敌方突袭
        /// </summary>
        public bool RapidMarch;
        /// <summary>
        /// 所属帮派ID
        /// </summary>
        public ObjectGuidInfo GuildID = new ObjectGuidInfo();
        /// <summary>
        /// 战斗副本创建时间(传递客户端协议用)
        /// </summary>
        public uint CreateTime;

        public object Clone()
        {
            CSNationalCityData st = new CSNationalCityData();
            st.CityID = CityID;
            st.Camp = Camp;
            st.GuildGuid = GuildGuid;
            st.GuildName = GuildName;
            st.ProvisionHour = ProvisionHour;
            st.SorceHour = SorceHour;
            foreach (MarchGuild item in MarchGuilds)
            {
                st.MarchGuilds.Add(item.Clone() as MarchGuild);
            }
            st.MainCity = MainCity;
            st.RapidMarch = RapidMarch;
            st.GuildID = GuildID.Clone() as ObjectGuidInfo;
            st.CreateTime = CreateTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(CityID);
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteUInt64(GuildGuid);
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteUInt(ProvisionHour);
                oByteArray.WriteUInt(SorceHour);
                oByteArray.WriteUShort((ushort)MarchGuilds.Count);
                for (int i = 0; i < MarchGuilds.Count; i++)
                {
                    MarchGuilds[i].Serializtion(oByteArray, bSerialize);
                }
                oByteArray.WriteBoolean(MainCity);
                oByteArray.WriteBoolean(RapidMarch);
                GuildID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(CreateTime);
            }
            else
            {
                CityID = oByteArray.ReadUInt();
                Camp = oByteArray.ReadUInt();
                GuildGuid = oByteArray.ReadUInt64();
                GuildName = oByteArray.ReadUTF();
                ProvisionHour = oByteArray.ReadUInt();
                SorceHour = oByteArray.ReadUInt();
                int MarchGuildsCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < MarchGuildsCount; i++)
                {
                    MarchGuild obj = new MarchGuild();
                    obj.Serializtion(oByteArray, bSerialize);
                    MarchGuilds.Add(obj);
                }
                MainCity = oByteArray.ReadBoolean();
                RapidMarch = oByteArray.ReadBoolean();
                GuildID.Serializtion(oByteArray, bSerialize);
                CreateTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            CityID = 0;
            Camp = 0;
            GuildGuid = 0;
            GuildName = string.Empty;
            ProvisionHour = 0;
            SorceHour = 0;
            MarchGuilds.Clear();
            MainCity = false;
            RapidMarch = false;
            GuildID = default(ObjectGuidInfo);
            CreateTime = 0;

        }
    }

    /// <summary>
    /// 跨服红包单个玩家数据
    /// </summary>
    public class UserTransferRedbag : IStruct
    {
        /// <summary>
        /// 玩家Guid
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家名称
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 货币数量
        /// </summary>
        public uint MoneyNum;
        /// <summary>
        /// 货币类型
        /// </summary>
        public int MoneyType;

        public object Clone()
        {
            UserTransferRedbag st = new UserTransferRedbag();
            st.PlayerGuid = PlayerGuid;
            st.PlayerName = PlayerName;
            st.MoneyNum = MoneyNum;
            st.MoneyType = MoneyType;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(PlayerName);
                oByteArray.WriteUInt(MoneyNum);
                oByteArray.WriteInt(MoneyType);
            }
            else
            {
                PlayerGuid = oByteArray.ReadUInt64();
                PlayerName = oByteArray.ReadUTF();
                MoneyNum = oByteArray.ReadUInt();
                MoneyType = oByteArray.ReadInt();
            }
        }


        public void Reset()
        {
            PlayerGuid = 0;
            PlayerName = string.Empty;
            MoneyNum = 0;
            MoneyType = 0;

        }
    }

    /// <summary>
    /// 国战帮派信息
    /// </summary>
    public class CSNationalGuildInfo : IStruct
    {
        /// <summary>
        /// 帮派唯一ID
        /// </summary>
        public ulong GuildGuid;
        /// <summary>
        /// 国战帮派所在阵营
        /// </summary>
        public uint GuildCamp;
        /// <summary>
        /// 帮派当前行军城池
        /// </summary>
        public uint MarchCity;
        /// <summary>
        /// 当前天已行军次数
        /// </summary>
        public uint MarchTimes;
        /// <summary>
        /// 当前天取消行军次数
        /// </summary>
        public uint CancelTimes;
        /// <summary>
        /// 当前拥有粮草(物资)数量
        /// </summary>
        public uint ProvisionCount;
        /// <summary>
        /// 帮派当前所在城池
        /// </summary>
        public uint CurCity;
        /// <summary>
        /// 上次行军粮草消耗
        /// </summary>
        public uint LastMarchCost;
        /// <summary>
        /// 上次行军城池
        /// </summary>
        public uint LastMarchCity;
        /// <summary>
        /// 积分
        /// </summary>
        public uint Score;
        /// <summary>
        /// 帮派对象ID
        /// </summary>
        public ObjectGuidInfo GuildID = new ObjectGuidInfo();

        public object Clone()
        {
            CSNationalGuildInfo st = new CSNationalGuildInfo();
            st.GuildGuid = GuildGuid;
            st.GuildCamp = GuildCamp;
            st.MarchCity = MarchCity;
            st.MarchTimes = MarchTimes;
            st.CancelTimes = CancelTimes;
            st.ProvisionCount = ProvisionCount;
            st.CurCity = CurCity;
            st.LastMarchCost = LastMarchCost;
            st.LastMarchCity = LastMarchCity;
            st.Score = Score;
            st.GuildID = GuildID.Clone() as ObjectGuidInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(GuildGuid);
                oByteArray.WriteUInt(GuildCamp);
                oByteArray.WriteUInt(MarchCity);
                oByteArray.WriteUInt(MarchTimes);
                oByteArray.WriteUInt(CancelTimes);
                oByteArray.WriteUInt(ProvisionCount);
                oByteArray.WriteUInt(CurCity);
                oByteArray.WriteUInt(LastMarchCost);
                oByteArray.WriteUInt(LastMarchCity);
                oByteArray.WriteUInt(Score);
                GuildID.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                GuildGuid = oByteArray.ReadUInt64();
                GuildCamp = oByteArray.ReadUInt();
                MarchCity = oByteArray.ReadUInt();
                MarchTimes = oByteArray.ReadUInt();
                CancelTimes = oByteArray.ReadUInt();
                ProvisionCount = oByteArray.ReadUInt();
                CurCity = oByteArray.ReadUInt();
                LastMarchCost = oByteArray.ReadUInt();
                LastMarchCity = oByteArray.ReadUInt();
                Score = oByteArray.ReadUInt();
                GuildID.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            GuildGuid = 0;
            GuildCamp = 0;
            MarchCity = 0;
            MarchTimes = 0;
            CancelTimes = 0;
            ProvisionCount = 0;
            CurCity = 0;
            LastMarchCost = 0;
            LastMarchCity = 0;
            Score = 0;
            GuildID = default(ObjectGuidInfo);

        }
    }

    /// <summary>
    /// 军备信息
    /// </summary>
    public class ArmsInfo : IStruct
    {
        /// <summary>
        /// 军备类型
        /// </summary>
        public uint ArmsType;
        /// <summary>
        /// 军备等级
        /// </summary>
        public uint ArmsLv;
        /// <summary>
        /// 当前经验值
        /// </summary>
        public uint CurExp;

        public object Clone()
        {
            ArmsInfo st = new ArmsInfo();
            st.ArmsType = ArmsType;
            st.ArmsLv = ArmsLv;
            st.CurExp = CurExp;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(ArmsType);
                oByteArray.WriteUInt(ArmsLv);
                oByteArray.WriteUInt(CurExp);
            }
            else
            {
                ArmsType = oByteArray.ReadUInt();
                ArmsLv = oByteArray.ReadUInt();
                CurExp = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            ArmsType = 0;
            ArmsLv = 0;
            CurExp = 0;

        }
    }

    /// <summary>
    /// 国战排行信息（玩家/行会）
    /// </summary>
    public class CSNationalRankInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong Guid;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 积分
        /// </summary>
        public int Score;
        /// <summary>
        /// 排名
        /// </summary>
        public uint Rank;
        /// <summary>
        /// 阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 对象ID
        /// </summary>
        public ObjectGuidInfo ObjectID = new ObjectGuidInfo();

        public object Clone()
        {
            CSNationalRankInfo st = new CSNationalRankInfo();
            st.Guid = Guid;
            st.Name = Name;
            st.Score = Score;
            st.Rank = Rank;
            st.Camp = Camp;
            st.ObjectID = ObjectID.Clone() as ObjectGuidInfo;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(Guid);
                oByteArray.WriteUTF(Name);
                oByteArray.WriteInt(Score);
                oByteArray.WriteUInt(Rank);
                oByteArray.WriteUInt(Camp);
                ObjectID.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                Guid = oByteArray.ReadUInt64();
                Name = oByteArray.ReadUTF();
                Score = oByteArray.ReadInt();
                Rank = oByteArray.ReadUInt();
                Camp = oByteArray.ReadUInt();
                ObjectID.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            Guid = 0;
            Name = string.Empty;
            Score = 0;
            Rank = 0;
            Camp = 0;
            ObjectID = default(ObjectGuidInfo);

        }
    }

    /// <summary>
    /// 跨服阵营排名信息
    /// </summary>
    public class CSCampRankInfo : IStruct
    {
        /// <summary>
        /// 阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 积分
        /// </summary>
        public int Score;
        /// <summary>
        /// 排名
        /// </summary>
        public uint Rank;
        /// <summary>
        /// 帮派数量
        /// </summary>
        public uint GuildCount;
        /// <summary>
        /// 占领城池数量
        /// </summary>
        public uint CityCount;

        public object Clone()
        {
            CSCampRankInfo st = new CSCampRankInfo();
            st.Camp = Camp;
            st.Score = Score;
            st.Rank = Rank;
            st.GuildCount = GuildCount;
            st.CityCount = CityCount;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteInt(Score);
                oByteArray.WriteUInt(Rank);
                oByteArray.WriteUInt(GuildCount);
                oByteArray.WriteUInt(CityCount);
            }
            else
            {
                Camp = oByteArray.ReadUInt();
                Score = oByteArray.ReadInt();
                Rank = oByteArray.ReadUInt();
                GuildCount = oByteArray.ReadUInt();
                CityCount = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Camp = 0;
            Score = 0;
            Rank = 0;
            GuildCount = 0;
            CityCount = 0;

        }
    }

    /// <summary>
    /// 顺网平台信息
    /// </summary>
    public class TShunPlatformData : IStruct
    {
        /// <summary>
        /// 新手奖励领取标识(微端)
        /// </summary>
        public uint MicroFirst;
        /// <summary>
        /// VIP会员等级
        /// </summary>
        public uint VipLv;
        /// <summary>
        /// 玩家等级奖励领取标识
        /// </summary>
        public uint HumLvAward;
        /// <summary>
        /// 登录奖励领取标识
        /// </summary>
        public uint LoginAward;
        /// <summary>
        /// 登录天数
        /// </summary>
        public uint LoginDays;
        /// <summary>
        /// 今日是否登录标识
        /// </summary>
        public byte isLoginToday;

        public object Clone()
        {
            TShunPlatformData st = new TShunPlatformData();
            st.MicroFirst = MicroFirst;
            st.VipLv = VipLv;
            st.HumLvAward = HumLvAward;
            st.LoginAward = LoginAward;
            st.LoginDays = LoginDays;
            st.isLoginToday = isLoginToday;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
                oByteArray.WriteUInt(VipLv);
                oByteArray.WriteUInt(HumLvAward);
                oByteArray.WriteUInt(LoginAward);
                oByteArray.WriteUInt(LoginDays);
                oByteArray.WriteByte(isLoginToday);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
                VipLv = oByteArray.ReadUInt();
                HumLvAward = oByteArray.ReadUInt();
                LoginAward = oByteArray.ReadUInt();
                LoginDays = oByteArray.ReadUInt();
                isLoginToday = oByteArray.ReadByte();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;
            VipLv = 0;
            HumLvAward = 0;
            LoginAward = 0;
            LoginDays = 0;
            isLoginToday = 0;

        }
    }

    /// <summary>
    /// 阵营已占领过的城池id
    /// </summary>
    public class NationalCampViewCityID : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 
        /// </summary>
        public List<uint> CityIDs = new List<uint>();

        public object Clone()
        {
            NationalCampViewCityID st = new NationalCampViewCityID();
            st.Camp = Camp;
            foreach (var item in CityIDs)
            {
                st.CityIDs.Add(item);
            }
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteUShort((ushort)CityIDs.Count);
                for (int i = 0; i < CityIDs.Count; i++)
                {
                    oByteArray.WriteUInt(CityIDs[i]);
                }
            }
            else
            {
                Camp = oByteArray.ReadUInt();
                int CityIDsCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < CityIDsCount; i++)
                {
                    CityIDs.Add(oByteArray.ReadUInt());
                }
            }
        }


        public void Reset()
        {
            Camp = 0;
            CityIDs.Clear();

        }
    }

    /// <summary>
    /// 国战城门战旗车信息
    /// </summary>
    public class CSNationalObject : IStruct
    {
        /// <summary>
        /// 国战怪物类型
        /// </summary>
        public uint MonsterType;
        /// <summary>
        /// 对象ID
        /// </summary>
        public ObjectGuidInfo ObjectID = new ObjectGuidInfo();
        /// <summary>
        /// 唯一ID
        /// </summary>
        public ulong Guid;
        /// <summary>
        /// 当前血量
        /// </summary>
        public ulong CurHP;
        /// <summary>
        /// 最大血量
        /// </summary>
        public ulong MaxHP;
        /// <summary>
        /// 到皇宫距离(战旗车)
        /// </summary>
        public uint Distance;
        /// <summary>
        /// 最近时距离
        /// </summary>
        public uint NewlyDistance;
        /// <summary>
        /// 最近距离首次时间
        /// </summary>
        public uint NewlyTime;
        /// <summary>
        /// 所属帮派唯一ID
        /// </summary>
        public ulong GuildGuid;
        /// <summary>
        /// 所属帮派名
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 帮派ID
        /// </summary>
        public ObjectGuidInfo GuildID = new ObjectGuidInfo();
        /// <summary>
        /// 所属阵营
        /// </summary>
        public uint Camp;

        public object Clone()
        {
            CSNationalObject st = new CSNationalObject();
            st.MonsterType = MonsterType;
            st.ObjectID = ObjectID.Clone() as ObjectGuidInfo;
            st.Guid = Guid;
            st.CurHP = CurHP;
            st.MaxHP = MaxHP;
            st.Distance = Distance;
            st.NewlyDistance = NewlyDistance;
            st.NewlyTime = NewlyTime;
            st.GuildGuid = GuildGuid;
            st.GuildName = GuildName;
            st.GuildID = GuildID.Clone() as ObjectGuidInfo;
            st.Camp = Camp;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MonsterType);
                ObjectID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(Guid);
                oByteArray.WriteUInt64(CurHP);
                oByteArray.WriteUInt64(MaxHP);
                oByteArray.WriteUInt(Distance);
                oByteArray.WriteUInt(NewlyDistance);
                oByteArray.WriteUInt(NewlyTime);
                oByteArray.WriteUInt64(GuildGuid);
                oByteArray.WriteUTF(GuildName);
                GuildID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(Camp);
            }
            else
            {
                MonsterType = oByteArray.ReadUInt();
                ObjectID.Serializtion(oByteArray, bSerialize);
                Guid = oByteArray.ReadUInt64();
                CurHP = oByteArray.ReadUInt64();
                MaxHP = oByteArray.ReadUInt64();
                Distance = oByteArray.ReadUInt();
                NewlyDistance = oByteArray.ReadUInt();
                NewlyTime = oByteArray.ReadUInt();
                GuildGuid = oByteArray.ReadUInt64();
                GuildName = oByteArray.ReadUTF();
                GuildID.Serializtion(oByteArray, bSerialize);
                Camp = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MonsterType = 0;
            ObjectID = default(ObjectGuidInfo);
            Guid = 0;
            CurHP = 0;
            MaxHP = 0;
            Distance = 0;
            NewlyDistance = 0;
            NewlyTime = 0;
            GuildGuid = 0;
            GuildName = string.Empty;
            GuildID = default(ObjectGuidInfo);
            Camp = 0;

        }
    }

    /// <summary>
    /// 钓鱼小游戏
    /// </summary>
    public class STFishingData : IStruct
    {
        /// <summary>
        /// 钓鱼斤两(原来斤两的10倍)
        /// </summary>
        public int Weight;
        /// <summary>
        /// 体力
        /// </summary>
        public uint PhysicalStrength;
        /// <summary>
        /// 钓鱼等级
        /// </summary>
        public uint FishingLevel;
        /// <summary>
        /// 钓鱼经验值
        /// </summary>
        public uint FishingExp;
        /// <summary>
        /// 渔具ID
        /// </summary>
        public uint FishingGearID;
        /// <summary>
        /// 上次上钩的鱼
        /// </summary>
        public uint LastHookedFish;
        /// <summary>
        /// 上次开始钓鱼的时间
        /// </summary>
        public uint LastStartFishTime;

        public object Clone()
        {
            STFishingData st = new STFishingData();
            st.Weight = Weight;
            st.PhysicalStrength = PhysicalStrength;
            st.FishingLevel = FishingLevel;
            st.FishingExp = FishingExp;
            st.FishingGearID = FishingGearID;
            st.LastHookedFish = LastHookedFish;
            st.LastStartFishTime = LastStartFishTime;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(Weight);
                oByteArray.WriteUInt(PhysicalStrength);
                oByteArray.WriteUInt(FishingLevel);
                oByteArray.WriteUInt(FishingExp);
                oByteArray.WriteUInt(FishingGearID);
                oByteArray.WriteUInt(LastHookedFish);
                oByteArray.WriteUInt(LastStartFishTime);
            }
            else
            {
                Weight = oByteArray.ReadInt();
                PhysicalStrength = oByteArray.ReadUInt();
                FishingLevel = oByteArray.ReadUInt();
                FishingExp = oByteArray.ReadUInt();
                FishingGearID = oByteArray.ReadUInt();
                LastHookedFish = oByteArray.ReadUInt();
                LastStartFishTime = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            Weight = 0;
            PhysicalStrength = 0;
            FishingLevel = 0;
            FishingExp = 0;
            FishingGearID = 0;
            LastHookedFish = 0;
            LastStartFishTime = 0;

        }
    }

    /// <summary>
    /// 爱奇艺平台信息
    /// </summary>
    public class IQYPlatformData : IStruct
    {
        /// <summary>
        /// 爱奇艺微端登录礼包领取表示
        /// </summary>
        public uint MicroFirst;

        public object Clone()
        {
            IQYPlatformData st = new IQYPlatformData();
            st.MicroFirst = MicroFirst;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(MicroFirst);
            }
            else
            {
                MicroFirst = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            MicroFirst = 0;

        }
    }

    /// <summary>
    /// 钓鱼-天选之人信息
    /// </summary>
    public class TodayKoiPlayer : IStruct
    {
        /// <summary>
        /// 玩家名
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 衣服ID
        /// </summary>
        public uint ClothesID;
        /// <summary>
        /// 武器ID
        /// </summary>
        public uint WeaponID;
        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;
        /// <summary>
        /// 时装衣服ID
        /// </summary>
        public uint FashionClothesID;
        /// <summary>
        /// 时装武器ID
        /// </summary>
        public uint FashionWeaponID;

        public object Clone()
        {
            TodayKoiPlayer st = new TodayKoiPlayer();
            st.PlayerName = PlayerName;
            st.ClothesID = ClothesID;
            st.WeaponID = WeaponID;
            st.Sex = Sex;
            st.FashionClothesID = FashionClothesID;
            st.FashionWeaponID = FashionWeaponID;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(PlayerName);
                oByteArray.WriteUInt(ClothesID);
                oByteArray.WriteUInt(WeaponID);
                oByteArray.WriteByte(Sex);
                oByteArray.WriteUInt(FashionClothesID);
                oByteArray.WriteUInt(FashionWeaponID);
            }
            else
            {
                PlayerName = oByteArray.ReadUTF();
                ClothesID = oByteArray.ReadUInt();
                WeaponID = oByteArray.ReadUInt();
                Sex = oByteArray.ReadByte();
                FashionClothesID = oByteArray.ReadUInt();
                FashionWeaponID = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerName = string.Empty;
            ClothesID = 0;
            WeaponID = 0;
            Sex = 0;
            FashionClothesID = 0;
            FashionWeaponID = 0;

        }
    }

    /// <summary>
    /// 钓鱼-锦鲤排行榜玩家信息
    /// </summary>
    public class FishingKoiRankInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string playerName;
        /// <summary>
        /// 锦鲤数量
        /// </summary>
        public uint koiNum;
        /// <summary>
        /// 转生等级
        /// </summary>
        public uint stateLv;
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerGuid;

        public object Clone()
        {
            FishingKoiRankInfo st = new FishingKoiRankInfo();
            st.playerName = playerName;
            st.koiNum = koiNum;
            st.stateLv = stateLv;
            st.PlayerGuid = PlayerGuid;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(playerName);
                oByteArray.WriteUInt(koiNum);
                oByteArray.WriteUInt(stateLv);
                oByteArray.WriteUInt64(PlayerGuid);
            }
            else
            {
                playerName = oByteArray.ReadUTF();
                koiNum = oByteArray.ReadUInt();
                stateLv = oByteArray.ReadUInt();
                PlayerGuid = oByteArray.ReadUInt64();
            }
        }


        public void Reset()
        {
            playerName = string.Empty;
            koiNum = 0;
            stateLv = 0;
            PlayerGuid = 0;

        }
    }

    /// <summary>
    /// 玩家基础信息
    /// </summary>
    public class PlayerBasicInfo : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 
        /// </summary>
        public string PlayerName;
        /// <summary>
        /// 
        /// </summary>
        public uint Career;
        /// <summary>
        /// 
        /// </summary>
        public uint Sex;

        public object Clone()
        {
            PlayerBasicInfo st = new PlayerBasicInfo();
            st.PlayerGuid = PlayerGuid;
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerName = PlayerName;
            st.Career = Career;
            st.Sex = Sex;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerGuid);
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUTF(PlayerName);
                oByteArray.WriteUInt(Career);
                oByteArray.WriteUInt(Sex);
            }
            else
            {
                PlayerGuid = oByteArray.ReadUInt64();
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerName = oByteArray.ReadUTF();
                Career = oByteArray.ReadUInt();
                Sex = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            PlayerGuid = 0;
            PlayerID = default(ObjectGuidInfo);
            PlayerName = string.Empty;
            Career = 0;
            Sex = 0;

        }
    }

    /// <summary>
    /// 天下第一武道会对局信息
    /// </summary>
    public class BudokaiRoom : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public PlayerBasicInfo Player1 = new PlayerBasicInfo();
        /// <summary>
        /// 
        /// </summary>
        public PlayerBasicInfo Player2 = new PlayerBasicInfo();
        /// <summary>
        /// 
        /// </summary>
        public PlayerBasicInfo Winner = new PlayerBasicInfo();
        /// <summary>
        /// 对局地图唯一id
        /// </summary>
        public ulong MapGuid;
        /// <summary>
        /// Player1被押注次数
        /// </summary>
        public uint BetCount1;
        /// <summary>
        /// Player2被押注次数
        /// </summary>
        public uint BetCount2;
        /// <summary>
        /// Player1是否弃赛
        /// </summary>
        public bool Player1Giveup;
        /// <summary>
        /// Player2是否弃赛
        /// </summary>
        public bool Player2Giveup;

        public object Clone()
        {
            BudokaiRoom st = new BudokaiRoom();
            st.Player1 = Player1.Clone() as PlayerBasicInfo;
            st.Player2 = Player2.Clone() as PlayerBasicInfo;
            st.Winner = Winner.Clone() as PlayerBasicInfo;
            st.MapGuid = MapGuid;
            st.BetCount1 = BetCount1;
            st.BetCount2 = BetCount2;
            st.Player1Giveup = Player1Giveup;
            st.Player2Giveup = Player2Giveup;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                Player1.Serializtion(oByteArray, bSerialize);
                Player2.Serializtion(oByteArray, bSerialize);
                Winner.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(MapGuid);
                oByteArray.WriteUInt(BetCount1);
                oByteArray.WriteUInt(BetCount2);
                oByteArray.WriteBoolean(Player1Giveup);
                oByteArray.WriteBoolean(Player2Giveup);
            }
            else
            {
                Player1.Serializtion(oByteArray, bSerialize);
                Player2.Serializtion(oByteArray, bSerialize);
                Winner.Serializtion(oByteArray, bSerialize);
                MapGuid = oByteArray.ReadUInt64();
                BetCount1 = oByteArray.ReadUInt();
                BetCount2 = oByteArray.ReadUInt();
                Player1Giveup = oByteArray.ReadBoolean();
                Player2Giveup = oByteArray.ReadBoolean();
            }
        }


        public void Reset()
        {
            Player1 = default(PlayerBasicInfo);
            Player2 = default(PlayerBasicInfo);
            Winner = default(PlayerBasicInfo);
            MapGuid = 0;
            BetCount1 = 0;
            BetCount2 = 0;
            Player1Giveup = false;
            Player2Giveup = false;

        }
    }

    /// <summary>
    /// 钓鱼鱼王排行榜
    /// </summary>
    public class FishingKingRank : IStruct
    {
        /// <summary>
        /// 玩家唯一ID
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 排名
        /// </summary>
        public uint Rank;
        /// <summary>
        /// 斤两
        /// </summary>
        public uint Weight;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string NickName;

        public object Clone()
        {
            FishingKingRank st = new FishingKingRank();
            st.PlayerGuid = PlayerGuid;
            st.Rank = Rank;
            st.Weight = Weight;
            st.NickName = NickName;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUInt(Rank);
                oByteArray.WriteUInt(Weight);
                oByteArray.WriteUTF(NickName);
            }
            else
            {
                PlayerGuid = oByteArray.ReadUInt64();
                Rank = oByteArray.ReadUInt();
                Weight = oByteArray.ReadUInt();
                NickName = oByteArray.ReadUTF();
            }
        }


        public void Reset()
        {
            PlayerGuid = 0;
            Rank = 0;
            Weight = 0;
            NickName = string.Empty;

        }
    }

    /// <summary>
    /// 物品数量
    /// </summary>
    public class ItemUpdateNum : IStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int ItemPos;
        /// <summary>
        /// 
        /// </summary>
        public uint ItemNum;
        /// <summary>
        /// 
        /// </summary>
        public bool Notice;
        /// <summary>
        /// 
        /// </summary>
        public uint Type;

        public object Clone()
        {
            ItemUpdateNum st = new ItemUpdateNum();
            st.ItemPos = ItemPos;
            st.ItemNum = ItemNum;
            st.Notice = Notice;
            st.Type = Type;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteInt(ItemPos);
                oByteArray.WriteUInt(ItemNum);
                oByteArray.WriteBoolean(Notice);
                oByteArray.WriteUInt(Type);
            }
            else
            {
                ItemPos = oByteArray.ReadInt();
                ItemNum = oByteArray.ReadUInt();
                Notice = oByteArray.ReadBoolean();
                Type = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            ItemPos = 0;
            ItemNum = 0;
            Notice = false;
            Type = 0;

        }
    }

    /// <summary>
    /// 跨服竞技场玩家信息
    /// </summary>
    public class CSArenaPlayerData : IStruct
    {
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ObjectGuidInfo PlayerID = new ObjectGuidInfo();
        /// <summary>
        /// 玩家唯一id
        /// </summary>
        public ulong PlayerGuid;
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string NickName;
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank;
        /// <summary>
        /// 当前分数
        /// </summary>
        public int Score;
        /// <summary>
        /// 赛季历史最高分
        /// </summary>
        public int HistoryMaxScore;
        /// <summary>
        /// 是否购买尊享奖励
        /// </summary>
        public int HasBuy;
        /// <summary>
        /// 当日已购买的匹配次数
        /// </summary>
        public int HasBuyTimes;
        /// <summary>
        /// 当日剩余可匹配次数
        /// </summary>
        public int RemainingTimes;
        /// <summary>
        /// 赛季总胜场
        /// </summary>
        public int TotalWinCount;
        /// <summary>
        /// 赛季总败场
        /// </summary>
        public int TotalLoseCount;
        /// <summary>
        /// 已领取奖励列表
        /// </summary>
        public List<int> AcquiredAwardList = new List<int>();
        /// <summary>
        /// 已领取尊享奖励列表
        /// </summary>
        public List<int> AcquiredExAwardList = new List<int>();
        /// <summary>
        /// 0:无;1:匹配中;2:对战中
        /// </summary>
        public int MatchState;
        /// <summary>
        /// 战区id
        /// </summary>
        public int ZoneID;
        /// <summary>
        /// 武道会报名状态  0:没资格 1:可报名  2:已报名
        /// </summary>
        public int BudokaiSignupStatus;
        /// <summary>
        /// 武道会押注信息
        /// </summary>
        public List<BudokaiBetInfo> BudokaiBetInfos = new List<BudokaiBetInfo>();

        public object Clone()
        {
            CSArenaPlayerData st = new CSArenaPlayerData();
            st.PlayerID = PlayerID.Clone() as ObjectGuidInfo;
            st.PlayerGuid = PlayerGuid;
            st.NickName = NickName;
            st.Rank = Rank;
            st.Score = Score;
            st.HistoryMaxScore = HistoryMaxScore;
            st.HasBuy = HasBuy;
            st.HasBuyTimes = HasBuyTimes;
            st.RemainingTimes = RemainingTimes;
            st.TotalWinCount = TotalWinCount;
            st.TotalLoseCount = TotalLoseCount;
            foreach (var item in AcquiredAwardList)
            {
                st.AcquiredAwardList.Add(item);
            }
            foreach (var item in AcquiredExAwardList)
            {
                st.AcquiredExAwardList.Add(item);
            }
            st.MatchState = MatchState;
            st.ZoneID = ZoneID;
            st.BudokaiSignupStatus = BudokaiSignupStatus;
            foreach (BudokaiBetInfo item in BudokaiBetInfos)
            {
                st.BudokaiBetInfos.Add(item.Clone() as BudokaiBetInfo);
            }
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt64(PlayerGuid);
                oByteArray.WriteUTF(NickName);
                oByteArray.WriteInt(Rank);
                oByteArray.WriteInt(Score);
                oByteArray.WriteInt(HistoryMaxScore);
                oByteArray.WriteInt(HasBuy);
                oByteArray.WriteInt(HasBuyTimes);
                oByteArray.WriteInt(RemainingTimes);
                oByteArray.WriteInt(TotalWinCount);
                oByteArray.WriteInt(TotalLoseCount);
                oByteArray.WriteUShort((ushort)AcquiredAwardList.Count);
                for (int i = 0; i < AcquiredAwardList.Count; i++)
                {
                    oByteArray.WriteInt(AcquiredAwardList[i]);
                }
                oByteArray.WriteUShort((ushort)AcquiredExAwardList.Count);
                for (int i = 0; i < AcquiredExAwardList.Count; i++)
                {
                    oByteArray.WriteInt(AcquiredExAwardList[i]);
                }
                oByteArray.WriteInt(MatchState);
                oByteArray.WriteInt(ZoneID);
                oByteArray.WriteInt(BudokaiSignupStatus);
                oByteArray.WriteUShort((ushort)BudokaiBetInfos.Count);
                for (int i = 0; i < BudokaiBetInfos.Count; i++)
                {
                    BudokaiBetInfos[i].Serializtion(oByteArray, bSerialize);
                }
            }
            else
            {
                PlayerID.Serializtion(oByteArray, bSerialize);
                PlayerGuid = oByteArray.ReadUInt64();
                NickName = oByteArray.ReadUTF();
                Rank = oByteArray.ReadInt();
                Score = oByteArray.ReadInt();
                HistoryMaxScore = oByteArray.ReadInt();
                HasBuy = oByteArray.ReadInt();
                HasBuyTimes = oByteArray.ReadInt();
                RemainingTimes = oByteArray.ReadInt();
                TotalWinCount = oByteArray.ReadInt();
                TotalLoseCount = oByteArray.ReadInt();
                int AcquiredAwardListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AcquiredAwardListCount; i++)
                {
                    AcquiredAwardList.Add(oByteArray.ReadInt());
                }
                int AcquiredExAwardListCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < AcquiredExAwardListCount; i++)
                {
                    AcquiredExAwardList.Add(oByteArray.ReadInt());
                }
                MatchState = oByteArray.ReadInt();
                ZoneID = oByteArray.ReadInt();
                BudokaiSignupStatus = oByteArray.ReadInt();
                int BudokaiBetInfosCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < BudokaiBetInfosCount; i++)
                {
                    BudokaiBetInfo obj = new BudokaiBetInfo();
                    obj.Serializtion(oByteArray, bSerialize);
                    BudokaiBetInfos.Add(obj);
                }
            }
        }


        public void Reset()
        {
            PlayerID = default(ObjectGuidInfo);
            PlayerGuid = 0;
            NickName = string.Empty;
            Rank = 0;
            Score = 0;
            HistoryMaxScore = 0;
            HasBuy = 0;
            HasBuyTimes = 0;
            RemainingTimes = 0;
            TotalWinCount = 0;
            TotalLoseCount = 0;
            AcquiredAwardList.Clear();
            AcquiredExAwardList.Clear();
            MatchState = 0;
            ZoneID = 0;
            BudokaiSignupStatus = 0;
            BudokaiBetInfos.Clear();

        }
    }

    /// <summary>
    /// 国战行军记录
    /// </summary>
    public class CsnationalMarchRecord : IStruct
    {
        /// <summary>
        /// 帮派名
        /// </summary>
        public string GuildName;
        /// <summary>
        /// 0:行军1:撤消行军2:急行军
        /// </summary>
        public uint Type;
        /// <summary>
        /// 城池ID
        /// </summary>
        public uint CityID;
        /// <summary>
        /// 阵营
        /// </summary>
        public uint Camp;
        /// <summary>
        /// 时间戳
        /// </summary>
        public uint Time;

        public object Clone()
        {
            CsnationalMarchRecord st = new CsnationalMarchRecord();
            st.GuildName = GuildName;
            st.Type = Type;
            st.CityID = CityID;
            st.Camp = Camp;
            st.Time = Time;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(GuildName);
                oByteArray.WriteUInt(Type);
                oByteArray.WriteUInt(CityID);
                oByteArray.WriteUInt(Camp);
                oByteArray.WriteUInt(Time);
            }
            else
            {
                GuildName = oByteArray.ReadUTF();
                Type = oByteArray.ReadUInt();
                CityID = oByteArray.ReadUInt();
                Camp = oByteArray.ReadUInt();
                Time = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            GuildName = string.Empty;
            Type = 0;
            CityID = 0;
            Camp = 0;
            Time = 0;

        }
    }

    /// <summary>
    /// 迅玩平台信息
    /// </summary>
    public class XWPlatformData : IStruct
    {
        /// <summary>
        /// 称号奖励
        /// </summary>
        public uint VipTitleAward;
        /// <summary>
        /// 特权礼包
        /// </summary>
        public uint VipdGiftAward;
        /// <summary>
        /// 每日礼包
        /// </summary>
        public uint VipDailyAward;
        /// <summary>
        /// 是否VIP(取值:0(非会员),1、2(会员))
        /// </summary>
        public uint IsVip;
        /// <summary>
        /// 是否年费；0不是，1是
        /// </summary>
        public uint IsYear;
        /// <summary>
        /// 会员类型（1、2、3（白金会员），4、5、6、7（超级会员））
        /// </summary>
        public uint VasType;

        public object Clone()
        {
            XWPlatformData st = new XWPlatformData();
            st.VipTitleAward = VipTitleAward;
            st.VipdGiftAward = VipdGiftAward;
            st.VipDailyAward = VipDailyAward;
            st.IsVip = IsVip;
            st.IsYear = IsYear;
            st.VasType = VasType;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUInt(VipTitleAward);
                oByteArray.WriteUInt(VipdGiftAward);
                oByteArray.WriteUInt(VipDailyAward);
                oByteArray.WriteUInt(IsVip);
                oByteArray.WriteUInt(IsYear);
                oByteArray.WriteUInt(VasType);
            }
            else
            {
                VipTitleAward = oByteArray.ReadUInt();
                VipdGiftAward = oByteArray.ReadUInt();
                VipDailyAward = oByteArray.ReadUInt();
                IsVip = oByteArray.ReadUInt();
                IsYear = oByteArray.ReadUInt();
                VasType = oByteArray.ReadUInt();
            }
        }


        public void Reset()
        {
            VipTitleAward = 0;
            VipdGiftAward = 0;
            VipDailyAward = 0;
            IsVip = 0;
            IsYear = 0;
            VasType = 0;

        }
    }

    /// <summary>
    /// 玩家信息
    /// </summary>
    public class PlayerData : IStruct
    {
        /// <summary>
        /// 兑奖码通码已领取ID
        /// </summary>
        public string GetAwardGeneralCodeStr;
        /// <summary>
        /// YY平台信息
        /// </summary>
        public YYPlatformData YYData = new YYPlatformData();
        /// <summary>
        /// 盛大平台信息
        /// </summary>
        public SODPlatformData SODData = new SODPlatformData();
        /// <summary>
        /// 每日礼包购买记录(key:TID,value:已购数量)
        /// </summary>
        public List<ProtocolPair> DailyGiftBuy = new List<ProtocolPair>();
        /// <summary>
        /// 360平台信息
        /// </summary>
        public TSZPlatformData TSZData = new TSZPlatformData();
        /// <summary>
        /// 37平台信息
        /// </summary>
        public TSPlatformData TSData = new TSPlatformData();
        /// <summary>
        /// 登录额外信息
        /// </summary>
        public string LoginExt;
        /// <summary>
        /// 2144平台信息
        /// </summary>
        public TOFFPlatformData TOFFData = new TOFFPlatformData();
        /// <summary>
        /// 贪玩平台信息
        /// </summary>
        public TWPlatformData TWData = new TWPlatformData();
        /// <summary>
        /// 9377平台信息
        /// </summary>
        public NTSSPlatformData NTSSData = new NTSSPlatformData();
        /// <summary>
        /// 元素通天塔历史最高等级
        /// </summary>
        public uint ElementBabelMaxLv;
        /// <summary>
        /// 元素通天塔完成时间戳
        /// </summary>
        public uint ElementBabelFinishTime;
        /// <summary>
        /// 进入元素通天塔剩余次数
        /// </summary>
        public int ElementBabelTimes;
        /// <summary>
        /// 元素通天塔领奖信息
        /// </summary>
        public string ElementBabelAwardStr;
        /// <summary>
        /// 地狱点数
        /// </summary>
        public long PurgatoryMapExp;
        /// <summary>
        /// 地狱天珠经验数
        /// </summary>
        public long PurgatoryHumExp;
        /// <summary>
        /// 老号标记位
        /// </summary>
        public uint OldAccountTag;
        /// <summary>
        /// 国战商店国币
        /// </summary>
        public uint NationalCoins;
        /// <summary>
        /// 地狱天珠池最大经验数
        /// </summary>
        public long PurgatoryHumMaxExp;
        /// <summary>
        /// 地狱天珠池卡使用次数
        /// </summary>
        public string PurgatoryCardUseNum;
        /// <summary>
        /// 顺网平台信息
        /// </summary>
        public TShunPlatformData TShunData = new TShunPlatformData();
        /// <summary>
        /// 钓鱼
        /// </summary>
        public STFishingData FishingData = new STFishingData();
        /// <summary>
        /// 爱奇艺平台信息
        /// </summary>
        public IQYPlatformData IQYData = new IQYPlatformData();
        /// <summary>
        /// 迅玩平台信息
        /// </summary>
        public XWPlatformData XWData = new XWPlatformData();

        public object Clone()
        {
            PlayerData st = new PlayerData();
            st.GetAwardGeneralCodeStr = GetAwardGeneralCodeStr;
            st.YYData = YYData.Clone() as YYPlatformData;
            st.SODData = SODData.Clone() as SODPlatformData;
            foreach (ProtocolPair item in DailyGiftBuy)
            {
                st.DailyGiftBuy.Add(item.Clone() as ProtocolPair);
            }
            st.TSZData = TSZData.Clone() as TSZPlatformData;
            st.TSData = TSData.Clone() as TSPlatformData;
            st.LoginExt = LoginExt;
            st.TOFFData = TOFFData.Clone() as TOFFPlatformData;
            st.TWData = TWData.Clone() as TWPlatformData;
            st.NTSSData = NTSSData.Clone() as NTSSPlatformData;
            st.ElementBabelMaxLv = ElementBabelMaxLv;
            st.ElementBabelFinishTime = ElementBabelFinishTime;
            st.ElementBabelTimes = ElementBabelTimes;
            st.ElementBabelAwardStr = ElementBabelAwardStr;
            st.PurgatoryMapExp = PurgatoryMapExp;
            st.PurgatoryHumExp = PurgatoryHumExp;
            st.OldAccountTag = OldAccountTag;
            st.NationalCoins = NationalCoins;
            st.PurgatoryHumMaxExp = PurgatoryHumMaxExp;
            st.PurgatoryCardUseNum = PurgatoryCardUseNum;
            st.TShunData = TShunData.Clone() as TShunPlatformData;
            st.FishingData = FishingData.Clone() as STFishingData;
            st.IQYData = IQYData.Clone() as IQYPlatformData;
            st.XWData = XWData.Clone() as XWPlatformData;
            return st;
        }

        public void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                oByteArray.WriteUTF(GetAwardGeneralCodeStr);
                YYData.Serializtion(oByteArray, bSerialize);
                SODData.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUShort((ushort)DailyGiftBuy.Count);
                for (int i = 0; i < DailyGiftBuy.Count; i++)
                {
                    DailyGiftBuy[i].Serializtion(oByteArray, bSerialize);
                }
                TSZData.Serializtion(oByteArray, bSerialize);
                TSData.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUTF(LoginExt);
                TOFFData.Serializtion(oByteArray, bSerialize);
                TWData.Serializtion(oByteArray, bSerialize);
                NTSSData.Serializtion(oByteArray, bSerialize);
                oByteArray.WriteUInt(ElementBabelMaxLv);
                oByteArray.WriteUInt(ElementBabelFinishTime);
                oByteArray.WriteInt(ElementBabelTimes);
                oByteArray.WriteUTF(ElementBabelAwardStr);
                oByteArray.WriteInt64(PurgatoryMapExp);
                oByteArray.WriteInt64(PurgatoryHumExp);
                oByteArray.WriteUInt(OldAccountTag);
                oByteArray.WriteUInt(NationalCoins);
                oByteArray.WriteInt64(PurgatoryHumMaxExp);
                oByteArray.WriteUTF(PurgatoryCardUseNum);
                TShunData.Serializtion(oByteArray, bSerialize);
                FishingData.Serializtion(oByteArray, bSerialize);
                IQYData.Serializtion(oByteArray, bSerialize);
                XWData.Serializtion(oByteArray, bSerialize);
            }
            else
            {
                GetAwardGeneralCodeStr = oByteArray.ReadUTF();
                YYData.Serializtion(oByteArray, bSerialize);
                SODData.Serializtion(oByteArray, bSerialize);
                int DailyGiftBuyCount = (int)oByteArray.ReadUShort();
                for (int i = 0; i < DailyGiftBuyCount; i++)
                {
                    ProtocolPair obj = new ProtocolPair();
                    obj.Serializtion(oByteArray, bSerialize);
                    DailyGiftBuy.Add(obj);
                }
                TSZData.Serializtion(oByteArray, bSerialize);
                TSData.Serializtion(oByteArray, bSerialize);
                LoginExt = oByteArray.ReadUTF();
                TOFFData.Serializtion(oByteArray, bSerialize);
                TWData.Serializtion(oByteArray, bSerialize);
                NTSSData.Serializtion(oByteArray, bSerialize);
                ElementBabelMaxLv = oByteArray.ReadUInt();
                ElementBabelFinishTime = oByteArray.ReadUInt();
                ElementBabelTimes = oByteArray.ReadInt();
                ElementBabelAwardStr = oByteArray.ReadUTF();
                PurgatoryMapExp = oByteArray.ReadInt64();
                PurgatoryHumExp = oByteArray.ReadInt64();
                OldAccountTag = oByteArray.ReadUInt();
                NationalCoins = oByteArray.ReadUInt();
                PurgatoryHumMaxExp = oByteArray.ReadInt64();
                PurgatoryCardUseNum = oByteArray.ReadUTF();
                TShunData.Serializtion(oByteArray, bSerialize);
                FishingData.Serializtion(oByteArray, bSerialize);
                IQYData.Serializtion(oByteArray, bSerialize);
                XWData.Serializtion(oByteArray, bSerialize);
            }
        }


        public void Reset()
        {
            GetAwardGeneralCodeStr = string.Empty;
            YYData = default(YYPlatformData);
            SODData = default(SODPlatformData);
            DailyGiftBuy.Clear();
            TSZData = default(TSZPlatformData);
            TSData = default(TSPlatformData);
            LoginExt = string.Empty;
            TOFFData = default(TOFFPlatformData);
            TWData = default(TWPlatformData);
            NTSSData = default(NTSSPlatformData);
            ElementBabelMaxLv = 0;
            ElementBabelFinishTime = 0;
            ElementBabelTimes = 0;
            ElementBabelAwardStr = string.Empty;
            PurgatoryMapExp = 0;
            PurgatoryHumExp = 0;
            OldAccountTag = 0;
            NationalCoins = 0;
            PurgatoryHumMaxExp = 0;
            PurgatoryCardUseNum = string.Empty;
            TShunData = default(TShunPlatformData);
            FishingData = default(STFishingData);
            IQYData = default(IQYPlatformData);
            XWData = default(XWPlatformData);

        }
    }


}
