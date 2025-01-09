using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace RXHWRobot.Serializes
{
    public class RpgDataIO
    {
        private List<IChunkIO> mChunkQueue = new List<IChunkIO>();
        private Dictionary<int, IChunkIO> mChunkMap = new Dictionary<int, IChunkIO>();

        public virtual string Flag { get { throw new NotImplementedException("子类必需重写实现 Flag 属性来获取标志字符串!"); } }

        public void addChunkIO(IChunkIO iIChunkIO)
        {
            mChunkQueue.Add(iIChunkIO);
            mChunkMap[iIChunkIO.Code] = iIChunkIO;
        }

        public int ReadChunk(ByteArray bytes, ByteArray chunkBytes)
        {
            chunkBytes.Reset();

            int code = 0; int len;

            int fixedCode = bytes.ReadByte();
            if (fixedCode == 0xFF)
            {
                code = bytes.ReadByte();
                len = bytes.ReadInt();
                if (len != 0 && len <= bytes.BytesAvailable)
                {
                    byte[] temp = bytes.ReadBytes(len, false);
                    chunkBytes.WrapBuffer(temp, temp.Length);
                    chunkBytes.Position = 0;
                }
            }

            return code;
        }

        protected ByteArray _chunkByteArray = new ByteArray();
        public bool Decode(ByteArray bytes, object data)
        {
            int code = 0; IChunkIO chunkIO; bool decoded = false;
            ByteArray temp = new ByteArray();
            if (bytes.BytesAvailable > 0)
            {
                bool compressed = bytes.ReadBoolean();
                uint uncompressLen = bytes.ReadUInt();
                uint len = bytes.ReadUInt();
                temp.WrapBuffer(bytes.ReadBytes((int)len, false), (int)len);
                if (compressed) temp = temp.Uncompress();
                if (uncompressLen != temp.Length) return false;
                code = ReadChunk(temp, _chunkByteArray);
                if (code != (int)ChunkType.FILE_START)
                {
                    return false;
                }
                chunkIO = mChunkMap[code];
                chunkIO.Read(data, _chunkByteArray);
                decoded = true;
            }
            else
            {
                return false;
            }

            while (temp.BytesAvailable > 0)
            {
                code = ReadChunk(temp, _chunkByteArray);
                chunkIO = mChunkMap[code];
                if (chunkIO != null) chunkIO.Read(data, _chunkByteArray);
                decoded = true;
                if (code == (int)ChunkType.FILE_LAST)
                {
                    break;
                }
            }
            _chunkByteArray.Reset();
            return decoded;
        }

        public ByteArray DetachFromFileBytes(ByteArray fileBytes, string flag, ByteArray dataBytes = null)
        {
            if (dataBytes == null)
            {
                dataBytes = new ByteArray();
            }
            dataBytes.Reset();

            uint startPos = CalcDataBytesStartPos(fileBytes, flag);
            if (startPos == fileBytes.Length)
            {
                return dataBytes;
            }
            fileBytes.Position = (int)startPos;

            int len = (int)(fileBytes.BytesAvailable - flag.Length - 4);
            dataBytes.WrapBuffer(fileBytes.ReadBytes(len, false), len);

            uint dataLength = fileBytes.ReadUInt();
            if (dataBytes.Length != dataLength)
            {//校验字节流长度
                dataBytes.Reset();
            }

            return dataBytes;
        }

        public uint CalcDataBytesStartPos(ByteArray fileBytes, String flag)
        {
            if (fileBytes.Length < (flag.Length + 4))
            {
                return (uint)fileBytes.Length;
            }

            int lengthPosition = fileBytes.Length - flag.Length - 4;//文件大小 - 数据字节流标志长度 - 数据字节流大小字段的长度(uint)
            fileBytes.Position = lengthPosition;
            uint dataLength = fileBytes.ReadUInt();//数据信息字节流的长度在FLAG标志前

            String str = string.Empty;
            for (int i = 0; i < flag.Length; i++)
            {
                int c = fileBytes.ReadByte();
                str += (char)c;// String.fromCharCode(c);
            }

            if (str == flag)
            {
                return (uint)(lengthPosition - dataLength);
            }

            return (uint)fileBytes.Length;
        }

        public bool DetachAndDecode(ByteArray fileBytes, object data)
        {
            ByteArray bytes = DetachFromFileBytes(fileBytes, Flag);
            return Decode(bytes, data);
        }
    }
}
