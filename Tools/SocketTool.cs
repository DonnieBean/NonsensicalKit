using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 连接本机socket服务端
    /// </summary>
    public class SocketTool:MonoBehaviour
    {
        private  int post;

        public Encoding encoding = Encoding.UTF8;

        private Socket socket;

        public Action onConnectSuccess;
        public Action<string> onConnectFail;
        public Action<string> onReceived;

        private Thread receiveThread;
        private Thread connectThread;

        public  bool state
        {
            get
            {
                return socket.Connected;
            }
        }

        public void Init(int post)
        {
            this.post = post;
        }

        Queue<string> datas=new Queue<string>();

        private void Update()
        {
            while (datas.Count>0)
            {
                onReceived?.Invoke(datas.Dequeue());
            }
        }

        private void OnDestroy()
        {
            connectThread?.Abort();
            receiveThread?.Abort();
        }
        public void Send(string msg)
        {
            if (socket!=null&&socket.Connected)
            {

                socket.Send(encoding.GetBytes(msg));
            }
        }

        public void Caoa(string str)
        {
            datas.Enqueue(str);
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
                    onConnectFail?.Invoke(address.ToString() + "\n错误原因: " + e.ToString());
                }

            }
            if (socket == null)
            {
                onConnectFail?.Invoke("无可用连接");
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
                    Caoa(resMsg);
                       //onReceived?.Invoke(resMsg);
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