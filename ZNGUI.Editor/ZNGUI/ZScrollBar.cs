using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Events;

[RequireComponent(typeof(UIScrollBar))]
public class ZScrollBar : ZUIObject, IZScrollBar
{
    public override ZUIObjectType Type { get { return ZUIObjectType.ScrollBar; } }

    public ZButton UpButton;

    public ZButton DownButton;

    public float ScrollStep = 0.1F;

    private UIScrollBar mUIScrollBar;

    public override void InitSelf()
    {
        base.InitSelf();
        mUIScrollBar = GetComponent<UIScrollBar>();
    }

    public override void Initialize()
    {
        if (UpButton != null) UpButton.AddEventListener(ZEvent.CLICK, OnUpButtonClickHandler, false);
        if (DownButton != null) DownButton.AddEventListener(ZEvent.CLICK, OnDownButtonClickHandler, false);
    }

    private void OnUpButtonClickHandler(ZEvent obj)
    {
        mUIScrollBar.value = Math.Max(mUIScrollBar.value - ScrollStep, 0);
    }

    private void OnDownButtonClickHandler(ZEvent obj)
    {
        mUIScrollBar.value = Math.Min(mUIScrollBar.value + ScrollStep, 1);
    }

    public float ScrollValue
    {
        get { return mUIScrollBar.value; }
        set { mUIScrollBar.value = value; }
    }

    public override void Dispose()
    {
        if (UpButton != null) UpButton.RemoveEventListener(ZEvent.CLICK, OnUpButtonClickHandler, false);
        if (DownButton != null) DownButton.RemoveEventListener(ZEvent.CLICK, OnDownButtonClickHandler, false);

        base.Dispose();
    }
}
