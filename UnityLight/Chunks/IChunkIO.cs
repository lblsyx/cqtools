using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace UnityExt.Chunks
{
    public interface IChunkIO
    {
        int Code { get; }

        void ReadData(object data, ByteArray bytes);

        void WriteData(object data, ByteArray bytes);
    }
}
