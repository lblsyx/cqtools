using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityLight.Internets;
using UnityLight.Loggers;
using zlib;

namespace UnityLight.Tpls
{
    public class TplMgr
    {
        private static IList<TplMode> _parsing = new List<TplMode>();
        private static Dictionary<string, TplMode> _dict = new Dictionary<string, TplMode>();

        public static Callback OnParseDoneCallback;
        public static bool ParseDone { get; private set; }

        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;

            Type[] list = assembly.GetTypes();
            Type tAttributeType = typeof(TplSearchableAttribute);

            foreach (Type type in list)
            {
                if (type.IsClass == false) continue;

                TplSearchableAttribute[] attributes = (TplSearchableAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    FieldInfo[] fields = type.GetFields();

                    foreach (FieldInfo field in fields)
                    {
                        if (field.IsStatic == false) continue;

                        AddTplMode(field.GetValue(null) as TplMode);
                    }
                }
            }
        }

        public static void AddTplMode(TplMode mode)
        {
            if (mode == null) return;

            if (_dict.ContainsKey(mode.Name))
            {
                XLogger.ErrorFormat("模板名称重复，{0}模板配置已存在！", mode.Name);

                return;
            }

            _dict[mode.Name] = mode;
        }

        public static TplMode GetTplMode(string name)
        {
            if (_dict.ContainsKey(name))
            {
                return _dict[name];
            }
            return null;
        }

        public static bool ParseByteArray(byte[] bytes)
        {
            byte[] content = null;
            ByteArray oByteArray = new ByteArray(bytes, bytes.Length);

            uint unCompressLen = oByteArray.ReadUInt();
            uint compressLen = oByteArray.ReadUInt();
            content = oByteArray.ReadBytes((int)compressLen, false);
            oByteArray.WrapBuffer(content, (int)compressLen);

            Clear();

            ByteArray byteArray = oByteArray.Uncompress();// new ByteArray(content, content.Length);

            ByteArray ba = new ByteArray();
            while (byteArray.BytesAvailable > 0)
            {
                string tplName = byteArray.ReadUTF();

                string dirtyStr = null;
                dirtyStr = byteArray.ReadUTF();
                dirtyStr = byteArray.ReadUTF();
                dirtyStr = byteArray.ReadUTF();

                uint len = byteArray.ReadUInt();

                //byte[] temp = byteArray.ReadBytes((int)len);
                byte[] temp = new byte[(int)len];
                byteArray.CopyTo(temp, 0, byteArray.Position);
                byteArray.Position = byteArray.Position + (int)len;

                if (_dict.ContainsKey(tplName))
                {
                    TplMode mode = _dict[tplName];
                    mode.Parse(new ByteArray(temp, temp.Length));
                    _parsing.Add(mode);
                }
                else
                {
                    XLogger.Error(string.Format("没有找到 {0} 对应的模板数据模型对象!", tplName));
                }
            }

            return _parsing.Count != 0;
        }

        public static void Clear()
        {
            foreach (var item in _dict)
            {
                item.Value.Clear();
            }
            _parsing.Clear();
            ParseDone = false;
        }

        public static void Update()
        {
            if (_parsing.Count > 0)
            {
                bool bAllDone = true;

                for (int i = 0; i < _parsing.Count; i++)
                {
                    TplMode mode = _parsing[i];
                    if (mode.IsDone) continue;
                    bAllDone = false;
                    mode.Update();
                }

                if (bAllDone)
                {//表示全部解析完成
                    _parsing.Clear();
                    ParseDone = true;
                    if (OnParseDoneCallback != null)
                    {
                        OnParseDoneCallback();
                    }
                }
            }
        }
    }
}
