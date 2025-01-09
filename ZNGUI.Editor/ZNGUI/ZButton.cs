
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight;
using UnityLight.Loggers;
using UnityEngine;
using UnityLight.Events;

[RequireComponent(typeof(UIButton))]
[RequireComponent(typeof(BoxCollider))]
public class ZButton : ZUIObject, IZButton
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Button; } }

    private UIButton mButton;

    public override bool Enable
    {
        get { return mButton.enabled; }
        set { mButton.enabled = value; }
    }

    public override void InitSelf()
    {
        base.InitSelf();
        mButton = GetComponent<UIButton>();
    }
}