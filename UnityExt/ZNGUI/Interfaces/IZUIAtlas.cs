using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZUIAtlas : IZUIObject
    {
        MonoBehaviour UIAtlas { get; }
    }
}
