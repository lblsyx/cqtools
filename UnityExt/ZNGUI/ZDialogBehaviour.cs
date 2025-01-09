using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Loggers;

namespace UnityExt.ZNGUI
{
    public class ZDialogBehaviour : MonoBehaviour, IDisposable
    {
        public IZDialog ParentDialog { get; set; }

        public IZWindow WinCtrl { get; protected set; }

        public IZUIAtlas DialogAtlas { get; protected set; }

        protected Transform Trans { get; private set; }

        protected IList<ZUIObject> _list = new List<ZUIObject>();
        protected Dictionary<string, ZUIObject> _dict = new Dictionary<string, ZUIObject>();

        public void FindAllChildren()
        {
            _dict.Clear();
            Trans = transform;
            _list.Clear();
            _dict.Clear();
            ZUIManager.FindAllUIObject(Trans, _list, _dict, true, false);

            foreach (var item in _list)
            {
                if (item.InitedSelf) continue;
                item.InitSelf();
                item.InitedSelf = true;
            }

            foreach (var item in _list)
            {
                if (item.Inited) continue;
                item.Initialize();
                item.Inited = true;
            }
        }

        public virtual void Initialize()
        {
        }

        public ZUIObject GetUIObject(string _objName)
        {
            if(_dict.ContainsKey(_objName))
            {
                return _dict[_objName];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, ZUIObject> ZUIObjects
        {
            get { return _dict; }
        }

        public IList<ZUIObject> ZUIObjectList
        {
            get { return _list; }
        }

        public void DebugShowChildren()
        {
            var idx = 0;
            XLogger.DebugFormat("Number of children:{0}", _dict.Count);
            foreach (var item in _dict)
            {
                XLogger.DebugFormat("{0}:{1}", idx, item.Key);
                idx += 1;
            }
        }

        public void Dispose()
        {
            foreach (var item in _list)
            {
                item.Dispose();
            }
            _list.Clear();
            _dict.Clear();
            Trans = null;
            WinCtrl = null;
            DialogAtlas = null;
            ParentDialog = null;
        }
    }
}
