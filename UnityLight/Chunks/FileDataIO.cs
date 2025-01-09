using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace UnityExt.Chunks
{
    public class FileDataIO
    {
        protected List<IChunkIO> _chunkList = new List<IChunkIO>();

        protected Dictionary<int, IChunkIO> _chunkDict = new Dictionary<int, IChunkIO>();


        protected void AddChunkIO(IChunkIO iChunkIO)
        {
            if (_chunkDict.ContainsKey(iChunkIO.Code)) return;
            
            _chunkList.Add(iChunkIO);
            _chunkDict.Add(iChunkIO.Code, iChunkIO);
        }

        protected int ReadChunk(ByteArray bytes, ByteArray chunkBytes)
        {
            chunkBytes.Reset();

            int code = 0;
            uint len = 0;

            byte fix = bytes.ReadByte();
            if (fix == 0xFF)
            {
                code = bytes.ReadByte();
                len = bytes.ReadUInt();

                if (len != 0 && bytes.BytesAvailable > 0)
                {
                    //byte[] ba = bytes.ReadBytes((int)len);
                    byte[] ba = new byte[(int)len];
                    bytes.CopyTo(ba, 0, bytes.Position);
                    chunkBytes.WriteBytes(ba);
                    chunkBytes.Position = 0;
                }
            }

            return code;
        }

        protected void WriteChunk(ByteArray bytes, int code, ByteArray chunkBytes)
        {
            bytes.AutoResize = true;
            bytes.WriteByte(0xFF);
            bytes.WriteByte((byte)code);
            bytes.WriteUInt((uint)chunkBytes.Length);
            bytes.WriteBytes(chunkBytes.Buffer, 0, chunkBytes.Length);
        }

        public virtual byte[] Encode(object data, bool compress)
        {
            ByteArray allChunkBytes = new ByteArray();
            allChunkBytes.AutoResize = true;
            ByteArray chunkBytes = new ByteArray();
            chunkBytes.AutoResize = true;

            foreach (IChunkIO iChunkIO in _chunkList)
            {
                chunkBytes.Reset();
                iChunkIO.WriteData(data, chunkBytes);
                WriteChunk(allChunkBytes, iChunkIO.Code, chunkBytes);
            }

            uint uncompressLen = (uint)allChunkBytes.Length;

            if (uncompressLen != 0 && compress)
            {
                allChunkBytes = allChunkBytes.Compress();
            }

            ByteArray bytes = new ByteArray();
            bytes.AutoResize = true;
            bytes.WriteBoolean(compress);
            bytes.WriteUInt(uncompressLen);
            bytes.WriteUInt((uint)allChunkBytes.Length);
            bytes.WriteBytes(allChunkBytes.Buffer, 0, allChunkBytes.Length);

            byte[] contents = new byte[bytes.Length];
            Array.Copy(bytes.Buffer, contents, bytes.Length);

            return contents;
        }

        public virtual bool Decode(object data, byte[] bytes)
        {
            int code = 0;
            bool decoded = false;
            IChunkIO iChunkIO = null;
            ByteArray ba = new ByteArray(bytes, bytes.Length);
            ByteArray allChunkBytes = null;
            ByteArray chunkBytes = new ByteArray();
            chunkBytes.AutoResize = true;
            if (ba.BytesAvailable > 0)
            {
                bool compressed = ba.ReadBoolean();
                uint uncompressLen = ba.ReadUInt();
                uint compressLen = ba.ReadUInt();
               
                //byte[] tmp = ba.ReadBytes((int)compressLen);
                byte[] tmp = new byte[(int)compressLen];
                ba.CopyTo(tmp, 0, ba.Position);

                allChunkBytes = new ByteArray(tmp, tmp.Length);
                allChunkBytes.AutoResize = true;
                if (compressed) allChunkBytes = allChunkBytes.Uncompress();
                if (uncompressLen != allChunkBytes.Length) return false;
                code = ReadChunk(allChunkBytes, chunkBytes);

                if (code != ChunkType.FILE_START) return false;

                iChunkIO = _chunkDict[code];
                iChunkIO.ReadData(data, chunkBytes);
                decoded = true;
            }
            else
            {
                return false;
            }

            while (allChunkBytes.BytesAvailable > 0)
            {
                code = ReadChunk(allChunkBytes, chunkBytes);
                if (_chunkDict.ContainsKey(code))
                {
                    iChunkIO = _chunkDict[code];
                    iChunkIO.ReadData(data, chunkBytes);
                }
                decoded = true;

                if (code == ChunkType.FILE_END) break;
            }

            return decoded;
        }
    }
}
