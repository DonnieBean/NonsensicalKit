using System;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NonsensicalKit.Manager
{

    public class WebSocketManager : NonsensicalManagerBase<WebSocketManager>
    {

        /// <summary>
        /// 发送消息回调Action
        /// </summary>
        public Action<bool> _SendMessageCallback;

        /// <summary>
        /// 接受消息Action
        /// </summary>
        public Action<string> _ReceiveMessage;

        private WebSocketWrap _ws;                  //websocket实例
        private bool _ReConnect = false;            //线程重连用
        private bool _isQuitting = false;           //是否正在退出应用，用于防止在退出时尝试实例化新物体

        private Thread _HeartbeatThread;            //心跳线程

        private float _ReconnectionInterval;        //重连间隔
        private float _HeartbeatInterval;           //心跳间隔


        private string uri;

        protected override void Awake()
        {
            base.Awake();
            _ReconnectionInterval = 2f;
            _HeartbeatInterval = 29f;
        }

        void Update()
        {
            if (_ReConnect)
            {
                StartCoroutine(ReConnect());
                _ReConnect = false;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _isQuitting = true;

            if (_HeartbeatThread != null)
            {
                _HeartbeatThread.Abort();
            }

            if (_ws != null)
            {
                _ws.Abort();
                _ReConnect = false;
            }
        }

        /// <summary>
        /// 登出时执行
        /// </summary>
        public void Abort()
        {
            if (_HeartbeatThread != null)
            {
                _HeartbeatThread.Abort();
            }

            if (_ws != null)
            {
                _ws.Abort();
                _ReConnect = false;
            }
        }

        /// <summary>
        /// 初始化websocket并连接
        /// </summary>
        public void WebsocketConnect(string uri)
        {
            this.uri = uri;
            _ws = new WebSocketWrap();

            _ws.OnConnectError += () => { Debug.LogError($"Websocket发生错误"); };

            _ws.OnMessage += (message) => { Debug.Log("收到消息：" + message); _ReceiveMessage?.Invoke(message); };

            _ws.OnWebSocketState += (state) =>
            {
                Debug.Log("Websocket状态改变：" + state.ToString());
                switch (state)
                {
                    case WebSocketState.Aborted:
                        break;
                    case WebSocketState.Closed:
                        if (!_isQuitting)
                        {
                            _ReConnect = true;
                        }
                        break;
                    case WebSocketState.CloseReceived:
                        break;
                    case WebSocketState.CloseSent:
                        break;
                    case WebSocketState.Connecting:
                        break;
                    case WebSocketState.None:
                        break;
                    case WebSocketState.Open:
                        break;
                    default:
                        break;
                }
            };
            _ws.Connect(uri);
            if (_HeartbeatThread != null)
            {
                _HeartbeatThread.Abort();
            }
            StartCoroutine(StartHeartbeat());
        }

        /// <summary>
        /// 重连协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReConnect()
        {
            yield return new WaitForSeconds(_ReconnectionInterval);
            WebsocketConnect(uri);
        }

        /// <summary>
        /// 开启心跳协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartHeartbeat()
        {
            while (_ws.IsConnected == false)
            {
                yield return new WaitForSeconds(0.5f);
            }
            _HeartbeatThread = new Thread(new ThreadStart(() => { Heartbeat(); }));
            _HeartbeatThread.Start();
        }

        /// <summary>
        /// 心跳线程方法
        /// </summary>
        private void Heartbeat()
        {
            while (_ws != null)
            {
                SendMessageToServer("");
                Thread.Sleep((int)(_HeartbeatInterval * 1000));
            }
        }

        /// <summary>
        /// 向服务器发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessageToServer(string msg)
        {
            _ws.Send(msg);
        }

        protected override void InitStart()
        {
            InitComplete();
        }

        protected override void LateInitStart()
        {
            LateInitComplete();
        }
    }

    /// <summary>
    /// 封装WebSocket
    /// </summary>
    public class WebSocketWrap
    {
        //连接状态改变
        public Action<WebSocketState> OnWebSocketState;
        //连接失败
        public Action OnConnectError;
        //数据返回
        public Action<string> OnMessage;

        public bool IsConnected
        {
            get
            {
                return ws.State == WebSocketState.Open;
            }
        }

        private ClientWebSocket ws;

        private CancellationToken ct;

        public async void Connect(string uri)
        {
            try
            {
                Debug.LogFormat("[WebSocket] 开始连接 {0}", uri);
                ws = new ClientWebSocket();
                ct = new CancellationToken();
                Uri url = new Uri(uri);
                await ws.ConnectAsync(url, ct);

                if (OnWebSocketState != null)
                    OnWebSocketState(ws.State);

                while (true)
                {
                    var result = new byte[1024];
                    await ws.ReceiveAsync(new ArraySegment<byte>(result), new CancellationToken());//接受数据
                    var str = Encoding.UTF8.GetString(result, 0, result.Length);
                    str = str.Replace("\0", "");//去掉尾部空字符

                        OnMessage?.Invoke(str);
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("[WebSocket] {0}\nCloseStatus: {1}\nCloseStatusDescription: {2}", ex.Message, ws.CloseStatus, ws.CloseStatusDescription);
                if (OnConnectError != null)
                    OnConnectError();
            }
            finally
            {
                if (ws != null)
                    ws.Dispose();
                ws = null;
            }
        }

        // 发送心跳
        public void SendHeartBeat()
        {
            if (ws == null || ws.State != WebSocketState.Open)
                return;

            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"id\":101}")), WebSocketMessageType.Text, true, ct);
        }

        // 发送数据
        public void Send(string data)
        {
            if (ws == null || ws.State != WebSocketState.Open)
            {
                Debug.LogErrorFormat("[WebSocket] 发送数据失败，未连接上服务器。");
                return;
            }
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, ct);
        }

        //中止WebSocket连接并取消任何挂起的IO操作
        public void Abort()
        {
            if (ws == null)
                return;
            ws.Abort();
        }
    }

}