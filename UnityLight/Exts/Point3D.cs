using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Exts
{
    public struct Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3D(uint x, uint y, uint z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point3D)
            {
                Point3D oPoint3D = (Point3D)obj;

                return this.x == oPoint3D.x && this.y == oPoint3D.y && this.z == oPoint3D.z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)Math.Round(x + y + z);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x - b.x;
            oPoint3D.y = a.y - b.y;
            oPoint3D.z = a.z - b.z;
            return oPoint3D;
        }
        public static bool operator !=(Point3D lhs, Point3D rhs)
        {
            return lhs.x != rhs.x && lhs.y != rhs.y && lhs.z != rhs.z;
        }
        public static bool operator ==(Point3D lhs, Point3D rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }
        public static Point3D operator *(float d, Point3D a)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x * d;
            oPoint3D.y = a.y * d;
            oPoint3D.z = a.z * d;
            return oPoint3D;
        }
        public static Point3D operator *(Point3D a, float d)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x * d;
            oPoint3D.y = a.y * d;
            oPoint3D.z = a.z * d;
            return oPoint3D;
        }
        public static Point3D operator /(Point3D a, float d)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x / d;
            oPoint3D.y = a.y / d;
            oPoint3D.z = a.z / d;
            return oPoint3D;
        }
        public static Point3D operator *(double d, Point3D a)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x * d;
            oPoint3D.y = a.y * d;
            oPoint3D.z = a.z * d;
            return oPoint3D;
        }
        public static Point3D operator *(Point3D a, double d)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x * d;
            oPoint3D.y = a.y * d;
            oPoint3D.z = a.z * d;
            return oPoint3D;
        }
        public static Point3D operator /(Point3D a, double d)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x / d;
            oPoint3D.y = a.y / d;
            oPoint3D.z = a.z / d;
            return oPoint3D;
        }
        public static Point3D operator +(Point3D a, Point3D b)
        {
            Point3D oPoint3D;
            oPoint3D.x = a.x + b.x;
            oPoint3D.y = a.y + b.y;
            oPoint3D.z = a.z + b.z;
            return oPoint3D;
        }
    }
}
