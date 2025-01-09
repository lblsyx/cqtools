using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProtocolCore.Generates
{
    public class GeneratorMgr
    {
        private static List<GeneratorInfo> mGeneratorInfoList = new List<GeneratorInfo>();
        private static Dictionary<string, IGenerator> mGeneratorDict = new Dictionary<string, IGenerator>();

        public static List<GeneratorInfo> GeneratorInfoList { get { return mGeneratorInfoList; } }

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

                    if (iGenerator != null && mGeneratorDict.ContainsKey(attribute.Type) == false)
                    {
                        if (attribute.Visible)
                        {
                            GeneratorInfo info = new GeneratorInfo();
                            info.Type = attribute.Type;
                            info.Name = iGenerator.Name;
                            mGeneratorInfoList.Add(info);
                        }

                        mGeneratorDict.Add(attribute.Type, iGenerator);
                    }
                }
            }
        }

        public static bool HasGeneratorType(string type)
        {
            foreach (var item in mGeneratorInfoList)
            {
                if (item.Type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static IGenerator GetGenerator(string type)
        {
            if (mGeneratorDict.ContainsKey(type))
            {
                return mGeneratorDict[type];
            }

            return null;
        }
    }
}
