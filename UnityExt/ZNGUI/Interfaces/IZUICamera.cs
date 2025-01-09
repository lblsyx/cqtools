using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZUICamera : IZUIObject
    {
        bool RaycastMouseHitUI();
        ZEventObject RaycastMouseHit();
        Vector3 ScreenToWorldPoint(Vector3 pos);
        Vector3 WorldToScreenPoint(Vector3 pos);
    }
}
