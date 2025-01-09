using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DatabaseTool.DatabaseCore.Generates
{
    public class StructGeneratorMgr
    {
        private static List<IStructGenerator> mStructGeneratorList = new List<IStructGenerator>();
        private static Dictionary<string, IStructGenerator> mStructGeneratorDict = new Dictionary<string, IStructGenerator>();

        public static List<IStructGenerator> StructGeneratorList { get { return mStructGeneratorList; } }

        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            Type[] types = assembly.GetTypes();

            string sInterfaceStr = typeof(IStructGenerator).Name;

            Type tAttributeType = typeof(StructGeneratorAttribute);

            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                StructGeneratorAttribute[] attributes = (StructGeneratorAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    StructGeneratorAttribute attribute = attributes[0];
                    IStructGenerator iIStructGenerator = Activator.CreateInstance(type) as IStructGenerator;

                    if (iIStructGenerator != null && mStructGeneratorDict.ContainsKey(iIStructGenerator.Type) == false)
                    {
                        if (attribute.Visible)
                        {
                            mStructGeneratorList.Add(iIStructGenerator);
                        }
                        mStructGeneratorDict.Add(iIStructGenerator.Type, iIStructGenerator);
                    }
                }
            }
        }

        public static IStructGenerator GetStructGenerator(string type)
        {
            if (mStructGeneratorDict.ContainsKey(type))
            {
                return mStructGeneratorDict[type];
            }

            return null;
        }
    }
}
