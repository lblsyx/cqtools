using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Serializes.Chunks
{
    public class FileEndChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.FILE_LAST; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
        }

        #endregion
    }
}
