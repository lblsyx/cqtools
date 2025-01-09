using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.Loaders;
using UnityLight;

namespace UnityExt.Preloads
{
    public interface IPreload
    {
        int SortIndex { get; }

        string PreloadPath { get; }

        Callback<IPreload, bool, string> OnDoneCallback { get; set; }

        void StartProcessPreload(URLLoader loader, bool success, string errMsg);
    }

    public class PreloadCompare : IComparer<IPreload>
    {
        public int Compare(IPreload x, IPreload y)
        {
            return x.SortIndex.CompareTo(y.SortIndex);
        }
    }
}
