using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace UnityExt.Chunks
{
    public class FileStartChunkIO : IChunkIO
    {

        public int Code
        {
            get { return ChunkType.FILE_START; }
        }

        public void ReadData(object data, UnityLight.Internets.ByteArray bytes)
        {
            ChunkData chunkData = data as ChunkData;
            chunkData.MajorVer = bytes.ReadByte();
            chunkData.MinorVer = bytes.ReadByte();
            chunkData.RevisionVer = bytes.ReadByte();
            chunkData.ObjectType = bytes.ReadByte();
        }

        public void WriteData(object data, UnityLight.Internets.ByteArray bytes)
        {
            ChunkData chunkData = data as ChunkData;
            bytes.WriteByte((byte)chunkData.MajorVer);
            bytes.WriteByte((byte)chunkData.MinorVer);
            bytes.WriteByte((byte)chunkData.RevisionVer);
            bytes.WriteByte((byte)chunkData.ObjectType);
        }
    }

    public class FileEndChunkIO : IChunkIO
    {

        public int Code
        {
            get { return ChunkType.FILE_END; }
        }

        public void ReadData(object data, ByteArray bytes)
        {
        }

        public void WriteData(object data, ByteArray bytes)
        {
        }
    }
}
