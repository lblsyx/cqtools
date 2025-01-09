using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Timers
{
    public interface ITimer
    {
        /// <summary>
        /// 定时执行方法。
        /// </summary>
        /// <param name="oTimerData">定时器数据。</param>
        void Execute(TimerData oTimerData);
    }
}
