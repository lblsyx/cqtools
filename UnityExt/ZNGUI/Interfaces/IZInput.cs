﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZInput : IZUIObject
    {
        string Value { get; set; }
    }
}
