using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.ZScene.CameraLocks
{
    public class LockCameraData
    {
        /// <summary>
        /// X轴角度
        /// </summary>
        public float AngleX;
        /// <summary>
        /// Y轴角度
        /// </summary>
        public float AngleY;
        /// <summary>
        /// 坐标轴
        /// </summary>
        public Vector3 Position = Vector3.zero;
        /// <summary>
        /// 与镜头距离
        /// </summary>
        public float Distance;
    }
}
