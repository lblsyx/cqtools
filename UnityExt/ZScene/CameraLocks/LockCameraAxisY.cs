using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    [LockCamera((int)CameraLockType.LockAngleY)]
    public class LockCameraAxisY : ILockCamera
    {
        private float mAngleY;

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
            mAngleY = Convert.ToSingle(lockParams);
        }

        public void UpdateLock(ref LockCameraData oLockCameraData)
        {
            oLockCameraData.AngleY = mAngleY;
        }
    }
}
