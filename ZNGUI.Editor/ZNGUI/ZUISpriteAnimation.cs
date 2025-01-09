using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using UnityLight;

namespace ZNGUI.Editor.ZNGUI
{
    [RequireComponent(typeof(UISpriteAnimation))]
    public class ZUISpriteAnimation : ZUIObject, IZUISpriteAnimation
    {
        private UISpriteAnimation mUISpriteAnimation;
        public override ZUIObjectType Type { get { return ZUIObjectType.Animation; } }

        public override void InitSelf()
        {
            base.InitSelf();
            mUISpriteAnimation = GetComponent<UISpriteAnimation>();
        }

        public object CallbackArg 
        {
            get { return mUISpriteAnimation.mCallbackArg; }
            set { mUISpriteAnimation.mCallbackArg = value; } 
        }

        public Callback<object> OnCallback 
        {
            get { return mUISpriteAnimation.mCallback; }
            set { mUISpriteAnimation.mCallback = value; }
        }

        public int Fps
        {
            get { return mUISpriteAnimation.framesPerSecond; }
            set { mUISpriteAnimation.framesPerSecond = value; }
        }

        public int Frames
        {
            get { return mUISpriteAnimation.frames; }
        }

        public string NamePrefix
        {
            get { return mUISpriteAnimation.namePrefix; }
            set { mUISpriteAnimation.namePrefix = value; }
        }

        public bool Loop
        {
            get { return mUISpriteAnimation.loop; }
            set { mUISpriteAnimation.loop = value; }
        }

        public bool IsPlaying
        {
            get { return mUISpriteAnimation.isPlaying; }
        }

        public void Play()
        {
             mUISpriteAnimation.Play();
        }

        public void Pause() 
        {
            mUISpriteAnimation.Pause();
        }

        public void ResetToBeginning()
        {
            mUISpriteAnimation.ResetToBeginning();
        }

        public void RebuildSpriteList()
        {
            mUISpriteAnimation.RebuildSpriteList();
        }
    }
}
