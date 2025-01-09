using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;


namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZListItem : IZUIObject
    {
        int Index { get; }

        IZList Parent { get; }

        GameObject CurrentObject { get; }

        ZUIObject GetObject(string sNameKey);

        void DebugShowObjectKeys();
    }
}
