using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Pools
{
    /// <summary>
    /// 对象池中的对象接口。
    /// </summary>
    public interface IPoolItem
    {
        /// <summary>
        /// 重置对象。
        /// </summary>
        void Reset();
    }
}
