using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace UnityLight.Tpls
{
    public class Tpl
    {
        public int TID;

        public virtual void ReadFrom(ByteArray bytes)
        {
            TID = bytes.ReadInt();
        }

        public virtual void WriteTo(ByteArray bytes)
        {
            bytes.WriteInt(TID);
        }
    }
}
