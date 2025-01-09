using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityLight.Exts
{
    /// <summary>  
    /// 队列元素包装类  
    /// </summary>  
    /// <typeparam name="T">实际元素类型</typeparam>  
    public class QueueElementInt<T>
    {
        /// <summary>  
        /// Key值  
        /// </summary>  
        public int KeyValue { get; internal set; }
        /// <summary>  
        /// 实际对象  
        /// </summary>  
        public T Element { get; private set; }
        public QueueElementInt(T Item, int KeyVal)
        {
            KeyValue = KeyVal;
            Element = Item;
        }
    }

    /// <summary>  
    /// 最小优先级队列  
    /// </summary>  
    /// <typeparam name="T"></typeparam>  
    public class MinHeapInt<T>
    {
        /// <summary>  
        /// 队列元素存放，采用List实现.  
        /// </summary>  
        private List<QueueElementInt<T>> _queueValues = new List<QueueElementInt<T>>();
        /// <summary>  
        /// 队列元素数目  
        /// </summary>  
        public int Count { get { return _queueValues.Count; } }
        /// <summary>  
        /// 获取队列Key值最小的元素  
        /// </summary>  
        /// <returns></returns>  
        public T GetMinimum()
        {
            if (_queueValues.Count <= 0)
            {
                return default(T);
            }
            return _queueValues[0].Element;
        }
        /// <summary>  
        /// 从队列中取出Key值最小的元素  
        /// </summary>  
        /// <returns></returns>  
        public T ExtractMin()
        {
            if (_queueValues.Count <= 0)
            {
                return default(T);
                //throw new Exception("队列为空");
            }
            T theMin = _queueValues[0].Element;
            int theTail = Count - 1;
            _queueValues[0] = _queueValues[theTail];
            _queueValues.RemoveAt(theTail);
            MinHeapify(0);
            return theMin;
        }
        /// <summary>  
        /// 整理堆元素，保持最小堆特性，这个函数跟DownAdjust功能相同  
        /// </summary>  
        /// <param name="i"></param>  
        public void MinHeapify(int i)
        {
            int HeapSize = Count;
            int theL = HeapL(i);
            int theR = HeapR(i);
            int theLeast = i;
            if (theL < HeapSize && _queueValues[theL].KeyValue < _queueValues[theLeast].KeyValue)
            {
                theLeast = theL;
            }
            if (theR < HeapSize && _queueValues[theR].KeyValue < _queueValues[theLeast].KeyValue)
            {
                theLeast = theR;
            }
            if (theLeast != i)
            {
                SwapElement(i, theLeast);
                MinHeapify(theLeast);
            }
        }
        /// <summary>  
        /// 改变元素key值  
        /// </summary>  
        /// <param name="SelectFunc"></param>  
        /// <param name="NewKey"></param>  
        public void ChangeKey(Func<T, bool> SelectFunc, int NewKey)
        {
            int theIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                if (SelectFunc(_queueValues[i].Element) == true)
                {
                    theIndex = i;
                    break;
                }
            }

            if (theIndex < 0) return;

            if (_queueValues[theIndex].KeyValue < NewKey)
            {
                _queueValues[theIndex].KeyValue = NewKey;
                DownAdjust(theIndex);
                return;
            }

            if (_queueValues[theIndex].KeyValue > NewKey)
            {
                _queueValues[theIndex].KeyValue = NewKey;
                UpAdjust(theIndex);
                return;
            }
        }
        /// <summary>  
        /// 沿树根方向整理元素，保持最小堆特性  
        /// </summary>  
        /// <param name="i"></param>  
        private void UpAdjust(int i)
        {
            int theIndex = i;
            int thePIndex = HeapP(theIndex);
            while (thePIndex >= 0 && _queueValues[theIndex].KeyValue < _queueValues[thePIndex].KeyValue)
            {
                SwapElement(thePIndex, theIndex);
                theIndex = thePIndex;
                thePIndex = HeapP(theIndex);
            }
        }
        /// <summary>  
        /// 沿树叶方向整理元素，保持最小堆特性  
        /// </summary>  
        /// <param name="i"></param>  
        private void DownAdjust(int i)
        {
            int HeapSize = Count;
            int theL = HeapL(i);
            int theR = HeapR(i);
            int theLeast = i;
            if (theL < HeapSize && _queueValues[theL].KeyValue < _queueValues[theLeast].KeyValue)
            {
                theLeast = theL;
            }
            if (theR < HeapSize && _queueValues[theR].KeyValue < _queueValues[theLeast].KeyValue)
            {
                theLeast = theR;
            }
            if (theLeast != i)
            {
                SwapElement(i, theLeast);
                DownAdjust(theLeast);
            }
        }
        /// <summary>  
        /// 改变元素key值  
        /// </summary>  
        /// <param name="i"></param>  
        /// <param name="NewKey"></param>  
        public void ChangeKey(int i, int NewKey)
        {
            int theIndex = i;
            if (_queueValues[theIndex].KeyValue > NewKey)
            {
                _queueValues[theIndex].KeyValue = NewKey;
                UpAdjust(theIndex);
                return;
            }
            if (_queueValues[theIndex].KeyValue < NewKey)
            {
                _queueValues[theIndex].KeyValue = NewKey;
                DownAdjust(theIndex);
                return;
            }
        }
        /// <summary>
        /// 删除队列元素
        /// </summary>
        /// <param name="obj"></param>
        public void HeapDelete(T obj)
        {
            int theIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                if (_queueValues[i].Element.Equals(obj))
                {
                    theIndex = i;
                    break;
                }
            }
            if (theIndex < 0)
            {
                return;
            }
            SwapElement(theIndex, Count - 1);
            _queueValues.RemoveAt(Count - 1);
            if (theIndex < Count)
            {
                int theP = HeapP(theIndex);
                bool theUp = false;
                if (theP >= 0)
                {
                    if (_queueValues[theIndex].KeyValue < _queueValues[theP].KeyValue)
                    {
                        UpAdjust(theIndex);
                        theUp = true;
                    }
                }
                if (theUp == false)
                {
                    MinHeapify(theIndex);
                }
            }
        }
        /// <summary>  
        /// 删除队列元素  
        /// </summary>  
        /// <param name="SelectFunc"></param>  
        public void HeapDelete(Func<T, bool> SelectFunc)
        {
            int theIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                if (SelectFunc(_queueValues[i].Element) == true)
                {
                    theIndex = i;
                    break;
                }
            }
            if (theIndex < 0)
            {
                return;
            }
            SwapElement(theIndex, Count - 1);
            _queueValues.RemoveAt(Count - 1);
            if (theIndex < Count)
            {
                int theP = HeapP(theIndex);
                bool theUp = false;
                if (theP >= 0)
                {
                    if (_queueValues[theIndex].KeyValue < _queueValues[theP].KeyValue)
                    {
                        UpAdjust(theIndex);
                        theUp = true;
                    }
                }
                if (theUp == false)
                {
                    MinHeapify(theIndex);
                }
            }

        }
        /// <summary>  
        /// 队列元素交换位置  
        /// </summary>  
        /// <param name="i"></param>  
        /// <param name="j"></param>  
        private void SwapElement(int i, int j)
        {
            QueueElementInt<T> theTmp = _queueValues[i];
            _queueValues[i] = _queueValues[j];
            _queueValues[j] = theTmp;
        }
        /// <summary>  
        /// 将元素插入队列  
        /// </summary>  
        /// <param name="Element"></param>  
        /// <param name="Key"></param>  
        public void HeapInsert(T Element, int Key)
        {
            _queueValues.Add(new QueueElementInt<T>(Element, int.MinValue));
            ChangeKey(Count - 1, Key);
        }
        /// <summary>  
        /// 取节点的左孩子节点  
        /// </summary>  
        /// <param name="i"></param>  
        /// <returns></returns>  
        private int HeapL(int i)
        {
            return i * 2 + 1;
        }
        /// <summary>  
        /// 取节点的右孩子节点  
        /// </summary>  
        /// <param name="i"></param>  
        /// <returns></returns>  
        private int HeapR(int i)
        {
            return i * 2 + 2;
        }
        /// <summary>  
        /// 取节点的父节点  
        /// </summary>  
        /// <param name="i"></param>  
        /// <returns></returns>  
        private int HeapP(int i)
        {
            return (i + 1) / 2 - 1;
        }
    }  
}
