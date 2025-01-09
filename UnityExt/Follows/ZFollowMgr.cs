using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.Follows
{
    public class ZFollowMgr
    {
        private static Transform mTransform;
        private static Dictionary<object, IFollow> mIFollowList = new Dictionary<object, IFollow>();

        public static void SetMain(Transform oTransform)
        {
            mTransform = oTransform;
        }

        public static void AddFollow(IFollow iIFollow)
        {
            if (iIFollow == null) return;
            if (mIFollowList.ContainsKey(iIFollow.ThisObject) == false)
            {
                mIFollowList.Add(iIFollow.ThisObject, iIFollow);
            }
        }

        public static void DelFollow(object obj)
        {
            if (obj == null) return;
            if (mIFollowList.ContainsKey(obj))
            {
                mIFollowList.Remove(obj);
            }
        }

        public static void DelFollow(IFollow iIFollow)
        {
            if (iIFollow == null) return;
            DelFollow(iIFollow.ThisObject);
        }

        public static void Update()
        {
            if (mTransform == null) return;

            foreach (var followItem in mIFollowList)
            {
                followItem.Value.Update(mTransform);
            }
        }

        public static void LateUpdate()
        {
            if (mTransform == null) return;

            foreach (var followItem in mIFollowList)
            {
                followItem.Value.LateUpdate(mTransform);
            }
        }
    }
}
