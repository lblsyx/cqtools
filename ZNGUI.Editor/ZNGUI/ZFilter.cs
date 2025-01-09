using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityExt.ZNGUI;
using UnityExt.ZNGUI.Interfaces;
using ZNGUI.Editor.NGUI.Scripts.Interaction;

    public class ZFilter : ZUIObject, IZFilter
    {
        public override ZUIObjectType Type { get { return ZUIObjectType.Filter; } }

        private UIFilter mUIFilter;

        public bool Enabled
        {
            get { return mUIFilter.Enabled; }
            set { mUIFilter.Enabled = value; }
        }

        public override void InitSelf()
        {
            base.InitSelf();

            mUIFilter = GetComponent<UIFilter>();
        }
    }
