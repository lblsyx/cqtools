using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI
{
    public interface IZDialog
    {
        string UIName { get; }

        string FileName { get; }

        Transform Trans { get; }

        bool LoadComplete { get; }

        void UpdateUI();

        void DebugShowObjectKeys();
    }
}
