using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    public interface ILockCamera
    {
        bool BeforeUpdate { get; }

        bool AfterUpdate { get; }

        void ParseParams(string lockParams);

        void UpdateLock(ref LockCameraData oLockCameraData);
    }
}
