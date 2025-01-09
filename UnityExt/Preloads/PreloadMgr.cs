using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityExt.Loaders;
using UnityLight;
using UnityLight.Loggers;

namespace UnityExt.Preloads
{
        public class PreloadItem
        {
            public bool IsDone;
            public bool Success;
            public string ErrorMsg;
            public IPreload Preload;
        }

    public class PreloadMgr
    {
        public static Callback<LoaderQueue, double> OnProgressCallback;
        public static Callback<List<PreloadItem>, bool> OnDoneCallback;

        private static Dictionary<IPreload, PreloadItem> mIPreloadResult = new Dictionary<IPreload, PreloadItem>();
        private static List<IPreload> mPreloads = new List<IPreload>();
        private static LoaderQueue mLoaderQueue = new LoaderQueue();

        private static IComparer<IPreload> Comparer = new PreloadCompare();
        private static bool mLoading = false;

        public static void SearchAssembly(Assembly assembly)
        {
            Type[] list = assembly.GetTypes();

            string sInterfaceStr = typeof(IPreload).ToString();

            Type tAttributeType = typeof(PreloadAttribute);

            foreach (var type in list)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                PreloadAttribute[] attributes = (PreloadAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    PreloadAttribute attribute = attributes[0];

                    mPreloads.Add((IPreload)Activator.CreateInstance(type));
                }
            }

            mPreloads.Sort(Comparer);
        }

        public static void Start()
        {
            Start(LoaderConfig.DefaultLoadingNum);
        }

        public static void Start(int maxLoadNum)
        {
            if (mLoading) return;
            if (mPreloads.Count == 0) return;
            mLoading = true;
            LoaderItem li = null;
            mLoaderQueue.OnProgressEvent = OnProcessHandler;
            foreach (var iPreload in mPreloads)
            {
                li = mLoaderQueue.AddLoad(iPreload.PreloadPath, OnPreloadDoneHandler);
                li.Tag = iPreload;
                var item = new PreloadItem();
                item.Preload = iPreload;
                mIPreloadResult.Add(iPreload, item);
            }
            mLoaderQueue.Start(maxLoadNum);
        }

        private static void OnProcessHandler(LoaderQueue queue, double progress)
        {
            if (OnProgressCallback != null)
            {
                OnProgressCallback(queue, progress);
            }
        }

        private static void OnPreloadDoneHandler(URLLoader loader, bool success, string errMsg)
        {
            IPreload iPreload = loader.Tag as IPreload;
            if (iPreload == null)
            {
                XLogger.ErrorFormat("预加载项不能为空!URL:{0}", loader.URL);
                return;
            }
            iPreload.OnDoneCallback = OnProcessDoneHandler;
            iPreload.StartProcessPreload(loader, success, errMsg);
        }

        private static void OnProcessDoneHandler(IPreload iPreload, bool success, string errMsg)
        {
            mIPreloadResult[iPreload].IsDone = true;
            mIPreloadResult[iPreload].Success = success;
            mIPreloadResult[iPreload].ErrorMsg = errMsg;

            bool bAllSuccess = true;
            foreach (var rlt in mIPreloadResult)
            {
                if (rlt.Value.IsDone == false)
                {
                    return;
                }

                if (rlt.Value.Success == false)
                {
                    bAllSuccess = false;
                }
            }

            if (OnDoneCallback != null)
            {
                OnDoneCallback(mIPreloadResult.Values.ToList(), bAllSuccess);
            }
        }
    }
}
