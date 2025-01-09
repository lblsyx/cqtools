using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Pools;

namespace UnityLight.Timers
{
    public class TimerData : IPoolItem
    {
        /// <summary>
        /// 是否循环定时器
        /// </summary>
        public bool Loop;

        /// <summary>
        /// 是否移除当前定时器
        /// true表示移除
        /// false表示不移除
        /// </summary>
        public bool Remove;

        /// <summary>
        /// 循环间隔时间(毫秒)
        /// </summary>
        public float Interval;

        /// <summary>
        /// 定时器下次执行时间
        /// </summary>
        public float NextTime;

        /// <summary>
        /// 定时器执行对象
        /// </summary>
        public ITimer ITimerObj;

        /// <summary>
        /// 执行对象参数
        /// </summary>
        public object[] Args;

        /// <summary>
        /// 重置对象，由对象池调用。
        /// </summary>
        public void Reset()
        {
            Args = null;
            Remove = false;
            ITimerObj = null;
        }
    }
}
