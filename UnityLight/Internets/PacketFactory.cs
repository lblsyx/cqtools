using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Internets
{
    public interface IPacketCreator
    {
        Packet CreatePacket();
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PackageAttribute : Attribute
    {
        public PackageAttribute(int nPacketID)
        {
            PacketID = nPacketID;
        }

        public int PacketID { get; set; }
    }

    public class PacketFactory
    {
        private static Dictionary<int, IPacketCreator> mCreators = new Dictionary<int, IPacketCreator>();

        public static void SearchAssembly(Assembly assembly)
        {
            Type[] list = assembly.GetTypes();

            string sInterfaceStr = typeof(IPacketCreator).ToString();

            Type tAttributeType = typeof(PackageAttribute);

            foreach (var type in list)
            {
                if (type.IsClass == false || type.GetInterface(sInterfaceStr) == null) continue;

                PackageAttribute[] attributes = (PackageAttribute[])type.GetCustomAttributes(tAttributeType, false);

                if (attributes != null && attributes.Length > 0 && attributes[0] != null)
                {
                    PackageAttribute attribute = attributes[0];

                    if (mCreators.ContainsKey(attribute.PacketID))
                    {
                        XLogger.ErrorFormat("协议结构类已存在!PacketID：{0}", attribute.PacketID);
                        continue;
                    }

                    mCreators.Add(attribute.PacketID, (IPacketCreator)Activator.CreateInstance(type));
                }
            }
        }

        public static Packet CreatePacket(int nPacketID)
        {
            if (mCreators.ContainsKey(nPacketID) == false)
            {
                XLogger.ErrorFormat("没有找到对应的协议结构类!PacketID: {0}", nPacketID);
                return null;
            }

            IPacketCreator iIPacketCreator = mCreators[nPacketID];

            return iIPacketCreator.CreatePacket();
        }
    }
}