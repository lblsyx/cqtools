using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight
{
    public interface IServerMain
    {
        void Exit();
        string OnStart();
        string Update();
        string OnFinally();
    }
}
