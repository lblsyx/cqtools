using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityLight;
using UnityLight.Loggers;

namespace UnityExt.Loaders
{
    public class URLLoader : IDisposable
    {
        private const float ZeroProgress = -0.01f;
        private float mLastProgress = ZeroProgress;
        private bool mStarted = false;

        public bool AutoUnloadBundle { get; set; }

        public object Tag { get; set; }

        public LoaderItem Item { get; set; }

        protected WWW www { get; private set; }

        public object Data { get; internal set; }

        public string URL { get; internal set; }

        public int LoadedSize { get; private set; }

        public int TotalSize { get; private set; }

        public float Progress { get; private set; }

        public bool IsDone { get; private set; }

        public string ErrorMsg { get; private set; }

        public Callback<URLLoader> OnStartEvent;
        public Callback<URLLoader> OnCancelEvent;
        public Callback<URLLoader> OnCompleteEvent;
        public Callback<URLLoader, string> OnErrorEvent;
        public Callback<URLLoader, bool, string> OnDoneEvent;
        public Callback<URLLoader, int, int, double> OnProgressEvent;

        public URLLoader()
        {
            AutoUnloadBundle = LoaderConfig.DefaultAutoUnloadBundle;
        }

        public void Load(string path)
        {
            Load(path, AutoUnloadBundle);
        }

        public void Load(string path, bool autoUnload)
        {
            //if (www != null)
            //{
            //    www = null;
            //    //try { www.Dispose(); }
            //    //catch { }
            //    //finally { www = null; }
            //}

            Data = null;

            URL = path;
            IsDone = false;
            Progress = 0;
            TotalSize = 0;
            LoadedSize = 0;
            mLastProgress = ZeroProgress;
            AutoUnloadBundle = autoUnload;
            //www = WWW.LoadFromCacheOrDownload(URL, LoaderConfig.Version);
            //XLogger.DebugFormat("加载：{0}", URL);
            www = new WWW(URL);
            LoaderMgr.AddLoader(this);

            if (mStarted == false)
            {
                mStarted = true;
                if (OnStartEvent != null)
                {
                    try
                    {
                        OnStartEvent(this);
                    }
                    catch (Exception ex)
                    {
                        XLogger.Error(ex);
                    }
                }
            }

            OnProgress();
        }

        public void Cancel()
        {
            if (www == null) return;

            IsDone = true;
            ErrorMsg = "Cancel";

            try { www.Dispose(); }
            catch { }
            finally { www = null; }

            OnCancel();
        }

        public void UnloadBundle()
        {
            UnloadBundle(false);
        }

        public void UnloadBundle(bool unloadAllLoadedObjects)
        {
            if (www != null && www.assetBundle != null)
            {
                www.assetBundle.Unload(unloadAllLoadedObjects);
            }
        }

        public string DataText
        {
            get { return Data as string; }
        }

        public byte[] DataBytes
        {
            get { return (byte[])Data; }
        }

        public Texture DataTexture
        {
            get { return Data as Texture; }
        }

        public AudioClip DataAudioClip
        {
            get { return Data as AudioClip; }
        }

        public AssetBundle DataBundle
        {
            get { return Data as AssetBundle; }
        }

        private int CalcTotalSize(int bytesLoaded, float progress)
        {
            return (int)Math.Round(bytesLoaded / progress);
        }

        public void Update()
        {
            if (www == null || IsDone) return;
            if (www.isDone)
            {
                IsDone = www.isDone;
                ErrorMsg = www.error;

                if (string.IsNullOrEmpty(ErrorMsg))
                {//加载成功
                    Progress = 1f;
                    mLastProgress = 1f;
                    TotalSize = www.bytesDownloaded;
                    LoadedSize = www.bytesDownloaded;

                    SwitchData();

                    OnProgress();
                    OnComplete();
                }
                else
                {//加载失败
                    OnError();
                }
            }
            else
            {
                //加载进度
                //TotalSize = www.size;
                Progress = www.progress;
                //LoadedSize = www.bytesDownloaded;
                //if (TotalSize == 0) Progress = 0;
                //else Progress = LoadedSize / TotalSize;

                if (mLastProgress != Progress)
                {
                    mLastProgress = Progress;
                    //LoadedSize = www.bytesDownloaded;
                    //TotalSize = CalcTotalSize(LoadedSize, Progress);

                    OnProgress();
                }
            }
        }

        internal void AsyncComplete()
        {
            if (Data != null)
            {
                TotalSize = 1;
                LoadedSize = 1;
                OnProgress();
                OnComplete();
            }
            else
            {
                TotalSize = 0;
                LoadedSize = 0;
                OnError();
            }
        }

        protected virtual void SwitchData()
        {
            string ext = Path.GetExtension(URL).ToLower();
            ext = ext.Replace(".", "");
            switch (ext)
            {
                case "unity3d":
                case "assetbundle":
                    Data = www.assetBundle;
                    break;
                case "txt":
                    Data = www.text;
                    break;
                case "png":
                case "jpg":
                case "jpeg":
                    Data = www.texture;
                    break;
                case "ogg":
                case "mp3":
                case "wav":
                    Data = www.audioClip;
                    break;
                default:
                    Data = www.bytes;
                    break;
            }
        }

        protected virtual void OnCancel()
        {
            mStarted = false;

            if (OnCancelEvent != null)
            {
                try
                {
                    OnCancelEvent(this);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            //OnDone(false, ErrorMsg);
        }

        protected virtual void OnProgress()
        {
            if (OnProgressEvent != null)
            {
                try
                {
                    OnProgressEvent(this, LoadedSize, TotalSize, Progress);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }
        }

        protected virtual void OnComplete()
        {
            
            if (OnCompleteEvent != null)
            {
                try
                {
                    OnCompleteEvent(this);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            OnDone(true, null);

            if (AutoUnloadBundle) UnloadBundle();
        }

        protected virtual void OnError()
        {
            XLogger.ErrorFormat("加载失败：{0}", URL);
            if (OnErrorEvent != null)
            {
                try
                {
                    OnErrorEvent(this, ErrorMsg);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            OnDone(false, ErrorMsg);
        }

        protected virtual void OnDone(bool success, string errMsg)
        {
            if (OnDoneEvent != null)
            {
                try
                {
                    OnDoneEvent(this, success, errMsg);
                }
                catch (Exception ex)
                {
                    XLogger.Error(ex);
                }
            }

            mStarted = false;
        }

        public void Dispose()
        {
            OnDoneEvent = null;
            OnErrorEvent = null;
            OnCancelEvent = null;
            OnCompleteEvent = null;
            OnProgressEvent = null;

            try { www.Dispose(); }
            catch { }
            finally { www = null; }
        }
    }
}
