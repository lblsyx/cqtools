using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;

namespace UnityExt.ZNGUI.Interfaces
{
    public interface IZSprite : IZUIObject
    {
        void MakePixelPerfect();
        MonoBehaviour CurrentAtlas { get; }
        string CurrentSpriteName { get; }
        Vector2 PivotOffset { get; set; }
        float FillAmount { get; set; }
        bool FillInvert { get; set; }
        Color FillColor { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        void ClearSprite();
        void SetSprite(MonoBehaviour atlas, string spriteName);
        void SetSprite(string atlasName, string spriteName);
        void SetSprite(string atlasName, string spriteName, bool bCacheAtlas);
    }
}
