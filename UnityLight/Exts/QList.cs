/*----------------------------------------------------------------
// Copyright (C) 2015 DefaultCompany
//
// 模块名：QList
// 创建者：Tang WenBin
// 修改者列表：
// 创建日期：2015年09月11日 16时48分
// 模块描述：带有队列方法的List方法
//----------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;

namespace UnityLight.Exts
{
    public class QList<T> : List<T>
    {

        public QList() : base() { }

        public QList(IEnumerable<T> collection) : base(collection) { }

        public QList(int capacity) : base(capacity) { }


        public T Dequeue()
        {
            if (this.Count > 0)
            {
                T t = this[0];
                this.RemoveAt(0);
                return t;
            }
            return default(T);
        }


        public void Enqueue(T item)
        {
            this.Add(item);
        }

    }
}