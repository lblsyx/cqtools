using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityExt
{
    public class MeshCombineUtility
    {
        public struct MeshInstance
        {
            public Mesh mesh;
            public int subMeshIndex;
            public Matrix4x4 transform;
            public Matrix4x4 invTranspose;
            public int lightMapIndex;
            public Vector4 lightMapScaleOffset;
        }

        public const int MAX_VERTEX_COUNT = 65535;

        public static MeshInstance[] CombineMeshs(MeshInstance[] combines)
        {
            int oVertexCount = 0;
            Dictionary<int, List<List<MeshInstance>>> oMeshInstanceDict = new Dictionary<int, List<List<MeshInstance>>>();

            for (int i = 0; i < combines.Length; i++)
            {
                // 获取后续网格需要的组
                List<MeshInstance> oMeshInstance = null;
                int vertexCount = oVertexCount + combines[i].mesh.vertexCount;

                List<List<MeshInstance>> oMeshInstanceList = null;
                int lightMapIndex = combines[i].lightMapIndex;
                if (oMeshInstanceDict.ContainsKey(lightMapIndex))
                {
                    oMeshInstanceList = oMeshInstanceDict[lightMapIndex];
                }
                else
                {
                    oMeshInstanceList = new List<List<MeshInstance>>();
                    oMeshInstanceDict.Add(lightMapIndex, oMeshInstanceList);
                }

                if (vertexCount >= MAX_VERTEX_COUNT)
                {
                    oMeshInstance = new List<MeshInstance>();
                    oMeshInstanceList.Add(oMeshInstance);
                    oVertexCount = 0;
                }
                else
                {
                    if (oMeshInstanceList.Count > 0)
                    {
                        oMeshInstance = oMeshInstanceList[oMeshInstanceList.Count - 1];
                    }
                    else
                    {
                        oMeshInstance = new List<MeshInstance>();
                        oMeshInstanceList.Add(oMeshInstance);
                    }
                }
                oVertexCount = oVertexCount + combines[i].mesh.vertexCount;
                oMeshInstance.Add(combines[i]);
            }

            List<MeshInstance> oMeshList = new List<MeshInstance>();
            foreach (var key in oMeshInstanceDict.Keys)
            {
                List<List<MeshInstance>> oMeshInstanceList = oMeshInstanceDict[key];
                for (int i = 0; i < oMeshInstanceList.Count; i++)
                {
                    Mesh mesh = Combine(oMeshInstanceList[i].ToArray());
                    MeshInstance meshInstance = new MeshInstance();
                    meshInstance.mesh = mesh;
                    meshInstance.lightMapIndex = key;
                    oMeshList.Add(meshInstance);
                }
            }
            return oMeshList.ToArray();
        }

        public static Mesh Combine(MeshInstance[] combines)
        {
            int oVertexCount = 0;
            int oTriangleCount = 0;
            for (int i = 0; i < combines.Length; i++)
            {
                MeshInstance meshInstance = combines[i];
                if (meshInstance.mesh != null)
                {
                    oVertexCount = oVertexCount + meshInstance.mesh.vertexCount;
                    oTriangleCount = oTriangleCount + meshInstance.mesh.GetTriangles(meshInstance.subMeshIndex).Length;
                }
            }

            Vector3[] vertices = new Vector3[oVertexCount];
            Vector3[] normals = new Vector3[oVertexCount];
            Vector4[] tangents = new Vector4[oVertexCount];
            Vector2[] uv = new Vector2[oVertexCount];
            Vector2[] uv1 = new Vector2[oVertexCount];
            Vector2[] uv2 = new Vector2[oVertexCount];
            Vector2[] uv3 = new Vector2[oVertexCount];
            Color[] colors = new Color[oVertexCount];
            int[] triangles = new int[oTriangleCount];

            int offset;

            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                    Copy(combine.mesh.vertexCount, combine.mesh.vertices, vertices, ref offset, combine.transform);
            }

            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    Matrix4x4 invTranspose = combine.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyNormal(combine.mesh.vertexCount, combine.mesh.normals, normals, ref offset, invTranspose);
                }

            }
            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    Matrix4x4 invTranspose = combine.transform;
                    invTranspose = invTranspose.inverse.transpose;
                    CopyTangents(combine.mesh.vertexCount, combine.mesh.tangents, tangents, ref offset, invTranspose);
                }

            }
            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    Copy(combine.mesh.vertexCount, combine.mesh.uv, uv, ref offset);
                }
            }

            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    Copy(combine.mesh.vertexCount, combine.mesh.uv1, uv1, ref offset);
                }
            }

            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    //if (combine.lightMapIndex == -1)
                    //{
                    //    Copy(combine.mesh.vertexCount, combine.mesh.uv2, uv2, ref offset);
                    //}
                    //else
                    //{
                    Vector2[] lightmapUVs = combine.mesh.uv2;
                    Vector4 lightmapTilingOffset = combine.lightMapScaleOffset;
                    Vector2 uvscale = new Vector2(lightmapTilingOffset.x, lightmapTilingOffset.y);
                    Vector2 uvoffset = new Vector2(lightmapTilingOffset.z, lightmapTilingOffset.w);
                    for (int i = 0; i < lightmapUVs.Length; i++)
                    {
                        lightmapUVs[i] = uvoffset + new Vector2(uvscale.x * lightmapUVs[i].x, uvscale.y * lightmapUVs[i].y);
                        uv2[i + offset] = lightmapUVs[i];
                    }
                    offset += combine.mesh.vertexCount;
                    //}
                }
            }

            offset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                    CopyColors(combine.mesh.vertexCount, combine.mesh.colors, colors, ref offset);
            }

            int triangleOffset = 0;
            int vertexOffset = 0;
            foreach (MeshInstance combine in combines)
            {
                if (combine.mesh)
                {
                    int[] inputtriangles = combine.mesh.GetTriangles(combine.subMeshIndex);
                    for (int i = 0; i < inputtriangles.Length; i++)
                    {
                        triangles[i + triangleOffset] = inputtriangles[i] + vertexOffset;
                    }
                    triangleOffset += inputtriangles.Length;

                    vertexOffset += combine.mesh.vertexCount;
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "Combined Mesh";
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.uv1 = uv1;
            mesh.uv2 = uv2;
            mesh.tangents = tangents;
            mesh.triangles = triangles;

            return mesh;
        }

        private static void Copy(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = transform.MultiplyPoint(src[i]);
            offset += vertexcount;
        }

        private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
            offset += vertexcount;
        }

        private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = src[i];
            offset += vertexcount;
        }

        private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i + offset] = src[i];
            offset += vertexcount;
        }

        private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < src.Length; i++)
            {
                Vector4 p4 = src[i];
                Vector3 p = new Vector3(p4.x, p4.y, p4.z);
                p = transform.MultiplyVector(p).normalized;
                dst[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
            }
            offset += vertexcount;
        }

    }
}