using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public class Packet
    {
        public short Header;
        public uint PacketSize;
        public ushort PacketID;
        public uint OwnerID1;
        public uint OwnerID2;
        public byte SourceID1;
        public byte SourceID2;
        public byte TargetID1;
        public byte TargetID2;
        public TCPClient Client;

        public void SetOwnerID(ulong guid)
        {
            OwnerID1 = (uint)(guid >> 32);
            OwnerID2 = (uint)(guid & 0xFFFFFFFF);
        }

        public virtual void Serializtion(ByteArray oByteArray, bool bSerialize)
        {
            if (bSerialize)
            {
                Header = PacketConfig.PackageHeader;
                oByteArray.WriteShort(Header);
                oByteArray.WriteUInt(0);
                oByteArray.WriteUShort(PacketID);
                oByteArray.WriteUInt(OwnerID1);
                oByteArray.WriteUInt(OwnerID2);
                oByteArray.WriteByte(SourceID1);
                oByteArray.WriteByte(SourceID2);
                oByteArray.WriteByte(TargetID1);
                oByteArray.WriteByte(TargetID2);
            }
            else
            {
                Header = oByteArray.ReadShort();
                PacketSize = oByteArray.ReadUInt();
                PacketID = oByteArray.ReadUShort();
                OwnerID1 = oByteArray.ReadUInt();
                OwnerID2 = oByteArray.ReadUInt();
                SourceID1 = oByteArray.ReadByte();
                SourceID2 = oByteArray.ReadByte();
                TargetID1 = oByteArray.ReadByte();
                TargetID2 = oByteArray.ReadByte();
            }
        }

        public virtual Packet Clone()
        {
            Packet pkg = new Packet();

            pkg.Header = Header;
            pkg.PacketID = PacketID;
            pkg.OwnerID1 = OwnerID1;
            pkg.OwnerID2 = OwnerID2;
            pkg.SourceID1 = SourceID1;
            pkg.SourceID2 = SourceID2;
            pkg.TargetID1 = TargetID1;
            pkg.TargetID2 = TargetID2;

            return pkg;
        }
    }
}