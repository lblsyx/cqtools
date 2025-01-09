using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace UnityLight.Utils
{
    public class Tick
    {
        private static long StopwatchFrequencyMilliseconds = Stopwatch.Frequency / 1000;

        /// <summary>
        /// 获取计时器机制中的当前最小时间单位数。
        /// </summary>
        /// <returns>一个长整型，表示基础计时器机制中的计时周期计数器值。</returns>
        public static long GetTicks()
        {
            return Stopwatch.GetTimestamp() / StopwatchFrequencyMilliseconds;
        }
    }
}
