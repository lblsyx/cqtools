using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Events;

public class ZGroup : ZUIObject, IZGroup
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Group; } }

    private ZToggle[] mZToggleArray;

    public override void InitSelf()
    {
        base.InitSelf();

        UIToggle oUIToggle = GetComponentInChildren<UIToggle>();
        if (oUIToggle != null) GroupID = oUIToggle.group;

        mZToggleArray = GetComponentsInChildren<ZToggle>();
    }

    public override void Initialize()
    {
        for (int i = 0; i < mZToggleArray.Length; i++)
        {
            ZToggle oZToggle = mZToggleArray[i];
            oZToggle.AddEventListener(ZEvent.SELECTED_CHANGE, OnItemSelectedChangeHandler, false);
        }
    }

    private void OnItemSelectedChangeHandler(ZEvent obj)
    {
        ZToggle oZToggle = obj.CurrentTarget as ZToggle;
        if (oZToggle.Selected)
        {
            DispatchEvent(new ZEvent(ZEvent.SELECTED_CHANGE));
        }
        else if (CanBeNone && SelectedIndex == -1)
        {
            DispatchEvent(new ZEvent(ZEvent.SELECTED_CHANGE));
        }
    }

    public int GroupID { get; private set; }

    public bool CanBeNone
    {
        get
        {
            UIToggle oUIToggle = GetComponentInChildren<UIToggle>();
            return oUIToggle.optionCanBeNone;
        }
        set
        {
            var oUIToggleArray = GetComponentsInChildren<UIToggle>();
            foreach (var item in oUIToggleArray)
            {
                item.optionCanBeNone = value;
            }
        }
    }

    public int SelectedIndex
    {
        get
        {
            UIToggle oUIToggle = UIToggle.GetActiveToggle(GroupID);
            if (oUIToggle == null) return -1;
            ZToggle oZToggle = oUIToggle.gameObject.GetComponent<ZToggle>();
            if (oZToggle == null) return -1;
            return Array.IndexOf(mZToggleArray, oZToggle);
        }
        set
        {
            if (value < 0)
            {
                for (int i = 0; i < mZToggleArray.Length; i++)
                {
                    mZToggleArray[i].Selected = false;
                }
            }
            else if (value < mZToggleArray.Length)
            {
                mZToggleArray[value].Selected = true;
            }
            else if (mZToggleArray.Length > 0)
            {
                mZToggleArray[mZToggleArray.Length - 1].Selected = true;
            }
        }
    }

    public IZToggle SelectedItem
    {
        get
        {
            UIToggle oUIToggle = UIToggle.GetActiveToggle(GroupID);
            ZToggle oZToggle = oUIToggle.gameObject.GetComponent<ZToggle>();
            return oZToggle;
        }
        set
        {
            if (value != null) value.Selected = true;
            else SelectedIndex = -1;
        }
    }
}
