using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityEngine;
using UnityLight;
using UnityLight.Loggers;
using UnityLight.Events;

//[RequireComponent(typeof(UIEventListener))]
public class ZListItem : ZUIObject, IZListItem
{
    public override ZUIObjectType Type { get { return ZUIObjectType.ListItem; } }

    public int Index { get; set; }

    public IZList Parent { get; internal set; }

    public GameObject CurrentObject { get { return gameObject; } }

    private Dictionary<string, ZUIObject> mUIObjectDict = new Dictionary<string, ZUIObject>();

    public override void InitSelf()
    {
        base.InitSelf();
        ZUIManager.FindAllUIObject(transform, null, mUIObjectDict, false, true);

        foreach (var item in mUIObjectDict.Values)
        {
            item.InitedSelf = true;
            item.InitSelf();
        }
    }

    public override void Initialize()
    {
        foreach (var item in mUIObjectDict.Values)
        {
            item.Inited = true;
            item.Initialize();
        }
    }

    public ZUIObject GetObject(string sNameKey)
    {
        if (mUIObjectDict.ContainsKey(sNameKey))
        {
            return mUIObjectDict[sNameKey];
        }

        return null;
    }

    public void DebugShowObjectKeys()
    {
        int index = 1;
        XLogger.DebugFormat("ZListItem num children： {0}", mUIObjectDict.Count);
        foreach (var item in mUIObjectDict.Keys)
        {
            XLogger.DebugFormat("{0}:{1}", index, item);
            index += 1;
        }
    }
}
