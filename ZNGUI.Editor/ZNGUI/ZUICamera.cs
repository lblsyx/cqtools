using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;

public class ZUICamera : ZUIObject, IZUICamera
{
    public bool RaycastMouseHitUI()
    {
        return UICamera.Raycast(Input.mousePosition);
    }

    public ZEventObject RaycastMouseHit()
    {
        if (UICamera.Raycast(Input.mousePosition))
        {
            GameObject go = UICamera.lastHit.collider.gameObject;
            if (go != null) return go.GetComponentInParent<ZEventObject>();
        }
        return null;
    }

    public Vector3 ScreenToWorldPoint(Vector3 pos)
    {
        if (UICamera.currentCamera == null)
        {
            return Vector3.zero;
        }
        return UICamera.currentCamera.ScreenToWorldPoint(pos);
    }

    public Vector3 WorldToScreenPoint(Vector3 pos)
    {
        if (UICamera.currentCamera == null)
        {
            return Vector3.zero;
        }
        return UICamera.currentCamera.WorldToScreenPoint(pos);
    }
}
