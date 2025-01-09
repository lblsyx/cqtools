using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Loggers;
using UnityEngine;
using UnityLight;
using UnityLight.Events;

[RequireComponent(typeof(UIInput))]
[RequireComponent(typeof(BoxCollider))]
public class ZInput : ZUIObject, IZInput
{
    private string mText;
    private UIInput mInput;

    public override ZUIObjectType Type { get { return ZUIObjectType.Input; } }

    public override void Initialize()
    {
        mText = string.Empty;
        mInput = GetComponent<UIInput>();
        EventDelegate.Add(mInput.onSubmit, OnInputSubmit);
        EventDelegate.Add(mInput.onChange, OnInputValueChange);
    }

    public string Value
    {
        get
        {
            if (mInput == null)
            {
                return string.Empty;
            }
            mText = mInput.value;
            return mText;
        }
        set
        {
            mText = value;
            if (mInput != null)
            {
                mInput.value = mText;
            }
        }
    }

    public override bool Enable
    {
        get
        {
            return mInput.enabled;
        }
        set
        {
            mInput.enabled = value;
        }
    }

    void OnInputSubmit()
    {
        DispatchEvent(new ZEvent(ZEvent.SUBMIT));
    }

    void OnInputValueChange()
    {
        if (HasListener(ZEvent.CHANGED))
        {
            DispatchEvent(new ZEvent(ZEvent.CHANGED));
        }
    }
}
