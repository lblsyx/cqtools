using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Datas;

namespace RXHWRobot.Serializes.Chunks
{
    public class FileStartChunkIO : IChunkIO
    {
        #region IChunkIO 成员

        public int Code
        {
            get { return (int)ChunkType.FILE_START; }
        }

        public void Read(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            RpgData bd = data as RpgData;
            bd.majorVer = chunkBytes.ReadByte();
            bd.minorVer = chunkBytes.ReadByte();
            bd.revisionVer = chunkBytes.ReadByte();
            bd.objectType = chunkBytes.ReadByte();
        }

        public void Write(object data, UnityLight.Internets.ByteArray chunkBytes)
        {
            RpgData bd = data as RpgData;
            chunkBytes.WriteByte(bd.majorVer);
            chunkBytes.WriteByte(bd.minorVer);
            chunkBytes.WriteByte(bd.revisionVer);
            chunkBytes.WriteByte(bd.objectType);
        }

        #endregion
    }
}
