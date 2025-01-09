using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;
using UnityLight.Events;
using UnityLight.Exts;

namespace UnityExt.ZNGUI
{
    public interface IZUIObject
    {
        ZUIObjectType Type { get; }

        Transform Trans { get; }

        int TagInt { get; }

        string TagStr { get; }

        GameObject TagObj { get; }

        GameObject gameObject { get; }

        bool Actived { get; set; }

        bool Enable { get; set; }

        void UpdateUI();

        bool HasListener(string type);

        bool DispatchEvent(ZEvent evt);

        void AddEventListener(string type, Callback<ZEvent> listener, bool useCapture);

        void RemoveEventListener(string type, Callback<ZEvent> listener, bool useCapture);

        void RemoveAllListener(string type, bool useCapture);

        void RemoveAllListener(bool useCapture);

        void RemoveAllListener();
    }
}
