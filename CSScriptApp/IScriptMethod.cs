using System;
using System.Collections.Generic;
using System.Text;

namespace CSScriptApp
{
    public interface IScriptMethod
    {
        object Do(params object[] args);
    }
}
