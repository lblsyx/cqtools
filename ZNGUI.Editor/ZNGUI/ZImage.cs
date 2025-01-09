using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Loaders;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight;
using UnityLight.Events;
using UnityLight.Loggers;

[RequireComponent(typeof(UITexture))]
public class ZImage : ZUIObject, IZImage
{
    public override ZUIObjectType Type { get { return ZUIObjectType.Image; } }

    private bool mCacheTexture;
    private string mTextureName;
    private UITexture mUITexture;
    private LoaderItem mLoaderItem;

    private ZImageSource mZImageSource;
    //private List<Transform> mSourceList;
    private Transform mSourceRootTrans;
    private float mRotationY;
    private bool mMouseDownImg = false;
    private Camera mCamera;

    public override bool Enable
    {
        get { return mUITexture.enabled; }
        set { mUITexture.enabled = value; }
    }

    public bool EnableRotateY { get; set; }
    public float RotateSpeedY { get; set; }
    public GameObject SourceRoot { get { if (mZImageSource != null) return mZImageSource.gameObject; return null; } }

    public override void Initialize()
    {
        RotateSpeedY = -10.0f;
        mTextureName = string.Empty;
        mUITexture = GetComponent<UITexture>();
        if (mUITexture != null && mUITexture.mainTexture != null)
        {
            mTextureName = mUITexture.mainTexture.name;
        }

        mZImageSource = GetComponentInChildren<ZImageSource>();
        if (mZImageSource != null) mSourceRootTrans = mZImageSource.Trans;
        else mSourceRootTrans = null;

        mCamera = GetComponentInChildren<Camera>();

        AddEventListener(ZEvent.MOUSE_UP, OnMouseUpHandler, false);
        AddEventListener(ZEvent.MOUSE_DOWN, OnMouseDownHandler, false);
    }

    private void OnMouseUpHandler(ZEvent obj)
    {
        mMouseDownImg = false;
    }

    private void OnMouseDownHandler(ZEvent obj)
    {
        mMouseDownImg = true;
    }

    public void SetTexture(Texture oTexture, bool bCache)
    {
        mCacheTexture = bCache;
        if (mUITexture != null) mUITexture.mainTexture = oTexture;
        mTextureName = oTexture == null ? string.Empty : oTexture.name;
        if (mCacheTexture && oTexture != null && ZUIResource.UITextureManager.ContainsKey(mTextureName) == false)
        {
            ZUIResource.UITextureManager[oTexture.name] = oTexture;
        }
        DispatchEvent(new ZEvent(ZEvent.CHANGED));
    }

    public void SetTexture(string sTextureName, bool bCache)
    {
        if (ZUIResource.UITextureManager.ContainsKey(sTextureName))
        {
            SetTexture(ZUIResource.UITextureManager[sTextureName], false);
        }
        else
        {
            mCacheTexture = bCache;
            mTextureName = sTextureName;
            if (mLoaderItem != null) ZUIManager.UIDownloadQueue.Cancel(mLoaderItem);
            var path = ZUIResource.GetTexturePath(sTextureName);
            mLoaderItem = ZUIManager.UIDownloadQueue.Load(path, OnLoadDoneHandler);
        }
    }

    public int Width
    {
        get { if (mUITexture != null) return mUITexture.width; return 0; }
        set { if (mUITexture != null) mUITexture.width = value; }
    }

    public int Height
    {
        get { if (mUITexture != null) return mUITexture.height; return 0; }
        set { if (mUITexture != null) mUITexture.height = value; }
    }

    public void ResetSourceParent()
    {
        if (mCamera != null) mCamera.transform.parent = null;
        if (mSourceRootTrans != null) mSourceRootTrans.parent = null;
    }

    public void ResetRotationY()
    {
        mRotationY = 0;

        mSourceRootTrans.rotation = Quaternion.Euler(0, mRotationY, 0);
    }

    public void MakePixelPerfect()
    {
        if (mUITexture != null) mUITexture.MakePixelPerfect();
    }

    private void OnLoadDoneHandler(URLLoader loader, bool success, string errMsg)
    {
        if (success == false) return;
        AssetBundle bundle = loader.DataBundle;

        SetTexture(bundle.mainAsset as Texture, mCacheTexture);

        mLoaderItem = null;

        DispatchEvent(new ZEvent(ZEvent.COMPLETE));
    }

    public override void Update()
    {
        if (mCamera != null) mCamera.aspect = 1;
        if (mMouseDownImg && EnableRotateY && Enable && mSourceRootTrans != null)
        {
            //SourceRootTrans.rotation
            mRotationY += Input.GetAxis("Mouse X") * RotateSpeedY;

            mSourceRootTrans.rotation = Quaternion.Euler(0, mRotationY, 0);
        }
    }

    public override void Dispose()
    {
        RemoveEventListener(ZEvent.MOUSE_UP, OnMouseUpHandler, false);
        RemoveEventListener(ZEvent.MOUSE_DOWN, OnMouseDownHandler, false);

        base.Dispose();
    }
}
