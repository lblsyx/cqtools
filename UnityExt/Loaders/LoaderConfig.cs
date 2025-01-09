using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Loaders
{
    public class LoaderConfig
    {
        public static int Version = 0;
        public static long MaxCacheSize = 1024 * 1024 * 1024;

        public static uint MaxRetryNum = 2;
        public static uint MaxPriority = 5;
        public static uint DefaultPriority = 2;
        public static int DefaultLoadingNum = 2;
        public static bool DefaultAutoUnloadBundle = true;
    }
}
