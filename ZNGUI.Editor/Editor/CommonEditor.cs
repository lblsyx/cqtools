using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityExt.ZNGUI;

public class CommonEditor : Editor
{
    #region 通用工具

    [MenuItem("Assets/通用工具/生成Bundle文件", false, 1121)]
    public static void PackAssetbundle()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = EditorUtility.SaveFilePanel("保存Assetbundle资源文件", "", obj.name, "unity3d");
        PackAssetbundle(path);
    }

    [MenuItem("Assets/通用工具/生成UnityPackage包", false, 1121)]
    public static void PackUnityPackage()
    {
        if (Selection.objects == null) return;

        List<string> paths = new List<string>();
        foreach (UnityEngine.Object o in Selection.objects)
        {
            paths.Add(AssetDatabase.GetAssetPath(o));
        }

        string savePath = EditorUtility.SaveFilePanel("Save Resource", "", Selection.objects[0].name, "unitypackage");
        AssetDatabase.ExportPackage(paths.ToArray(), savePath, ExportPackageOptions.IncludeDependencies);
        AssetDatabase.Refresh();
        Debug.Log("打包unitypackage资源文件成功!");
    }

    public static void PackAssetbundle(string savePath)
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (savePath.Length != 0)
        {
            //从当前选中的所有对象创建资源文件
            UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, savePath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.WebPlayer);
            Selection.objects = selection;
            Debug.Log("导出Assetbundle资源文件成功!");
        }
    }

    #endregion
}
