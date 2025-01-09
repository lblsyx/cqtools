using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene
{
    [Serializable]
    public class ZSceneNodeInfo
    {
        public string NodeName;

        public Vector3 MinPoint;

        public Vector3 MaxPoint;

        public Vector3 LocalPosition;

        public Vector3 LocalRotation;

        public Vector3 LocalScale;
    }

    [Serializable]
    public class ZLightInfo
    {
        public ZLightInfo()
        {
            LightExists = false;
            LightColorA = 1f;
            LightColorR = 1f;
            LightColorG = 1f;
            LightColorB = 1f;

            LightFixRX = 0f;
            LightOffsetRY = 0f;
            LightIntensity = 0f;
        }
        public bool LightExists;
        public float LightColorA;
        public float LightColorR;
        public float LightColorG;
        public float LightColorB;
        public float LightFixRX;
        public float LightOffsetRY;
        public float LightIntensity;
    }

    public class ZSceneInfo : ScriptableObject
    {
        public string SceneName;
        //角色灯光
        public ZLightInfo RoleLight;
        public ZLightInfo AssistLight;
        // 场景灯光
        public ZLightInfo SceneLight;
        //光照贴图
        public Texture2D[] LightmapFar;
        public Texture2D[] LightmapNear;
        //天空盒
        public bool SkyBoxFog;
        public float SkyBoxFogColorA;
        public float SkyBoxFogColorR;
        public float SkyBoxFogColorG;
        public float SkyBoxFogColorB;
        public byte SkyBoxFogMode;
        public float SkyBoxFogDensity;
        public float SkyBoxLinearFogStart;
        public float SkyBoxLinearFogEnd;
        public float SkyBoxAmbientLightColorA;
        public float SkyBoxAmbientLightColorR;
        public float SkyBoxAmbientLightColorG;
        public float SkyBoxAmbientLightColorB;
        public string SkyBoxMaterial;
        //区域节点
        public List<ZSceneNodeInfo> Nodes;
    }
}
