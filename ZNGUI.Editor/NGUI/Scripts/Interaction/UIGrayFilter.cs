using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIGrayFilter : UIFilter
{
    private UIAtlas orginAtlas;

    private UIAtlas grayAtlas;

    private Material grayMat;

    private bool isGray = false;

    public override bool Enabled
    {
        get
        {
            return isGray;
        }
        set
        {
            isGray = value;
            SetGray(isGray);
        }
    }

    private void SetGray(bool gray)
    {
        UIBasicSprite basicSprite = GetComponent<UIBasicSprite>();
        if (basicSprite != null)
        {
            Type type = basicSprite.GetType();
            if (type == typeof(UISprite))
            {
                if (grayAtlas == null || orginAtlas == null)
                {
                    return;
                }
                UISprite sprite = basicSprite as UISprite;
                UIAtlas atlas = null;
                if (gray == true)
                {
                    atlas = grayAtlas;
                }
                else
                {
                    atlas = orginAtlas;
                }
                sprite.atlas = atlas;
            }
            else if (type == typeof(UITexture))
            {
                UITexture sprite = basicSprite as UITexture;
                Shader shader = null;
                if (gray == true)
                {
                    shader = Shader.Find("Unlit/GrayShader");
                }
                else
                {
                    shader = Shader.Find("Unlit/Transparent Colored");
                }
                sprite.shader = shader;
                sprite.MarkAsChanged();
            }
        }
    }

    void OnEnable()
    {
        UISprite sprite = gameObject.GetComponent<UISprite>();
        if (sprite != null)
        {
            if (orginAtlas == null)
            {
                orginAtlas = sprite.atlas;
            }

            if (grayMat == null)
            {
                grayMat = Instantiate(orginAtlas.spriteMaterial) as Material;
                grayMat.hideFlags = HideFlags.HideAndDontSave;
                grayMat.shader = Shader.Find("Unlit/GrayShader");
                grayMat.name = grayMat.name + "(Gray)";
            }

            if (grayAtlas == null)
            {
                GameObject ins = Instantiate(orginAtlas.gameObject) as GameObject;
                ins.hideFlags = HideFlags.HideAndDontSave;
                grayAtlas = ins.GetComponent<UIAtlas>();
                grayAtlas.spriteMaterial = grayMat;
                grayAtlas.name = grayAtlas.name + "(Gray)";
            }
        }
    }
}
