using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace UnityLight.Utils
{
    public class Serializer
    {
        public static string XmlSerialize<T>(T t)
        {
            string content = "";
            XmlSerializer xs = new XmlSerializer(t.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, t);

                byte[] bytes = ms.ToArray();

                content = Encoding.Default.GetString(bytes);
            }
            return content;
        }

        public static string XmlSerialize<T>(IList<T> list)
        {
            string content = "";
            XmlSerializer xs = new XmlSerializer(list.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, list);

                byte[] bytes = ms.ToArray();

                content = Encoding.Default.GetString(bytes);
            }
            return content;
        }

        public static T XmlDeserialize<T>(string str) where T : class
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            using (StringReader sr = new StringReader(str))
            {
                return xs.Deserialize(sr) as T;
            }
        }

        public static void XmlDeserialize<T>(string str, ref List<T> list)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            using (StringReader sr = new StringReader(str))
            {
                list = xs.Deserialize(sr) as List<T>;
            }
        }

        /// <summary>
        /// 序列化对象为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize<T>(T o)
        {
            Type type = typeof(T);
            List<string> list = new List<string>();

            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();

            StringBuilder sb = new StringBuilder();

            foreach (FieldInfo field in fields)
            {
                string val = Convert2String(field.GetValue(o));

                list.Add(string.Format("{0}=\"{1}\"", field.Name, val));
            }

            foreach (PropertyInfo property in properties)
            {
                string val = Convert2String(property.GetValue(o, null));

                list.Add(string.Format("{0}=\"{1}\"", property.Name, val));
            }

            sb.Append(string.Format("<{0} {1} />", type.Name, string.Join(" ", list.ToArray())));

            return sb.ToString();
        }

        /// <summary>
        /// 序列化对象数组为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public static string Serialize<T>(T[] list, string rootName = null)
        {
            Type type = typeof(T);

            if (string.IsNullOrEmpty(rootName)) rootName = "ArrayOf" + type.Name;

            if (list == null || list.Length == 0) return "<" + rootName + " />";

            List<string> nodes = new List<string>();

            foreach (T item in list)
            {
                nodes.Add(Serialize<T>(item));
            }

            string xmlStr = string.Format("<{0}>\r\n    {1}\r\n</{0}>", rootName, string.Join("\r\n    ", nodes.ToArray()));

            return xmlStr;
        }

        /// <summary>
        /// 序列化对象列表为Xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public static string Serialize<T>(IList<T> list, string rootName = null)
        {
            T[] l = new T[list.Count];
            list.CopyTo(l, 0);
            return Serialize<T>(l, rootName);
        }

        private static string Convert2String(object obj)
        {
            object val = "";

            if (obj is Enum) val = (int)obj;
            else if (obj is DateTime) val = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
            else if (obj is Array)
            {
                Array array = obj as Array;
                string str = "";
                foreach (object item in array)
                {
                    if (item is Enum)
                    {
                        str = str + ((int)item).ToString() + " ";
                    }
                    else if (obj is DateTime)
                    {
                        str = str + ((DateTime)item).ToString("yyyy-MM-dd HH:mm:ss") + " ";
                    }
                    else
                    {
                        str = str + item.ToString() + " ";
                    }
                }

                if (str != "") val = str.Substring(0, str.Length - 1);
            }
            else val = obj;

            return Convert.ToString(val);
        }


        /// <summary>
        /// 反序列化Xml节点为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T o, XmlNode node)
        {
            Type type = typeof(T);

            if (type.GetProperty("TemplateID") != null) type.GetProperty("TemplateID").SetValue(o, Convert.ChangeType(node.Attributes["TemplateID"].Value, typeof(int)), null);
            if (type.GetProperty("TemplateName") != null) type.GetProperty("TemplateName").SetValue(o, Convert.ChangeType(node.Attributes["TemplateName"].Value, typeof(string)), null);

            if (type.GetField("TemplateID") != null) type.GetField("TemplateID").SetValue(o, Convert.ChangeType(node.Attributes["TemplateID"].Value, typeof(int)));
            if (type.GetField("TemplateName") != null) type.GetField("TemplateName").SetValue(o, Convert.ChangeType(node.Attributes["TemplateName"].Value, typeof(string)));

            foreach (XmlAttribute attr in node.Attributes)
            {
                Type itemType = null;
                object tmp = null;
                Array array = null;


                object obj = null;

                //遍历类型对象公共属性
                PropertyInfo property = type.GetProperty(attr.LocalName);

                if (property != null)
                {
                    try
                    {
                        if (property.PropertyType.IsArray)
                        {
                            itemType = property.PropertyType.Module.Assembly.GetType(property.PropertyType.FullName.Substring(0, property.PropertyType.FullName.Length - 2));
                            //itemType = Type.GetType(property.PropertyType.FullName.Substring(0, property.PropertyType.FullName.Length - 2));

                            string[] strlist = attr.Value.Split(' ');
                            array = CreateArrayInstance(property.PropertyType, strlist.Length);

                            for (int i = 0; i < strlist.Length; i++)
                            {
                                tmp = DeserializeImp(itemType, strlist[i], property, attr, node);
                                array.SetValue(tmp, i);
                            }

                            obj = array;
                        }
                        else
                        {
                            obj = DeserializeImp(property.PropertyType, attr.Value, property, attr, node);
                        }

                        property.SetValue(o, obj, null);
                    }
                    catch (Exception ex)
                    {
                        ThrowException(ex.Message, property, attr, node);
                    }

                    continue;
                }



                //遍历类型对象公共字段
                FieldInfo field = type.GetField(attr.LocalName);
                if (field != null)
                {
                    try
                    {
                        if (field.FieldType.IsArray)
                        {
                            string[] strlist = attr.Value.Split(' ');
                            itemType = Type.GetType(field.FieldType.FullName.Substring(0, field.FieldType.FullName.Length - 2));
                            array = CreateArrayInstance(field.FieldType, strlist.Length);

                            for (int i = 0; i < strlist.Length; i++)
                            {
                                tmp = DeserializeImp(itemType, strlist[i], field, attr, node);
                                array.SetValue(tmp, i);
                            }

                            obj = array;
                        }
                        else
                        {
                            obj = DeserializeImp(property.PropertyType, attr.Value, field, attr, node);
                        }

                        field.SetValue(o, obj);
                    }
                    catch (Exception ex)
                    {
                        ThrowException(ex.Message, field, attr, node);
                    }
                }
            }

            return o;
        }

        /// <summary>
        /// 创建数组类型的实例对象
        /// </summary>
        /// <param name="arrayType"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static Array CreateArrayInstance(Type arrayType, int len)
        {
            object array = new object();

            array = arrayType.InvokeMember("Set", BindingFlags.CreateInstance, null, array, new object[] { len });

            return array as Array;
        }

        /// <summary>
        /// 反序列化单个字段的值
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <param name="propertyOrField"></param>
        /// <param name="attr"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static object DeserializeImp(Type t, string value, object propertyOrField, XmlAttribute attr, XmlNode node)
        {
            if (t.IsEnum)
            {

                if (Enum.IsDefined(t, int.Parse(value)))
                {
                    string enumStr = Enum.GetName(t, int.Parse(value));
                    return Enum.Parse(t, enumStr);
                }
                else
                {
                    throw new Exception(string.Format("{0}  枚举中不存在值为 {1} 的类型。", t.FullName, value));

                    //ThrowException(string.Format("{0}  枚举中不存在值为 {1} 的类型。", t.FullName, value), propertyOrField, attr, node);

                    //return null;
                }
            }
            else
            {
                return Convert.ChangeType(value, t);
            }
        }

        private static void ThrowException(string errorMsg, object propertyOrField, XmlAttribute attr, XmlNode node)
        {
            if (propertyOrField is PropertyInfo)
            {
                PropertyInfo property = propertyOrField as PropertyInfo;

                throw new Exception(string.Format("{0} \r\n模板类型: {4}\r\n模板ID: {2};  模板名称：{3}\r\n模板字段: {5}; 值：{1}; 类型: {6}\r\n", errorMsg, attr.Value, node.Attributes["TemplateID"].Value, node.Attributes["TemplateName"].Value, node.LocalName, property.Name, property.PropertyType.FullName));
            }
            else
            {
                FieldInfo field = propertyOrField as FieldInfo;

                throw new Exception(string.Format("{0} \r\n模板类型: {4}\r\n模板ID: {2};  模板名称：{3}\r\n模板字段: {5}; 值：{1}; 类型: {6}\r\n", errorMsg, attr.Value, node.Attributes["TemplateID"].Value, node.Attributes["TemplateName"].Value, node.LocalName, field.Name, field.FieldType.FullName));
            }
        }

        /// <summary>
        /// 反序列化Xml节点为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Deserialize<T>(XmlNode node)
        {
            Type type = typeof(T);

            Assembly assembly = Assembly.GetAssembly(type);

            T o = (T)assembly.CreateInstance(type.FullName);

            return Deserialize<T>(o, node);
        }

        /// <summary>
        /// 反序列化Xml节点列表为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public static List<T> Deserialize<T>(XmlNodeList nodeList)
        {
            List<T> list = new List<T>();

            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    //Console.WriteLine(node.OuterXml);

                    T o = Deserialize<T>(node);

                    list.Add(o);
                }
            }

            return list;
        }
    }
}
