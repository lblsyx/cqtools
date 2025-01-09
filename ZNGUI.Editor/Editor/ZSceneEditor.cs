using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityLight.Utils;
using UnityExt.ZScene;

public class ZSceneEditor : Editor
{
    #region 配置项

    public const string SceneOutputRoot = "X:/SceneAssets";
    public const string PreloadName = "Preload";

    #endregion

    #region 场景工具

    public static ZSceneInfo oZSceneInfo;

    [MenuItem("Assets/场景工具/生成场景预加载节点", false, 1111)]
    public static void GenerateScenePreload()
    {
        GenSceneAll(true, false);
    }

    [MenuItem("Assets/场景工具/生成场景的其他节点", false, 1112)]
    public static void GenerateSceneOthers()
    {
        GenSceneAll(false, true);
    }

    [MenuItem("Assets/场景工具/生成场景配置和资源(所有)", false, 1113)]
    public static void GenerateSceneAll()
    {
        GenSceneAll(true, true);
    }

    private static void GenSceneAll(bool packPreloadNode, bool packOtherNodes)
    {
        string sceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);

        GameObject selectedGO = Selection.activeGameObject;
        if (null == selectedGO || PrefabType.Prefab != PrefabUtility.GetPrefabType(selectedGO))
        {
            Debug.LogError("Please select scene prefab file!");
            return;
        }

        if (sceneName != selectedGO.name)
        {
            Debug.LogError("Please open scene unity3d project!");
            return;
        }

        string assetSelectedPath = AssetDatabase.GetAssetPath(selectedGO).Replace("\\", "/");
        string assetSelectedDir = Path.GetDirectoryName(assetSelectedPath).Replace("\\", "/");
        string selectedAbsolutePath = Path.Combine(Application.dataPath, assetSelectedPath.Substring(7)).Replace("\\", "/");
        string selectedAbsoluteDir = Path.GetDirectoryName(selectedAbsolutePath).Replace("\\", "/");
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(selectedAbsolutePath);

        string sceneFolder = Path.Combine(SceneOutputRoot, fileNameWithoutExt).Replace("\\", "/");
        if (Directory.Exists(sceneFolder) == false) Directory.CreateDirectory(sceneFolder);

        selectedGO = GameObject.Instantiate(Selection.activeGameObject) as GameObject;

        //Assetbundle附加资源
        List<UnityEngine.Object> assets = new List<UnityEngine.Object>();

        if (packPreloadNode)
        {
            oZSceneInfo = ScriptableObject.CreateInstance<ZSceneInfo>();
            //oZSceneInfo.name = "ZSceneInfo";
            oZSceneInfo.SceneName = fileNameWithoutExt;
            oZSceneInfo.Nodes = new List<ZSceneNodeInfo>();
        }

        if (packPreloadNode)
        {
            //查找场景内的角色灯光配置对象并保存
            GameObject mRoleLight = GameObject.Find("RoleLight");
            oZSceneInfo.RoleLight = new ZLightInfo();
            if (mRoleLight != null)
            {
                Light oLight = mRoleLight.GetComponent<Light>();
                if (oLight != null)
                {
                    if (oLight.enabled == false)
                    {
                        oZSceneInfo.RoleLight.LightExists = false;
                    }
                    else
                    {
                        oZSceneInfo.RoleLight.LightExists = mRoleLight.activeSelf;
                    }
                }
                else
                {
                    oZSceneInfo.RoleLight.LightExists = false;
                }
                
                if (oZSceneInfo.RoleLight.LightExists)
                {
                    oZSceneInfo.RoleLight.LightColorA = oLight.color.a;
                    oZSceneInfo.RoleLight.LightColorR = oLight.color.r;
                    oZSceneInfo.RoleLight.LightColorG = oLight.color.g;
                    oZSceneInfo.RoleLight.LightColorB = oLight.color.b;
                    oZSceneInfo.RoleLight.LightIntensity = oLight.intensity;
                    oZSceneInfo.RoleLight.LightFixRX = oLight.transform.eulerAngles.x;
                    oZSceneInfo.RoleLight.LightOffsetRY = oLight.transform.eulerAngles.y;
                }
                else
                {
                    Debug.LogWarning("The role light is not config,Please set it!");
                }
            }
            else
            {
                Debug.LogWarning("The role light is not found,Please create it and named 'RoleLight'!");
            }

            GameObject mAssistLight = GameObject.Find("AssistLight");
            oZSceneInfo.AssistLight = new ZLightInfo();
            if (mAssistLight != null)
            {
                Light oLight = mAssistLight.GetComponent<Light>();
                if (oLight != null)
                {
                    if (oLight.enabled == false)
                    {
                        oZSceneInfo.AssistLight.LightExists = false;
                    }
                    else
                    {
                        oZSceneInfo.AssistLight.LightExists = mRoleLight.activeSelf;
                    }
                }
                else
                {
                    oZSceneInfo.AssistLight.LightExists = false;
                }
                if (oZSceneInfo.AssistLight.LightExists)
                {
                    oZSceneInfo.AssistLight.LightColorA = oLight.color.a;
                    oZSceneInfo.AssistLight.LightColorR = oLight.color.r;
                    oZSceneInfo.AssistLight.LightColorG = oLight.color.g;
                    oZSceneInfo.AssistLight.LightColorB = oLight.color.b;
                    oZSceneInfo.AssistLight.LightIntensity = oLight.intensity;
                    oZSceneInfo.AssistLight.LightFixRX = oLight.transform.eulerAngles.x;
                    oZSceneInfo.AssistLight.LightOffsetRY = oLight.transform.eulerAngles.y;
                }
                else
                {
                    Debug.LogWarning("The assist light is not config,Please set it!");
                }
            }
            else
            {
                Debug.LogWarning("The assist light is not found,Please create it and named 'RoleLight'!");
            }
        }

        if (packPreloadNode)
        {
            //遍历场景工程的所有光照贴图对象并保存
            int count = LightmapSettings.lightmaps.Length;
            if (count > 0)
            {
                oZSceneInfo.LightmapFar = new Texture2D[count];
                oZSceneInfo.LightmapNear = new Texture2D[count];

                for (int i = 0; i < count; i++)
                {
                    oZSceneInfo.LightmapFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
                    oZSceneInfo.LightmapNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
                }
            }

            //保存天空盒信息
            oZSceneInfo.SkyBoxFog = RenderSettings.fog;
            oZSceneInfo.SkyBoxFogColorA = RenderSettings.fogColor.a;
            oZSceneInfo.SkyBoxFogColorR = RenderSettings.fogColor.r;
            oZSceneInfo.SkyBoxFogColorG = RenderSettings.fogColor.g;
            oZSceneInfo.SkyBoxFogColorB = RenderSettings.fogColor.b;
            oZSceneInfo.SkyBoxFogMode = (byte)RenderSettings.fogMode;
            oZSceneInfo.SkyBoxFogDensity = RenderSettings.fogDensity;
            oZSceneInfo.SkyBoxLinearFogStart = RenderSettings.fogStartDistance;
            oZSceneInfo.SkyBoxLinearFogEnd = RenderSettings.fogEndDistance;
            oZSceneInfo.SkyBoxAmbientLightColorA = RenderSettings.ambientLight.a;
            oZSceneInfo.SkyBoxAmbientLightColorR = RenderSettings.ambientLight.r;
            oZSceneInfo.SkyBoxAmbientLightColorG = RenderSettings.ambientLight.g;
            oZSceneInfo.SkyBoxAmbientLightColorB = RenderSettings.ambientLight.b;
            if (RenderSettings.skybox != null)
            {
                //加载天空盒材质
                oZSceneInfo.SkyBoxMaterial = RenderSettings.skybox.name;
                string materialPath = AssetDatabase.GetAssetPath(RenderSettings.skybox);
                UnityEngine.Object materialAsset = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material));
                assets.Add(materialAsset);
            }
        }

        //记录并创建场景加载节点资源
        var min = new Vector3();
        var max = new Vector3();
        Transform preloadTrans = null;
        Transform selectedTrans = selectedGO.transform;
        for (int i = 0; i < selectedTrans.childCount; i++)
        {
            Transform child = selectedTrans.GetChild(i);
            if (child.name.ToLower() == PreloadName.ToLower())
            {
                preloadTrans = child;
                continue;
            }

            ZSceneNodeInfo node = null;
            if (oZSceneInfo != null)
            {
                //添加节点信息到列表
                node = new ZSceneNodeInfo();
                node.NodeName = child.name;
                node.LocalPosition = child.localPosition;
                node.LocalRotation = child.localEulerAngles;
                node.LocalScale = child.localScale;
                oZSceneInfo.Nodes.Add(node);
                Debug.Log(string.Format("Add scene node:[{0}] to list", child.name));

                min.x = min.y = min.z = float.MaxValue;
                max.x = max.y = max.z = float.MinValue;
                Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    min.x = Math.Min(min.x, renderer.bounds.min.x);
                    min.y = Math.Min(min.y, renderer.bounds.min.y);
                    min.z = Math.Min(min.z, renderer.bounds.min.z);
                    max.x = Math.Max(max.x, renderer.bounds.max.x);
                    max.y = Math.Max(max.y, renderer.bounds.max.y);
                    max.z = Math.Max(max.z, renderer.bounds.max.z);
                }

                if (renderers.Length != 0)
                {
                    node.MinPoint = min;
                    node.MaxPoint = max;
                }
                else
                {
                    node.MinPoint = Vector3.zero;
                    node.MaxPoint = Vector3.zero;
                }
            }

            if (packOtherNodes)
            {
                //将节点资源打包为Assetbundle文件
                UnityEngine.Object oCloneObject = ClonePrefab(child);
                if (oCloneObject == null)
                {
                    Debug.LogError(string.Format("Scene node:[{0}] clone failed!", child.name));
                    continue;
                }
                //场景节点资源路径格式：场景根目录/场景名称/节点名称.unity3d
                var outputPath = string.Format("{0}/{1}/{2}.unity3d", SceneOutputRoot, fileNameWithoutExt, child.name);
                BuildPipeline.BuildAssetBundle(oCloneObject, null, outputPath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.WebPlayer);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oCloneObject));
                Debug.Log(string.Format("Build scene node:[{0}] to assetbundle file.Path:{1}", child.name, outputPath));
            }
        }


        if (packPreloadNode)
        {
            //记录并创建预加载节点资源
            if (preloadTrans)
            {
                UnityEngine.Object oCloneObject = ClonePrefab(preloadTrans);

                if (oCloneObject == null)
                {
                    Debug.LogError(string.Format("Scene node:[{0}] clone failed!", preloadTrans.name));
                    return;
                }

                //创建临时场景信息资源
                string oZSceneInfoPath = "Assets/ZSceneInfo.asset";
                AssetDatabase.CreateAsset(oZSceneInfo, oZSceneInfoPath);
                UnityEngine.Object oZSceneInfoAsset = AssetDatabase.LoadAssetAtPath(oZSceneInfoPath, typeof(ZSceneInfo));
                assets.Add(oZSceneInfoAsset);

                //场景节点资源路径格式：场景根目录/场景名称/节点名称.unity3d
                var outputPath = string.Format("{0}/{1}/{2}.unity3d", SceneOutputRoot, fileNameWithoutExt, preloadTrans.name);
                BuildPipeline.BuildAssetBundle(oCloneObject, assets.ToArray(), outputPath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.WebPlayer);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oCloneObject));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(oZSceneInfoAsset));
                Debug.Log(string.Format("Build scene node:[{0}] to assetbundle file.Path:{1}", preloadTrans.name, outputPath));
            }
            else
            {
                Debug.LogError("The Preload node is not found!");
            }
        }

        GameObject.DestroyImmediate(selectedGO);
    }

    private static UnityEngine.Object ClonePrefab(Transform trans)
    {
        UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(string.Format("Assets/{0}.prefab", trans.name));

        if (prefab != null) prefab = PrefabUtility.ReplacePrefab(trans.gameObject, prefab);
        else Debug.LogError(string.Format("Create empty prefab failed!Name:{0}", trans.name));

        return prefab;
    }

    #endregion

    #region 其他工具

    [MenuItem("Assets/其他工具/生成Bundle(仅工具)", false, 1131)]
    static void PackToolAssetbundle()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = EditorUtility.SaveFilePanel("Save Resource", "", obj.name, "unity3d");
        if (path.Length != 0)
        {
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows);
        }
    }

    #endregion
}
