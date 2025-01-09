using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    [LockCamera((int)CameraLockType.LockAngleX)]
    public class LockCameraAxisX : ILockCamera
    {
        private float mAngleX;

        public bool BeforeUpdate
        {
            get { return true; }
        }

        public bool AfterUpdate
        {
            get { return false; }
        }

        public void ParseParams(string lockParams)
        {
            mAngleX = Convert.ToSingle(lockParams);
        }

        public void UpdateLock(ref LockCameraData oLockCameraData)
        {
            oLockCameraData.AngleX = mAngleX;
        }
    }
}
