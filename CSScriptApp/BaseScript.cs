using System;
using System.Collections.Generic;
using System.Text;

namespace CSScriptApp
{
    public class BaseScript : IScript
    {
        #region IScript 成员

        public virtual bool EnableRun
        {
            get { return false; }
        }

        public virtual int ThreadGroup
        {//0表示独立一个线程
            get { return 0; }
        }

        public virtual int ThreadGroupIndex
        {//线程组内排序索引
            get { return 0; }
        }

        public virtual bool RunInNewThread
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void Run(string exeDir)
        {
            throw new NotImplementedException();
        }

        public void Log(string format, params object[] args)
        {
            Program.WriteToConsole(format, args);
        }

        #endregion
    }
}
