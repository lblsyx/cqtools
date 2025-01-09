using System;
using System.Collections.Generic;
using System.Text;
using UnityLight.Pools;
using UnityLight.Utils;

namespace UnityLight.Internets
{
    #region 字节码储存顺序枚举

    /// <summary>
    /// 字节码储存顺序
    /// </summary>
    public enum EndianEnum
    {
        BIG_ENDIAN,
        LITTLE_ENDIAN
    }

    #endregion 字节码储存顺序枚举

    /// <summary>
    /// 字节数组类，默认高字节序，与AS3相同。
    /// </summary>
    public class ByteArray : IPoolItem
    {
        #region 属性/变量

        public int Position { get; set; }

        public byte[] Buffer { get; protected set; }

        public int Length { get; protected set; }

        public EndianEnum Endian { get; protected set; }

        public int BufferSize { get; protected set; }

        public int StepSize { get; set; }

        public bool AutoResize { get; set; }

        public int RemainedSize { get { return BufferSize - Length; } }

        public int BytesAvailable { get { return Length - Position; } }

        #endregion 属性/变量

        #region 构造函数

        public ByteArray()
            : this(2048)
        { }

        public ByteArray(int bufferSize)
            : this(bufferSize, EndianEnum.BIG_ENDIAN)
        { }

        public ByteArray(int bufferSize, EndianEnum endianEnum)
        {
            BufferSize = bufferSize;
            StepSize = bufferSize;
            AutoResize = false;
            Buffer = new byte[BufferSize];
            
            Endian = endianEnum;
            Reset();
        }

        public ByteArray(byte[] buff, int len)
            : this(buff, len, EndianEnum.BIG_ENDIAN)
        { }

        public ByteArray(byte[] buff, int len, EndianEnum endianEnum)
        {
            Buffer = buff;
            BufferSize = Buffer.Length;
            Endian = endianEnum;
            Reset();
            Length = len;
        }

        #endregion 构造函数

        #region 其他公共方法

        /// <summary>
        /// 重置字节数组
        /// </summary>
        public virtual void Reset()
        {
            Position = 0;
            Length = 0;
        }

        /// <summary>
        /// 替换字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        public void WrapBuffer(byte[] buffer, int len)
        {
            Buffer = buffer;
            Length = len;
            BufferSize = buffer.Length;
        }

        /// <summary>
        /// 在 src 参数指定的缓冲区中，从 srcOffset 参数指定的位置开始将 count 参数指定的长度
        /// 复制到起始于 offset 参数指定位置的当前 ByteArray 对象缓冲区
        /// </summary>
        /// <param name="src">源缓冲区</param>
        /// <param name="srcOffset"> src 中从零开始的字节偏移量</param>
        /// <param name="offset">当前ByteArray对象缓冲区从零开始的字节偏移量</param>
        /// <param name="count">要复制的字节数，如果超出缓冲区则仅复制有效缓冲区数据</param>
        /// <returns>复制的字节数</returns>
        public virtual int CopyFrom(byte[] src, int srcOffset, int offset, int count)
        {
            var canCopyLen = Math.Min(src.Length - srcOffset, BufferSize - offset);
            count = Math.Min(count, canCopyLen);

            if (count > 0)
            {
                System.Buffer.BlockCopy(src, srcOffset, Buffer, offset, count);
                Position = offset + canCopyLen;
                RefreshDataLength();
                Position = 0;
                return count;
            }

            return 0;
        }

        /// <summary>
        /// 将 ByteArray 对象的缓冲区中 offset 参数指定位置开始的缓冲区数据复制到
        /// 起始于 dstOffset 参数指定位置的 dst 参数指定的缓冲区中
        /// </summary>
        /// <param name="dst">要复制到的目标缓冲区</param>
        /// <param name="dstOffset"> dst 中从零开始的字节偏移量</param>
        /// <param name="offset">当前ByteArray对象缓冲区从零开始的字节偏移量</param>
        /// <returns></returns>
        public virtual int CopyTo(byte[] dst, int dstOffset, int offset)
        {
            return CopyTo(Buffer, offset, dst, dstOffset, Length);
        }

        /// <summary>
        /// 将 ByteArray 对象的缓冲区中 offset 参数指定位置开始的缓冲区数据复制到
        /// 起始于 dstOffset 参数指定位置的 dst 参数指定的缓冲区中
        /// </summary>
        /// <param name="dst">要复制到的目标缓冲区</param>
        /// <param name="dstOffset"> dst 中从零开始的字节偏移量</param>
        /// <param name="srcOffset">当前ByteArray对象缓冲区从零开始的字节偏移量</param>
        /// <returns></returns>
        public static int CopyTo(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        {
            int len = Math.Min(dst.Length - dstOffset, count - srcOffset);

            if (len > 0)
            {
                System.Buffer.BlockCopy(src, srcOffset, dst, dstOffset, len);
            }
            else
            {
                len = 0;
            }

            return len;
        }

        #endregion 其他公共方法

        #region 读取字节数组方法

        /// <summary>
        /// 读取一个8位有符号整数(char)
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            if (Position >= Length) throw new Exception("遇到文件尾!");
            return (sbyte)Buffer[Position++];
        }

        /// <summary>
        /// 读取一个8位无符号整数(uchar)
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            if (Position >= Length) throw new Exception("遇到文件尾!");
            return (byte)Buffer[Position++];
        }

        /// <summary>
        /// 读取布尔值(bool)
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            if (Position >= Length) throw new Exception("遇到文件尾!");
            return Buffer[Position++] != 0;
        }

        /// <summary>
        /// 读取指定长度的字节数组
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int len, bool refreshEndian = true)
        {
            len = (len <= 0 ? Length - Position : len);

            if ((Position + len) > Length) throw new Exception("遇到文件尾!");

            byte[] bytes = new byte[len];
            Array.Copy(Buffer, Position, bytes, 0, len);
            if (refreshEndian) RefreshBytesByEndian(bytes);
            Position += len;

            return bytes;
        }

        /// <summary>
        /// 读取一个64位整数
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            byte[] bytes = ReadBytes(8);
            long temp = BitConverter.ToInt64(bytes, 0);
            temp = System.Net.IPAddress.NetworkToHostOrder(temp);
            return temp;
        }

        /// <summary>
        /// 读取一个64位整数
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            byte[] bytes = ReadBytes(8);
            long temp = BitConverter.ToInt64(bytes, 0);
            ulong tmp = (ulong)System.Net.IPAddress.NetworkToHostOrder(temp);
            return tmp;
        }

        /// <summary>
        /// 读取双精度浮点数
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            byte[] bytes = ReadBytes(8);
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// 读取单精度浮点数
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            byte[] bytes = ReadBytes(4);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// 读取一个32位有符号整数
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            byte[] bytes = ReadBytes(4);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 读取一个32位无符号整数
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt()
        {
            byte[] bytes = ReadBytes(4);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// 读取16位有符号整数
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            byte[] bytes = ReadBytes(2);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// 读取16位无符号整数
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            byte[] bytes = ReadBytes(2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// 读取一个 UTF-8 字符串
        /// </summary>
        /// <returns></returns>
        public string ReadUTF()
        {
            ushort len = ReadUShort();
            if ((Position + len) > Length) throw new Exception("遇到文件尾!");
            string temp = Encoding.UTF8.GetString(Buffer, Position, len);
            Position += len;
            return temp.Replace("\0", "");
        }

        /// <summary>
        /// 读取一个 GBK 字符串
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            ushort len = ReadUShort();
            if ((Position + len) > Length) throw new Exception("遇到文件尾!");
            string temp = GBKEncoder.Read(Buffer, Position, len);
            //string temp = Encoding.GetEncoding("GBK").GetString(Buffer, Position, len);
            Position += len;
            return temp;
        }

        ///// <summary>
        ///// 读取一个 DateTime 对象
        ///// </summary>
        ///// <returns></returns>
        //public DateTime ReadDate()
        //{
        //    return ReadDouble().Timer();
        //    //return new DateTime(ReadShort(), ReadByte(), ReadByte(), ReadByte(), ReadByte(), ReadByte());
        //}

        //public DateTime ReadShortDate()
        //{
        //    return ReadDouble().Timer();
        //    //return new DateTime(ReadShort(), ReadByte(), ReadByte(), 0, 0, 0);
        //}

        #endregion 读取字节数组方法

        #region 写入字节数组方法

        /// <summary>
        /// 写入一个8位有符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteSByte(sbyte value)
        {
            WriteByte((byte)value);
        }

        /// <summary>
        /// 写入一个8位无符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
        {
            WriteBytes(new byte[] { value });
            RefreshDataLength();
        }

        /// <summary>
        /// 写入一个布尔值
        /// </summary>
        /// <param name="value"></param>
        public void WriteBoolean(bool value)
        {
            WriteByte((value ? (byte)1 : (byte)0));
        }

        /// <summary>
        /// 写入字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        public void WriteBytes(byte[] bytes, int offset = 0, int len = 0)
        {
            if (len <= 0 || len > (bytes.Length - offset)) len = bytes.Length - offset;

            if (AutoResize)
            {
                bool resized = false;
                while (len > (BufferSize - Position)) //throw new Exception("写入长度超过字节数组总长度!");
                {
                    resized = true;
                    BufferSize += StepSize;
                }
                if (resized)
                {
                    byte[] temp = new byte[BufferSize];
                    Array.Copy(Buffer, 0, temp, 0, Length);
                    Buffer = temp;
                }
            }
            else
            {
                if (len > (BufferSize - Position)) throw new Exception("写入长度超过字节数组总长度!");
            }

            Array.Copy(bytes, offset, Buffer, Position, len);
            Position += len;
            RefreshDataLength();
        }

        /// <summary>
        /// 写入一个64位整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt64(long value)
        {
            long temp = System.Net.IPAddress.HostToNetworkOrder((long)value);
            byte[] bytes = BitConverter.GetBytes(temp);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个64位整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt64(ulong value)
        {
            long temp = System.Net.IPAddress.HostToNetworkOrder((long)value);
            byte[] bytes = BitConverter.GetBytes(temp);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个双精度浮点数
        /// </summary>
        /// <param name="value"></param>
        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个单精度浮点数
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个32位有符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个32位无符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个16位有符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 定稿一个16位无符号整数
        /// </summary>
        /// <param name="value"></param>
        public void WriteUShort(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            RefreshBytesByEndian(bytes);
            WriteBytes(bytes);
        }

        /// <summary>
        /// 写入一个 UTF-8 字符串
        /// </summary>
        /// <param name="value"></param>
        public void WriteUTF(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteUShort((ushort)0);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                WriteUShort((ushort)(bytes.Length));
                WriteBytes(bytes);
            }
        }

        /// <summary>
        /// 写入一个 GBK 字符串
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteUShort((ushort)0);
            }
            else
            {
                byte[] bytes = GBKEncoder.ToBytes(value);
                //byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(value);
                WriteUShort((ushort)bytes.Length);
                WriteBytes(bytes);
            }
        }

        //public void WriteDate(DateTime date)
        //{
        //    double timer = date.Timer();

        //    WriteDouble(timer);
        //}

        //public void WriteShortDate(DateTime date)
        //{
        //    double timer = date.Timer();

        //    WriteDouble(timer);
        //}

        #endregion 写入字节数组方法

        #region 压缩/解压缩

        /// <summary>
        /// 使用zlib格式压缩字节流,生成新的对象。
        /// </summary>
        /// <returns></returns>
        public ByteArray Compress()
        {
            byte[] bytes = Util.CompressZLib(Buffer, 0, Length);
            ByteArray byteArray = new ByteArray();
            byteArray.AutoResize = true;
            byteArray.WriteBytes(bytes, 0, bytes.Length);
            return byteArray;
        }

        /// <summary>
        /// 使用zlib格式解压缩字节流,生成新的对象。
        /// </summary>
        /// <returns></returns>
        public ByteArray Uncompress()
        {
            byte[] bytes = Util.UncompressZLib(Buffer, 0, Length);
            //ByteArray byteArray = new ByteArray();
            //byteArray.AutoResize = true;
            //byteArray.WriteBytes(bytes, 0, bytes.Length);
            //byteArray.Position = 0;
            return new ByteArray(bytes, bytes.Length);
        }

        #endregion 压缩/解压缩

        #region 辅助函数

        protected void RefreshDataLength()
        {
            Length = (Position > Length ? Position : Length);
        }

        protected void RefreshBytesByEndian(byte[] bytes)
        {
            if (Endian == EndianEnum.BIG_ENDIAN)
            {
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }
            }
            else if (Endian == EndianEnum.LITTLE_ENDIAN)
            {
                if (BitConverter.IsLittleEndian == false)
                {
                    Array.Reverse(bytes);
                }
            }
        }

        #endregion 辅助函数
    }
}