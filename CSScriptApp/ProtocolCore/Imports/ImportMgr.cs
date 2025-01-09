using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSScriptApp.ProtocolCore.Imports
{
    public class ImportMgr
    {
        private static List<IImport> mImportList = new List<IImport>();
        private static Dictionary<string, IImport> mImportDict = new Dictionary<string, IImport>();

        public static List<IImport> ImportList { get { return mImportList; } }

        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            Type[] types = assembly.GetTypes();

            string sInterfaceStr = typeof(IImport).Name;

            Type tAttributeType = typeof(ImportAttribute);

            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                ImportAttribute[] attributes = (ImportAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    ImportAttribute attribute = attributes[0];
                    IImport iIExport = Activator.CreateInstance(type) as IImport;

                    if (iIExport != null && mImportDict.ContainsKey(iIExport.Name) == false)
                    {
                        if (attribute.Visible)
                        {
                            mImportList.Add(iIExport);
                        }
                        mImportDict.Add(iIExport.Name, iIExport);
                    }
                }
            }
        }

        public static IImport GetExport(string name)
        {
            if (mImportDict.ContainsKey(name))
            {
                return mImportDict[name];
            }

            return null;
        }
    }
}
