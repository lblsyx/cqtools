using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityExt.Follows;
using UnityExt.ZScene;
using UnityExt.ZScene.CameraLocks;

namespace UnityExt.ZScene.Follows
{
    public class CameraFollow : IFollow
    {
        private Camera mCamera;
        private Transform mCameraTrans;

        private Vector3 distanceDir = Vector3.zero;

        private Quaternion orginRotation;

        private Quaternion targetRotation;

        private Vector3 targetOffset;

        private LockCameraData mLockCameraData;

        private float mTargetDistLerp;

        private bool mIsMouseDown = false;

        public bool EnableRotation = true;

        public bool EnableAreaRotation = false;

        public CameraFollow(Camera camera)
        {
            mCamera = camera;
            mCameraTrans = camera.transform;
            orginRotation = mCameraTrans.rotation;

            mLockCameraData = new LockCameraData();
            mLockCameraData.AngleX = 40f;
            mLockCameraData.AngleY = 0f;
            mLockCameraData.Distance = ZSceneMgr.DefaultCameraDistance;

            targetOffset = Vector3.zero;
            targetOffset.y = 0.8f;
        }

        public object ThisObject
        {
            get { return mCamera; }
        }

        public void Update(Transform trans)
        {
        }

        public void ResetDistance()
        {
            mLockCameraData.Distance = ZSceneMgr.MaxCameraDistance;
        }

        private float targetAngle = 0;
        private float sign = 0;
        private Vector3 targetScaleOffset = new Vector3(0, 1.8f, 0);
        RaycastHit hit;

        public void LateUpdate(Transform trans)
        {
            if (Input.GetMouseButtonDown(1))
            {
                mIsMouseDown = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                mIsMouseDown = false;
            }

            if (Input.GetMouseButton(1) && EnableRotation)
            {
                float mouseX = Input.GetAxis("Mouse X") * ZSceneMgr.AngleSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * ZSceneMgr.AngleSpeed;
                mLockCameraData.AngleY += mouseX;
                mLockCameraData.AngleX -= mouseY;
                if (mouseX < 0) sign = -1;
                else if (mouseX == 0) sign = 0;
                else if (mouseX > 0) sign = 1;
            }

            mLockCameraData.Distance -= Input.GetAxis("Mouse ScrollWheel") * ZSceneMgr.AngleSpeed;
            mLockCameraData.Distance = Mathf.Clamp(mLockCameraData.Distance, ZSceneMgr.MinCameraDistance, ZSceneMgr.MaxCameraDistance);
            mLockCameraData.AngleX = Mathf.Clamp(mLockCameraData.AngleX, ZSceneMgr.MinCameraAngleX, ZSceneMgr.MaxCameraAngleX);

            //mLockCameraData.AngleY = mLockCameraData.AngleY % 360;
            //if (mLockCameraData.AngleY < 0) mLockCameraData.AngleY = 360 + mLockCameraData.AngleY;

            if (EnableAreaRotation)
            {
                if (mIsMouseDown == true)
                {
                    if (sign == 1)
                    {
                        if (mLockCameraData.AngleY >= 0)
                        {
                            targetAngle = mLockCameraData.AngleY + (45 - mLockCameraData.AngleY % 45);
                        }
                        else
                        {
                            targetAngle = mLockCameraData.AngleY - mLockCameraData.AngleY % 45;
                        }
                    }
                    else if (sign == -1)
                    {
                        if (mLockCameraData.AngleY >= 0)
                        {
                            targetAngle = mLockCameraData.AngleY - mLockCameraData.AngleY % 45;
                        }
                        else
                        {
                            targetAngle = mLockCameraData.AngleY - (45 + mLockCameraData.AngleY % 45);
                        }
                    }
                }
                else
                {
                    mLockCameraData.AngleY = Mathf.Lerp(mLockCameraData.AngleY, targetAngle, Time.deltaTime * 10);   
                }
            }

            foreach (var item in ZSceneMgr.CurrentBeforeUpdateLockList)
            {
                item.UpdateLock(ref mLockCameraData);
            }

            if (ZSceneMgr.IsCameraAutoZoom)
            {
                Vector3 orginscale = trans.position + targetScaleOffset;
                Vector3 camDir = (mCameraTrans.position - orginscale).normalized;
                float camorginDist = Vector3.Distance(mCameraTrans.position, orginscale);
                Ray oRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(orginscale, camDir, out hit, ZSceneMgr.MaxCameraDistance, ~ZSceneMgr.CameraRayLayer))
                {
                    float distance = 0;
                    if (hit.distance > mLockCameraData.Distance)
                    {
                        distance = mLockCameraData.Distance;
                    }
                    else
                    {
                        distance = Mathf.Clamp(hit.distance, 0.5f, ZSceneMgr.MaxCameraDistance);
                    }
                    float distLerp = (distance - ZSceneMgr.MinCameraDistance) / (ZSceneMgr.MaxCameraDistance - ZSceneMgr.MinCameraDistance);
                    mTargetDistLerp = Mathf.Lerp(mTargetDistLerp, distLerp, Time.deltaTime * 10);
                }
                else
                {
                    float distLerp = (mLockCameraData.Distance - ZSceneMgr.MinCameraDistance) / (ZSceneMgr.MaxCameraDistance - ZSceneMgr.MinCameraDistance);
                    mTargetDistLerp = Mathf.Lerp(mTargetDistLerp, distLerp, Time.deltaTime * ZSceneMgr.DistSpeed);
                }
            }
            else
            {
                float distLerp = (mLockCameraData.Distance - ZSceneMgr.MinCameraDistance) / (ZSceneMgr.MaxCameraDistance - ZSceneMgr.MinCameraDistance);
                mTargetDistLerp = Mathf.Lerp(mTargetDistLerp, distLerp, Time.deltaTime * ZSceneMgr.DistSpeed);
            }

            //float angleX = (ZSceneMgr.MinCameraAngleX - ZSceneMgr.MaxCameraAngleX) * mLockCameraData.AngleX * mTargetDistLerp /
            //    -ZSceneMgr.MaxCameraAngleX + ZSceneMgr.MinCameraAngleX;
            float angleX = mLockCameraData.AngleX;

            targetRotation = orginRotation * Quaternion.Euler(angleX, mLockCameraData.AngleY, 0);

            distanceDir.z = -((ZSceneMgr.MaxCameraDistance - ZSceneMgr.MinCameraDistance) * mTargetDistLerp + ZSceneMgr.MinCameraDistance);

            Vector3 orgin = trans.position + targetOffset;

            Vector3 camTargetPos = targetRotation * distanceDir + orgin;

            mLockCameraData.Position = camTargetPos;

            foreach (var item in ZSceneMgr.CurrentAfterUpdateLockList)
            {
                item.UpdateLock(ref mLockCameraData);
            }

            mCameraTrans.position = mLockCameraData.Position;
            mCameraTrans.rotation = targetRotation;
        }
    }
}
