using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene
{
    public class SceneInfo : ScriptableObject
    {
        public MeshInfo[] MeshInfos;
        public MaterialInfo[] MaterialInfos;
        public TextureInfo[] TextureInfos;
        public ModelInfo[] ModelInfos;
        public LightProbes LightProbes;
        public Texture2D[] LightmapFar;
        public Texture2D[] LightmapNear;
        public string SceneName;
        public int AreaUnit;
        public int AreaX;
        public int AreaY;
        public float SkyBoxAmbientLightColorA;
        public float SkyBoxAmbientLightColorB;
        public float SkyBoxAmbientLightColorG;
        public float SkyBoxAmbientLightColorR;
        public bool SkyBoxFog;
        public float SkyBoxFogColorA;
        public float SkyBoxFogColorB;
        public float SkyBoxFogColorG;
        public float SkyBoxFogColorR;
        public float SkyBoxFogDensity;
        public byte SkyBoxFogMode;
        public float SkyBoxLinearFogEnd;
        public float SkyBoxLinearFogStart;
        public Material SkyBoxMaterial;
        public LightInfo SceneLight;
    }

    [Serializable]
    public class MeshInfo
    {
        public string MeshGuid;
    }

    [Serializable]
    public class MaterialInfo
    {
        public string MaterialGuid;
        public string ShaderName;
        //public string MaterialName;
        //public string GameName;
        /// <summary>
        /// PropertyType { Color = 0,Vector = 1,Float = 2,Range = 3,TexEnv = 4 }
        /// 格式: PropertyType|PropertyName
        /// </summary>
        public string[] ShaderPropertys;
        public string[] ShaderValue;
    }

    [Serializable]
    public class TextureInfo
    {
        public string TexGuid;
        public static int anisotropicFiltering;
        public static int masterTextureLimit;
        public int anisoLevel;
        public int filterMode;
        public int width;
        public int height;
        public float mipMapBias;
        public int wrapMode;
        public int format;
        public int mipmapCount;
    }

    [Serializable]
    public class ModelInfo
    {
        public string ModelName;
        public Vector3 Position;
        public Vector3 EulerAngle;
        public Vector3 Scale;
        public string MeshGuid;
        public string[] MaterialGuids;
        public int LightMapIndex;
        public Vector4 LightMapScaleOffset;
        /// <summary>
        /// 当前范围舍弃Y轴, 其中X,Y保存center.x,center.y; Z,W保存size.x,size.z
        /// </summary>
        public Vector4 Bound;
        /// <summary>
        /// 格式：0,没有碰撞 1,BoxCollider 2,MeshCollider
        /// </summary>
        public byte ColliderType;
        public Vector3 ColliderCenter;
        public Vector3 ColliderSize;
    }

    [Serializable]
    public class LightInfo
    {
        public float LightColorA;
        public float LightColorB;
        public float LightColorG;
        public float LightColorR;
        public bool LightExists;
        public float LightFixRX;
        public float LightIntensity;
        public float LightOffsetRY;
        // 0 None, 1 HardShadow, 2 SoftShadow
        public int ShadowType;
        public float ShadowStrength;
    }
}