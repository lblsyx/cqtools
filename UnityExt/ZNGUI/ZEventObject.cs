using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight;
using UnityLight.Events;


namespace UnityExt.ZNGUI
{
    public class ZEventObject : MonoBehaviour, IDisposable
    {
        public const float DoubleClickSpan = 0.3f;

        public bool Inited { get; set; }

        public bool InitedSelf { get; set; }

        protected bool mMouseDown;
        protected bool mMouseRightDown;
        protected float mLastClickTicks;
        protected float mLastRightClickTicks;

        protected ZEventDispatcher EventDispatcher;

        protected ZEventDispatcher CaptureEventDispatcher;

        public virtual void InitSelf()
        {
            mMouseDown = false;
            mMouseRightDown = false;
            EventDispatcher = new ZEventDispatcher(this);
            CaptureEventDispatcher = new ZEventDispatcher(this);
        }

        public virtual void Initialize()
        {
        }

        public virtual bool Actived
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        public virtual bool Enable
        {
            get { return Actived; }
            set { Actived = value; }
        }

        public virtual void OnMouseOver()
        {
            DispatchEvent(new ZEvent(ZEvent.MOUSE_OVER));
        }

        public virtual void OnMouseMove()
        {
            DispatchEvent(new ZEvent(ZEvent.MOUSE_MOVE));
        }

        public virtual void OnMouseOut()
        {
            DispatchEvent(new ZEvent(ZEvent.MOUSE_OUT));
        }

        public virtual void OnMouseDown()
        {
            if (mMouseDown == false)
            {
                mMouseDown = true;
                DispatchEvent(new ZEvent(ZEvent.MOUSE_DOWN));
            }
        }

        public virtual void OnRightMouseDown()
        {
            if (mMouseRightDown == false)
            {
                mMouseRightDown = true;
                DispatchEvent(new ZEvent(ZEvent.RIGHT_MOUSE_DOWN));
            }
        }

        public virtual void OnMouseUp(bool bMouseIn)
        {
            if (mMouseDown)
            {
                mMouseDown = false;
                DispatchEvent(new ZEvent(ZEvent.MOUSE_UP));
                if (bMouseIn)
                {
                    float ticks = Time.realtimeSinceStartup - mLastClickTicks;

                    if (ticks < DoubleClickSpan)
                    {
                        mLastClickTicks = 0;
                        DispatchEvent(new ZEvent(ZEvent.DOUBLE_CLICK));
                    }
                    else
                    {
                        DispatchEvent(new ZEvent(ZEvent.CLICK));
                        mLastClickTicks = Time.realtimeSinceStartup;
                    }
                }
            }
        }

        public virtual void OnRightMouseUp(bool bMouseIn)
        {
            if (mMouseRightDown)
            {
                mMouseRightDown = false;
                DispatchEvent(new ZEvent(ZEvent.RIGHT_MOUSE_UP));
                if (bMouseIn)
                {
                    float ticks = Time.realtimeSinceStartup - mLastClickTicks;
                    if (ticks < DoubleClickSpan)
                    {
                        mLastRightClickTicks = 0;
                        DispatchEvent(new ZEvent(ZEvent.RIGHT_DOUBLE_CLICK));
                    }
                    else
                    {
                        DispatchEvent(new ZEvent(ZEvent.RIGHT_CLICK));
                        mLastClickTicks = Time.realtimeSinceStartup;
                    }
                }
            }
        }

        public virtual bool DispatchEvent(ZEvent evt)
        {
            if (EventDispatcher == null || Actived == false || Enable == false) return false;

            evt.Target = this;
            ZEventObject[] oZEventObjects = GetComponentsInParent<ZEventObject>();

            for (int i = oZEventObjects.Length - 1; i >= 0; i--)
            {
                ZEventObject oZEventObject = oZEventObjects[i];
                if (oZEventObject is IZGroup) continue;
                if (oZEventObject.CaptureEventDispatcher != EventDispatcher && oZEventObject.CaptureEventDispatcher.HasListener(evt.Type))
                {
                    oZEventObject.CaptureEventDispatcher.DispatchEvent(evt);
                }
            }

            bool rlt = EventDispatcher.DispatchEvent(evt);

            for (int i = 0; i < oZEventObjects.Length; i++)
            {
                ZEventObject oZEventObject = oZEventObjects[i];
                if (oZEventObject is IZGroup) continue;
                if (oZEventObject.EventDispatcher != EventDispatcher && oZEventObject.EventDispatcher.HasListener(evt.Type))
                {
                    oZEventObject.EventDispatcher.DispatchEvent(evt);
                }
            }

            return rlt;
        }

        public virtual bool HasListener(string type)
        {
            return EventDispatcher.HasListener(type);
        }

        public virtual void AddEventListener(string type, Callback<ZEvent> listener, bool useCapture)
        {
            if (useCapture)
            {
                CaptureEventDispatcher.AddEventListener(type, listener);
            }
            else
            {
                EventDispatcher.AddEventListener(type, listener);
            }
        }

        public virtual void RemoveEventListener(string type, Callback<ZEvent> listener, bool useCapture)
        {
            if (useCapture)
            {
                CaptureEventDispatcher.RemoveEventListener(type, listener);
            }
            else
            {
                EventDispatcher.RemoveEventListener(type, listener);
            }
        }

        public virtual void RemoveAllListener(string type, bool useCapture)
        {
            if (useCapture)
            {
                CaptureEventDispatcher.RemoveAllListener(type);
            }
            else
            {
                EventDispatcher.RemoveAllListener(type);
            }
        }

        public virtual void RemoveAllListener(bool useCapture)
        {
            if (useCapture)
            {
                CaptureEventDispatcher.RemoveAllListener();
            }
            else
            {
                EventDispatcher.RemoveAllListener();
            }
        }

        public virtual void RemoveAllListener()
        {
            EventDispatcher.RemoveAllListener();
            CaptureEventDispatcher.RemoveAllListener();
        }

        public virtual void Update()
        {
        }

        //public override void InitSelf()
        //{
        //    base.InitSelf();

        //    //UIEventListener.Get(gameObject).onClick += OnItemClick;
        //    //UIEventListener.Get(gameObject).onDoubleClick += OnItemDoubleClick;
        //}

        //private void OnItemClick(GameObject go)
        //{
        //    DispatchEvent(new ZEvent(ZEvent.CLICK));
        //}

        //private void OnItemDoubleClick(GameObject go)
        //{
        //    DispatchEvent(new ZEvent(ZEvent.DOUBLE_CLICK));
        //}

        public virtual void Dispose()
        {
            RemoveAllListener();
        }
    }
}
