using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZImage : IZUIObject
    {
        int Width { get; set; }

        int Height { get; set; }

        bool EnableRotateY { get; set; }

        float RotateSpeedY { get; set; }

        GameObject SourceRoot { get; }

        void ResetRotationY();

        void ResetSourceParent();

        void MakePixelPerfect();

        void SetTexture(Texture oTexture, bool bCache);

        void SetTexture(string sTextureName, bool bCache);
    }
}
