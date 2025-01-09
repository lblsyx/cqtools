using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZNGUI
{
    public class ZUIResource
    {
        public static Dictionary<string, Texture> UITextureManager = new Dictionary<string, Texture>();

        public static Dictionary<string, MonoBehaviour> UIAtlasManager = new Dictionary<string, MonoBehaviour>();

        public static string GetAtlasPath(string sAtlasName)
        {
            return Path.Combine(ZUIManager.UIRootPath, string.Format("Atlas/{0}.unity3d", sAtlasName));
        }

        public static string GetTexturePath(string sTextureName)
        {
            return Path.Combine(ZUIManager.UIRootPath, string.Format("Texture/{0}.unity3d", sTextureName));
        }
    }
}
