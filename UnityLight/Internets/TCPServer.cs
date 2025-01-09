using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using UnityLight.Loggers;

namespace UnityLight.Internets
{
    /// <summary>
    /// 异步接受到传入连接时的委托。
    /// </summary>
    /// <param name="sender">继承自 BasicServer 的网络服务器类。</param>
    /// <param name="socket">接受到的 Socket 连接对象</param>
    public delegate void AcceptHandler(object sender, Socket socket);

    /// <summary>
    /// 侦听被停止时的委托。
    /// </summary>
    public delegate void StopHandler();

    /// <summary>
    /// 网络服务器基类。
    /// 仅实现 Socket 监听。
    /// </summary>
    public class TCPServer : BaseSocket
    {
        /// <summary>
        /// 用于异步接受到传入连接后的事件。
        /// </summary>
        public event AcceptHandler Accepted;

        /// <summary>
        /// 用于侦听被停止后的事件。
        /// </summary>
        public event StopHandler Stopped;

        /// <summary>
        /// 挂起连接队列的最大长度。
        /// </summary>
        public int Backlog { get; protected set; }

        /// <summary>
        /// 指示套接字是否处于侦听状态。
        /// </summary>
        public bool Listening { get; protected set; }

        /// <summary>
        /// 指示套接字是否启动异步接受传入连接。
        /// </summary>
        public bool Accepting { get; protected set; }

        /// <summary>
        /// 异步套接字操作的 System.Net.Sockets.SocketAsyncEventArgs 对象。
        /// </summary>
        protected SocketAsyncEventArgs mAcceptAsyncEvent;

        public TCPServer()
        {
            Socket = null;
            IPPoint = null;
            Listening = false;
            Accepting = false;
            mAcceptAsyncEvent = null;
        }

        public virtual bool Listen(int nPort, int nBacklog)
        {
            if (Listening) throw new Exception("套接字处于侦听状态，无法重复侦听。");

            IPPoint = new IPEndPoint(IPAddress.Any, nPort);

            Backlog = nBacklog;

            if (Socket == null) Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                Socket.Bind(IPPoint);

                Socket.Listen(Backlog);

                Listening = true;

                AcceptAsync();
            }
            catch (Exception ex)
            {
                Listening = false;

                throw ex;
            }

            return Listening;
        }

        /// <summary>
        /// 启动异步接受传入连接。
        /// </summary>
        public virtual void AcceptAsync()
        {
            if (Listening == false) throw new Exception("套接字未处于侦听状态，无法启动异步接受传入连接。请使用 Listen(int, int) 方法将套接字置于侦听状态。");

            if (Accepting) throw new Exception("套接字处于异步接受传入连接状态，无法重复启动。");

            Accepting = true;

            if (mAcceptAsyncEvent == null)
            {
                mAcceptAsyncEvent = new SocketAsyncEventArgs();
                mAcceptAsyncEvent.Completed += new EventHandler<SocketAsyncEventArgs>(mAcceptAsyncEvent_Completed);
            }

            AcceptAsyncImp();
        }

        private void AcceptAsyncImp()
        {
            if (mAcceptAsyncEvent == null) return;

            mAcceptAsyncEvent.AcceptSocket = null;

            if (Socket.AcceptAsync(mAcceptAsyncEvent) == false)
            {//I/O 操作同步完成
                mAcceptAsyncEvent_Completed(Socket, mAcceptAsyncEvent);
            }
        }

        private void mAcceptAsyncEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            System.Net.Sockets.Socket sock = e.AcceptSocket;

            if (sock != null)
            {//处理传入的客户端 Socket 连接对象
                if (sock.Connected)
                {
                    try
                    {
                        OnAccepted(sock);
                    }
                    catch (Exception ex)
                    {
                        XLogger.Error("处理接受传入连接时错误!IP：" + ((IPEndPoint)sock.RemoteEndPoint).Address.ToString(), ex);

                        if (sock.Connected) sock.Close();
                    }

                    AcceptAsyncImp();
                }
                else
                {
                    try
                    {
                        OnStopped();
                    }
                    catch (Exception ex)
                    {
                        XLogger.Error("处理停止监听时错误!IP：" + ((IPEndPoint)sock.RemoteEndPoint).Address.ToString(), ex);

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 接受到传入连接后的处理方法。
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void OnAccepted(Socket socket)
        {
            if (Accepted != null) Accepted(this, socket);
        }

        /// <summary>
        /// 停止侦听并释放所有关联资源。
        /// </summary>
        public virtual void Stop()
        {
            if (Listening && Socket != null)
            {
                try { Socket.Close(); }
                catch { }
                Socket = null;
            }
        }

        /// <summary>
        /// 侦听被停止后的处理方法。
        /// </summary>
        protected virtual void OnStopped()
        {
            if (Stopped != null) Stopped();

            Socket = null;
            IPPoint = null;
            Listening = false;
            Accepting = false;
            mAcceptAsyncEvent = null;
        }
    }
}