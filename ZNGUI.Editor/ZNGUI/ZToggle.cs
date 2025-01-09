using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Loggers;
using UnityLight;
using UnityEngine;
using UnityLight.Events;

[RequireComponent(typeof(UIToggle))]
[RequireComponent(typeof(BoxCollider))]
public class ZToggle : ZUIObject, IZToggle
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Toggle; } }

    protected UIToggle mToggle;

    public override void Initialize()
    {
        mToggle = GetComponent<UIToggle>();
        EventDelegate.Add(mToggle.onChange, OnSelectedChangeHandler);
    }

    private void OnSelectedChangeHandler()
    {
        DispatchEvent(new ZEvent(ZEvent.SELECTED_CHANGE));
    }

    public int GroupID
    {
        get { return mToggle.group; }
    }

    public bool Selected
    {
        get { return mToggle.value; }
        set { mToggle.value = value; }
    }

    public override bool Enable
    {
        get { return mToggle.enabled; }
        set { mToggle.enabled = value; }
    }
}
