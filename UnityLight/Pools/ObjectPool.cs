using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using UnityLight.Loggers;

namespace UnityLight.Pools
{
    public delegate T CreateObject<T>();

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : IPoolItem, new()
    {
        private Queue<T> m_pool;

        private bool m_inited;

        private int m_initSize;

        private HybridDictionary m_usings;

        public object SyncRoot { get; protected set; }

        public CreateObject<T> CreateMethod;

        public bool ResetAtRelease { get; set; }

        public ObjectPool(CreateObject<T> method = null, int initSize = 1000)
        {
            m_initSize = initSize;

            CreateMethod = method;

            m_pool = new Queue<T>(m_initSize);

            m_usings = new HybridDictionary();

            SyncRoot = new object();
        }

        public void InitPool()
        {
            if (m_inited) return;
            m_inited = true;

            lock (SyncRoot)
            {
                for (int i = 0; i < m_initSize; i++)
                {
                    m_pool.Enqueue(CreateItem());
                }
            }
        }

        private T CreateItem()
        {
            if (CreateMethod != null)
            {
                return CreateMethod();
            }

            return new T();
        }

        public void SetCreateMethod(CreateObject<T> method)
        {
            CreateMethod = method;
        }

        public T Acquire()
        {
            lock (SyncRoot)
            {
                T o;

                if (m_pool.Count > 0)
                {
                    o = m_pool.Dequeue();
                }
                else
                {
                    o = CreateItem();
                }

                if (ResetAtRelease == false)
                {
                    ((IPoolItem)o).Reset();
                }

                m_usings.Add(o, o);

                return o;
            }
        }

        public void Release(T o)
        {
            if (o == null) return;

            lock (SyncRoot)
            {
                if (m_pool.Contains(o))
                {
                    XLogger.ErrorFormat("释放回对象池失败!");
                    return;
                }

                m_usings.Remove(o);

                if (ResetAtRelease) o.Reset();

                m_pool.Enqueue(o);
            }
        }

        public int Count
        {
            get
            {
                lock (SyncRoot)
                {
                    return m_pool.Count;
                }
            }
        }
    }
}
