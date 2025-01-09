using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Exts
{
    public struct Point2D
    {
        public int x;
        public int y;

        public Point2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point2D)
            {
                Point2D oPoint2D = (Point2D)obj;

                return this.x == oPoint2D.x && this.y == oPoint2D.y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return x + y;
        }

        public static Point2D operator -(Point2D a, Point2D b)
        {
            Point2D oPoint2D;
            oPoint2D.x = a.x - b.x;
            oPoint2D.y = a.y - b.y;
            return oPoint2D;
        }

        public static bool operator !=(Point2D lhs, Point2D rhs)
        {
            return lhs.x != rhs.x && lhs.y != rhs.y;
        }
        public static bool operator ==(Point2D lhs, Point2D rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }
        public static Point2D operator *(int d, Point2D a)
        {
            Point2D oPoint2D;
            oPoint2D.x = a.x * d;
            oPoint2D.y = a.y * d;
            return oPoint2D;
        }
        public static Point2D operator *(Point2D a, int d)
        {
            Point2D oPoint2D;
            oPoint2D.x = a.x * d;
            oPoint2D.y = a.y * d;
            return oPoint2D;
        }
        public static Point2D operator /(Point2D a, int d)
        {
            Point2D oPoint2D;
            oPoint2D.x = a.x / d;
            oPoint2D.y = a.y / d;
            return oPoint2D;
        }
        public static Point2D operator +(Point2D a, Point2D b)
        {
            Point2D oPoint2D;
            oPoint2D.x = a.x + b.x;
            oPoint2D.y = a.y + b.y;
            return oPoint2D;
        }

        public static implicit operator Point3D(Point2D v)
        {
            Point3D oPoint3D;
#if UNITY_MIR
            oPoint3D.x = v.y;
            oPoint3D.y = 0;
            oPoint3D.z = v.x;
#else
            oPoint3D.x = v.x;
            oPoint3D.y = v.y;
            oPoint3D.z = 0;
#endif
            return oPoint3D;
        }

        public static implicit operator Point2D(Point3D v)
        {
            Point2D oPoint2D;
#if UNITY_MIR
            oPoint2D.x = (int)Math.Round(v.z);
            oPoint2D.y = (int)Math.Round(v.x);
#else
            oPoint2D.x = (int)Math.Round(v.x);
            oPoint2D.y = (int)Math.Round(v.y);
#endif
            return oPoint2D;
        }
    }
}
