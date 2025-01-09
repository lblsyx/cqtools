using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityLight;
using UnityLight.Events;
using UnityLight.Loggers;

namespace UnityExt.ZNGUI
{
    public class ZInputMgr
    {
        private static Vector3 mMousePosition;

        private static bool mInStage;
        private static ZEventObject mStageZEventObject;

        private static ZEventObject mMouseOverZEventObject;
        private static ZEventObject mMouseDownZEventObject;
        private static ZEventObject mMouseRightDownZEventObject;

        public static void Init(ZEventObject oZEventObject)
        {
            mInStage = true;
            mStageZEventObject = oZEventObject;
        }

        public static void Update()
        {
            bool bMoved = false;
            if (mMousePosition.Equals(Input.mousePosition) == false)
            {
                mMousePosition = Input.mousePosition;

                ZEventObject oZEventObject = ZUIManager.GetMouseHitFirstObject();

                if (null != oZEventObject)
                {
                    if (null != mMouseOverZEventObject)
                    {
                        if (mMouseOverZEventObject != oZEventObject)
                        {
                            mMouseOverZEventObject.OnMouseOut();
                            mMouseOverZEventObject = oZEventObject;
                            mMouseOverZEventObject.OnMouseOver();
                        }
                    }
                    else
                    {
                        mMouseOverZEventObject = oZEventObject;
                        mMouseOverZEventObject.OnMouseOver();
                    }
                    bMoved = true;
                    oZEventObject.OnMouseMove();
                }
                else
                {
                    if (null != mMouseOverZEventObject)
                    {
                        mMouseOverZEventObject.OnMouseOut();
                        mMouseOverZEventObject = null;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (null != mMouseDownZEventObject)
                {
                    ZEventObject oZEventObject = ZUIManager.GetMouseHitFirstObject();
                    mMouseDownZEventObject.OnMouseUp(oZEventObject == mMouseDownZEventObject);
                    mMouseDownZEventObject = null;
                }
                else
                {
                    if (mStageZEventObject != null) mStageZEventObject.OnMouseUp(true);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (null != mMouseRightDownZEventObject)
                {
                    ZEventObject oZEventObject = ZUIManager.GetMouseHitFirstObject();
                    mMouseRightDownZEventObject.OnRightMouseUp(oZEventObject == mMouseRightDownZEventObject);
                    mMouseRightDownZEventObject = null;
                }
                else
                {
                    if (mStageZEventObject != null) mStageZEventObject.OnRightMouseUp(true);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                mMouseDownZEventObject = ZUIManager.GetMouseHitFirstObject();

                if (null != mMouseDownZEventObject)
                {
                    mMouseDownZEventObject.OnMouseDown();
                }
                else
                {
                    if (mStageZEventObject != null) mStageZEventObject.OnMouseDown();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                mMouseRightDownZEventObject = ZUIManager.GetMouseHitFirstObject();

                if (null != mMouseRightDownZEventObject)
                {
                    mMouseRightDownZEventObject.OnRightMouseDown();
                }
                else
                {
                    if (mStageZEventObject != null) mStageZEventObject.OnRightMouseDown();
                }
            }

            if (mStageZEventObject != null)
            {
                Vector3 mouse = Input.mousePosition;
                if (mInStage)
                {
                    if (bMoved == false)
                    {
                        bMoved = true;
                        mStageZEventObject.OnMouseMove();
                    }
                    if (mouse.x < 0 || mouse.y < 0 || mouse.x >= Screen.width || mouse.y >= Screen.height)
                    {
                        mInStage = false;
                        mStageZEventObject.DispatchEvent(new ZEvent(ZEvent.MOUSE_LEAVE));
                        //XLogger.Debug("LeaveStage");
                    }
                }
                else
                {
                    if (mouse.x > 0 && mouse.y > 0 && mouse.x < Screen.width && mouse.y < Screen.height)
                    {
                        mInStage = true;
                        mStageZEventObject.DispatchEvent(new ZEvent(ZEvent.MOUSE_ENTER));
                        //XLogger.Debug("EnterStage");
                    }
                }
            }
        }
    }
}
