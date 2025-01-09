using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZToggle : IZUIObject
    {
        int GroupID { get; }

        bool Selected { get; set; }
    }
}
