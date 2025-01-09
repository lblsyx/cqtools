using System;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight.Loggers;
using UnityExt.Loaders;
using UnityLight.Events;

[RequireComponent(typeof(UISprite))]
public class ZSprite : ZUIObject, IZSprite
{
    string mSpriteName;
    string mAtlasName;
    bool mCacheAtlas;

    LoaderItem mLoaderItem;
    UISprite mUISprite;
    UIAtlas mUIAtlas;
    public override ZUIObjectType Type { get { return ZUIObjectType.Sprite; } }

    public MonoBehaviour CurrentAtlas
    {
        get { return mUIAtlas; }
    }

    public string CurrentSpriteName
    {
        get { return mSpriteName; }
    }

    public Vector2 PivotOffset
    {
        get { return mUISprite.pivotOffset; }
        set { mUISprite.pivot = NGUIMath.GetPivot(value); }
    }

    public override bool Enable
    {
        get { return mUISprite.enabled; }
        set { mUISprite.enabled = value; }
    }

    public Color FillColor
    {
        get { return mUISprite.color; }
        set { mUISprite.color = value; }
    }

    public int Width
    {
        get { return mUISprite.width; }
        set { mUISprite.width = value; }
    }

    public int Height
    {
        get { return mUISprite.height; }
        set { mUISprite.height = value; }
    }

    public float FillAmount
    {
        get { return mUISprite.fillAmount; }
        set { mUISprite.fillAmount = value; }
    }

    public bool FillInvert
    {
        get { return mUISprite.invert; }
        set { mUISprite.invert = value; }
    }

    public override void InitSelf()
    {
        base.InitSelf();

        mUISprite = GetComponent<UISprite>();
        mUIAtlas = mUISprite.atlas;
        mSpriteName = mUISprite.spriteName;
    }

    public void MakePixelPerfect()
    {
        if (mUISprite != null) mUISprite.MakePixelPerfect();
    }

    public virtual void ClearSprite()
    {
        mAtlasName = string.Empty;
        mSpriteName = string.Empty;
        MonoBehaviour atlas = null;
        SetSprite(atlas, mSpriteName);
    }

    public virtual void SetSprite(MonoBehaviour atlas, string spriteName)
    {
        mUIAtlas = atlas as UIAtlas;
        mSpriteName = spriteName;

        if (mUIAtlas == null) mAtlasName = string.Empty;

        if (string.IsNullOrEmpty(spriteName) == false && mUIAtlas != null && mUIAtlas.GetSprite(spriteName) == null)
        {
            XLogger.ErrorFormat("找不到指定SpriteName:{0}", spriteName);
        }

        if (mUISprite != null)
        {
            mUISprite.atlas = mUIAtlas;
            mUISprite.spriteName = spriteName;
        }

        DispatchEvent(new ZEvent(ZEvent.CHANGED));
    }

    public void SetSprite(string atlasName, string spriteName)
    {
        SetSprite(atlasName, spriteName, true);
    }

    public void SetSprite(string atlasName, string spriteName, bool bCacheAtlas)
    {
        //if (mAtlasName == atlasName && mSpriteName == spriteName) return;

        if (string.IsNullOrEmpty(atlasName))
        {
            ClearSprite();
            return;
        }

        mCacheAtlas = bCacheAtlas;
        mAtlasName = atlasName;
        mSpriteName = spriteName;

        if (ZUIResource.UIAtlasManager.ContainsKey(mAtlasName))
        {
            SetSprite(ZUIResource.UIAtlasManager[mAtlasName], mSpriteName);
        }
        else
        {
            if (mUISprite != null)
            {
                mUISprite.atlas = null;
                mUISprite.spriteName = string.Empty;
            }
            if (mLoaderItem != null) ZUIManager.UIDownloadQueue.Cancel(mLoaderItem);
            string url = ZUIResource.GetAtlasPath(mAtlasName);
            mLoaderItem = ZUIManager.UIDownloadQueue.Load(url, OnLoadDoneHandler);
        }

    }

    private void OnLoadDoneHandler(URLLoader loader, bool success, string errMsg)
    {
        if (success == false) return;

        AssetBundle bundle = loader.DataBundle;

        try
        {
            UIAtlas loadAtlas = bundle.mainAsset as UIAtlas;

            if (mCacheAtlas && ZUIResource.UIAtlasManager.ContainsKey(loadAtlas.name) == false)
            {
                ZUIResource.UIAtlasManager[loadAtlas.name] = loadAtlas;
            }

            SetSprite(loadAtlas, mSpriteName);

            mLoaderItem = null;

            DispatchEvent(new ZEvent(ZEvent.COMPLETE));
        }
        catch (Exception ex)
        {
            XLogger.Fatal(ex);
        }
    }
}
