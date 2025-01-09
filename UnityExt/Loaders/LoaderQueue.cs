using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;
using UnityLight.Loggers;

namespace UnityExt.Loaders
{
    public class LoaderQueue
    {
        public Callback<LoaderQueue, double> OnProgressEvent;
        public Callback<LoaderQueue, bool, string> OnDoneEvent;

        private IList<IList<LoaderItem>> mQueueList;
        private IList<LoaderItem> mLoadingList;
        private Queue<URLLoader> mLoaderList;
        private IList<LoaderItem> mAllItems;

        public bool AutoLoad { get; set; }

        public bool AutoUnloadBundle { get; set; }

        public bool UnloadAllLoadedObjects { get; set; }

        public int MaxLoading { get; set; }

        public int NumLoading { get; private set; }

        public LoaderQueue()
        {
            Initialize(LoaderConfig.DefaultLoadingNum);
        }

        public LoaderQueue(int maxLoading)
        {
            Initialize(maxLoading);
        }

        private void Initialize(int maxLoading)
        {
            NumLoading = 0;
            AutoLoad = true;
            MaxLoading = maxLoading;
            AutoUnloadBundle = false;
            UnloadAllLoadedObjects = false;

            mAllItems = new List<LoaderItem>();
            mLoaderList = new Queue<URLLoader>();
            mLoadingList = new List<LoaderItem>();
            mQueueList = new List<IList<LoaderItem>>();

            for (int i = 0; i < LoaderConfig.MaxPriority; i++)
            {
                mQueueList.Add(new List<LoaderItem>());
            }
        }

        public double Progress
        {
            get
            {
                double p = 0;
                for (int i = 0; i < mAllItems.Count; i++)
                {
                    p += mAllItems[i].Progress;
                }
                p = p / mAllItems.Count;

                return p;
            }
        }

        public object GetData(string url)
        {
            foreach (var item in mAllItems)
            {
                if (item.URL == url)
                {
                    return item.Data;
                }
            }
            return null;
        }

        public LoaderItem AddLoad(string url)
        {
            return AddLoad(url, LoaderConfig.DefaultPriority, null, null);
        }

        public LoaderItem AddLoad(string url, Callback<URLLoader, bool, string> doneCallback)
        {
            return AddLoad(url, LoaderConfig.DefaultPriority, doneCallback, null);
        }

        public LoaderItem AddLoad(string url, uint priority, Callback<URLLoader, bool, string> doneCallback)
        {
            return AddLoad(url, priority, doneCallback, null);
        }

        public LoaderItem AddLoad(string url, uint priority, Callback<URLLoader, bool, string> doneCallback, Callback<URLLoader, int, int, double> progressCallback)
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
            item.OnProgress = progressCallback;

            mAllItems.Add(item);
            mQueueList[(int)priority].Add(item);
            if (AutoLoad) tryLoadNext();

            return item;
        }

        public void Start()
        {
            tryLoadNext();
        }

        public void Start(int maxLoadNum)
        {
            MaxLoading = maxLoadNum;

            tryLoadNext();
        }

        public void Stop()
        {
            NumLoading = 0;
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
                mQueueList[(int)item.Priority].Insert(0, item);
            }
            mLoadingList.Clear();
        }

        public void ClearQueue()
        {
            ClearQueue(false);
        }

        public void ClearQueue(bool bUnloadAllLoadedObjects)
        {
            Stop();
            UnloadBundle(bUnloadAllLoadedObjects);
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
        }

        public void UnloadBundle(bool bUnloadAllLoadedObjects)
        {
            //if (AutoUnloadBundle) return;

            for (int i = 0; i < mAllItems.Count; i++)
            {
                LoaderItem item = mAllItems[i];
                if (item.Complete && item.Data != null && item.Data is AssetBundle)
                {
                    AssetBundle bundle = item.Data as AssetBundle;
                    bundle.Unload(bUnloadAllLoadedObjects);
                }
            }
            mAllItems.Clear();
        }

        private void tryLoadNext()
        {
            while (NumLoading < MaxLoading)
            {
                bool did = false;
                for (int i = 0; i < LoaderConfig.MaxPriority; i++)
                {
                    IList<LoaderItem> q = mQueueList[i];
                    
                    if (q.Count != 0)
                    {
                        LoaderItem item = q[0];
                        q.RemoveAt(0);
                        doLoad(item);
                        did = true;
                        break;
                    }
                }

                if (did == false || NumLoading >= MaxLoading) break;
            }
        }

        private void doLoad(LoaderItem item)
        {
            NumLoading += 1;
            URLLoader loader = null;
            if (mLoaderList.Count != 0)
            {
                loader = mLoaderList.Dequeue();
            }
            else
            {
                loader = new URLLoader();
                loader.AutoUnloadBundle = false;
                loader.OnStartEvent = OnStartLoadHandler;
                loader.OnDoneEvent = OnDoneLoadHandler;
                loader.OnProgressEvent = OnProgressHandler;
            }

            loader.Item = item;
            item.Loader = loader;
            loader.Tag = null;
            mLoadingList.Add(item);
            loader.Load(item.URL);
        }

        private void endLoad(LoaderItem item, bool complete, string errorMsg)
        {
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
                    item.OnDone(item.Loader, complete, errorMsg);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }
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
            if (item == null) return;
            loader.Tag = item.Tag;
            item.Data = loader.Data;
            endLoad(item, complete, errorMsg);
            mLoaderList.Enqueue(item.Loader);
            mLoadingList.Remove(item);
            item.Loader = null;
            loader.Item = null;
            loader.Tag = null;
            //if (complete)
            //{
            //    item.Tag = null;
            //}
            //loader.Unload();
            NumLoading -= 1;

            OnQueueProgress(Progress);
            tryLoadNext();

            if (NumLoading <= 0) OnQueueDone();
        }

        private void OnProgressHandler(URLLoader loader, int loadedBytes, int totalBytes, double progress)
        {
            LoaderItem item = loader.Item;
            if (item == null) return;
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

            OnQueueProgress(Progress);
        }

        protected virtual void OnQueueProgress(double progress)
        {
            if (OnProgressEvent != null)
            {
                try
                {
                    OnProgressEvent(this, progress);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }
        }

        protected virtual void OnQueueDone()
        {
            if (OnDoneEvent != null)
            {
                bool success = true;
                string errMsgs = string.Empty;
                for (int i = 0; i < mAllItems.Count; i++)
                {
                    LoaderItem item = mAllItems[i];
                    if (item.Complete == false)
                    {
                        errMsgs += item.ErrorMsg;
                        errMsgs += "\r\n";
                        success = false;
                        break;
                    }
                }

                try
                {
                    OnDoneEvent(this, success, errMsgs);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            if (AutoUnloadBundle) UnloadBundle(UnloadAllLoadedObjects);
        }
    }
}
