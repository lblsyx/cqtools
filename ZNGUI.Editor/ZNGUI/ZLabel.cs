using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Loggers;

[RequireComponent(typeof(UILabel))]
public class ZLabel : ZUIObject, IZLabel
{
    private string mText;
    private UILabel mLabel;

    public override ZUIObjectType Type { get { return ZUIObjectType.Label; } }

    public override void Initialize()
    {
        mText = string.Empty;
        mLabel = GetComponent<UILabel>();
    }

    public string Value
    {
        get
        {
            if (mLabel == null)
                return string.Empty;
            mText = mLabel.text;
            return mText;
        }
        set
        {
            mText = value;
            if (mText == null)
                mText = string.Empty;
            if (mLabel != null)
                mLabel.text = mText;
        }
    }

    public override bool Enable
    {
        get
        {
            return mLabel.enabled;
        }
        set
        {
            mLabel.enabled = value;
        }
    }

    public int Width
    {
        get
        {
            if (mLabel != null)
                return mLabel.width;
            return 0;
        }
        set
        {
            if (mLabel != null)
                mLabel.width = value;
        }
    }


    public int Height
    {
        get
        {
            if (mLabel != null)
                return mLabel.height;
            return 0;
        }
        set
        {
            if (mLabel != null)
                mLabel.height = value;
        }
    }

    public Color Color
    {
        get
        {
            if (mLabel != null)
                return mLabel.color;
            else
                return Color.white;
        }
        set
        {
            if (mLabel != null)
                mLabel.color = value;
        }
    }
}