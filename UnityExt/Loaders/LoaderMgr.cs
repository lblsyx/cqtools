using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.Loaders
{
    public class LoaderMgr
    {
        public static float DelayMS = 0.033f;
        private static float mDeltaTime = 0.0f;
        private static URLLoader mURLLoadingItem;
        private static IList<URLLoader> mLoadingList = new List<URLLoader>();
        private static IList<URLLoader> mAsyncDoneList = new List<URLLoader>();

        public static void AsyncDone(URLLoader loader)
        {
            if (mAsyncDoneList.Contains(loader)) return;

            mAsyncDoneList.Add(loader);
        }

        public static void AddLoader(URLLoader loader)
        {
            if (mLoadingList.IndexOf(loader) == -1)
            {
                mLoadingList.Add(loader);
            }
        }

        public static void DelLoader(URLLoader loader)
        {
            int index = mLoadingList.IndexOf(loader);
            if (index != -1) mLoadingList.RemoveAt(index);
        }

        public static void Update()
        {
            mDeltaTime += Time.deltaTime;
            if (mDeltaTime <= DelayMS) return;
            mDeltaTime = 0;
            for (int i = 0; i < mLoadingList.Count; i++)
            {
                mURLLoadingItem = mLoadingList[i];
                mURLLoadingItem.Update();

                if (mURLLoadingItem.IsDone)
                {
                    mLoadingList.RemoveAt(i);
                    i -= 1;
                }
            }

            for (int i = 0; i < mAsyncDoneList.Count; i++)
            {
                mURLLoadingItem = mAsyncDoneList[i];
                mURLLoadingItem.AsyncComplete();
            }
            mAsyncDoneList.Clear();
        }
    }
}
