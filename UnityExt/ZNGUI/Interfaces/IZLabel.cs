using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZLabel : IZUIObject
    {
        string Value { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        Color Color { get; set; }
    }
}
