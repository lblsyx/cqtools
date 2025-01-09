using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Exts;
using UnityLight.Loggers;
using UnityLight.Pools;

namespace UnityLight.Timers
{
    public class TimerMgr
    {
        private static ObjectPool<TimerData> TimerDataPool = new ObjectPool<TimerData>() { ResetAtRelease = true };

        private static MinHeapFloat<TimerData> mTimerHeap = new MinHeapFloat<TimerData>();

        private static float mCurTime = 0f;

        public static TimerData OnceTimer(float fInterval, Callback cCallback)
        {
            return OnceTimer(fInterval, new CallbackTimer(cCallback));
        }

        public static TimerData OnceTimer<T>(float fInterval, Callback<T> cCallback, T obj)
        {
            return OnceTimer(fInterval, new CallbackTimer<T>(cCallback), obj);
        }

        public static TimerData OnceTimer<T1, T2>(float fInterval, Callback<T1, T2> cCallback, T1 obj1, T2 obj2)
        {
            return OnceTimer(fInterval, new CallbackTimer<T1, T2>(cCallback), obj1, obj2);
        }

        public static TimerData OnceTimer<T1, T2, T3>(float fInterval, Callback<T1, T2, T3> cCallback, T1 obj1, T2 obj2, T3 obj3)
        {
            return OnceTimer(fInterval, new CallbackTimer<T1, T2, T3>(cCallback), obj1, obj2, obj3);
        }

        public static TimerData OnceTimer<T1, T2, T3, T4>(float fInterval, Callback<T1, T2, T3, T4> cCallback, T1 obj1, T2 obj2, T3 obj3, T4 obj4)
        {
            return OnceTimer(fInterval, new CallbackTimer<T1, T2, T3, T4>(cCallback), obj1, obj2, obj3, obj4);
        }

        public static TimerData OnceTimer<T1, T2, T3, T4, T5>(float fInterval, Callback<T1, T2, T3, T4, T5> cCallback, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5)
        {
            return OnceTimer(fInterval, new CallbackTimer<T1, T2, T3, T4, T5>(cCallback), obj1, obj2, obj3, obj4, obj5);
        }

        public static TimerData OnceTimer(float fInterval, ITimer iITimer, params object[] args)
        {
            TimerData oTimerData = TimerDataPool.Acquire();

            oTimerData.Args = args;
            oTimerData.Loop = false;
            oTimerData.Remove = true;
            oTimerData.ITimerObj = iITimer;
            oTimerData.Interval = fInterval;
            oTimerData.NextTime = mCurTime + fInterval;

            mTimerHeap.HeapInsert(oTimerData, oTimerData.NextTime);

            return oTimerData;
        }

        public static TimerData LoopTimer(float fInterval, Callback cCallback, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer(cCallback), bAtOnce);
        }

        public static TimerData LoopTimer<T>(float fInterval, Callback<T> cCallback, T obj, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer<T>(cCallback), bAtOnce, obj);
        }

        public static TimerData LoopTimer<T1, T2>(float fInterval, Callback<T1, T2> cCallback, T1 obj1, T2 obj2, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer<T1, T2>(cCallback), bAtOnce, obj1, obj2);
        }

        public static TimerData LoopTimer<T1, T2, T3>(float fInterval, Callback<T1, T2, T3> cCallback, T1 obj1, T2 obj2, T3 obj3, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer<T1, T2, T3>(cCallback), bAtOnce, obj1, obj2, obj3);
        }

        public static TimerData LoopTimer<T1, T2, T3, T4>(float fInterval, Callback<T1, T2, T3, T4> cCallback, T1 obj1, T2 obj2, T3 obj3, T4 obj4, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer<T1, T2, T3, T4>(cCallback), bAtOnce, obj1, obj2, obj3, obj4);
        }

        public static TimerData LoopTimer<T1, T2, T3, T4, T5>(float fInterval, Callback<T1, T2, T3, T4, T5> cCallback, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, bool bAtOnce = false)
        {
            return LoopTimer(fInterval, new CallbackTimer<T1, T2, T3, T4, T5>(cCallback), bAtOnce, obj1, obj2, obj3, obj4, obj5);
        }

        public static TimerData LoopTimer(float fInterval, ITimer iITimer, bool bAtOnce, params object[] args)
        {
            TimerData oTimerData = TimerDataPool.Acquire();

            oTimerData.Args = args;
            oTimerData.Loop = true;
            oTimerData.Remove = false;
            oTimerData.ITimerObj = iITimer;
            oTimerData.Interval = fInterval;

            if (bAtOnce)
            {
                oTimerData.NextTime = 0;
            }
            else
            {
                oTimerData.NextTime = mCurTime + fInterval;
            }

            mTimerHeap.HeapInsert(oTimerData, oTimerData.NextTime);

            return oTimerData;
        }

        public static void Update(float deltaTime)
        {
            mCurTime += deltaTime;
            //XLogger.DebugFormat("已运行：{0}秒", mCurTime);
            List<TimerData> list = new List<TimerData>();
            TimerData oTimerData = mTimerHeap.GetMinimum();

            while (oTimerData != null && oTimerData.NextTime <= mCurTime)
            {
                try
                {
                    oTimerData.ITimerObj.Execute(oTimerData);
                }
                catch (Exception ex)
                {
                    XLogger.ErrorFormat("定时器执行失败！ErrMsg:{0}\r\nStackTrace:{1}", ex.Message, ex.StackTrace);
                }

                if (oTimerData.Loop && oTimerData.Remove == false)
                {
                    oTimerData.NextTime = mCurTime + oTimerData.Interval;

                    if (oTimerData.Interval <= 0)
                    {
                        list.Add(oTimerData);
                        mTimerHeap.ExtractMin();
                        oTimerData = mTimerHeap.GetMinimum();
                    }
                    else
                    {
                        mTimerHeap.ChangeKey(0, oTimerData.NextTime);
                        oTimerData = mTimerHeap.GetMinimum();
                    }
                    
                }
                else
                {
                    mTimerHeap.ExtractMin();
                    TimerDataPool.Release(oTimerData);
                    oTimerData = mTimerHeap.GetMinimum();
                }
            }

            foreach (var item in list)
            {
                mTimerHeap.HeapInsert(item, item.NextTime);
            }
        }
    }
}
