using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;
using UnityLight.Loggers;

namespace UnityExt.Loaders
{
    public class DownloadQueue
    {
        private Dictionary<string, object> mCachePool;
        private IList<IList<LoaderItem>> mQueueList;
        private IList<LoaderItem> mLoadingList;
        private IList<LoaderItem> mWaitingList;
        private Queue<URLLoader> mLoaderList;

        public int MaxLoading { get; set; }

        public int NumLoading { get { return mLoadingList.Count; } }

        public bool LoadDataCached { get; set; }

        public Callback<bool, string, object> OnItemClearCache;

        public DownloadQueue()
        {
            Initialize(LoaderConfig.DefaultLoadingNum);
        }

        public DownloadQueue(int maxLoading)
        {
            Initialize(maxLoading);
        }

        private void Initialize(int maxLoading)
        {
            LoadDataCached = true;
            MaxLoading = maxLoading;
            OnItemClearCache = OnClearCacheDefaultHandler;

            mLoaderList = new Queue<URLLoader>();
            mLoadingList = new List<LoaderItem>();
            mWaitingList = new List<LoaderItem>();
            mQueueList = new List<IList<LoaderItem>>();
            mCachePool = new Dictionary<string, object>();

            for (int i = 0; i < LoaderConfig.MaxPriority; i++)
            {
                mQueueList.Add(new List<LoaderItem>());
            }
        }

        private void OnClearCacheDefaultHandler(bool bUnloadAllLoadedObjects, string key, object value)
        {
            if (value is AssetBundle)
            {
                AssetBundle bundle = value as AssetBundle;
                bundle.Unload(bUnloadAllLoadedObjects);
            }
        }

        public LoaderItem Load(string url)
        {
            return Load(url, LoaderConfig.DefaultPriority, null, null, null);
        }
        
        public LoaderItem Load(string url, Callback<URLLoader, bool, string> doneCallback)
        {
            return Load(url, LoaderConfig.DefaultPriority, doneCallback, null, null);
        }

        public LoaderItem Load(string url, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader> startCallback)
        {
            return Load(url, LoaderConfig.DefaultPriority, doneCallback, null, startCallback);
        }

        public LoaderItem Load(string url, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader, int, int, double> progressCallback)
        {
            return Load(url, LoaderConfig.DefaultPriority, doneCallback, progressCallback, null);
        }

        public LoaderItem Load(string url, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader, int, int, double> progressCallback, Callback<URLLoader> startCallback)
        {
            return Load(url, LoaderConfig.DefaultPriority, doneCallback, progressCallback, startCallback);
        }

        public LoaderItem Load(string url, uint priority)
        {
            return Load(url, priority, null, null, null);
        }

        public LoaderItem Load(string url, uint priority, Callback<URLLoader, bool, string> doneCallback)
        {
            return Load(url, priority, doneCallback, null, null);
        }
        
        public LoaderItem Load(string url, uint priority, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader, int, int, double> progressCallback)
        {
            return Load(url, priority, doneCallback, progressCallback, null);
        }

        public LoaderItem Load(string url, uint priority, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader, int, int, double> progressCallback, Callback<URLLoader> startCallback)
        {
            priority = Math.Min(priority, LoaderConfig.MaxPriority - 1);

            LoaderItem item = new LoaderItem();
            item.URL = url;
            item.Tag = null;
            item.Data = null;
            item.Done = false;
            item.RetryNum = 0;
            item.Progress = 0;
            item.Loader = null;
            item.ErrorMsg = null;
            item.Complete = false;
            item.Priority = priority;
            item.OnDone = doneCallback;
            item.OnStart = startCallback;
            item.OnProgress = progressCallback;

            if (mCachePool.ContainsKey(item.URL))
            {
                doLoad(item);
                return item;
            }

            if (isLoading(url))
            {
                mWaitingList.Add(item);
            }
            else
            {
                mQueueList[(int)priority].Add(item);
                tryLoadNext();
            }

            return item;
        }

        public void Cancel(LoaderItem item)
        {
            if (item == null || item.Done) return;
            item.Tag = null;
            if (item.Loader != null)
            {
                URLLoader loader = item.Loader;
                for (int i = 0; i < mWaitingList.Count; i++)
                {
                    if (mWaitingList[i].URL == item.URL)
                    {//有正在等待下载完成的下载项，转移给等待项
                        item.Loader = null;
                        loader.Item = mWaitingList[i];
                        loader.Tag = mWaitingList[i].Tag;
                        mWaitingList[i].Loader = loader;
                        mLoadingList.Add(mWaitingList[i]);
                        mWaitingList.RemoveAt(i);
                        break;
                    }
                }

                mLoadingList.Remove(item);
                item.Tag = null;

                if (item.Loader != null)
                {
                    item.Loader.Cancel();
                    item.ErrorMsg = item.Loader.ErrorMsg;
                    mLoaderList.Enqueue(item.Loader);
                    item.Loader.Item = null;
                    item.Loader = null;
                    tryLoadNext();
                }

                return;
            }

            for (int i = 0; i < mWaitingList.Count; i++)
            {
                if (mWaitingList[i] == item)
                {
                    mWaitingList[i].Tag = null;
                    mWaitingList.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i < mQueueList.Count; i++)
            {
                IList<LoaderItem> q = mQueueList[i];
                for (int j = 0; j < q.Count; j++)
                {
                    if (q[j] == item)
                    {
                        q.RemoveAt(j);
                        return;
                    }
                }
            }
        }

        public bool ClearCache(bool bUnloadAllLoadedObjects)
        {
            if (OnItemClearCache != null)
            {
                foreach (var item in mCachePool)
                {
                    OnItemClearCache(bUnloadAllLoadedObjects, item.Key, item.Value);
                }
            }

            bool cleared = mCachePool.Count != 0;

            mCachePool.Clear();

            return cleared;
        }

        public void ClearQueue()
        {
            for (int i = 0; i < mLoadingList.Count; i++)
            {
                LoaderItem item = mLoadingList[i];
                URLLoader loader = item.Loader;
                item.Loader = null;
                item.Tag = null;

                if (loader != null)
                {
                    loader.Cancel();
                    loader.Item = null;
                    loader.Tag = null;
                    mLoaderList.Enqueue(loader);
                }
            }
            mLoadingList.Clear();

            for (int i = 0; i < mQueueList.Count; i++)
            {
                foreach (var item in mQueueList[i])
                {
                    item.Loader = null;
                    item.Data = null;
                    item.Tag = null;
                }
                mQueueList[i].Clear();
            }

            for (int i = 0; i < mWaitingList.Count; i++)
            {
                var item = mWaitingList[i];
                item.Loader = null;
                item.Data = null;
                item.Tag = null;
            }
            mWaitingList.Clear();
        }

        private void tryLoadNext()
        {
            while (mLoadingList.Count < MaxLoading)
            {
                LoaderItem item = tryGetNextItem();
                if (item != null) doLoad(item);
                if (item == null || mLoadingList.Count >= MaxLoading) break;
            }
        }

        private LoaderItem tryGetNextItem()
        {
            for (int i = 0; i < LoaderConfig.MaxPriority; i++)
            {
                IList<LoaderItem> q = mQueueList[i];

                LoaderItem itm = null;
                while (q.Count != 0)
                {
                    itm = q[0];
                    q.RemoveAt(0);
                    if (isLoading(itm.URL))
                    {
                        mWaitingList.Add(itm);
                        itm = null;
                    }
                    else
                    {
                        moveSameToWaitingList(itm.URL);
                        return itm;
                    }
                }
            }

            return null;
        }

        private bool isLoading(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            foreach (var item in mLoadingList)
            {
                if (url == item.URL)
                {
                    return true;
                }
            }
            return false;
        }

        private void moveSameToWaitingList(string url)
        {
            for (int i = 0; i < LoaderConfig.MaxPriority; i++)
            {
                IList<LoaderItem> q = mQueueList[i];

                for (int j = 0; j < q.Count; j++)
                {
                    LoaderItem item = q[j];
                    if (item.URL == url)
                    {
                        mWaitingList.Add(item);
                    }
                }
            }
        }

        private void doLoad(LoaderItem item)
        {
            URLLoader loader =null;
            if (mLoaderList.Count != 0)
            {
                loader = mLoaderList.Dequeue();
            }
            else
            {
                loader = new URLLoader();
                loader.OnStartEvent = OnStartLoadHandler;
                loader.OnDoneEvent = OnDoneLoadHandler;
                loader.OnProgressEvent = OnProgressHandler;
            }

            loader.Item = item;
            item.Loader = loader;

            if (mCachePool.ContainsKey(item.URL))
            {
                loader.URL = item.URL;
                loader.Data = mCachePool[item.URL];
                LoaderMgr.AsyncDone(loader);
                //OnProgressHandler(loader, 1, 1, 1);
                //OnDoneLoadHandler(loader, true, null);
            }
            else
            {
                loader.Load(item.URL, !LoadDataCached);
                mLoadingList.Add(item);
            }
        }

        private void endLoad(LoaderItem item, URLLoader loader, bool complete, string errorMsg)
        {
            loader.Item = item;
            item.Loader = loader;
            loader.Tag = item.Tag;
            item.Data = loader.Data;

            if (complete == false)
            {
                if (item.RetryNum < LoaderConfig.MaxRetryNum)
                {
                    item.Done = false;
                    item.Progress = 0;
                    item.RetryNum += 1;
                    item.Complete = false;
                    //item.Loader = null;
                    mQueueList[(int)LoaderConfig.MaxPriority - 1].Add(item);
                    return;
                }
            }

            item.Done = true;
            item.Complete = complete;
            item.ErrorMsg = errorMsg;

            if (item.OnDone != null)
            {
                try
                {
                    item.OnDone(loader, complete, errorMsg);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            item.Loader = null;
            loader.Item = null;
            loader.Tag = null;
            item.Tag = null;
        }

        private void OnStartLoadHandler(URLLoader loader)
        {
            LoaderItem item = loader.Item;
            if (item == null)
            {
                XLogger.WarnFormat("队列加载项的对象变量不能为空!Url:{0}", loader.URL);
                return;
            }


            loader.Item = item;
            item.Loader = loader;
            loader.Tag = item.Tag;

            item.Data = null;
            item.Progress = 0;
            //item.RetryNum = 0;
            item.Done = false;
            item.Complete = false;
            item.ErrorMsg = null;

            if (item.OnStart != null)
            {
                try
                {
                    item.OnStart(loader);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }
        }

        private void OnDoneLoadHandler(URLLoader loader, bool complete, string errorMsg)
        {
            LoaderItem item = loader.Item;
            if (item == null)
            {
                XLogger.ErrorFormat("队列加载项的对象变量不能为空!Url:{0}", loader.URL);
                return;
            }

            if (LoadDataCached && mCachePool.ContainsKey(item.URL) == false)
            {
                mCachePool.Add(item.URL, loader.Data);
            }

            endLoad(item, loader, complete, errorMsg);

            if (item.Done)
            {
                for (int i = 0; i < mWaitingList.Count; i++)
                {
                    var tmp = mWaitingList[i];
                    if (tmp.URL == item.URL)
                    {
                        mWaitingList.RemoveAt(i);
                        i -= 1;
                        endLoad(tmp, loader, complete, errorMsg);
                    }
                }
            }

            mLoaderList.Enqueue(loader);
            mLoadingList.Remove(item);
            //loader.Unload();
            
            tryLoadNext();
        }

        private void OnProgressHandler(URLLoader loader, int loadedBytes, int totalBytes, double progress)
        {
            LoaderItem item = loader.Item;
            if (item == null) return;

            progressImp(item, loader, loadedBytes, totalBytes, progress);
            for (int i = 0; i < mWaitingList.Count; i++)
            {
                var itm = mWaitingList[i];
                if (itm.URL == item.URL)
                {
                    progressImp(itm, loader, loadedBytes, totalBytes, progress);
                }
            }
            loader.Tag = item.Tag;
        }

        private void progressImp(LoaderItem item, URLLoader loader, int loadedBytes, int totalBytes, double progress)
        {
            loader.Tag = item.Tag;
            item.Progress = progress;

            if (item.OnProgress != null)
            {
                try
                {
                    item.OnProgress(loader, loadedBytes, totalBytes, progress);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }
        }

        public void DebugCacheKeys()
        {
            List<string> list = new List<string>();
            foreach (var item in mCachePool)
            {
                list.Add(item.Key);
            }
            XLogger.DebugFormat("{0}", string.Join(",", list.ToArray()));
        }
    }
}
