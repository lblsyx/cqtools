using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityLight.Loggers;

namespace UnityLight.Internets
{
    public delegate void ClientHandler(TCPClient client);

    public class TCPClient : BaseSocket
    {
        /// <summary>
        /// 数据传输协议对象。
        /// </summary>
        public IProtocol Protocol { get; private set; }

        public bool SendingTCP { get; private set; }

        public byte TargetID1 { get; set; }

        public byte TargetID2 { get; set; }

        /// <summary>
        /// 当前客户端的ID1。
        /// </summary>
        public uint OwnerID1 { get; set; }

        /// <summary>
        /// 当前客户端的ID2。
        /// </summary>
        public uint OwnerID2 { get; set; }

        /// <summary>
        /// 是否已关闭，0表示未关闭，1表示已关闭。
        /// </summary>
        protected int mClosed;

        /// <summary>
        /// 是否正在关闭，0表示不关闭，1表示正在关闭，2表示已关闭。
        /// </summary>
        protected int mClosing;

        /// <summary>
        /// 用于客户端连接关闭的事件。
        /// </summary>
        public event ClientHandler Closed;

        /// <summary>
        /// 接收数据时断包缓冲区位置。
        /// </summary>
        protected int mDataRemain;

        /// <summary>
        /// 正在发送的数据大小。
        /// </summary>
        protected int mSendingSize;

        /// <summary>
        /// 待发送的数据包队列。
        /// </summary>
        protected Queue mPacketQueue;
       
        /// <summary>
        /// 套接字异步发送数据缓冲区。
        /// </summary>
        protected ByteArray mSendByteArray;

        /// <summary>
        /// 套接字异步接收数据缓冲区。
        /// </summary>
        protected ByteArray mRecvByteArray;

        /// <summary>
        /// 异步发送数据事件参数对象
        /// </summary>
        protected SocketAsyncEventArgs mSendAsyncEventArgs;

        /// <summary>
        /// 异步接收数据事件参数对象
        /// </summary>
        protected SocketAsyncEventArgs mRecvAsyncEventArgs;


        public FSM mSendFSM;

        public FSM mRecvFSM;

        public Dictionary<uint, int> mPkgCountDic = new Dictionary<uint, int>();

        public bool mIsPkgCount = false;


        public TCPClient(IProtocol iIProtocol)
        {
            Protocol = iIProtocol;

            mDataRemain = 0;
            mSendingSize = 0;
            mPacketQueue = new Queue();
            mSendByteArray = new ByteArray(PacketConfig.PackageBufSize * 4);
            mRecvByteArray = new ByteArray(PacketConfig.PackageBufSize * 4);

            mSendAsyncEventArgs = new SocketAsyncEventArgs();
            mSendAsyncEventArgs.UserToken = this;
            mSendAsyncEventArgs.SetBuffer(mSendByteArray.Buffer, 0, 0);
            mSendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(mSendAsyncEventArgs_Completed);

            mRecvAsyncEventArgs = new SocketAsyncEventArgs();
            mRecvAsyncEventArgs.UserToken = this;
            mRecvAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(mRecvAsyncEventArgs_Completed);

            mSendFSM = new FSM("Send");
            mRecvFSM = new FSM("Recv");
        }

        ///// <summary>
        ///// 每发送一个完整的数据包后调用执行一次。
        ///// </summary>
        ///// <param name="pkg">已发送的数据包对象。</param>
        //protected virtual void OnePackageSended(Packet pkg)
        //{
        //}

        ///// <summary>
        ///// 每接收一个完整的数据包后调用执行一次。
        ///// </summary>
        ///// <param name="pkg">已接收的数据包对象。</param>
        //protected virtual void OnePackageRecved(Packet pkg)
        //{
        //}

        #region TCP端发送Packet数据包

        
        public void SendTCP(Packet pkg)
        {
            SendTCP(pkg, TargetID1, TargetID2);
        }
        
        public void SendTCP(Packet pkg, byte targetID1)
        {
            SendTCP(pkg, targetID1, TargetID2);
        }

        /// <summary>
        /// 发送TCP数据包。
        /// </summary>
        /// <param name="pkg">要发送的数据包对象。</param>
        public void SendTCP(Packet pkg, byte targetID1, byte targetID2)
        {
            if (pkg == null || Socket == null || Socket.Connected == false) return;

            int rlt = Interlocked.CompareExchange(ref mClosing, 1, 1);

            if (rlt == 1)
            {
                XLogger.ErrorFormat("Socket连接即将关闭，无法继续发送数据!IP:{0}；PacketID={1}", IPPoint.Address.ToString(), pkg.PacketID);
                return;
            }

            lock (mPacketQueue.SyncRoot)
            {
                try
                {
                    if (Protocol != null)
                    {
                        Packet packet = pkg.Clone();
                        packet.OwnerID1 = OwnerID1;
                        packet.OwnerID2 = OwnerID2;

                        if (targetID1 != 0) packet.TargetID1 = targetID1;
                        if (targetID2 != 0) packet.TargetID2 = targetID2;

                        mPacketQueue.Enqueue(packet);

                        if (SendingTCP) return;
                        SendingTCP = true;

                        ThreadPool.QueueUserWorkItem(new WaitCallback(SendAsyncImp), this);
                    }
                }
                catch (Exception ex)
                {
                    XLogger.ErrorFormat("数据包发送失败!原因:{0}", ex.Message);
                }
            }
        }

        private static void SendAsyncImp(object target)
        {
            TCPClient client = target as TCPClient;

            try
            {
                mSendAsyncEventArgs_Completed(client.Socket, client.mSendAsyncEventArgs);
            }
            catch (Exception ex)
            {
                XLogger.Error("数据包发送失败!", ex);
                client.Close();
            }
        }

        private static void mSendAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            TCPClient client = e.UserToken as TCPClient;

            Queue q = client.mPacketQueue;

            try
            {
                lock (q.SyncRoot)
                {
                    int nSended = e.BytesTransferred;

                    ByteArray oSendByteArray = client.mSendByteArray;
                    int nSendCount = client.mSendingSize - nSended;

                    if (nSendCount > 0)
                    {
                        Array.Copy(oSendByteArray.Buffer, nSended, oSendByteArray.Buffer, 0, nSendCount);
                        oSendByteArray.WrapBuffer(oSendByteArray.Buffer, nSendCount);
                    }
                    else
                    {
                        nSendCount = 0;
                    }

                    oSendByteArray.Position = nSendCount;

                    e.SetBuffer(0, 0);

                    int nWriteSize = 0;
                    Packet pkg;
                    Packet tmp;
                    while (q.Count > 0)
                    {
                        pkg = q.Peek() as Packet;

                        if (client.Protocol != null)
                        {
                            nWriteSize = client.Protocol.OnSendData(client, pkg, oSendByteArray);
                            if (nWriteSize > 0)
                            {
                                nSendCount += nWriteSize;
                                tmp = q.Dequeue() as Packet;
                                //client.OnePackageSended(tmp);
                                //XLogger.DebugFormat("已发送一个数据包!PacketID：{0}，OwnerID1：{1}，OwnerID2：{2}，TargetID1：{3}，TargetID2：{4}。", tmp.PacketID, tmp.OwnerID1, tmp.OwnerID2, tmp.TargetID1, tmp.TargetID2);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            XLogger.ErrorFormat("数据传输协议对象不存在,无法发送数据!");
                        }
                    }

                    if (nSendCount <= 0)
                    {
                        client.SendingTCP = false;
                        client.mSendingSize = 0;
                        oSendByteArray.Reset();

                        int rlt = Interlocked.CompareExchange(ref client.mClosing, 1, 1);

                        if (rlt == 1)
                        {
                            client.CloseSocket();
                        }

                        return;
                    }

                    client.mSendingSize = nSendCount;
                    e.SetBuffer(0, nSendCount);

                    if (client.Socket.SendAsync(e) == false)
                    {
                        mSendAsyncEventArgs_Completed(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ObjectDisposedException)
                {
                    XLogger.Error("Socket连接已关闭，无法发送!", ex);
                }
                else
                {
                    XLogger.Error("数据包发送失败!", ex);
                }
                //if (LibConfig.SocketStrict) player.Close(true);
                client.Close(true);
            }
        }

        #endregion TCP端发送Packet数据包

        #region TCP端连接初始化

        public void Initialize()
        {
            Socket = null;
            IPPoint = null;

            Closed = null;

            lock (mPacketQueue.SyncRoot)
            {
                mPacketQueue.Clear();
                SendingTCP = false;
                mSendingSize = 0;
                mDataRemain = 0;
            }

            Interlocked.Exchange(ref mClosed, 0);
            Interlocked.Exchange(ref mClosing, 0);
        }

        public virtual bool Connect(string ip, int port)
        {
            if (Socket != null && Socket.Connected) throw new Exception("TCP端已连接，无法重复连接!");

            IPPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                sock.Connect(IPPoint);

                Accept(sock);

                mSendFSM.Reset(PacketConfig.RECV_CRYPT_KEY, PacketConfig.CRYPT_MULITPER);
                mRecvFSM.Reset(PacketConfig.SEND_CRYPT_KEY, PacketConfig.CRYPT_MULITPER);

                return true;
            }
            catch (Exception ex)
            {
                XLogger.Error("远程主机连接失败!IP：" + IPPoint.Address.ToString() + "，Port：" + IPPoint.Port, ex);
            }

            return false;
        }

        public virtual void Accept(Socket socket)
        {
            if (Socket != null) throw new Exception("TCP端已连接，无法重复连接!");

            Socket = socket;

            mSendingSize = 0;
            SendingTCP = false;
            Interlocked.Exchange(ref mClosed, 0);
            Interlocked.Exchange(ref mClosing, 0);
            if (IPPoint == null)
            {
                IPEndPoint ipep = (IPEndPoint)Socket.RemoteEndPoint;
                IPPoint = new IPEndPoint(ipep.Address, ipep.Port);
            }

            RecvAsyncImp();
        }

        #endregion TCP端连接初始化

        #region 异步接收Socket数据

        private void RecvAsyncImp()
        {
            if (Socket != null && Socket.Connected)
            {
                if (mDataRemain >= mRecvByteArray.BufferSize)
                {
                    XLogger.Error("无可用缓冲区接收数据.");
                    //if (SLConfig.SocketStrict) Close();
                    Close();
                }
                else
                {
                    mRecvAsyncEventArgs.SetBuffer(mRecvByteArray.Buffer, mDataRemain, mRecvByteArray.BufferSize - mDataRemain);

                    if (Socket.ReceiveAsync(mRecvAsyncEventArgs) == false)
                    {
                        mRecvAsyncEventArgs_Completed(Socket, mRecvAsyncEventArgs);
                    }
                }
            }
        }

        private void mRecvAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                int nums = e.BytesTransferred;

                if (nums > 0)
                {
                    if (nums > 0)
                    {
                        int recvSize = mDataRemain + nums;
                        if (recvSize < PacketConfig.PackageHeaderSize)
                        {
                            mDataRemain = recvSize;
                            return;
                        }

                        mDataRemain = 0;
                        if (Protocol != null)
                        {
                            //Console.WriteLine("RecvAsync:recvSize:{0}", recvSize);
                            mRecvByteArray.WrapBuffer(mRecvByteArray.Buffer, recvSize);
                            mRecvByteArray.Position = 0;
                            int uParseLen = Protocol.OnRecvData(this, mRecvByteArray);
                            mDataRemain = recvSize - uParseLen;
                            if (mDataRemain > 0)
                            {
                                mRecvByteArray.CopyFrom(mRecvByteArray.Buffer, uParseLen, 0, mDataRemain);
                            }     
                        }
                        else
                        {
                            Console.WriteLine("数据传输协议对象不存在,无法接收数据!");
                        }
                    }

                    RecvAsyncImp();
                }
                else
                {
                    Interlocked.Exchange(ref mClosed, 1);
                    Interlocked.Exchange(ref mClosing, 2);

                    OnClosed();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("mRecvAsyncEventArgs_Completed[{0}]", ex);
                //XLogger.ErrorFormat("{0} mRecvAsyncEventArgs_Completed:{1}", IPPoint.ToString(), ex);
                //if (LibConfig.SocketStrict) Close();
                Close();
            }
        }

        public virtual bool RecvPacket(Packet pkg)
        {
            return false;
        }

        #endregion 异步接收Socket数据

        #region TCP端关闭连接

        public void Close(bool bForce = false)
        {
            int rlt = Interlocked.CompareExchange(ref mClosing, 1, 0);

            if (rlt == 0)
            {
                lock (mPacketQueue.SyncRoot)
                {
                    if (SendingTCP && bForce == false)
                    {
                        return;
                    }

                    CloseSocket();
                }
            }
        }

        protected void CloseSocket()
        {
            int nCloseResult = Interlocked.Exchange(ref mClosed, 1);

            if (nCloseResult == 0)
            {
                Interlocked.Exchange(ref mClosing, 2);

                try
                {
                    Socket.Shutdown(SocketShutdown.Both);
                }
                catch// (Exception ex)
                { }

                try
                {
                    Socket.Close();
                }
                catch// (Exception ex)
                { }

                OnClosed();
            }
        }

        protected void OnClosed()
        {
            lock (mPacketQueue.SyncRoot)
            {
                mPacketQueue.Clear();
                SendingTCP = false;
                mSendingSize = 0;
                mDataRemain = 0;
            }

            if (Socket != null)
            {
                Socket = null;
                XLogger.DebugFormat("连接已关闭!Remote:" + (IPPoint == null ? "" : IPPoint.ToString()));
            }

            IPPoint = null;

            try
            {
                if (Closed != null)
                {
                    Closed(this);
                }
            }
            catch (Exception ex)
            {
                XLogger.Error("执行关闭事件失败!", ex);
            }

            //Interlocked.Exchange(ref mClosed, 0);
            //Interlocked.Exchange(ref mClosing, 0);
        }

        #endregion TCP端关闭连接
    }
}