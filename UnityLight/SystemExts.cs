using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class SystemExts
    {
        private static DateTime dt1970 = new DateTime(1970, 01, 01, 0, 0, 0);

        public static uint TotalSeconds(this DateTime dt)
        {
            TimeSpan ts = dt - dt1970;

            return (uint)ts.TotalSeconds;
        }
    }
}
