using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Tpls
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TplSearchableAttribute : Attribute
    {
        public TplSearchableAttribute()
        {
        }
    }
}
