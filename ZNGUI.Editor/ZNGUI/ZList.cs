using System;
using System.Collections.Generic;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityEngine;

public class ZList : ZUIObject, IZList
{
    public override ZUIObjectType Type { get { return ZUIObjectType.List; } }

    List<ZListItem> items = new List<ZListItem>();

    private UIGrid mUIGrid;

    public int RowCount = 1;
    public int ColCount = 1;

    public override void InitSelf()
    {
        base.InitSelf();

        mUIGrid = GetComponent<UIGrid>();

        items.Clear();
        var array = transform.GetComponentsInChildren<ZListItem>();
        for (int i = 0; i < array.Length; i++)
        {
            ZListItem oZListItem = array[i];
            oZListItem.Index = i;
            oZListItem.Parent = this;
            items.Add(oZListItem);
        }

        if (array.Length != 0)
        {
            var item = array[0];
            var cls = item.gameObject;
            var total = RowCount * ColCount;
            
            for (int i = array.Length; i < total; i++)
            {
                GameObject obj = Instantiate(cls) as GameObject;
                obj.name = item.name + i.ToString();
                obj.transform.parent = item.transform.parent;
                obj.transform.position = item.transform.position;
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;

                var tmp = obj.GetComponent<ZListItem>();
                tmp.Index = i;
                tmp.Parent = this;

                items.Add(tmp);
            }

            if (mUIGrid != null) mUIGrid.Reposition();
        }

        foreach (var tmp in items)
        {
            if (tmp.InitedSelf == false)
            {
                tmp.InitedSelf = true;
                tmp.InitSelf();
            }
        }
    }

    public override void Initialize()
    {
        foreach (var tmp in items)
        {
            if (tmp.Inited == false)
            {
                tmp.Inited = true;
                tmp.Initialize();
            }
        }
    }

    public int ItemCount
    {
        get { return items.Count; }
    }

    public int RowNum
    {
        get { return RowCount; }
    }

    public int ColumnNum
    {
        get { return ColCount; }
    }

    public IZListItem this[int index]
    {
        get { return items[index]; }
    }

    public IZListItem AddItem(GameObject obj)
    {
        ZListItem item = obj.GetComponent<ZListItem>();
        if (item == null) item = obj.AddComponent<ZListItem>();
        obj.transform.parent = transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        item.Index = items.Count;
        items.Add(item);

        if (item.InitedSelf == false)
        {
            item.InitedSelf = true;
            item.InitSelf();
        }

        if (item.Inited == false)
        {
            item.Inited = true;
            item.Initialize();
        }

        if (mUIGrid != null) mUIGrid.Reposition();

        return item;
    }

    public IZListItem[] AddItem(GameObject obj, int nCloneNum)
    {
        List<IZListItem> list = new List<IZListItem>();
        for (int i = 0; i < nCloneNum; i++)
        {
            var tmp = Instantiate(obj) as GameObject;
            list.Add(AddItem(tmp));
        }
        return list.ToArray();
    }

    public void RemoveItem(GameObject obj)
    {
        if (obj == null) return;
        ZListItem item = obj.GetComponent<ZListItem>();
        if (item == null) return;
        items.Remove(item as ZListItem);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Index = i;
        }
        if (mUIGrid != null) mUIGrid.Reposition();
        DestroyImmediate(obj, true);
    }

    public void RemoveItem(IZListItem item)
    {
        if (item == null) return;
        RemoveItem(item.CurrentObject);
    }
}
