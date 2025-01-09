using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.Loaders;
using UnityLight;
using UnityLight.Loggers;

namespace UnityExt.Preloads
{
    public class PreloadBase : IPreload
    {
        public int SortIndex { get; protected set; }

        public string PreloadPath { get; protected set; }

        public Callback<IPreload, bool, string> OnDoneCallback { get; set; }

        public virtual void StartProcessPreload(URLLoader loader, bool success, string errMsg)
        {
            OnProcessDone(success, errMsg);
        }

        protected virtual void OnProcessDone(bool success, string errMsg)
        {
            if (OnDoneCallback != null) OnDoneCallback(this, success, errMsg);
            if (success == false) XLogger.ErrorFormat("Preload process failed!{0}:{1}", PreloadPath, errMsg);
        }
    }
}
