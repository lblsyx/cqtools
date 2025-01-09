using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProtocolCore.Exports
{
    public class ExportMgr
    {
        private static List<IExport> mExportList = new List<IExport>();
        private static Dictionary<string, IExport> mExportDict = new Dictionary<string, IExport>();

        public static List<IExport> ExportList { get { return mExportList; } }

        public static void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;
            Type[] types = assembly.GetTypes();

            string sInterfaceStr = typeof(IExport).Name;

            Type tAttributeType = typeof(ExportAttribute);

            foreach (var type in types)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                ExportAttribute[] attributes = (ExportAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    ExportAttribute attribute = attributes[0];
                    IExport iIExport = Activator.CreateInstance(type) as IExport;

                    if (iIExport != null && mExportDict.ContainsKey(iIExport.Name) == false)
                    {
                        if (attribute.Visible)
                        {
                            mExportList.Add(iIExport);
                        }
                        mExportDict.Add(iIExport.Name, iIExport);
                    }
                }
            }
        }

        public static IExport GetExport(string name)
        {
            if (mExportDict.ContainsKey(name))
            {
                return mExportDict[name];
            }

            return null;
        }
    }
}
