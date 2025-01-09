/*----------------------------------------------------------------
// Copyright (C) 2015 DefaultCompany
//
// 模块名：ActionQueue
// 创建者：Tang WenBin
// 修改者列表：
// 创建日期：2015年10月08日 17时45分
// 模块描述：
//----------------------------------------------------------------*/

using System.Collections;
using System;
using UnityLight.Exts;
using UnityLight.Loggers;

namespace UnityLight.Action
{
    public class ActionQueue : IDisposable
    {
        // 行为队列
        private QList<BaseAction> mActionQueue;
        // 是否多行为任务
        private bool mIsMultiple;


        public ActionQueue(bool multiple)
        {
            mActionQueue = new QList<BaseAction>();
            mIsMultiple = multiple;
        }


        /// <summary>
        /// 当前行为数量
        /// </summary>
        /// <returns></returns>
        public int ActionLenght
        {
            get { return mActionQueue.Count; }
        }


        /// <summary>
        /// 清除所有Action
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            while (mActionQueue != null && mActionQueue.Count > 0)
            {
                BaseAction a = mActionQueue.Dequeue() as BaseAction;
                if (a.IsPrepared == false) a.Prepare();
                a.Finish();
            }
        }


        /// <summary>
        /// 添加Action对象
        /// </summary>
        public void AddAction(BaseAction a)
        {
            if (a == null) return;
            a.ActionQueue = this;
            for (int i = 0; i < mActionQueue.Count; i++)
            {
                BaseAction at = mActionQueue[i];
                if (at == a) return;
                if (at.Connect(a)) return;
                if (at.Replace(a))
                {
                    if (a.IsPrepared && a.IsFinished == false)
                    {
                        a.IsPrepared = true;
                        a.Prepare();
                    }
                    mActionQueue[i] = a;
                    return;
                }
            }
            mActionQueue.Enqueue(a);
        }

        public void ReplaceAction(BaseAction a)
        {
            if (a == null) return;
            a.ActionQueue = this;
            for (int i = 0; i < mActionQueue.Count; i++)
            {
                BaseAction at = mActionQueue[i];
                if (at == a) return;
                if (at.Replace(a))
                {
                    if (a.IsPrepared && a.IsFinished == false)
                    {
                        a.IsPrepared = true;
                        a.Prepare();
                    }
                    mActionQueue[i] = a;
                    return;
                }
            }
        }

        public BaseAction[] ToArray()
        {
            return mActionQueue.ToArray();
        }

        public BaseAction this[int index]
        {
            get
            {
                if (index > -1 && index < mActionQueue.Count)
                {
                    return mActionQueue[index];
                }
                return null;
            }
        }

        public BaseAction GetCurrentAction()
        {
            if (mActionQueue.Count > 0)
            {
                return mActionQueue[0];
            }
            return null;
        }

        /// <summary>
        /// 获取下一个动作
        /// </summary>
        /// <returns>返回行为</returns>
        public BaseAction GetNextAction()
        {
            if (mActionQueue.Count > 1)
            {
                return mActionQueue[1];
            }
            return null;
        }

        /// <summary>
        /// 删除下一个动作
        /// </summary>
        /// <returns></returns>
        public BaseAction DelNextAction()
        {
            if (mActionQueue.Count > 1)
            {
                BaseAction oBaseAction = mActionQueue[1];
                mActionQueue.RemoveAt(1);
                return oBaseAction;
            }
            return null;
        }

        /// <summary>
        /// 行为更新
        /// </summary>
        public void Update()
        {
            if (mIsMultiple) ExecuteMulti();
            else ExecuteLine();
        }


        protected void ExecuteLine()
        {
            //XLogger.Debug("ActionQueue update...");
            if (mActionQueue.Count == 0) return;

            if (mActionQueue[0].IsPrepared == false)
            {
                mActionQueue[0].IsPrepared = true;
                mActionQueue[0].Prepare();
            }

            if (mActionQueue[0].IsFinished)
            {
                mActionQueue[0].Finish();
                mActionQueue.RemoveAt(0);
            }
            else
            {
                mActionQueue[0].Execute();
                if (mActionQueue[0].IsFinished)
                {
                    mActionQueue[0].Finish();
                    mActionQueue.RemoveAt(0);
                }
            }
        }


        protected int i = 0;

        protected void ExecuteMulti()
        {
            for (i = 0; i < mActionQueue.Count; i++)
            {
                if (mActionQueue[i].IsPrepared == false)
                {
                    mActionQueue[i].IsPrepared = true;
                    mActionQueue[i].Prepare();
                }

                if (mActionQueue[i].IsFinished)
                {
                    mActionQueue[i].Finish();
                    mActionQueue.RemoveAt(i);
                    i--;
                }
                else
                {
                    mActionQueue[i].Execute();
                    if (mActionQueue[i].IsFinished)
                    {
                        mActionQueue[i].Finish();
                        mActionQueue.RemoveAt(i);
                        i--;
                    }
                }
            }
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Clear();
            mActionQueue = null;
        }
    }
}
