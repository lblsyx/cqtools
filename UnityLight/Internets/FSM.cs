using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public class FSM
    {
        protected int mState;
        protected int mAdder;
        protected int mMultp;
        protected int mCount;
        protected string mFSM;

        public FSM(string str)
        {
            mFSM = str;
        }

        public int UpdateState()
        {
            //mState = ((~mState) + mAdder) * mMultp;
            //mState = mState ^ (mState >> 16);
            ++mCount;
            //Console.WriteLine("[UpdateState]{0}[{1}][{2}][{3}]", mFSM, mAdder, mMultp, mState);
            return mState;
        }

        public int Reset(int adder, int mulitper)
        {
            mCount = 0;
            mAdder = adder;
            mMultp = mulitper;
            //Console.WriteLine("[Reset]{0}[{1}][{2}]", mFSM, mAdder, mMultp);
            return UpdateState();
        }

        public void XOR(ByteArray srcArray, ByteArray desArray, int len)
        {
            //Console.WriteLine("[XOR]{0}[{1}]", mFSM, mState);
            byte[] oBytes = srcArray.Buffer;
            byte[] bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                //bytes[i] = (byte)(oBytes[srcArray.Position + i] ^ mState);
                bytes[i] = (byte)(oBytes[srcArray.Position + i]);
            }
            desArray.WriteBytes(bytes, 0, len);
        }
    }
}