using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene
{
    public class ZSceneMesh
    {
        private Dictionary<int, Dictionary<Material, List<ZMeshCtrl>>> mMeshDatas;

        public Transform Root { get; private set; }

        public ZSceneMesh()
        {
            mMeshDatas = new Dictionary<int, Dictionary<Material, List<ZMeshCtrl>>>();
        }

        public void Init(Transform root)
        {
            Root = root;
        }

        public void AddMeshData(Material _material, MeshCombineUtility.MeshInstance _combine)
        {
            Dictionary<Material, List<ZMeshCtrl>> lightMapBranch = null;

            if (_combine.mesh != null)
            {
                if (mMeshDatas.ContainsKey(_combine.lightMapIndex))
                {
                    lightMapBranch = mMeshDatas[_combine.lightMapIndex];
                }
                else
                {
                    lightMapBranch = new Dictionary<Material, List<ZMeshCtrl>>();
                    mMeshDatas.Add(_combine.lightMapIndex, lightMapBranch);
                }

                List<ZMeshCtrl> iMeshDatas = null;
                if (lightMapBranch.ContainsKey(_material))
                {
                    iMeshDatas = lightMapBranch[_material];
                }
                else
                {
                    iMeshDatas = new List<ZMeshCtrl>();
                    lightMapBranch.Add(_material, iMeshDatas);
                }

                ZMeshCtrl oMeshCtrl = null;
                if (iMeshDatas.Count > 0)
                {
                    oMeshCtrl = iMeshDatas[iMeshDatas.Count - 1];
                }
                else
                {
                    oMeshCtrl = new ZMeshCtrl();
                    oMeshCtrl.gameobject = CreateGameObject(_material, _combine.lightMapIndex);
                    iMeshDatas.Add(oMeshCtrl);
                }

                Mesh mesh = _combine.mesh;

                int vertexCount = _combine.mesh.vertexCount;
                if (oMeshCtrl.vertexIndex + vertexCount >= 65535)
                {
                    oMeshCtrl = new ZMeshCtrl();
                    oMeshCtrl.gameobject = CreateGameObject(_material, _combine.lightMapIndex);
                    iMeshDatas.Add(oMeshCtrl);
                }

                Matrix4x4 matrix = _combine.transform;
                oMeshCtrl.CopyVertex(mesh.vertexCount, mesh.vertices, matrix);
                int[] triangles = mesh.GetTriangles(_combine.subMeshIndex);
                oMeshCtrl.CopyTriangle(mesh.vertexCount, triangles);
                oMeshCtrl.CopyNormal(mesh.vertexCount, mesh.normals, _combine.invTranspose);
                oMeshCtrl.CopyTangents(mesh.vertexCount, mesh.tangents, _combine.invTranspose);
                oMeshCtrl.CopyUVs(mesh.vertexCount, mesh.uv);
                oMeshCtrl.CopyUV1s(mesh.vertexCount, mesh.uv1);
                oMeshCtrl.CopyUV2s(mesh.vertexCount, mesh.uv2, _combine.lightMapScaleOffset);
                
                oMeshCtrl.RefreshMesh();

            }
        }

        public bool RemoveMeshData()
        {
            return false;
        }

        private GameObject CreateGameObject(Material _mat, int _lightMapIndex)
        {
            GameObject go = new GameObject("Combined mesh");
            go.transform.parent = Root;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent(typeof(MeshFilter));
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.material = _mat;
            renderer.lightmapIndex = _lightMapIndex;
            MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
            return go;
        }

        //~ZSceneMesh()
        //{
        //    Dispose();
        //}

        //public void Dispose()
        //{
        //    mMeshDatas.Clear();
        //    mMeshDatas = null;
        //}
    }

    public class ZMeshCtrl
    {
        public GameObject gameobject;
        public MeshFilter meshfilter;

        public const int MAX_VERTEX_COUNT = 65534;

        public Vector3[] Vertices = new Vector3[MAX_VERTEX_COUNT];
        public Vector3[] Normals = new Vector3[MAX_VERTEX_COUNT];
        public Vector4[] Tangents = new Vector4[MAX_VERTEX_COUNT];
        public Vector2[] UVs = new Vector2[MAX_VERTEX_COUNT];
        public Vector2[] UV1s = new Vector2[MAX_VERTEX_COUNT];
        public Vector2[] UV2s = new Vector2[MAX_VERTEX_COUNT];
        public Color[] Colors = new Color[MAX_VERTEX_COUNT];
        public int[] Triangles = new int[MAX_VERTEX_COUNT * 3];

        public int vertexIndex = 0;
        public int triangleOffsetIndex = 0;
        public int triangleIndex = 0;
        public int normalIndex = 0;
        public int tangentIndex = 0;
        public int uvIndex = 0;
        public int uv1Index = 0;
        public int uv2Index = 0;
        public int uv3Index = 0;
        public int colorIndex = 0;

        private Mesh mesh = new Mesh();

        public MeshFilter MeshFilter
        {
            get
            {
                if (meshfilter == null)
                {
                    meshfilter = gameobject.GetComponent<MeshFilter>();
                }
                return meshfilter;
            }
        }

        public void CopyVertex(int vertexcount, Vector3[] src, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                Vertices[i + vertexIndex] = transform.MultiplyPoint(src[i]);
            vertexIndex += vertexcount;
        }

        public void CopyTriangle(int vertexcount, int[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                Triangles[i + triangleOffsetIndex] = src[i] + triangleIndex;
            }
            triangleOffsetIndex += src.Length;
            triangleIndex += vertexcount;
        }

        public void CopyNormal(int vertexcount, Vector3[] src, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                Normals[i + normalIndex] = transform.MultiplyVector(src[i]).normalized;
            normalIndex += vertexcount;
        }

        public void CopyTangents(int vertexcount, Vector4[] src, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                Vector4 p4 = src[i];
                Vector3 p = new Vector3(p4.x, p4.y, p4.z);
                p = transform.MultiplyVector(p).normalized;
                Tangents[i + tangentIndex] = new Vector4(p.x, p.y, p.z, p4.w);
            }
            tangentIndex += vertexcount;
        }

        public void CopyUVs(int vertexcount, Vector2[] src)
        {
            for (int i = 0; i < src.Length; i++)
                UVs[i + uvIndex] = src[i];
            uvIndex += vertexcount;
        }

        public void CopyUV1s(int vertexcount, Vector2[] src)
        {
            for (int i = 0; i < src.Length; i++)
                UV1s[i + uv1Index] = src[i];
            uv1Index += vertexcount;
        }

        public void CopyUV2s(int vertexcount, Vector2[] src, Vector4 lightmapTilingOffset)
        {
            Vector2 uvscale = new Vector2(lightmapTilingOffset.x, lightmapTilingOffset.y);
            Vector2 uvoffset = new Vector2(lightmapTilingOffset.z, lightmapTilingOffset.w);
            for (int i = 0; i < src.Length; i++)
            {
                UV2s[i + uv2Index] = uvoffset + new Vector2(uvscale.x * src[i].x, uvscale.y * src[i].y);
            }
            uv2Index += vertexcount;
        }

        public void CopyColors(int vertexcount, Color[] src)
        {
            for (int i = 0; i < src.Length; i++)
                Colors[i + colorIndex] = src[i];
            colorIndex += vertexcount;
        }

        public void RefreshMesh()
        {
            Vector3[] vertices = new Vector3[vertexIndex];
            int[] triangles = new int[triangleOffsetIndex];
            Vector3[] normals = new Vector3[normalIndex];
            Vector4[] tangents = new Vector4[tangentIndex];
            Vector2[] uv = new Vector2[uvIndex];
            Vector2[] uv1 = new Vector2[uv1Index];
            Vector2[] uv2 = new Vector2[uv2Index];
            Color[] colors = new Color[colorIndex];

            Array.Copy(Vertices, vertices, vertexIndex);
            Array.Copy(Triangles, triangles, triangleOffsetIndex);
            Array.Copy(Normals, normals, normalIndex);
            Array.Copy(Tangents, tangents, tangentIndex);
            Array.Copy(UVs, uv, uvIndex);
            Array.Copy(UV1s, uv1, uv1Index);
            Array.Copy(UV2s, uv2, uv2Index);
            Array.Copy(Colors, colors, colorIndex);

            mesh.Clear();
            mesh.name = "Combined Mesh";
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.uv1 = uv1;
            mesh.uv2 = uv2;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            MeshFilter.sharedMesh = mesh;
        }

    }

}
