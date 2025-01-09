using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;
using UnityLight.Events;
using UnityLight.Exts;

namespace UnityExt.ZNGUI
{
    public class ZUIObject : ZEventObject, IZUIObject
    {
        public int IntTag;

        public string StrTag;

        public GameObject ObjTag;

        #region IZUIObject接口实现

        public int TagInt
        {
            get { return IntTag; }
        }

        public string TagStr
        {
            get { return StrTag; }
        }

        public GameObject TagObj
        {
            get { return ObjTag; }
        }

        public virtual ZUIObjectType Type
        {
            get { return ZUIObjectType.Unknow; }
        }

        public Transform Trans
        {
            get { return gameObject.transform; }
        }

        public virtual void UpdateUI()
        {
        }

        #endregion
    }
}
