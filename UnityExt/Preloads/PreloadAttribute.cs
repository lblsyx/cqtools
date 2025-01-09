using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Preloads
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PreloadAttribute : Attribute
    {
        public PreloadAttribute()
        {
        }
    }
}
