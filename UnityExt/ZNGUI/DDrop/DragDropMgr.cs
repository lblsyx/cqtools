using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Events;
using UnityLight.Loggers;

namespace UnityExt.ZNGUI.DDrop
{
    public class DragDropMgr
    {
        public const string DEFAULT_GROUP = "default";

        class DDGroup
        {
            public string GrouName { get; private set; }

            public List<IDragDrop> Items { get; private set; }

            public DDGroup(string name)
            {
                GrouName = name;

                Items = new List<IDragDrop>();
            }

            public bool Contains(IDragDrop iIDragDrop)
            {
                return Items.IndexOf(iIDragDrop) != -1;
            }

            public int AddItem(IDragDrop iIDragDrop, int index = -1)
            {
                if (index < 0)
                {
                    Items.Add(iIDragDrop);
                    return Items.Count - 1;
                }
                else
                {
                    Items.Insert(index, iIDragDrop);
                    return index;
                }
            }

            public IDragDrop RemoveItem(IDragDrop iIDragDrop)
            {
                Items.Remove(iIDragDrop);
                return iIDragDrop;
            }

            public IDragDrop RemoveItemAt(int index)
            {
                if (index < 0 || index >= Items.Count) return null;

                IDragDrop iIDragDrop = Items[index];

                Items.RemoveAt(index);

                return iIDragDrop;
            }
        }

        private static ZEventObject mStage;
        private static bool mIconExists;
        private static IZSprite mIconContainer;
        private static Dictionary<string, DDGroup> mDDGroupDict = new Dictionary<string, DDGroup>();
        private static Dictionary<ZEventObject, IDragDrop> mTriggersDict = new Dictionary<ZEventObject, IDragDrop>();

        private static Vector3 mDragStartPos;
        private static Vector3 mMouseStartPos;
        private static IDragDrop mCurrentDragger;

        private static DDGroup GetGroup(string name)
        {
            DDGroup oDDGroup = null;
            if (mDDGroupDict.ContainsKey(name))
            {
                oDDGroup = mDDGroupDict[name];
            }
            else
            {
                oDDGroup = new DDGroup(name);
                mDDGroupDict.Add(name, oDDGroup);
            }
            return oDDGroup;
        }

        public static void Init(GameObject oIconObject, ZEventObject oStage)
        {
            mStage = oStage;
            mIconExists = false;
            mMouseStartPos = Vector3.zero;

            ZUIObject oZUIObject = oIconObject.GetComponent<ZUIObject>();
            if (oZUIObject != null)
            {
                oZUIObject.InitSelf();
                oZUIObject.InitedSelf = true;
                oZUIObject.Initialize();
                oZUIObject.Inited = true;
            }

            mIconContainer = oZUIObject as IZSprite;
            if (mIconContainer != null)
            {
                mIconContainer.Actived = false;

            }
        }

        public static void RegisterDragDrop(IDragDrop iIDragDrop)
        {
            if (iIDragDrop == null || iIDragDrop.Trigger == null) return;

            DDGroup oDDGroup = GetGroup(iIDragDrop.GroupName);

            if (oDDGroup.Contains(iIDragDrop)) return;

            oDDGroup.AddItem(iIDragDrop);

            mTriggersDict[iIDragDrop.Trigger] = iIDragDrop;

            if (iIDragDrop.ClickDrag)
            {
                iIDragDrop.Trigger.AddEventListener(ZEvent.CLICK, OnDragStartHandler, false);
            }
            else
            {
                iIDragDrop.Trigger.AddEventListener(ZEvent.MOUSE_DOWN, OnDragStartHandler, false);
            }
        }

        public static void UnregisterDragDrop(IDragDrop iIDragDrop)
        {
            if (iIDragDrop == null) return;

            DDGroup oDDGroup = GetGroup(iIDragDrop.GroupName);

            if (oDDGroup != null) oDDGroup.RemoveItem(iIDragDrop);

            iIDragDrop.Trigger.RemoveEventListener(ZEvent.CLICK, OnDragStartHandler, false);
            iIDragDrop.Trigger.RemoveEventListener(ZEvent.MOUSE_DOWN, OnDragStartHandler, false);
        }

        private static IDragDrop GetIDragDrop(ZEventObject oTrigger)
        {
            if (oTrigger == null) return null;
            if (mTriggersDict.ContainsKey(oTrigger))
            {
                return mTriggersDict[oTrigger];
            }
            return null;
        }

        private static void OnDragStartHandler(ZEvent obj)
        {
            IDragDrop iIDragDrop = GetIDragDrop(obj.CurrentTarget as ZEventObject);

            StartDrag(iIDragDrop);
        }

        private static void StartDrag(IDragDrop iIDragDrop)
        {
            if (iIDragDrop == null) return;
            if (mIconContainer == null)
            {
                XLogger.ErrorFormat("未初始化图标容器对象!");
                return;
            }

            mIconExists = iIDragDrop.UpdateDragIcon(mIconContainer);
            if (mIconExists == false) return;

            mCurrentDragger = iIDragDrop;

            if (mCurrentDragger.ShowAtOnce)
            {
                mIconContainer.Actived = true;
            }

            mDragStartPos = mCurrentDragger.Trigger.transform.position;
            mMouseStartPos = ZUIManager.ZUICamera.ScreenToWorldPoint(Input.mousePosition);
            mDragStartPos.z = mMouseStartPos.z = 0;

            mStage.AddEventListener(ZEvent.MOUSE_MOVE, OnDraggingHandler, false);
            if (mCurrentDragger.ClickDrag)
            {
                mStage.AddEventListener(ZEvent.CLICK, OnDropHandler, true);
            }
            else
            {
                mStage.AddEventListener(ZEvent.MOUSE_UP, OnDropHandler, false);
            }

            mStage.AddEventListener(ZEvent.DEACTIVE, OnDragAbortHandler, false);
            mStage.AddEventListener(ZEvent.MOUSE_LEAVE, OnDragAbortHandler, false);

            mCurrentDragger.OnDragCallback();
            if (iIDragDrop.GroupName != DEFAULT_GROUP)
            {
                DDGroup oDDGroup = GetGroup(iIDragDrop.GroupName);
                if (oDDGroup == null) return;
                foreach (var item in oDDGroup.Items)
                {
                    item.OnGroupDragCallback(mCurrentDragger, mCurrentDragger.DragData);
                }
            }
        }

        private static void OnDraggingHandler(ZEvent obj)
        {
            if (mIconContainer.Actived == false)
            {
                mIconContainer.Actived = true;
            }

            Vector3 temp = ZUIManager.ZUICamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = mDragStartPos + (temp - mMouseStartPos);

            mIconContainer.Trans.position = pos;

            mCurrentDragger.OnDragingCallback();
        }

        private static void OnDropHandler(ZEvent obj)
        {
            ReviseDraggerInStage();
            ExecGroupDropCallback();
            bool bManualDrop = DropAndHit(true);
            if (bManualDrop) return;
            StopDragging();
            RemoveDraggingEvents();
        }

        private static void OnDragAbortHandler(ZEvent obj)
        {
            ReviseDraggerInStage();
            ExecGroupDropCallback();
            DropAndHit(false);
            StopDragging();
            RemoveDraggingEvents();
        }

        private static void StopDragging()
        {
            if (mCurrentDragger == null) return;
            mIconExists = false;
            mCurrentDragger = null;
            mIconContainer.ClearSprite();
            mIconContainer.Actived = false;
        }

        private static void ReviseDraggerInStage()
        {
            if (mCurrentDragger == null || mIconExists == false) return;

            if (mCurrentDragger.ReviseInStage == false) return;

            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Math.Min(mousePos.x, Screen.width);
            mousePos.x = Math.Max(mousePos.x, 0);
            mousePos.y = Math.Min(mousePos.y, Screen.height);
            mousePos.y = Math.Max(mousePos.y, 0);

            Vector3 temp = ZUIManager.ZUICamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = mDragStartPos + (temp - mMouseStartPos);

            mIconContainer.Trans.position = pos;
        }

        private static void ExecGroupDropCallback()
        {
            if (mCurrentDragger == null || mCurrentDragger.OnlyDragPos || mCurrentDragger.GroupName == DEFAULT_GROUP) return;

            DDGroup oDDGroup = GetGroup(mCurrentDragger.GroupName);

            foreach (var item in oDDGroup.Items)
            {
                item.OnGroupDropCallback(mCurrentDragger, mCurrentDragger.DragData);
            }
        }

        private static void RemoveDraggingEvents()
        {
            mStage.RemoveEventListener(ZEvent.CLICK, OnDropHandler, true);
            mStage.RemoveEventListener(ZEvent.MOUSE_MOVE, OnDraggingHandler, false);
            mStage.RemoveEventListener(ZEvent.MOUSE_UP, OnDropHandler, false);
            mStage.RemoveEventListener(ZEvent.MOUSE_LEAVE, OnDragAbortHandler, false);
            mStage.RemoveEventListener(ZEvent.DEACTIVE, OnDragAbortHandler, false);
        }

        private static IDragDrop GetDropHitIDragDrop()
        {
            if (mCurrentDragger == null || mCurrentDragger.OnlyDragPos || mCurrentDragger.GroupName == DEFAULT_GROUP) return null;

            ZEventObject oZEventObject = ZUIManager.GetMouseHitFirstObject();

            return GetIDragDrop(oZEventObject);
        }

        private static bool DropAndHit(bool bCheckHit)
        {
            if (mCurrentDragger == null) return false;

            bool bManualDrop = false;
            IDragDrop iIDragDrop = null;
            if (bCheckHit)
            {
                iIDragDrop = GetDropHitIDragDrop();

                if (iIDragDrop != null)
                {
                    bManualDrop = iIDragDrop.ManualDrop;
                    iIDragDrop.OnDropHitCallback(mCurrentDragger, mCurrentDragger.DragData);
                }
            }

            mCurrentDragger.OnDropCallback(iIDragDrop);

            return bManualDrop || mCurrentDragger.ManualDrop;
        }
    }
}
