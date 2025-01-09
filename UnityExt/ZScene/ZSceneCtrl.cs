using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt;
using UnityExt.Chunks.MapChunks;
using UnityExt.Loaders;
using UnityLight;
using UnityLight.Events;
using UnityLight.Loggers;
using UnityLight.Utils;

namespace UnityExt.ZScene
{
    public class ZSceneCtrl : ZEventDispatcher
    {
        public GameObject SceneRoot { get; private set; }
        public string CurrentName { get; private set; }
        public GameObject CurrentScene { get; private set; }
        public GameObject After { get; private set; }
        public GameObject CurrentTerrain { get; private set; }
        public GameObject CurrentEffet { get; private set; }
        public SceneInfo SceneInfo { get; private set; }
        public SceneArea SceneArea { get; private set; }
        public ZSceneMesh SceneMesh { get; private set; }

        private DownloadQueue mSceneDownloadQueue;

        public Dictionary<string, MaterialInfo> MaterialInfoDict = new Dictionary<string, MaterialInfo>();
        public Dictionary<string, MeshInfo> MeshInfoDict = new Dictionary<string, MeshInfo>();
        public Dictionary<string, TextureInfo> TextureInfoDict = new Dictionary<string, TextureInfo>();
        public Dictionary<string, Mesh> MeshDict = new Dictionary<string, Mesh>();
        public Dictionary<string, Material> MatDict = new Dictionary<string, Material>();
        public Dictionary<string, Texture> TextureDict = new Dictionary<string, Texture>();
        private Dictionary<int, ModelInfo> AlreadyLoadedModel = new Dictionary<int, ModelInfo>();
        
        public ZSceneCtrl(GameObject root)
            : base()
        {
            SceneRoot = root;
            mSceneDownloadQueue = new DownloadQueue(1);
        }

        internal void OnPrechangeScene()
        {
            mSceneDownloadQueue.ClearQueue();
            foreach (var key in MeshDict.Keys)
            {
                if (MeshDict[key] != null)
                    UnityEngine.Object.DestroyImmediate(MeshDict[key], true);
            }
            foreach (var key in MatDict.Keys)
            {
                if (MatDict[key] != null)
                    UnityEngine.Object.DestroyImmediate(MatDict[key], true);
            }
            MeshDict.Clear();
            MatDict.Clear();
            MaterialInfoDict.Clear();
            MeshInfoDict.Clear();
            TextureInfoDict.Clear();
            TextureDict.Clear();
            AlreadyLoadedModel.Clear();
        }

        internal void ShowScene(string sceneName, AssetBundle assetBundle)
        {
            ClearScene();

            CurrentName = sceneName;
            
            CurrentScene = new GameObject();
            CurrentScene.name = CurrentName;
            CurrentScene.transform.parent = SceneRoot.transform;

            After = new GameObject("After");
            After.transform.parent = CurrentScene.transform;

            SceneMesh = new ZSceneMesh();
            SceneMesh.Init(CurrentScene.transform);

            //将地形资源显示到镜头上
            GameObject terrainCls = assetBundle.mainAsset as GameObject;
            CurrentTerrain = ExtUtil.InstantiateBundleAsset(terrainCls);
            CurrentTerrain.transform.parent = CurrentScene.transform;
            //解析场景节点配置信息
            SceneInfo = assetBundle.Load("SceneInfo") as SceneInfo;
            if (SceneInfo != null)
            {
                RenderSettings.fog = SceneInfo.SkyBoxFog;
                Color fogColor = new Color(SceneInfo.SkyBoxFogColorR, SceneInfo.SkyBoxFogColorG, SceneInfo.SkyBoxFogColorB, SceneInfo.SkyBoxFogColorA);
                RenderSettings.fogColor = fogColor;
                RenderSettings.fogMode = (FogMode)SceneInfo.SkyBoxFogMode;
                RenderSettings.fogDensity = SceneInfo.SkyBoxFogDensity;
                RenderSettings.fogStartDistance = SceneInfo.SkyBoxLinearFogStart;
                RenderSettings.fogEndDistance = SceneInfo.SkyBoxLinearFogEnd;
                Color ambientLightColor = new Color(SceneInfo.SkyBoxAmbientLightColorR, SceneInfo.SkyBoxAmbientLightColorG, SceneInfo.SkyBoxAmbientLightColorB, SceneInfo.SkyBoxAmbientLightColorA);
                RenderSettings.ambientLight = ambientLightColor;
                RenderSettings.skybox = SceneInfo.SkyBoxMaterial;
                LightmapSettings.lightProbes = SceneInfo.LightProbes; 
                LightmapData[] LightmapDatas = new LightmapData[SceneInfo.LightmapFar.Length];
                for (int i = 0; i < SceneInfo.LightmapFar.Length; i++)
                {
                    LightmapDatas[i] = new LightmapData();
                    LightmapDatas[i].lightmapFar = SceneInfo.LightmapFar[i];
                    LightmapDatas[i].lightmapNear = SceneInfo.LightmapNear[i];
                }
                LightmapSettings.lightmaps = LightmapDatas;

                for (int i = 0; i < SceneInfo.MeshInfos.Length; i++)
                {
                    MeshInfoDict.Add(SceneInfo.MeshInfos[i].MeshGuid, SceneInfo.MeshInfos[i]);
                }

                for (int i = 0; i < SceneInfo.MaterialInfos.Length; i++)
                {
                    MaterialInfoDict.Add(SceneInfo.MaterialInfos[i].MaterialGuid, SceneInfo.MaterialInfos[i]);
                }

                for (int i = 0; i < SceneInfo.TextureInfos.Length; i++)
                {
                    TextureInfoDict.Add(SceneInfo.TextureInfos[i].TexGuid, SceneInfo.TextureInfos[i]);
                }


                SceneArea = new SceneArea();
                SceneArea.InitArea(SceneInfo);
                TryLoadNextSceneNode();

                //foreach (var item in SceneInfo.Nodes)
                //{
                //    LoaderItem li = mSceneDownloadQueue.Load(ZSceneMgr.GetSceneNodePath(item.NodeName), OnNodeLoadDoneHandler);
                //    li.Tag = item;
                //}
            }
        }


        #region 一次性合并所有网格
        private void CombineMesh()
        {
            MeshFilter[] filters = After.GetComponentsInChildren<MeshFilter>();

            if (filters.Length <= 1)
            {
                Debug.LogWarning("只有一个网格，无需合并!");
                return;
            }

            Matrix4x4 myTransform = After.transform.worldToLocalMatrix;
            Hashtable materialToMesh = new Hashtable();

            for (int i = 0; i < filters.Length; i++)
            {
                MeshFilter filter = (MeshFilter)filters[i];
                Renderer curRenderer = filters[i].GetComponent<Renderer>();
                MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance();
                instance.mesh = filter.sharedMesh;
                if (curRenderer != null && curRenderer.enabled && instance.mesh != null)
                {
                    instance.transform = myTransform * filter.transform.localToWorldMatrix;
                    instance.lightMapIndex = curRenderer.lightmapIndex;
                    instance.lightMapScaleOffset = curRenderer.lightmapTilingOffset;
                    Material[] materials = curRenderer.sharedMaterials;
                    for (int m = 0; m < materials.Length; m++)
                    {
                        instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);
                        if (materials[m] == null) continue;
                        ArrayList objects = (ArrayList)materialToMesh[materials[m]];
                        if (objects != null)
                        {
                            objects.Add(instance);
                        }
                        else
                        {
                            objects = new ArrayList();
                            objects.Add(instance);
                            materialToMesh.Add(materials[m], objects);
                        }
                    }
                    curRenderer.enabled = false;
                }
            }

            foreach (DictionaryEntry de in materialToMesh)
            {
                ArrayList elements = (ArrayList)de.Value;
                MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));
                MeshCombineUtility.MeshInstance[] meshInstances = MeshCombineUtility.CombineMeshs(instances);

                for (int i = 0; i < meshInstances.Length; i++)
                {
                    GameObject go = new GameObject("Combined mesh");
                    go.transform.parent = CurrentScene.transform;
                    go.transform.localScale = Vector3.one;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localPosition = Vector3.zero;
                    go.AddComponent(typeof(MeshFilter));
                    MeshRenderer renderer = go.AddComponent<MeshRenderer>();
                    renderer.material = (Material)de.Key;
                    renderer.lightmapIndex = meshInstances[i].lightMapIndex;
                    MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
                    filter.mesh = meshInstances[i].mesh;
                }
            }
        }
        #endregion


        private Queue<ModelInfo> AreaModelQueue = new Queue<ModelInfo>();
        private void TryLoadNextSceneNode()
        {
            AreaInfo oAreaInfo = null;
            ModelInfo[] oModelInfos = null;
            AreaModelQueue.Clear();

            while (true)
            {
                oAreaInfo = SceneArea.TryLoadNextArea();
                if (oAreaInfo == null)
                {
                    mSceneDownloadQueue.Load(ZSceneMgr.GetSceneNodePath("Effect"), OnEffectLoadedHandler);
                    IsCanLoadNextModel = false;
                    //CombineMesh();
                    return;
                }
                oModelInfos = oAreaInfo.ReadModelInfos();
                if (oModelInfos.Length == 0)
                {
                    continue;
                }
                else
                {
                    for (int i = 0; i < oModelInfos.Length; i++)
                    {
                        ModelInfo oModelInfo = oModelInfos[i];
                        string meshname = oModelInfo.MeshGuid;
                        if (AlreadyLoadedModel.ContainsKey(oModelInfo.GetHashCode()) == false)
                        {
                            AreaModelQueue.Enqueue(oModelInfo);
                            AlreadyLoadedModel.Add(oModelInfo.GetHashCode(), oModelInfo);
                        }
                    }
                    if (AreaModelQueue.Count > 0)
                    {
                        break;
                    }
                }
            }

            LoadAreaModel();
        }

        private void LoadAreaModel()
        {
            IsCanLoadNextModel = true;
        }

        private void OnEffectLoadedHandler(URLLoader loader, bool sucess, string msg)
        {
            if (sucess == true)
            {
                AssetBundle assetBundle = loader.DataBundle;
                GameObject effect = assetBundle.mainAsset as GameObject;
                CurrentEffet = ExtUtil.InstantiateBundleAsset(effect);
                CurrentEffet.transform.parent = CurrentScene.transform;
            }
        }

        //private void OnNodeLoadDoneHandler(URLLoader loader, bool success, string errMsg)
        //{
        //    if (success)
        //    {
        //        ZSceneNodeInfo oZSceneNodeInfo = loader.Tag as ZSceneNodeInfo;
        //        GameObject nodeIns = ExtUtil.InstantiateBundleAsset(loader);
        //        nodeIns.transform.parent = CurrentScene.transform;
        //        nodeIns.transform.localPosition = oZSceneNodeInfo.LocalPosition;
        //        nodeIns.transform.localEulerAngles = oZSceneNodeInfo.LocalRotation;
        //        nodeIns.transform.localScale = oZSceneNodeInfo.LocalScale;
        //    }
        //    else
        //    {
        //        XLogger.ErrorFormat("场景节点资源加载失败!{0}", errMsg);
        //    }

        //    LoadNextSceneNode();
        //}

        private bool IsCanLoadNextModel = false;

        public void Update()
        {
            if (IsCanLoadNextModel == true)
            {
                ModelInfo oModelInfo = null;
                if (AreaModelQueue.Count > 0)
                {
                    oModelInfo = AreaModelQueue.Dequeue();
                }

                if (oModelInfo == null)
                {
                    TryLoadNextSceneNode();
                }
                else
                {
                    ModelLoadCtrl oModelLoadCtrl = new ModelLoadCtrl();
                    IsCanLoadNextModel = false;
                    oModelLoadCtrl.Load(oModelInfo, this, LoadAreaModel);
                }
            }
        }

        internal void ClearScene()
        {
            bool gc = false;
            
            if (CurrentScene != null)
            {
                CurrentTerrain = null;
                CurrentEffet = null;
                UnityEngine.Object.DestroyImmediate(CurrentScene);
                CurrentScene = null;
                CurrentName = null;
                //AlreadyLoadedNodes.Clear();
                //AlreadyLoadedNodes = null;
                gc = true;
            }

            if (mSceneDownloadQueue.ClearCache(true))
            {
                gc = true;
            }

            if (gc)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
        }


        public class ModelLoadCtrl
        {
            private ModelInfo ModelInfo;
            private ZSceneCtrl ZSceneCtrl;
            private MeshInfo MeshInfo;
            private MaterialInfo[] MaterialInfos;
            private TextureInfo[] TextureInfos;
            private Mesh Mesh;
            private Dictionary<string, Material> MaterialDict = new Dictionary<string, Material>();
            private Dictionary<string, Texture> TextureDict = new Dictionary<string, Texture>();
            private Callback Callback;

            public void Load(ModelInfo oModelInfo, ZSceneCtrl _zsceneCtrl, Callback _onfinished)
            {
                //Debug.Log(oModelInfo.ModelName);

                ZSceneCtrl = _zsceneCtrl;
                Callback = _onfinished;
                ModelInfo = oModelInfo;

                MeshInfo = ZSceneCtrl.MeshInfoDict[oModelInfo.MeshGuid];
                MaterialInfos = ParseMaterialInfos(oModelInfo.MaterialGuids);
                TextureInfos = ParesTextureInfos(MaterialInfos);

                if (TextureInfos.Length == 0)
                {
                    CreateMaterials();
                }

                for (int i = 0; i < TextureInfos.Length; i++)
                {
                    Texture texture = null;
                    string texguid = TextureInfos[i].TexGuid;
                    if (TextureDict.ContainsKey(texguid) == false)
                    {
                        if (ZSceneCtrl.TextureDict.ContainsKey(texguid))
                        {
                            texture = ZSceneCtrl.TextureDict[texguid];
                            TextureDict[texguid] = texture;
                            CalculateTexture();
                        }
                        else
                        {
                            ZSceneCtrl.mSceneDownloadQueue.Load(ZSceneMgr.GetSceneTexPath(texguid), OnTextureLoadedHandler);
                        }
                    }
                }

            }

            private MaterialInfo[] ParseMaterialInfos(string[] oMaterialGuids)
            {
                MaterialInfos = new MaterialInfo[oMaterialGuids.Length];
                for (int i = 0; i < MaterialInfos.Length; i++)
                {
                    MaterialInfos[i] = ZSceneCtrl.MaterialInfoDict[oMaterialGuids[i]];
                }
                return MaterialInfos;
            }

            private TextureInfo[] ParesTextureInfos(MaterialInfo[] oMaterialInfos)
            {
                List<TextureInfo> oTextureInfos = new List<TextureInfo>();
                for (int i = 0; i < oMaterialInfos.Length; i++)
                {
                    MaterialInfo oMaterialInfo = oMaterialInfos[i];
                    for (int j = 0; j < oMaterialInfo.ShaderPropertys.Length; j++)
                    {
                        string oPropertyStr = oMaterialInfo.ShaderPropertys[j];
                        string oTextureName = oMaterialInfo.ShaderValue[j];
                        string[] oPropertySplit = oPropertyStr.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        int oPropertyType = int.Parse(oPropertySplit[0]);
                        string oPropertyName = oPropertySplit[1];
                        if (oPropertyType == 4)
                        {
                            if (string.IsNullOrEmpty(oTextureName) == false)
                            {
                                oTextureInfos.Add(ZSceneCtrl.TextureInfoDict[oTextureName]);
                            }
                        }
                    }
                }
                return oTextureInfos.ToArray();
            }

            private void OnTextureLoadedHandler(URLLoader loader, bool sucess, string msg)
            {
                string name = Path.GetFileNameWithoutExtension(loader.URL);
                if (sucess == true)
                {
                    Texture tex = loader.DataBundle.mainAsset as Texture;
                    TextureDict[name] = tex;
                    ZSceneCtrl.TextureDict[name] = tex;
                    CalculateTexture();
                }
            }

            private void CalculateTexture()
            {
                if (TextureDict.Count == TextureInfos.Length)
                {
                    CreateMaterials();
                }
            }

            private void CreateMaterials()
            {
                for (int i = 0; i < MaterialInfos.Length; i++)
                {
                    MaterialInfo oMaterialInfo = MaterialInfos[i];
                    string matguid = oMaterialInfo.MaterialGuid;
                    Material oMat = null;
                    if (MaterialDict.ContainsKey(matguid) == false)
                    {
                        if (ZSceneCtrl.MatDict.ContainsKey(matguid) == true)
                        {
                            oMat = ZSceneCtrl.MatDict[matguid];
                            MaterialDict.Add(matguid, oMat);
                        }
                        else
                        {
                            Shader oShader = Shader.Find(oMaterialInfo.ShaderName);
                            if (oShader == null)
                            {
                                XLogger.ErrorFormat("没有找到Shader:{0}", oMaterialInfo.ShaderName);
                                continue;
                            }
                            oMat = new Material(oShader);
                            oMat.name = matguid;
                            ZSceneCtrl.MatDict.Add(matguid, oMat);
                            MaterialDict.Add(matguid, oMat);
                            for (int j = 0; j < oMaterialInfo.ShaderPropertys.Length; j++)
                            {
                                string oPropertyStr = oMaterialInfo.ShaderPropertys[j];

                                if (string.IsNullOrEmpty(oPropertyStr)) continue;

                                string[] oPropertySplit = oPropertyStr.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                                int oPropertyType = int.Parse(oPropertySplit[0]);
                                string oPropertyName = oPropertySplit[1];
                                // Color = 0,Vector = 1,Float = 2,Range = 3,TexEnv = 4 
                                switch (oPropertyType)
                                {
                                    case 0:
                                        {
                                            string[] values = oMaterialInfo.ShaderValue[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            Color color = Color.black;
                                            color.r = float.Parse(values[0]) / 255;
                                            color.g = float.Parse(values[1]) / 255;
                                            color.b = float.Parse(values[2]) / 255;
                                            color.a = float.Parse(values[3]) / 255;
                                            oMat.SetColor(oPropertyName, color);
                                        }
                                        break;
                                    case 1:
                                        {
                                            string[] values = oMaterialInfo.ShaderValue[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            Vector4 vec = Vector4.zero;
                                            vec.x = 1 / float.Parse(values[0]);
                                            vec.y = 1 / float.Parse(values[1]);
                                            vec.z = 1 / float.Parse(values[2]);
                                            vec.w = 1 / float.Parse(values[3]);
                                            oMat.SetVector(oPropertyName, vec);
                                        }
                                        break;
                                    case 2:
                                    case 3:
                                        {
                                            oMat.SetFloat(oPropertyName, float.Parse(oMaterialInfo.ShaderValue[j]));
                                        }
                                        break;
                                    case 4:
                                        {
                                            //Debug.Log(matguid + "       " + TextureDict[oMaterialInfo.ShaderValue[j]]);
                                            if (TextureDict.ContainsKey(oMaterialInfo.ShaderValue[j]) == true)
                                            {
                                                oMat.SetTexture(oPropertyName, TextureDict[oMaterialInfo.ShaderValue[j]]);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                CreateMesh();
            }

            private void CreateMesh()
            {
                if (ZSceneCtrl.MeshDict.ContainsKey(MeshInfo.MeshGuid) == true)
                {
                    Mesh = ZSceneCtrl.MeshDict[MeshInfo.MeshGuid];
                    CreateGameObject();
                }
                else
                {
                    ZSceneCtrl.mSceneDownloadQueue.Load(ZSceneMgr.GetSceneMeshPath(MeshInfo.MeshGuid), OnMeshLoadedHandler);
                }
            }

            private void OnMeshLoadedHandler(URLLoader loader, bool sucess, string msg)
            {
                string name = Path.GetFileNameWithoutExtension(loader.URL);
                if (sucess == true)
                {
                    Mesh = loader.DataBundle.mainAsset as Mesh;
                    ZSceneCtrl.MeshDict.Add(name, Mesh);
                    CreateGameObject();
                }
            }

            private void CreateGameObject()
            {
                GameObject obj = new GameObject(ModelInfo.ModelName);
                obj.transform.parent = ZSceneCtrl.After.transform;
                obj.transform.position = ModelInfo.Position;
                obj.transform.eulerAngles = ModelInfo.EulerAngle;
                obj.transform.localScale = ModelInfo.Scale;
                MeshFilter oMeshFilter = obj.AddComponent<MeshFilter>();
                oMeshFilter.sharedMesh = Mesh;
                MeshRenderer oMeshRender = obj.AddComponent<MeshRenderer>();
                oMeshRender.lightmapIndex = ModelInfo.LightMapIndex;
                oMeshRender.lightmapTilingOffset = ModelInfo.LightMapScaleOffset;
                Material[] mats = new Material[ModelInfo.MaterialGuids.Length];
                for (int i = 0; i < ModelInfo.MaterialGuids.Length; i++)
                {
                    Material mat = null;
                    if (MaterialDict.ContainsKey(ModelInfo.MaterialGuids[i]))
                    {
                        mat = MaterialDict[ModelInfo.MaterialGuids[i]];
                    }
                    else
                    {
                        mat = new Material(Shader.Find("Diffuse"));
                    }
                    mats[i] = mat;
                }
                oMeshRender.sharedMaterials = mats;
                // 0-没有碰撞, 1-BoxCollider, 2-MeshCollider
                if (ModelInfo.ColliderType == 1)
                {
                    BoxCollider oBoxCollider = obj.AddComponent<BoxCollider>();
                    oBoxCollider.center = ModelInfo.ColliderCenter;
                    oBoxCollider.size = ModelInfo.ColliderSize;
                    //oBoxCollider.isTrigger = true;
                }
                else if (ModelInfo.ColliderType == 2)
                {
                    MeshCollider oMeshCollider = obj.AddComponent<MeshCollider>();
                    oMeshCollider.convex = false;
                    //oMeshCollider.isTrigger = true;
                }
                Material[] materials = oMeshRender.sharedMaterials;
                for (int m = 0; m < materials.Length; m++)
                {
                    MeshCombineUtility.MeshInstance combine = new MeshCombineUtility.MeshInstance();
                    combine.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                    combine.subMeshIndex = System.Math.Min(m, combine.mesh.subMeshCount - 1);
                    combine.transform = ZSceneCtrl.After.transform.worldToLocalMatrix * obj.transform.localToWorldMatrix;
                    combine.invTranspose = combine.transform.inverse.transpose;
                    combine.lightMapIndex = oMeshRender.lightmapIndex;
                    combine.lightMapScaleOffset = oMeshRender.lightmapTilingOffset;
                    ZSceneCtrl.SceneMesh.AddMeshData(materials[m], combine);
                    oMeshRender.enabled = false;
                }

                if (Callback != null)
                {
                    Callback();
                }
            }

        }

    }


}
