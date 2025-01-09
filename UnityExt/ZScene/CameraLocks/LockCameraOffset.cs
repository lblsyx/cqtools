using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    [LockCamera((int)CameraLockType.LockOffset)]
    public class LockCameraOffset : ILockCamera
    {
        public bool BeforeUpdate
        {
            get { return false; }
        }

        public bool AfterUpdate
        {
            get { return true; }
        }

        private Vector3 mOffsetPos;

        public void ParseParams(string lockParams)
        {
            mOffsetPos = Vector3.zero;
            string[] array = lockParams.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length == 3)
            {
                mOffsetPos.x = Convert.ToSingle(array[0]);
                mOffsetPos.y = Convert.ToSingle(array[1]);
                mOffsetPos.z = Convert.ToSingle(array[2]);
            }
        }

        public void UpdateLock(ref LockCameraData oLockCameraData)
        {
            oLockCameraData.Position += mOffsetPos;
        }
    }
}
