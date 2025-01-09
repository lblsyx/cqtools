using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityExt;
using UnityExt.Follows;
using UnityExt.ZScene.CameraLocks;
using UnityExt.ZScene.Follows;
using UnityLight;
using UnityLight.Events;
using UnityLight.Loggers;

namespace UnityExt.ZScene
{
    public class ZSceneMgr
    {
        private static ZEventDispatcher<string, ZSceneEvent> EventDispatcher;

        public static float MinCameraDistance = 3f;
        public static float MaxCameraDistance = 10f;
        public static float DefaultCameraDistance = 10f;
        public static float MinCameraAngleX = 10f;
        public static float MaxCameraAngleX = 40f;
        public static float AngleSpeed = 10f;
        public static float DistSpeed = 6.0f;
        public static bool IsCameraAutoZoom = true;
        public static int CameraRayLayer = 0;

        public static Light MainLight { get; set; }

        public static Light RoleLight { get; set; }

        public static Light AssistLight { get; set; }

        public static Camera ShadowCamera { get; set; }

        public static GameObject SceneObjectPoolRoot { get; set; }

        public static string RootPath { get; private set; }

        public static ZSceneCtrl SceneCtrl { get; private set; }

        public static SceneInfo SceneInfo { get; private set; }

        public static GameObject SceneTerrain { get; private set; }

        public static string CurrentSceneName { get; private set; }

        public static string LoadingSceneName { get; private set; }

        public static List<ILockCamera> CurrentBeforeUpdateLockList { get; private set; }

        public static List<ILockCamera> CurrentAfterUpdateLockList { get; private set; }

        private static Dictionary<int, ILockCamera> mLockCameraDict = new Dictionary<int, ILockCamera>();

        private static CameraFollow mCameraFollow;
        private static LightFollow mRoleLightFollow;
        private static LightFollow mAssistLightFollow;
        //private static LightFollow mMainLightFollow;

        public static void Init(string sRootPath, GameObject sceneRoot, Camera cMainCamera)
        {
            if (SceneCtrl != null) return;

            RootPath = sRootPath;
            SceneCtrl = new ZSceneCtrl(sceneRoot);
            EventDispatcher = new ZEventDispatcher<string, ZSceneEvent>();

            CurrentBeforeUpdateLockList = new List<ILockCamera>();
            CurrentAfterUpdateLockList = new List<ILockCamera>();

            SearchAssembly(typeof(ZSceneMgr).Assembly);

            mCameraFollow = new CameraFollow(cMainCamera);
            mRoleLightFollow = new LightFollow(RoleLight, cMainCamera);
            mAssistLightFollow = new LightFollow(AssistLight, cMainCamera);
            //mMainLightFollow = new LightFollow(MainLight, cMainCamera);

            ZFollowMgr.AddFollow(mCameraFollow);
            ZFollowMgr.AddFollow(mRoleLightFollow);
            ZFollowMgr.AddFollow(mAssistLightFollow);
        }

        public static void SearchAssembly(Assembly assembly)
        {
            Type[] list = assembly.GetTypes();

            string sInterfaceStr = typeof(ILockCamera).ToString();

            Type tAttributeType = typeof(LockCameraAttribute);

            foreach (var type in list)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                LockCameraAttribute[] attributes = (LockCameraAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    LockCameraAttribute attribute = attributes[0];

                    if (mLockCameraDict.ContainsKey(attribute.LockType))
                    {
                        XLogger.ErrorFormat("镜头锁定处理类已存在!LockType：{0}", attribute.LockType);
                        continue;
                    }

                    mLockCameraDict.Add(attribute.LockType, (ILockCamera)Activator.CreateInstance(type));
                }
            }
        }

        public static void EnableCameraRotation()
        {
            if (mCameraFollow != null) mCameraFollow.EnableRotation = true;
        }

        public static void DisableCameraRotation()
        {
            if (mCameraFollow != null) mCameraFollow.EnableRotation = false;
        }

        public static void ResetCameraDistance()
        {
            if (mCameraFollow != null) mCameraFollow.ResetDistance();
        }

        public static void ChangeScene(string name)
        {
            LoadingSceneName = name;
            SceneCtrl.OnPrechangeScene();
            DispatchEvent(ZSceneEvent.SCENE_CHANGING, new ZSceneEvent());
        }

        public static void OnSceneStartLoad()
        {
            SceneCtrl.ClearScene();
        }

        public static bool ParseLockCameraString(string sMapName, string sLockStr)
        {
            if (string.IsNullOrEmpty(sLockStr) || sLockStr == "0") return true;

            bool rlt = true;

            string[] locks = sLockStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var lockStr in locks)
            {
                string[] array = lockStr.Split(new char[] { ':', '：' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 2)
                {
                    int key = int.Parse(array[0]);

                    if (mLockCameraDict.ContainsKey(key))
                    {
                        ILockCamera iILockCamera = mLockCameraDict[key];
                        iILockCamera.ParseParams(array[1]);
                        if (iILockCamera.BeforeUpdate)
                        {
                            CurrentBeforeUpdateLockList.Add(iILockCamera);
                        }
                        if (iILockCamera.AfterUpdate)
                        {
                            CurrentAfterUpdateLockList.Add(iILockCamera);
                        }
                    }
                    else
                    {
                        XLogger.ErrorFormat("不支持的镜头锁定类型!");
                        rlt = false;
                    }
                }
                else
                {
                    XLogger.ErrorFormat("场景[{0}]锁定镜头：{1}配置错误！", sMapName, lockStr);
                    rlt = false;
                }
            }

            return rlt;
        }
        
        public static void ShowScene(AssetBundle assetBundle, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            ShowScene(assetBundle, layer, string.Empty);
        }

        public static void ShowScene(AssetBundle assetBundle, string layerName, string sLockCameraStr)
        {
            int layer = LayerMask.NameToLayer(layerName);
            ShowScene(assetBundle, layer, sLockCameraStr);
        }

        public static void ShowScene(AssetBundle abAssetBundle, int nTerrainLayer)
        {
            ShowScene(abAssetBundle, nTerrainLayer, string.Empty);
        }

        public static void ShowScene(AssetBundle abAssetBundle, int nTerrainLayer, string sLockCameraStr)
        {
            CurrentSceneName = LoadingSceneName;

            CurrentAfterUpdateLockList.Clear();
            CurrentBeforeUpdateLockList.Clear();
            ParseLockCameraString(CurrentSceneName, sLockCameraStr);

            SceneCtrl.ShowScene(CurrentSceneName, abAssetBundle);

            SceneInfo = SceneCtrl.SceneInfo;
            SceneTerrain = SceneCtrl.CurrentTerrain;

            if (SceneTerrain.layer != nTerrainLayer)
            {
                ChangeTerrainLayer(nTerrainLayer);
            }

            if (SceneInfo != null)
            {
                //if (RoleLight != null)
                //{
                //    RoleLight.gameObject.SetActive(SceneInfo.RoleLight.LightExists);

                //    Color color;
                //    color.a = SceneInfo.RoleLight.LightColorA;
                //    color.r = SceneInfo.RoleLight.LightColorR;
                //    color.g = SceneInfo.RoleLight.LightColorG;
                //    color.b = SceneInfo.RoleLight.LightColorB;
                //    RoleLight.color = color;
                //    RoleLight.intensity = SceneInfo.RoleLight.LightIntensity;
                //    mRoleLightFollow.RotationFixX = SceneInfo.RoleLight.LightFixRX;
                //    mRoleLightFollow.RotationOffsetY = SceneInfo.RoleLight.LightOffsetRY;
                //}

                //if (AssistLight != null)
                //{
                //    AssistLight.gameObject.SetActive(SceneInfo.AssistLight.LightExists);

                //    Color color;
                //    color.a = SceneInfo.AssistLight.LightColorA;
                //    color.r = SceneInfo.AssistLight.LightColorR;
                //    color.g = SceneInfo.AssistLight.LightColorG;
                //    color.b = SceneInfo.AssistLight.LightColorB;
                //    AssistLight.color = color;
                //    AssistLight.intensity = SceneInfo.AssistLight.LightIntensity;
                //    mAssistLightFollow.RotationFixX = SceneInfo.AssistLight.LightFixRX;
                //    mAssistLightFollow.RotationOffsetY = SceneInfo.AssistLight.LightOffsetRY;
                //}

                if (MainLight != null)
                {
                    MainLight.gameObject.SetActive(SceneInfo.SceneLight.LightExists);
                    Color color;
                    color.a = SceneInfo.SceneLight.LightColorA;
                    color.r = SceneInfo.SceneLight.LightColorR;
                    color.g = SceneInfo.SceneLight.LightColorG;
                    color.b = SceneInfo.SceneLight.LightColorB;
                    MainLight.color = color;
                    MainLight.intensity = SceneInfo.SceneLight.LightIntensity;
                    MainLight.shadows = (LightShadows)SceneInfo.SceneLight.ShadowType;
                    MainLight.shadowStrength = SceneInfo.SceneLight.ShadowStrength;
                    MainLight.transform.eulerAngles = new Vector3(SceneInfo.SceneLight.LightFixRX, SceneInfo.SceneLight.LightOffsetRY, 0);
                }

                if (ShadowCamera != null)
                {
                    float x = SceneInfo.SceneLight.LightFixRX == 0 ? 90 : SceneInfo.SceneLight.LightFixRX;
                    ShadowCamera.transform.eulerAngles = new Vector3(x, SceneInfo.SceneLight.LightOffsetRY, 0);
                }
            }

            DispatchEvent(ZSceneEvent.SCENE_CHANGED, new ZSceneEvent());
            
        }

        public static void Update()
        {
            SceneCtrl.Update();
        }

        public static void ChangeTerrainLayer(string name)
        {
            ExtUtil.ChangeGameObjectLayer(SceneTerrain, name);
        }

        public static void ChangeTerrainLayer(int layer)
        {
            ExtUtil.ChangeGameObjectLayer(SceneTerrain, layer);
        }

        public static string GetSceneNodePath(string nodeName)
        {
            string path = string.Format("{0}/{1}.unity3d", LoadingSceneName, nodeName);

            return Path.Combine(RootPath, path);
        }

        public static string GetSceneMeshPath(string meshname)
        {
            string path = string.Format("{0}/Models/{1}.unity3d", LoadingSceneName, meshname);

            return Path.Combine(RootPath, path);
        }

        public static string GetSceneTexPath(string texname)
        {
            string path = string.Format("{0}/Textures/{1}.unity3d", LoadingSceneName, texname);

            return Path.Combine(RootPath, path);
        }

        #region 事件相关方法

        public static bool DispatchEvent(string type, ZSceneEvent evt)
        {
            return EventDispatcher.DispatchEvent(type, evt);
        }

        public static void AddEventListener(string type, Callback<ZSceneEvent> listener)
        {
            EventDispatcher.AddEventListener(type, listener);
        }

        public static void RemoveEventListener(string type, Callback<ZSceneEvent> listener)
        {
            EventDispatcher.RemoveEventListener(type, listener);
        }

        public static bool HasListener(string type)
        {
            return EventDispatcher.HasListener(type);
        }

        public static void RemoveAllListener(string type)
        {
            EventDispatcher.RemoveAllListener(type);
        }

        public static void RemoveAllListener()
        {
            EventDispatcher.RemoveAllListener();
        }

        #endregion
    }
}
