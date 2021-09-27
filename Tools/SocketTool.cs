using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// ���ӱ���socket�����
    /// </summary>
    public class SocketTool
    {
        private readonly int post;

        public Encoding encoding = Encoding.UTF8;

        private Socket socket;

        public Action onConnectSuccess;
        public Action<string> onConnectFail;
        public Action<string> onReceived;

        private Thread receiveThread;
        private Thread connectThread;

        public SocketTool(int post)
        {
            this.post = post;
        }

        ~SocketTool()
        {
            connectThread?.Abort();
            receiveThread?.Abort();
        }

        public void Send(string msg)
        {
            socket.Send(encoding.GetBytes(msg));
        }

        public void Connect()
        {
            connectThread = new Thread(new ThreadStart(SocketConnect));
            connectThread.Start();
        }

        public void SocketConnect()
        {
            string host = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(host);
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, post);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    tempSocket.Connect(ipe);

                    if (tempSocket.Connected)
                    {
                        onConnectSuccess?.Invoke();
                        socket = tempSocket;
                        receiveThread = new Thread(new ThreadStart(ReceiveMsg));
                        receiveThread.Start();

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    onConnectFail?.Invoke(address.ToString() + "\n����ԭ��: " + e.ToString());
                }

            }
            if (socket == null)
            {
                onConnectFail?.Invoke("�޿�������");
            }
        }

        void ReceiveMsg()
        {
            while (true)
            {
                byte[] buffer = new byte[socket.ReceiveBufferSize];
                int length = socket.Receive(buffer);
                string resMsg = encoding.GetString(buffer, 0, length);
                if (string.IsNullOrEmpty(resMsg) == false)
                {
                    onReceived?.Invoke(resMsg);
                }
            }
        }

        public void Abort()
        {
            socket?.Close();
            connectThread?.Abort();
            receiveThread?.Abort();
        }
    }
}