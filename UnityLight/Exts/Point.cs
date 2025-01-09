using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Exts
{
    public class Point
    {
        public int X;
        public int Y;

        public Point() : this(0, 0)
        {
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point clone()
        {
            return new Point(X, Y);
        }
    }
}
