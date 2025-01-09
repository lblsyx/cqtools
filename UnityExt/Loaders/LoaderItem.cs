using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;

namespace UnityExt.Loaders
{
    public class LoaderItem
    {
        public bool Done;
        public string URL;
        public object Tag;
        public object Data;
        public bool Complete;
        public uint Priority;
        public uint RetryNum;
        public string ErrorMsg;
        public double Progress;
        public URLLoader Loader;

        public Callback<URLLoader> OnStart;
        public Callback<URLLoader, bool, string> OnDone;
        public Callback<URLLoader, int, int, double> OnProgress;
    }
}
