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
        public Action<string> onReceived;
        

        private ScoektClientInstace sci;
        private Queue<string> datas = new Queue<string>();

        public void Init(int port)
        {
            sci = new ScoektClientInstace();
            sci.SocketConnectAsync(port);
            sci.onConnectSuccess += () => { Debug.Log("连接成功"); };
            sci.onConnectFail += (msg)=> { Debug.LogWarning(msg); };
            sci.onReceived += (msg)=> { Debug.Log("收到消息"); datas.Enqueue(msg); };
        }
        private void Update()
        {
            while (datas.Count>0)
            {
                string str = datas.Dequeue();
                onReceived?.Invoke(str);
            }
        }

        private void OnDestroy()
        {
            Abort();
        }
        public void Send(string msg)
        {

            sci.Send(msg);
        }

        public void Abort()
        {
            sci?.About();
        }
    }
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }

    public class ScoektClientInstace
    {
        public readonly Encoding encoding = Encoding.UTF8;

        public Action onConnectSuccess;
        public Action<string> onConnectFail;
        public Action<string> onReceived;

        private Socket socket;

        public bool Connected
        {
            get
            {
                return socket.Connected;
            }
        }


        public async void SocketConnectAsync(int post)
        {
            string host = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(host);
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, post);
                Socket tempSocket =   new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                   await  tempSocket.ConnectAsync(ipe);

                    if (tempSocket.Connected)
                    {
                        onConnectSuccess?.Invoke();
                        socket = tempSocket;
                        ReceiveMsg();
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    onConnectFail?.Invoke(address.ToString() + "连接失败\n错误原因: " + e.ToString());
                }

            }
            if (socket == null)
            {
                onConnectFail?.Invoke("无可用连接");
            }
        }

      private   void ReceiveMsg()
        {
            StateObject state = new StateObject();
            state.workSocket = socket;
            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallBack), state);
           
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            string content = string.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));
                content = state.sb.ToString();

                onReceived?.Invoke(content);

                state.sb.Clear();
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallBack), state);
            }
            else
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                Debug.LogError("收到数据为空");
            }
        }
        public void Send(string msg)
        {
            if (socket != null && socket.Connected)
            {
                socket.Send(encoding.GetBytes(msg));
            }
        }

        public void About()
        {
            socket?.Shutdown(SocketShutdown.Both);
            socket?.Close();
        }
    }
}