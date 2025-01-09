using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Loaders;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Events;
using UnityLight.Loggers;

namespace UnityExt.ZNGUI
{
    public class ZDialogBase<T1, T2> : IZDialog, IDisposable
        where T1 : IZDialog, new()
        where T2 : ZDialogBehaviour
    {
        private static T1 _instance;

        private static object _syncRoot = new object();

        public bool Hided { get; private set; }
        public bool Loaded { get; private set; }
        public bool LoadComplete { get; private set; }
        public virtual string FileName
        {
            get
            {
                var str = this.ToString();
                str = str.Substring(str.LastIndexOf(".") + 1);
                return str;
            }
        }
        public virtual string UIName { get { return FileName; } }
        public GameObject GObject { get; private set; }

        public T2 Behaviour { get; private set; }

        protected virtual void InitSelf()
        {
            SetCloseButton(Behaviour.GetUIObject("btnClose") as IZButton);
        }

        protected void SetCloseButton(IZButton iIZButton)
        {
            if (iIZButton != null)
            {
                iIZButton.AddEventListener(ZEvent.CLICK, OnCloseClickHandler, false);
            }
        }

        private void OnCloseClickHandler(ZEvent obj)
        {
            Hide();
        }

        public virtual void Initialize()
        {
        }

        public void Show()
        {
            Hided = false;
            if (GObject != null)
            {
                GObject.SetActive(true);
                OnShow();
            }
            else
            {
                Load();
            }
        }

        public virtual void OnShow()
        {
        }

        public void Hide()
        {
            Hided = true;
            if (GObject != null) GObject.SetActive(false);
            OnHide();
        }

        public virtual void OnHide()
        {

        }

        public void Load()
        {
            if (Loaded) return;

            string path = string.Format("{0}.unity3d", FileName);
            string url = Path.Combine(ZUIManager.UIRootPath, path);

            XLogger.DebugFormat("加载UI:{0}", url);
            ZUIManager.UIDownloadQueue.Load(url, OnLoadDoneHandler, OnLoadProgressHandler, OnStartLoadHandler);
            LoadComplete = false;
            Loaded = true;
            Hided = false;
        }

        public void SetupDialog(AssetBundle bundle)
        {
            SetupDialog(bundle, ZUIManager.UIRoot);
        }

        public void SetupDialog(AssetBundle bundle, Transform tUIRoot)
        {
            if (bundle == null)
            {
                XLogger.ErrorFormat("AssetBundle对象不能为空!Name:{0}", FileName);
                return;
            }

            GameObject oGameObject = ExtUtil.InstantiateBundleAsset(bundle.mainAsset);

            SetupDialog(oGameObject, tUIRoot);
        }

        public void SetupDialog(GameObject oGameObject)
        {
            SetupDialog(oGameObject, ZUIManager.UIRoot);
        }

        public void SetupDialog(GameObject oGameObject, Transform tUIRoot)
        {
            if (oGameObject == null) return;

            try
            {
                GObject = oGameObject;
                GObject.name = FileName;
                GObject.transform.parent = tUIRoot;
                GObject.transform.localPosition = Vector3.zero;
                GObject.transform.localScale = Vector3.one;
                if (tUIRoot != null) ExtUtil.ChangeGameObjectLayer(GObject, tUIRoot.gameObject.layer);
                Behaviour = GObject.AddComponent<T2>();
                Behaviour.ParentDialog = this;
                Behaviour.FindAllChildren();
                Behaviour.Initialize();
                InitSelf();
                Initialize();

                if (Hided) Hide();
                else OnShow();
            }
            catch (Exception ex)
            {
                XLogger.Fatal(ex);
            }

            Loaded = true;
            LoadComplete = true;
        }

        private void OnStartLoadHandler(URLLoader loader)
        {
            ZUIManager.DispatchEvent(new ZUIEvent(ZUIEvent.UI_LOAD_START, this));
        }

        private void OnLoadProgressHandler(URLLoader loader, int bytesLoaded, int bytesTotal, double progress)
        {
            ZUIManager.DispatchEvent(new ZUIEvent(ZUIEvent.UI_LOAD_PROGRESS, this, bytesLoaded, bytesTotal, progress, (int)(progress * 100)));
        }

        private void OnLoadDoneHandler(URLLoader loader, bool success, string errMsg)
        {
            if (success == false)
            {
                XLogger.ErrorFormat("UI加载失败!{0}:{1}", FileName, errMsg);
                return;
            }

            SetupDialog(loader.DataBundle);

            ZUIManager.DispatchEvent(new ZUIEvent(ZUIEvent.UI_LOAD_DONE, this));
        }

        public static T1 Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new T1();
                        ZUIManager.AddDialog(_instance);
                    }
                }

                return _instance;
            }
        }

        public Transform Trans
        {
            get { return Behaviour.transform; }
        }

        public virtual void UpdateUI()
        {
            foreach (var item in Behaviour.ZUIObjectList)
            {
                item.UpdateUI();
            }
        }

        public void Bring2Top()
        {
            if (Behaviour.WinCtrl != null) Behaviour.WinCtrl.Bring2Top();
        }

        public void Bring2Bottom()
        {
            if (Behaviour.WinCtrl != null) Behaviour.WinCtrl.Bring2Bottom();
        }
        public void AdjustDepth(int depth)
        {
            if (Behaviour.WinCtrl != null) Behaviour.WinCtrl.AdjustDepth(depth);
        }

        public void DebugShowObjectKeys()
        {
            if (Behaviour != null && Behaviour.ZUIObjects != null)
            {
                int index = 1;
                XLogger.DebugFormat("ZDialog num children： {0}", Behaviour.ZUIObjects.Count);
                foreach (var item in Behaviour.ZUIObjects.Keys)
                {
                    XLogger.DebugFormat("{0}:{1}", index, item);
                    index += 1;
                }
            }
            else
            {
                XLogger.DebugFormat("未初始化 ZDialog 的Behaviour或Behaviour.ZUIObjects对象!{0}或{1}", Behaviour != null, Behaviour.ZUIObjects != null);
            }
        }

        public void ResetInstance()
        {
            if ((IZDialog)_instance == (IZDialog)this)
            {
                ZUIManager.DelDialog(this);
                _instance = default(T1);
            }
        }

        public void Dispose()
        {
            if ((IZDialog)_instance == (IZDialog)this)
            {
                ZUIManager.DelDialog(this);
            }

            if (Behaviour != null)
            {
                Behaviour.Dispose();
                Behaviour = null;
            }

            ExtUtil.DestroyImmediate(GObject);
            GObject = null;
        }
    }
}
