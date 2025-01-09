using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Loaders;
using UnityExt.ZNGUI.Interfaces;
using UnityLight;
using UnityLight.Events;
using UnityLight.Loggers;

namespace UnityExt.ZNGUI
{
    public class ZUIEventDispatcher : ZEventDispatcher<ZUIEvent>
    {

    }

    public class ZUIManager
    {
        public static DownloadQueue UIDownloadQueue = new DownloadQueue();

        private static ZUIEventDispatcher mDispatcher = new ZUIEventDispatcher();

        private static Dictionary<string, IZDialog> mDialogs = new Dictionary<string, IZDialog>();

        public static string UIRootPath { get; private set; }

        public static Transform UIRoot { get; private set; }

        public static Camera UIMainCamera { get; private set; }

        public static IZUICamera ZUICamera { get; private set; }

        public static ZEventObject StageEventObject { get; private set; }

        public static void Init(string sRootPath, Transform tUIRoot, Camera cUICamera)
        {
            UIRoot = tUIRoot;
            UIRootPath = sRootPath;
            UIMainCamera = cUICamera;

            ZUICamera = UIMainCamera.GetComponent<ZUIObject>() as IZUICamera;
            if (ZUICamera == null) throw new Exception("请对UI镜头添加ZUICamera组件!");

            StageEventObject = UIRoot.GetComponent<ZEventObject>();
            if (StageEventObject == null)
            {
                StageEventObject = UIRoot.gameObject.AddComponent<ZEventObject>();
                StageEventObject.InitSelf();
                StageEventObject.InitedSelf = true;
                StageEventObject.Initialize();
                StageEventObject.Inited = true;
            }
            ZInputMgr.Init(StageEventObject);
        }
        
        public static void UpdateUI()
        {
            foreach (var key in mDialogs.Keys)
            {
                if (mDialogs[key].LoadComplete)
                {
                    mDialogs[key].UpdateUI();
                }
            }
        }

        public static bool IsOnUIView
        {
            get
            {
                Ray ray = UIMainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool IsMouseHitUI
        {
            get { if (ZUICamera != null) return ZUICamera.RaycastMouseHitUI(); return false; }
        }

        public static ZEventObject GetMouseHitFirstObject()
        {
            if (ZUICamera != null)
            {
                return ZUICamera.RaycastMouseHit();
            }
            return null;
        }

        public static void FindAllUIObject(Transform trans, IList<ZUIObject> list, Dictionary<string, ZUIObject> dict, bool bCheckSkip, bool bSkipSelf)
        {
            ZUIObject[] array = trans.GetComponentsInChildren<ZUIObject>(true);
            //XLogger.DebugFormat("Item ZUIObject count: {0}", array.Length);
            foreach (ZUIObject item in array)
            {
                if (bSkipSelf && item.transform == trans) continue;

                if (list != null) list.Add(item);

                if (bCheckSkip && CheckSkip(item)) continue;
//#if NEW_CLIENT
                string itemKey = string.Format("{1}{0}", item.gameObject.name, GetShortTypeName(item.Type));
//#else
//                string itemKey = string.Format("{0}_{1}", item.gameObject.name, GetShortTypeName(item.Type));
//#endif
                //XLogger.DebugFormat("{0}:{1}", itemKey, item);
                if (dict.ContainsKey(itemKey) == false)
                {
                    dict.Add(itemKey, item);
                }
                else
                {
                    XLogger.ErrorFormat("已包含{0}项，无法添加！", itemKey);
                }
            }
        }

        static bool CheckSkip(ZUIObject item)
        {
            if (item is IZListItem) return true;

            var parents = item.GetComponentsInParent<ZUIObject>();
            foreach (var pnt in parents)
            {
                if (pnt is IZListItem && item != pnt)
                {
                    return true;
                }
            }
            return false;
        }
        
        public static void AddDialog(IZDialog iZDialog)
        {
            if (iZDialog == null) return;
            if (mDialogs.ContainsKey(iZDialog.FileName))
            {
                XLogger.ErrorFormat("{0}的UI对象已存在!", iZDialog.FileName);
                return;
            }
            mDialogs.Add(iZDialog.FileName, iZDialog);
        }

        public static void DelDialog(IZDialog iZDialog)
        {
            if (iZDialog == null) return;
            
            if (mDialogs.ContainsKey(iZDialog.FileName))
            {
                mDialogs.Remove(iZDialog.FileName);
            }
        }

        public static IZDialog GetDialog(string name)
        {
            if (mDialogs.ContainsKey(name))
            {
                return mDialogs[name];
            }

            return null;
        }

        static string GetShortTypeName(ZUIObjectType zUIObjectType)
        {
            switch (zUIObjectType)
            {
                case ZUIObjectType.Label:
                    return "lbl";
                case ZUIObjectType.Input:
                    return "ipt";
                case ZUIObjectType.Button:
                    return "btn";
                case ZUIObjectType.Toggle:
                    return "tgl";
                case ZUIObjectType.Image:
                    return "img";
                case ZUIObjectType.List:
                    return "lst";
                case ZUIObjectType.ListItem:
                    return "li";
                case ZUIObjectType.Progress:
                    return "prg";
                case ZUIObjectType.Sprite:
                    return "sp";
                case ZUIObjectType.Group:
                    return "grp";
                case ZUIObjectType.Atlas:
                    return "ats";
                case ZUIObjectType.Window:
                    return "win";
                case ZUIObjectType.Animation:
                    return "anm";
                case ZUIObjectType.ScrollBar:
                    return "scl";
                case ZUIObjectType.Filter:
                    return "flt";
                default:
                    return "un";
            }
        }


        public static bool DispatchEvent(ZUIEvent evt)
        {
            return mDispatcher.DispatchEvent(evt);
        }

        public static void AddEventListener(string type, Callback<ZUIEvent> listener)
        {
            mDispatcher.AddEventListener(type, listener);
        }

        public static void RemoveEventListener(string type, Callback<ZUIEvent> listener)
        {
            mDispatcher.RemoveEventListener(type, listener);
        }

        public static bool HasListener(string type)
        {
            return mDispatcher.HasListener(type);
        }

        public static void RemoveAllListener(string type)
        {
            mDispatcher.RemoveAllListener(type);
        }

        public static void RemoveAllListener()
        {
            mDispatcher.RemoveAllListener();
        }
    }
}
