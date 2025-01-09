using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CSScriptApp
{
    public class XMLUtil
    {
        public static void CopyObject(object source, object target)
        {
            if (source == null || target == null) return;
            if (source.GetType() != target.GetType()) return;

            System.Type type = source.GetType();
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (var field in fields)
            {
                field.SetValue(target, field.GetValue(source));
            }

            System.Reflection.PropertyInfo[] props = type.GetProperties();
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(target, prop.GetValue(source, null), null);
                }
            }
        }

        public static string Serialize(object obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static T Deserialize<T>(string s) where T : class
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T rlt = xs.Deserialize(sr) as T;
                return rlt;
            }
        }

        public static void Deserialize(object obj, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                object rlt = xs.Deserialize(sr);
                CopyObject(rlt, obj);
            }
        }
    }
}
