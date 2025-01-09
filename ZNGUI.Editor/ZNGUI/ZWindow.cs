using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;


public class ZWindow : ZUIObject, IZWindow
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Window; } }

    private const int ADD = 500;

    private UIPanel[] UIPanelArray;

    public override void Initialize()
    {
        UIPanelArray = gameObject.GetComponentsInChildren<UIPanel>();
    }

    public void Bring2Top()
    {
        AdjustDepth(ADD);
    }

    public void Bring2Bottom()
    {
        AdjustDepth(-1 * ADD);
    }

    public void AdjustDepth(int depth)
    {
        foreach (var panel in UIPanelArray)
        {
            panel.depth = panel.depth + depth;
        }
        NGUITools.NormalizePanelDepths();
    }
}
