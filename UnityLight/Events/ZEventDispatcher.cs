using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Events
{
    public class ZEventDispatcher : ZEventDispatcher<string, ZEvent>
    {
        public ZEventDispatcher() : base()
        {
        }

        public ZEventDispatcher(object target) : base(target)
        {
        }
    }

    public class ZEventDispatcher<T1, T2> where T2 : ZEvent
    {
        class ListenData
        {
            public Callback<T2> listeners;
        }

        public object CurrentTarget { get; set; }

        private Dictionary<T1, ListenData> _listeners = new Dictionary<T1, ListenData>();

        public ZEventDispatcher()
        {
            CurrentTarget = this;
        }

        public ZEventDispatcher(object target)
        {
            CurrentTarget = target;
        }

        public bool DispatchEvent(T1 id, T2 evt)
        {
            if (evt == null) return false;

            if (_listeners.ContainsKey(id))
            {
                ListenData ld = _listeners[id];

                if (ld.listeners != null)
                {
                    try
                    {
                        evt.CurrentTarget = CurrentTarget;
                        ld.listeners(evt);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        XLogger.Error(string.Format("事件派发出错！EventID：{0}", id), ex);
                    }
                }
            }

            return false;
        }

        public void AddEventListener(T1 id, Callback<T2> listener)
        {
            ListenData ld = null;
            if (_listeners.ContainsKey(id))
            {
                ld = _listeners[id];
            }
            else
            {
                ld = new ListenData();
                _listeners.Add(id, ld);
            }
            ld.listeners += listener;
        }

        public void RemoveEventListener(T1 id, Callback<T2> listener)
        {
            if (_listeners.ContainsKey(id))
            {
                ListenData ld = _listeners[id];
                ld.listeners -= listener;
            }
        }

        public bool HasListener(T1 id)
        {
            return _listeners.ContainsKey(id);
        }

        public void RemoveAllListener(T1 id)
        {
            if (_listeners.ContainsKey(id))
            {
                ListenData ld = _listeners[id];
                _listeners.Remove(id);
                ld.listeners = null;
            }
        }

        public void RemoveAllListener()
        {
            _listeners.Clear();
        }
    }

    #region DispatcherDemo

    //public class DispatcherDemo
    //{
    //    public bool DispatchEvent(ZEvent evt)
    //    {
    //        return false;
    //    }

    //    public void AddEventListener(string type, Callback<ZEvent> listener)
    //    {
    //    }

    //    public void RemoveEventListener(string type, Callback<ZEvent> listener)
    //    {
    //    }

    //    public bool HasListener(string type)
    //    {
    //        return false;
    //    }

    //    public void RemoveAllListener(string type)
    //    {
    //    }

    //    public void RemoveAllListener()
    //    {
    //    }
    //}

    #endregion
}
