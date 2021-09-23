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
        /// ������Ϣ�ص�Action
        /// </summary>
        public Action<bool> _SendMessageCallback;

        /// <summary>
        /// ������ϢAction
        /// </summary>
        public Action<string> _ReceiveMessage;

        private WebSocketWrap _ws;                  //websocketʵ��
        private bool _ReConnect = false;            //�߳�������
        private bool _isQuitting = false;           //�Ƿ������˳�Ӧ�ã����ڷ�ֹ���˳�ʱ����ʵ����������

        private Thread _HeartbeatThread;            //�����߳�

        private float _ReconnectionInterval;        //�������
        private float _HeartbeatInterval;           //�������


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
        /// �ǳ�ʱִ��
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
        /// ��ʼ��websocket������
        /// </summary>
        public void WebsocketConnect(string uri)
        {
            this.uri = uri;
            _ws = new WebSocketWrap();

            _ws.OnConnectError += () => { Debug.LogError($"Websocket��������"); };

            _ws.OnMessage += (message) => { Debug.Log("�յ���Ϣ��" + message); _ReceiveMessage?.Invoke(message); };

            _ws.OnWebSocketState += (state) =>
            {
                Debug.Log("Websocket״̬�ı䣺" + state.ToString());
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
        /// ����Э��
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReConnect()
        {
            yield return new WaitForSeconds(_ReconnectionInterval);
            WebsocketConnect(uri);
        }

        /// <summary>
        /// ��������Э��
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
        /// �����̷߳���
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
        /// �������������Ϣ
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
    /// ��װWebSocket
    /// </summary>
    public class WebSocketWrap
    {
        //����״̬�ı�
        public Action<WebSocketState> OnWebSocketState;
        //����ʧ��
        public Action OnConnectError;
        //���ݷ���
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
                Debug.LogFormat("[WebSocket] ��ʼ���� {0}", uri);
                ws = new ClientWebSocket();
                ct = new CancellationToken();
                Uri url = new Uri(uri);
                await ws.ConnectAsync(url, ct);

                if (OnWebSocketState != null)
                    OnWebSocketState(ws.State);

                while (true)
                {
                    var result = new byte[1024];
                    await ws.ReceiveAsync(new ArraySegment<byte>(result), new CancellationToken());//��������
                    var str = Encoding.UTF8.GetString(result, 0, result.Length);
                    str = str.Replace("\0", "");//ȥ��β�����ַ�

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

        // ��������
        public void SendHeartBeat()
        {
            if (ws == null || ws.State != WebSocketState.Open)
                return;

            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"id\":101}")), WebSocketMessageType.Text, true, ct);
        }

        // ��������
        public void Send(string data)
        {
            if (ws == null || ws.State != WebSocketState.Open)
            {
                Debug.LogErrorFormat("[WebSocket] ��������ʧ�ܣ�δ�����Ϸ�������");
                return;
            }
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, ct);
        }

        //��ֹWebSocket���Ӳ�ȡ���κι����IO����
        public void Abort()
        {
            if (ws == null)
                return;
            ws.Abort();
        }
    }

}