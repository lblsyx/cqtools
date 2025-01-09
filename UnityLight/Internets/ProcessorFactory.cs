using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Internets
{
    public interface IProcessor<T>
    {
        void Process(T sender, Packet pkg);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ProcessAttribute : Attribute
    {
        public int PacketID { get; set; }

        public ProcessAttribute(int nPacketID)
        {
            PacketID = nPacketID;
        }
    }

    public class ProcessorFactory<T>
    {
        private Dictionary<int, IProcessor<T>> mProcesses = new Dictionary<int, IProcessor<T>>();

        public void SearchAssembly(Assembly assembly)
        {
            if (assembly == null) return;

            Type[] list = assembly.GetTypes();

            string sInterfaceStr = typeof(IProcessor<T>).Name;

            Type tAttributeType = typeof(ProcessAttribute);

            foreach (var type in list)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                ProcessAttribute[] attributes = (ProcessAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    ProcessAttribute attribute = attributes[0];

                    if (mProcesses.ContainsKey(attribute.PacketID))
                    {
                        XLogger.ErrorFormat("协议结构类已存在!PacketID：{0}", attribute.PacketID);
                        continue;
                    }

                    mProcesses.Add(attribute.PacketID, (IProcessor<T>)Activator.CreateInstance(type));
                }
            }
        }

        public void Process(T sender, Packet pkg)
        {
            if (mProcesses.ContainsKey(pkg.PacketID) == false)
            {
                XLogger.ErrorFormat("没有找到对应的协议处理类!PacketID: {0}", pkg.PacketID);
                return;
            }

            IProcessor<T> processor = mProcesses[pkg.PacketID];

            try
            {
                processor.Process(sender, pkg);
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }
        }
    }
}