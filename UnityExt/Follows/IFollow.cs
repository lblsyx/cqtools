using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityExt.Follows
{
    public interface IFollow
    {
        object ThisObject { get; }
        void Update(Transform trans);
        void LateUpdate(Transform trans);
    }
}
