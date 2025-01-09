using System;
using System.Collections.Generic;
using System.Text;

namespace UnityLight.Internets
{
    public class PacketConfig
    {
        /// <summary>
        /// 数据包字节流缓冲区大小。
        /// </summary>
        public static int PackageBufSize = 4096;

        /// <summary>
        /// 数据包包头长度。
        /// </summary>
        public static int PackageHeaderSize = 20;

        /// <summary>
        /// 数据包包头识别常量。
        /// </summary>
        public static short PackageHeader = 0x7121;

        //加解密倍数;
        public static int CRYPT_MULITPER = 10110;
        //发送密钥;
        public static int SEND_CRYPT_KEY = 0x78102212;
        //接收密钥;
        public static int RECV_CRYPT_KEY = System.BitConverter.ToInt32(BitConverter.GetBytes(0x80880139), 0);
    }
}