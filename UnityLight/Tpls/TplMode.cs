using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight;
using UnityLight.Internets;
using UnityLight.Loggers;

namespace UnityLight.Tpls
{
    public class TplMode
    {
        public Callback<string> OnDoneEvent;

        public string Name { get; private set; }
        public bool IsDone { get; private set; }

        private IList<Tpl> _list = new List<Tpl>();
        private Dictionary<int, Tpl> _dict = new Dictionary<int, Tpl>();

        private int _step;
        private Type _type;
        private ByteArray _bytes;

        private uint _count;
        private uint _parsed;

        public TplMode(string name, Type type, int step)
        {
            Name = name;
            _step = step;
            _type = type;
            IsDone = false;
        }

        public void Parse(ByteArray bytes)
        {
            if (bytes == null || bytes.BytesAvailable <= 4) return;

            _bytes = bytes;
            _count = _bytes.ReadUInt();
        }

        public void Clear()
        {
            _count = 0;
            _parsed = 0;
            _bytes = null;
            IsDone = false;
            _list.Clear();
            _dict.Clear();
        }

        public void Update()
        {
            if (_bytes == null) return;

            for (; _parsed < _count; _parsed++)
            {
                Tpl tpl = Activator.CreateInstance(_type) as Tpl;

                //try
                //{
                    tpl.ReadFrom(_bytes);
                //}
                //catch (Exception ex)
                //{
                //    XLogger.ErrorFormat("{0} error!{1}", tpl.ToString(), ex.StackTrace);
                //}
                if (_dict.ContainsKey(tpl.TID))
                {
                    //XLogger.ErrorFormat("TID：{0}的模板在 {1} 表里已存在!", tpl.TID, Name);
                }
                else
                {
                    _list.Add(tpl);
                    _dict.Add(tpl.TID, tpl);
                }

                if ((_parsed % _step) == 0 && _parsed != 0)
                {
                    ++_parsed;
                    break;
                }
            }

            if (_parsed >= _count)
            {
                _bytes = null;
                IsDone = true;
                if (OnDoneEvent != null) OnDoneEvent(Name);
            }
        }

        public T Find<T>(int id) where T : Tpl
        {
            if (_dict.ContainsKey(id))
            {
                return _dict[id] as T;
            }
            return default(T);
        }

        public IList<T> FindAll<T>() where T : Tpl
        {
            IList<T> list = new List<T>();

            foreach (Tpl item in _list)
            {
                list.Add(item as T);
            }

            return list;
        }
    }

    //public class TplMode<T> where T : Tpl, new()
    //{
    //    public Callback<string> OnDoneEvent;

    //    public string Name { get; private set; }
    //    public bool IsDone { get; private set; }

    //    private IList<T> _list = new List<T>();
    //    private Dictionary<int, T> _dic = new Dictionary<int, T>();

    //    private int _step;
    //    private ByteArray _bytes;

    //    private uint _count;
    //    private uint _parsed;

    //    public TplMode(string name, int step)
    //    {
    //        Name = name;
    //        _step = step;
    //        IsDone = false;

    //        _count = _bytes.ReadUInt();
    //    }

    //    public void Parse(ByteArray bytes)
    //    {
    //        _bytes = bytes;
    //    }

    //    public void Update()
    //    {
    //        if (_bytes == null) return;

    //        for (; _parsed < _count; _parsed++)
    //        {
    //            T t = new T();
    //            t.ReadFrom(_bytes);
    //            if (_dic.ContainsKey(t.TID))
    //            {
    //                XLogger.ErrorFormat("TID：{0}的模板在 {1} 表里已存在!", t.TID, Name);
    //            }
    //            else
    //            {
    //                _list.Add(t);
    //                _dic.Add(t.TID, t);
    //            }

    //            if ((_parsed % _step) == 0 && _parsed != 0)
    //            {
    //                ++_parsed;
    //                break;
    //            }
    //        }

    //        if (_parsed >= _count)
    //        {
    //            _bytes = null;
    //            IsDone = true;
    //            if (OnDoneEvent != null) OnDoneEvent(Name);
    //        }
    //    }
    //}
}
