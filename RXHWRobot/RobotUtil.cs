using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Robots;
using RXHWRobot.Datas;

namespace RXHWRobot
{
    public class RobotUtil
    {
        public static double GetAngle(int startX, int startY, int targetX, int targetY)
        {
            double angle = 0;
            double radians = 0;
            int ty = targetY - startY;
            int tx = targetX - startX;
            if (tx == 0) radians = Math.Atan(ty);
            else radians = Math.Atan(ty / tx);
            angle = radians * 180 / Math.PI;
            return angle;
        }

        public static RobotDirect getDirect(int startX, int startY, int targetX, int targetY)
        {
            double angle = GetAngle(startX, startY, targetX, targetY);

            if (targetX >= startX)
            {//鼠标在右边
                if (targetY < startY)
                {//鼠标在上边
                    if (angle > -22.5)
                    {
                        return RobotDirect.Right;
                    }
                    else if (angle > -67.5)
                    {
                        return RobotDirect.RightUp;
                    }
                    else
                    {
                        return RobotDirect.Up;
                    }
                }
                else
                {//鼠标在下边
                    if (angle < 22.5)
                    {
                        return RobotDirect.Right;
                    }
                    else if (angle < 67.5)
                    {
                        return RobotDirect.RightDown;
                    }
                    else
                    {
                        return RobotDirect.Down;
                    }
                }
            }
            else
            {//鼠标在左边
                if (targetY < startY)
                {//鼠标在上边
                    if (angle < 22.5)
                    {
                        return RobotDirect.Left;
                    }
                    else if (angle < 67.5)
                    {
                        return RobotDirect.LeftUp;
                    }
                    else
                    {
                        return RobotDirect.Up;
                    }
                }
                else
                {//鼠标在下边
                    if (angle > -22.5)
                    {
                        return RobotDirect.Left;
                    }
                    else if (angle > -67.5)
                    {
                        return RobotDirect.LeftDown;
                    }
                    else
                    {
                        return RobotDirect.Down;
                    }
                }
            }
            //return RobotDirect.Down;
        }

        public static uint MakePlatformServerKey(uint platformID, uint serverID)
        {
            uint temp = platformID << 16;
            temp = temp | serverID;
            return temp;
        }

        public static UInt64 MakeServerObjectKey(uint platformID, uint serverID, uint playerID)
        {
            UInt64 temp = MakePlatformServerKey(platformID, serverID);
            temp = temp << 32;
            temp = temp | playerID;
            return temp;
        }

        public static UInt64 MakeServerObjectKey(ObjectGuidInfo kObjectGuidInfo)
        {
	        return MakeServerObjectKey(kObjectGuidInfo.PlatformID, kObjectGuidInfo.ServerID, kObjectGuidInfo.ObjectID);
        }

        public static bool CheckObjectIsMon(MapObject oMapObject)
        {
            if(oMapObject == null)
                return false;

            return CheckObjectIsMon(oMapObject.ObjectID);
        }

        public static bool CheckObjectIsMon(ObjectGuidInfo kObjectGuidInfo)
        {
            if (kObjectGuidInfo.PlatformID == (int)ObjectType.MON_PSID && kObjectGuidInfo.ServerID == (int)ObjectType.MON_PSID)
            {
                return true;
            }
            return false;
        }
    }
}
