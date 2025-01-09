using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;

public class ZUIAtlas : ZUIObject, IZUIAtlas
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Atlas; } }

    public UIAtlas Atlas;

    public MonoBehaviour UIAtlas
    {
        get { return Atlas; }
        //get { if (Atlas != null) return Atlas.atlas; return null; }
    }
}
