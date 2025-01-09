using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace RXHWRobot.Serializes
{
    public interface IChunkIO
    {
        int Code { get; }
        void Read(object data, ByteArray chunkBytes);
        void Write(object data, ByteArray chunkBytes);
    }
}
