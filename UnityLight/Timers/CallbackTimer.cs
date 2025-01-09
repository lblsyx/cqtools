using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Timers
{
    public class CallbackTimer : ITimer
    {
        private Callback mCallback;
        public CallbackTimer(Callback cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback();
        }
    }
    
    public class CallbackTimer<T> : ITimer
    {
        private Callback<T> mCallback;
        public CallbackTimer(Callback<T> cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback((T)oTimerData.Args[0]);
        }
    }

    public class CallbackTimer<T1, T2> : ITimer
    {
        private Callback<T1, T2> mCallback;
        public CallbackTimer(Callback<T1, T2> cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback((T1)oTimerData.Args[0], (T2)oTimerData.Args[1]);
        }
    }

    public class CallbackTimer<T1, T2, T3> : ITimer
    {
        private Callback<T1, T2, T3> mCallback;
        public CallbackTimer(Callback<T1, T2, T3> cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback((T1)oTimerData.Args[0], (T2)oTimerData.Args[1], (T3)oTimerData.Args[2]);
        }
    }

    public class CallbackTimer<T1, T2, T3, T4> : ITimer
    {
        private Callback<T1, T2, T3, T4> mCallback;
        public CallbackTimer(Callback<T1, T2, T3, T4> cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback((T1)oTimerData.Args[0], (T2)oTimerData.Args[1], (T3)oTimerData.Args[2], (T4)oTimerData.Args[3]);
        }
    }

    public class CallbackTimer<T1, T2, T3, T4, T5> : ITimer
    {
        private Callback<T1, T2, T3, T4, T5> mCallback;
        public CallbackTimer(Callback<T1, T2, T3, T4, T5> cCallback)
        {
            mCallback = cCallback;
        }

        public void Execute(TimerData oTimerData)
        {
            mCallback((T1)oTimerData.Args[0], (T2)oTimerData.Args[1], (T3)oTimerData.Args[2], (T4)oTimerData.Args[3], (T5)oTimerData.Args[4]);
        }
    }
}
