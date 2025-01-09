using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Loaders;
using UnityExt.ZScene;
using UnityLight.Loggers;

namespace UnityExt
{
    public class ExtUtil
    {
        public static GameObject InstantiateBundleAsset(URLLoader loader)
        {
            return InstantiateBundleAsset(loader.DataBundle.mainAsset);
        }

        public static GameObject InstantiateBundleAsset(UnityEngine.Object asset)
        {
            try
            {
                var cls = asset as GameObject;
                var ins = UnityEngine.Object.Instantiate(cls) as GameObject;
                return ins;
            }
            catch (Exception ex)
            {
                XLogger.Fatal(ex);
            }
            return null;
        }

        public static void ChangeGameObjectLayer(GameObject oGameObject, string name)
        {
            if (oGameObject == null) return;
            int layer = LayerMask.NameToLayer(name);
            ChangeGameObjectLayer(oGameObject, layer);
        }

        public static void ChangeGameObjectLayer(GameObject oGameObject, int layer)
        {
            if (oGameObject == null) return;
            oGameObject.layer = layer;
            Transform[] trans = oGameObject.GetComponentsInChildren<Transform>();
            foreach (var item in trans)
            {
                item.gameObject.layer = layer;
            }
        }

        public static void ChangeGameObjectShader(GameObject oGameObject, string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            Shader oShader = Shader.Find(name);
            if (oShader == null) return;

            Renderer[] oRenderer = oGameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in oRenderer)
            {
                foreach (var material in renderer.materials)
                {
                    material.shader = oShader;
                }
            }
        }


        public static void ChangeGameObjectShader(GameObject oGameObject)
        {
            Renderer[] oRenderer = oGameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in oRenderer)
            {
                foreach (var material in renderer.materials)
                {
                    string shaderName = material.shader.name;
                    Shader shader = Shader.Find(shaderName);
                    if (shader != null) material.shader = shader;
                }
            }
        }


        public static void GetGameObjectChildren(Transform trans, string[] names, List<GameObject> list)
        {
            int count = trans.childCount;

            for (int i = 0; i < count; i++)
            {
                Transform child = trans.GetChild(i);
                if (names.Contains(child.name) && list.Contains(child.gameObject) == false)
                {
                    list.Add(child.gameObject);
                }

                GetGameObjectChildren(child, names, list);
            }
        }

        public static GameObject GetGameObjectChild(Transform trans, string name, bool bIncludeSelf = false)
        {
            if (bIncludeSelf && trans.name == name) return trans.gameObject;

            int count = trans.childCount;
            GameObject oGameObject = null;
            for (int i = 0; i < count; i++)
            {
                Transform child = trans.GetChild(i);

                if (name == child.name) return child.gameObject;

                oGameObject = GetGameObjectChild(child, name, false);

                if (oGameObject != null) return oGameObject;
            }

            return null;
        }

        public static void GetGameObjectBounds(GameObject oGameObject, ref Vector3 min, ref Vector3 max)
        {
            min.x = min.y = min.z = float.MaxValue;
            max.x = max.y = max.z = float.MinValue;
            Type type = null;
            Type typeSkinmeshRender = typeof(SkinnedMeshRenderer);
            Type typeMeshRender = typeof(MeshRenderer);

            Renderer[] renderers = oGameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                type = renderer.GetType();
                if (type != typeSkinmeshRender && type != typeMeshRender) continue;
                min.x = Math.Min(min.x, renderer.bounds.min.x);
                min.y = Math.Min(min.y, renderer.bounds.min.y);
                min.z = Math.Min(min.z, renderer.bounds.min.z);
                max.x = Math.Max(max.x, renderer.bounds.max.x);
                max.y = Math.Max(max.y, renderer.bounds.max.y);
                max.z = Math.Max(max.z, renderer.bounds.max.z);
            }
        }

        public static ZSceneObject GetSceneObject(GameObject oGameObject)
        {
            if (oGameObject == null) return null;
            RelevanceBehaviour oRelevanceBehaviour = oGameObject.GetComponent<RelevanceBehaviour>();
            if (oRelevanceBehaviour == null) return null;

            if (oRelevanceBehaviour.Instance == oGameObject || oRelevanceBehaviour.MainObject == oGameObject)
            {
                return oRelevanceBehaviour.SceneObject;
            }

            return null;
        }

        public static void DestroyImmediate(UnityEngine.Object obj)
        {
            if (obj != null) UnityEngine.Object.DestroyImmediate(obj);
        }
    }
}
