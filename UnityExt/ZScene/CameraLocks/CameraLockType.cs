using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.ZScene.CameraLocks
{
    /// <summary>
    /// 锁头锁定枚举
    /// </summary>
    public enum CameraLockType
    {
        /// <summary>
        /// 不锁定
        /// </summary>
        Unlock = 0,
        /// <summary>
        /// 锁定旋转
        /// </summary>
        LockAngleY = 1,
        /// <summary>
        /// 锁定俯视
        /// </summary>
        LockAngleX = 2,
        /// <summary>
        /// 锁定距离
        /// </summary>
        LockDistance = 3,
        /// <summary>
        /// 锁定偏移
        /// </summary>
        LockOffset = 4
    }
}
