using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZList : IZUIObject
    {
        int ItemCount { get; }

        int RowNum { get; }

        int ColumnNum { get; }

        IZListItem this[int index] { get; }

        IZListItem AddItem(GameObject obj);

        IZListItem[] AddItem(GameObject obj, int nCloneNum);

        void RemoveItem(GameObject obj);

        void RemoveItem(IZListItem item);
    }
}
