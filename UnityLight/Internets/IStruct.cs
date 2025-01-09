using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public interface IStruct
    {
        object Clone();

        void Serializtion(ByteArray oByteArray, bool bSerialize);
    }
}