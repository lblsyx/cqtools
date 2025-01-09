using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight
{
    public class Singleton<T> where T : new()
    {
        public static object SyncRoot = new object();

        private static T _instance;
        public static T singleton
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }

                    return _instance;
                }
            }
        }
    }
}
