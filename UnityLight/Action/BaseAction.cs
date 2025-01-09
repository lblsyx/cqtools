/*----------------------------------------------------------------
// Copyright (C) 2015 DefaultCompany
//
// 模块名：Action
// 创建者：Tang WenBin
// 修改者列表：
// 创建日期：2015年10月08日 17时42分
// 模块描述：
//----------------------------------------------------------------*/
using System.Collections;
using UnityLight.Pools;

namespace UnityLight.Action
{
    public class BaseAction : IPoolItem
    {
        // 行为是否准备好
        public bool IsPrepared { get; set; }
        // 行为是否完成
        public bool IsFinished { get; set; }
        // 行为队列
        public ActionQueue ActionQueue { get; internal set; }

        /// <summary>
        /// 连接两个Action，释放参数act。
        /// </summary>
        /// <param name="act">新的Action</param>
        /// <returns>true时释放参数act对象，并且不加入Action队列;false则反之。</returns>
        public virtual bool Connect(BaseAction act)
        {
            return false;
        }

        /// <summary>
        /// 指示是否可替换队列中当前的Action对象为参数act对象。
        /// </summary>
        /// <param name="act">新的Action</param>
        /// <returns>true时替换当前的Action对象为参数act对象，并且释放掉当前的Action对象。</returns>
        public virtual bool Replace(BaseAction act)
        {
            return false;
        }

        /// <summary>
        /// 准备阶段, 仅执行一次
        /// </summary>
        public virtual void Prepare()
        {
        }

        /// <summary>
        /// 执行中，直接到设置isFinished为true后将执行Exit()方法完成并释放当前Action对象。
        /// </summary>
        public virtual void Execute()
        {
            IsFinished = true;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        public virtual void ExecuteAtOnce()
        {
            IsFinished = true;
        }

        /// <summary>
        /// 完成阶段
        /// </summary>
        public virtual void Finish()
        {
        }

        public virtual void Reset()
        {
            IsPrepared = false;
            IsFinished = false;
            ActionQueue = null;
        }
    }
}
