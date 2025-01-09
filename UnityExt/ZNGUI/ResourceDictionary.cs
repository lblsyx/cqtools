using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.ZNGUI
{
    public class ResourceDictionary<T>
    {
        Dictionary<string, T> mDict = new Dictionary<string, T>();

        public void Add(string key, T t)
        {
            mDict[key] = t;
        }

        public bool Del(string key)
        {
            return mDict.Remove(key);
        }

        public bool Contains(string key)
        {
            return mDict.ContainsKey(key);
        }

        public void Clear()
        {
            mDict.Clear();
        }

        public T Get(string key)
        {
            return mDict[key];
        }

        public T this[string key]
        {
            get { return mDict[key]; }
        }
    }
}
