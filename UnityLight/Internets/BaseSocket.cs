using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UnityLight.Internets
{
    public class BaseSocket
    {
        /// <summary>
        /// 网络套接字。
        /// </summary>
        public Socket Socket { get; protected set; }

        /// <summary>
        /// 网络端点(IP地址和端口号)。
        /// </summary>
        public IPEndPoint IPPoint { get; protected set; }
    }
}
