using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Events;
using UnityLight.Loggers;

[RequireComponent(typeof(UISlider))]
public class ZProgress : ZUIObject, IZProgress
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Progress; } }

    private UISlider mUISlider;

    public override void InitSelf()
    {
        base.InitSelf();
        mUISlider = GetComponent<UISlider>();
        EventDelegate.Add(mUISlider.onChange, OnChangeHandler);
    }

    public override bool Enable
    {
        get { return mUISlider.enabled; }
        set { mUISlider.enabled = value; }
    }

    public float Progress
    {
        get { return mUISlider.value; }
        set { mUISlider.value = value; }
    }

    private void OnChangeHandler()
    {
        if (HasListener(ZEvent.CHANGED))
        {
            DispatchEvent(new ZEvent(ZEvent.CHANGED));
        }
    }
}
