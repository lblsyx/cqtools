using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZWindow
    {
        void Bring2Top();
        void Bring2Bottom();
        void AdjustDepth(int depth);
    }
}
