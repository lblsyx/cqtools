using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.ZScene.CameraLocks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LockCameraAttribute : Attribute
    {
        public int LockType { get; set; }

        public LockCameraAttribute(int type)
        {
            LockType = type;
        }
    }
}
