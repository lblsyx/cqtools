using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExt.ZScene;

namespace UnityExt.ZScene
{
    public class SceneArea
    {
        private SceneInfo mSceneInfo;
        
        public List<AreaInfo> InAreaInfos = new List<AreaInfo>();

        public void InitArea(SceneInfo sceneInfo)
        {
            mSceneInfo = sceneInfo;
            int iAreaUnit = sceneInfo.AreaUnit;
            int iAreaX = sceneInfo.AreaX;
            int iAreaZ = sceneInfo.AreaY;

            for (int j = 0; j < iAreaZ; j++)
            {
                for (int i = 0; i < iAreaX; i++)
                {
                    AreaInfo oAreaInfo = new AreaInfo();
                    oAreaInfo.AreaX = i;
                    oAreaInfo.AreaZ = j;
                    float halfSize = iAreaUnit * 0.5f;
                    oAreaInfo.Center = new Vector3(i * iAreaUnit + halfSize, 0, j * iAreaUnit + halfSize);
                    InAreaInfos.Add(oAreaInfo);
                }
            }

            ModelInfo[] iModelInfo = sceneInfo.ModelInfos;
            for (int i = 0; i < iModelInfo.Length; i++)
            {
                ModelInfo oModelInfo = iModelInfo[i];
                Vector4 bound = oModelInfo.Bound;
                int minX = Mathf.CeilToInt(bound.x - bound.z * 0.5F);
                int maxX = Mathf.CeilToInt(bound.x + bound.z * 0.5F);
                int minZ = Mathf.CeilToInt(bound.y - bound.w * 0.5F);
                int maxZ = Mathf.CeilToInt(bound.y + bound.w * 0.5F);
                minX = Mathf.Clamp(Mathf.CeilToInt((float)minX / iAreaUnit) - 1, 0, iAreaX - 1);
                maxX = Mathf.Clamp(Mathf.CeilToInt((float)maxX / iAreaUnit) - 1, 0, iAreaX - 1);
                minZ = Mathf.Clamp(Mathf.CeilToInt((float)minZ / iAreaUnit) - 1, 0, iAreaZ - 1);
                maxZ = Mathf.Clamp(Mathf.CeilToInt((float)maxZ / iAreaUnit) - 1, 0, iAreaZ - 1);
                for (int w = minZ; w <= maxZ; w++)
                {
                    for (int t = minX; t <= maxX; t++)
                    {
                        AreaInfo oAreaInfo = InAreaInfos[w * iAreaZ + t];
                        oAreaInfo.AddModel(oModelInfo);
                    }
                }
            }
        }

        public AreaInfo TryLoadNextArea()
        {
            AreaInfo info = null;
            if (InAreaInfos.Count > 0)
            {
                InAreaInfos.Sort();
                info = InAreaInfos[0];
                InAreaInfos.Remove(info);
            }
            return info;
        }

    }

    public class AreaInfo : IComparable<AreaInfo>
    {
        public int AreaX = 0;
        public int AreaZ = 0;
        public Vector3 Center = Vector3.zero;
        public float dist
        {
            get
            {
                Vector3 cameraToCenter = Center - Camera.main.transform.position;
                return cameraToCenter.magnitude;
            }
        }

        private Dictionary<int, ModelInfo> mModelInfos;

        public AreaInfo()
        {
            mModelInfos = new Dictionary<int, ModelInfo>();
        }

        public void AddModel(ModelInfo model)
        {
            int key = model.GetHashCode();
            if (mModelInfos.ContainsKey(key) == false)
            {
                mModelInfos.Add(key, model);
            }
        }

        public void RemoveModel(ModelInfo model)
        {
            int key = model.GetHashCode();
            if (mModelInfos.ContainsKey(key) == true)
            {
                mModelInfos.Remove(key);
            }
        }

        public ModelInfo[] ReadModelInfos()
        {
            ModelInfo[] oModelInfos = new ModelInfo[mModelInfos.Count];
            mModelInfos.Values.CopyTo(oModelInfos, 0);
            return oModelInfos;
        }

        public int CompareTo(AreaInfo other)
        {
            if (dist > other.dist)
                return 1;
            else if (dist == other.dist)
                return 0;
            else
                return -1;
        }

    }

}
