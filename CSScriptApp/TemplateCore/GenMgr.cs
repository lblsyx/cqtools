using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityLight.Loggers;

namespace CSScriptApp.TemplateCore
{
    public class GenMgr
    {
        private static IList<GeneratorInfo> _infos = new List<GeneratorInfo>();
        private static Dictionary<int, IGenerator> _generators = new Dictionary<int, IGenerator>();

        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            Type[] types = assembly.GetTypes();

            string sInterfaceStr = typeof(IGenerator).Name;

            Type tAttributeType = typeof(GeneratorAttribute);

            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                GeneratorAttribute[] attributes = (GeneratorAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    GeneratorAttribute attribute = attributes[0];
                    IGenerator iGenerator = Activator.CreateInstance(type) as IGenerator;

                    if (iGenerator != null && _generators.ContainsKey(attribute.GeneratorType) == false)
                    {
                        _generators.Add(attribute.GeneratorType, iGenerator);

                        if (attribute.EnableBind)
                        {
                            GeneratorInfo info = new GeneratorInfo();
                            info.GeneratorType = attribute.GeneratorType;
                            info.GeneratorName = attribute.GeneratorName;
                            _infos.Add(info);
                        }
                    }
                }
            }
        }

        public static bool IsUseFolder(int generatorType)
        {
            if (_generators.ContainsKey(generatorType))
            {
                IGenerator iGenerator = _generators[generatorType];
                return iGenerator.UseFolder;
            }

            return false;
        }

        public static bool IsRequireSecondPath(int generatorType)
        {
            if (_generators.ContainsKey(generatorType))
            {
                IGenerator iGenerator = _generators[generatorType];
                return iGenerator.RequireSecondCode;
            }

            return false;
        }

        public static void Generate(int generatorType, IList<TableInfo> schemas, string outPath, string outPath2, object others = null)
        {
            if (_generators.ContainsKey(generatorType))
            {
                IGenerator iGenerator = _generators[generatorType];
                iGenerator.Generate(schemas, outPath, outPath2, others);
            }
            else
            {
                Program.WriteToConsole("类型：{0} 的生成器不存在!", generatorType);
            }
        }

        public static IList<GeneratorInfo> GetGeneratorInfoList()
        {
            return _infos;
        }

        public static GeneratorInfo GetGeneratorInfo(int index)
        {
            if (index > -1 && index < _infos.Count)
            {
                return _infos[index];
            }
            return null;
        }
    }
}
