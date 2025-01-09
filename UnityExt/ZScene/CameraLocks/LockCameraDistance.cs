using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    [LockCamera((int)CameraLockType.LockDistance)]
    public class LockCameraDistance : ILockCamera
    {
        private float mDistance;

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
            mDistance = Convert.ToSingle(lockParams);
        }

        public void UpdateLock(ref LockCameraData oLockCameraData)
        {
            oLockCameraData.Distance = mDistance;
        }
    }
}
