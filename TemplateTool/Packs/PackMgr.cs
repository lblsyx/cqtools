using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using UnityLight.Loggers;

namespace TemplateTool.Packs
{
    public class PackMgr
    {
        private static IList<PackerInfo> _infos = new List<PackerInfo>();
        private static Dictionary<int, IDataPacker> _packers = new Dictionary<int, IDataPacker>();
        
        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            Type[] types = assembly.GetTypes();

            string sInterfaceStr = typeof(IDataPacker).Name;

            Type tAttributeType = typeof(DataPackerAttribute);

            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                DataPackerAttribute[] attributes = (DataPackerAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    DataPackerAttribute attribute = attributes[0];
                    IDataPacker iDataPacker = Activator.CreateInstance(type) as IDataPacker;

                    if (iDataPacker != null && _packers.ContainsKey(attribute.PackerType) == false)
                    {
                        _packers.Add(attribute.PackerType, iDataPacker);

                        if (attribute.EnableBind)
                        {
                            PackerInfo info = new PackerInfo();
                            info.PackerType = attribute.PackerType;
                            info.PackerName = attribute.PackerName;
                            _infos.Add(info);
                        }
                    }
                }
            }
        }

        public static void PackData(int generatorType, IList<TableInfo> schemas, string outPath, object others = null)
        {
            if (_packers.ContainsKey(generatorType))
            {
                IDataPacker iDataPacker = _packers[generatorType];
                iDataPacker.PackData(schemas, outPath, others, null);
            }
            else
            {
                XLogger.ErrorFormat("类型：{0} 的打包器不存在!", generatorType);
            }
        }
        public static bool initPackData(int generatorType, string outPath, object others = null,bool needDelete = true)
        {
            if (_packers.ContainsKey(generatorType))
            {
                IDataPacker iDataPacker = _packers[generatorType];
                return iDataPacker.initData(outPath, others, needDelete);
            }
            else
            {
                XLogger.ErrorFormat("类型：{0} 的打包器不存在!", generatorType);
            }
            return false;
        }
        public static IList<PackerInfo> GetPackerInfoList()
        {
            return _infos;
        }

        public static PackerInfo GetPackerInfo(int index)
        {
            if (index > -1 && index < _infos.Count)
            {
                return _infos[index];
            }
            return null;
        }
    }
}
