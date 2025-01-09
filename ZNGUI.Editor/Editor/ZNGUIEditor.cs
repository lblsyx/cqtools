using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityExt.ZNGUI;

public class ZNGUIEditor : Editor
{
    #region 配置项

    public const string UIBundleOutputRoot = "X:/UIAssets";

    #endregion

    #region UI工具

    /// <summary>
    /// {0}：变量名
    /// {1}：变量类型
    /// </summary>
    public const string MEMBER_FORMAT = "public {1} {0} = null;";
    /// <summary>
    /// {0}：变量名
    /// {1}：变量类型
    /// </summary>
    public const string MEMBER_ASSIGN = "{0} = GetUIObject(\"{0}\") as {1};\r\n        if (null == {0}) XLogger.Error(\"{0} must be not null!\");";

    /// <summary>
    /// __LK__：左大括号{
    /// __RK__：右大括号}
    /// {0}：UI名称
    /// {1}：成员变量代码
    /// {2}：变量赋值代码
    /// </summary>
    public const string BEHAVIOUR_FORMAT =
        "using UnityEngine;\r\n" +
        "using UnityLight.Loggers;\r\n" +
        "using UnityExt.ZNGUI;\r\n" +
        "using UnityExt.ZNGUI.Interfaces;\r\n\r\n" +

        "public class {0}Behaviour : ZDialogBehaviour\r\n" +
        "__LK__\r\n" +
        "    {1}\r\n\r\n" +

        "    public override void Initialize()\r\n" +
        "    __LK__\r\n" +
        "        {2}\r\n" +
        "    __RK__\r\n" +
        "__RK__\r\n";
    /// <summary>
    /// __LK__：左大括号{
    /// __RK__：右大括号}
    /// {0}：UI名称
    /// {1}：UI名称(去掉ZUI)
    /// </summary>
    public const string LOGIC_FORMAT =
        @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;

namespace RXLWClient.AllUIs.{1}
__LK__
    public class {0} : ZDialogBase<{0}, {0}Behaviour>
    __LK__
        public override void Initialize()
        __LK__
        __RK__
    __RK__
__RK__";

    [MenuItem("Assets/UI工具/生成UI代码", false, 1101)]
    public static void GenUICode()
    {
        GameObject mainObject = Selection.activeGameObject;
        if (mainObject == null)
        {
            Debug.LogError("未选择UI预制体");
            return;
        }
        mainObject = Instantiate(mainObject) as GameObject;
        mainObject.name = Selection.activeGameObject.name;
        Transform mainTrans = mainObject.transform;

        string BehaviourFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BehaviourCode");
        ZUIObject[] array = mainTrans.GetComponentsInChildren<ZUIObject>(true);

        List<string> memberList = new List<string>();
        List<string> assignList = new List<string>();

        foreach (var item in array)
        {
            if (item is ZListItem) continue;
            var pi = item.GetComponentsInParent<ZListItem>();

            if (pi.Length != 0 || item.gameObject.name.Contains("#")) continue;

            string strName = string.Format("{1}{0}", item.gameObject.name, getShortTypeName(item.Type));
            string strType = GetUIObjectType(item.Type);
            memberList.Add(string.Format(MEMBER_FORMAT, strName, strType));
            assignList.Add(string.Format(MEMBER_ASSIGN, strName, strType));
        }

        assignList.Add(string.Format("WinCtrl = GetUIObject(\"win{0}\") as IZWindow;", mainTrans.name));

        assignList.Add("DialogAtlas = GetUIObject(\"atsDialogAtlas\") as IZUIAtlas;");

        string memberCode = string.Join("\r\n    ", memberList.ToArray());
        string assignCode = string.Join("\r\n        ", assignList.ToArray());

        string code = string.Format(BEHAVIOUR_FORMAT, mainTrans.name, memberCode, assignCode);
        code = code.Replace("__LK__", "{");
        code = code.Replace("__RK__", "}");
        string behaviourCodeFile = Path.Combine(BehaviourFolder, mainTrans.name) + "Behaviour.cs";
        Directory.CreateDirectory(BehaviourFolder);
        using (FileStream fs = File.Create(behaviourCodeFile))
        {
            byte[] bytes = Encoding.Default.GetBytes(code);
            fs.Write(bytes, 0, bytes.Length);
        }
        Debug.Log(string.Format("代码生成成功!{0}", behaviourCodeFile));


        code = string.Format(LOGIC_FORMAT, mainTrans.name, mainTrans.name.Replace("ZUI", ""));
        code = code.Replace("__LK__", "{");
        code = code.Replace("__RK__", "}");
        string logicCodeFile = Path.Combine(BehaviourFolder, mainTrans.name) + ".cs";
        using (FileStream fs = File.Create(logicCodeFile))
        {
            byte[] bytes = Encoding.Default.GetBytes(code);
            fs.Write(bytes, 0, bytes.Length);
        }
        Debug.Log(string.Format("代码生成成功!{0}", logicCodeFile));
        DestroyImmediate(mainObject);
    }

    [MenuItem("Assets/UI工具/生成UI图集", false, 1102)]
    public static void GenUIAtlas()
    {
        if (Selection.activeGameObject == null) return;

        UnityEngine.Object atlas = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetOrScenePath(Selection.activeGameObject), typeof(UIAtlas));

        if (atlas == null)
        {
            Debug.LogWarning("请选择 UIAtlas 对象");
            return;
        }

        string url = Path.Combine(UIBundleOutputRoot, Selection.activeGameObject.name + ".unity3d");

        BuildPipeline.BuildAssetBundle(atlas, null, url);

        Debug.Log("UI图集文件生成成功!" + url);
    }

    [MenuItem("Assets/UI工具/生成UI资源", false, 1103)]
    public static void GenUIBundle()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = Path.Combine(UIBundleOutputRoot, obj.name + ".unity3d");
        CommonEditor.PackAssetbundle(path);
        Debug.Log("UI资源文件生成成功!" + path);
    }

    [MenuItem("Assets/UI工具/生成UI代码和资源", false, 1104)]
    public static void GenUICodeAndPackAssetbundle()
    {
        GenUICode();
        GenUIBundle();
    }

    private static string getShortTypeName(ZUIObjectType zUIObjectType)
    {
        switch (zUIObjectType)
        {
            case ZUIObjectType.Label:
                return "lbl";
            case ZUIObjectType.Input:
                return "ipt";
            case ZUIObjectType.Button:
                return "btn";
            case ZUIObjectType.Toggle:
                return "tgl";
            case ZUIObjectType.Image:
                return "img";
            case ZUIObjectType.List:
                return "lst";
            case ZUIObjectType.ListItem:
                return "li";
            case ZUIObjectType.Progress:
                return "prg";
            case ZUIObjectType.Sprite:
                return "sp";
            case ZUIObjectType.Group:
                return "grp";
            case ZUIObjectType.Atlas:
                return "ats";
            case ZUIObjectType.Window:
                return "win";
            case ZUIObjectType.Animation:
                return "anm";
            case ZUIObjectType.ScrollBar:
                return "scl";
            case ZUIObjectType.Filter:
                return "flt";
            default:
                return "un";
        }
    }

    private static string GetUIObjectType(ZUIObjectType zUIObjectType)
    {
        switch (zUIObjectType)
        {
            case ZUIObjectType.Label:
                return "IZLabel";
            case ZUIObjectType.Input:
                return "IZInput";
            case ZUIObjectType.Button:
                return "IZButton";
            case ZUIObjectType.Toggle:
                return "IZToggle";
            case ZUIObjectType.Image:
                return "IZImage";
            case ZUIObjectType.List:
                return "IZList";
            case ZUIObjectType.ListItem:
                return "IZListItem";
            case ZUIObjectType.Progress:
                return "IZProgress";
            case ZUIObjectType.Sprite:
                return "IZSprite";
            case ZUIObjectType.Group:
                return "IZGroup";
            case ZUIObjectType.Atlas:
                return "IZUIAtlas";
            case ZUIObjectType.Window:
                return "IZWindow";
            case ZUIObjectType.Animation:
                return "IZUISpriteAnimation";
            case ZUIObjectType.ScrollBar:
                return "IZScrollBar";
            case ZUIObjectType.Filter:
                return "IZFilter";
            default:
                return "IZUIObject";
        }
    }

    #endregion
}
