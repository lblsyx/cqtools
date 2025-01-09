using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Follows;

namespace UnityExt.ZScene.Follows
{
    public class LightFollow : IFollow
    {
        private Light mLight;
        private Camera mCamera;
        private Vector3 mEulerAngles;

        public float RotationFixX;
        public float RotationOffsetY;

        public LightFollow(Light light, Camera camera)
        {
            mLight = light;
            mCamera = camera;
            mEulerAngles = mLight.transform.eulerAngles;
        }
        public object ThisObject
        {
            get { return mLight; }
        }

        public void Update(Transform trans)
        {
        }

        public void LateUpdate(Transform trans)
        {
            //mEulerAngles.x = RotationFixX;
            //mEulerAngles.y = mCamera.transform.eulerAngles.y + RotationOffsetY;

            //mLight.transform.eulerAngles = mEulerAngles;
        }
    }
}
