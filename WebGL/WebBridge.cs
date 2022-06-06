using NonsensicalKit;
using NonsensicalKit.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// <summary>
/// 用于规范化与webgl的js代码通讯
/// </summary>
public class WebBridge : MonoSingleton<WebBridge>
{
    [DllImport("__Internal")]
    private static extern void sendMessageToJS(string key, string values);

    [DllImport("__Internal")]
    private static extern void sendMessageToJsTest(string key, string values);

    private Queue<string[]> buffer = new Queue<string[]>();

    protected override void Awake()
    {
        base.Awake();
         
        Subscribe<string, string[]>("SendMessageToJS", SendMessageToJS);
    }

    private void Update()
    {
        while (buffer.Count > 0)
        {
            var v = buffer.Dequeue();
            switch (v[0])
            {
                case "choiceFiles":
                    ChoiceFiles(v[1]);
                    break;
                case "urlQuery":
                    Publish("urlQuery", v[1]);
                    break;
                case "socketIOMessage":
                    Publish("socketIOMessage", v[1],v[2]);
                    break;
                default:
                    Publish(v[0], v);
                    break;
            }
        }
    }

    #region SendMethod
    public void SendMessageToJS(string key, string[] values)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in values)
        {
            sb.Append(item);
            sb.Append("|");
        }
        sendMessageToJS(key, sb.ToString()) ;
    }
    public void SendMessageToJS(string key, ArrayList values)
    {
        sendMessageToJsTest(key, JsonHelper.SerializeObject(values)) ;
    }

    public void SendChoiceFile(string type, bool isMultiple)
    {
        SendMessageToJS("fileSelector", new string[] { type, isMultiple ? "true" : "false" });
    }
    public void SendChoiceFileTest(string type, bool isMultiple)
    {
        SendMessageToJS("fileSelector", new ArrayList { type, isMultiple  });
    }
    public void ConnectSocketIO(string url)
    {
        SendMessageToJS("socketIO", new string[] { "connectSocketIO" , url });
    }

    public void SocketIOAddListener(string eventName)
    {
        SendMessageToJS("socketIO", new string[] { "addListener", eventName });
    }

    public void SocketIOSendMessage(string eventName, string msg)
    {
        SendMessageToJS("socketIO", new string[] { "sendMessage", eventName,msg });
    }
    #endregion


    #region ReceiveMethod
    public void SendMessageToUnity(string str)
    {
        Debug.Log("webgl数据  " + str);
        string[] values = str.Split('|');
        buffer.Enqueue(values);
    }

    private void ChoiceFiles(string nameWithUrl)
    {
        string[] ss = nameWithUrl.Split('|');

        List<string> names = new List<string>();
        List<string> urls = new List<string>();
        for (int i = 0; i < ss.Length - 1; i++)
        {
            if ((i & 1) == 0)
            {
                names.Add(ss[i]);
            }
            else
            {
                urls.Add(ss[i]);
            }
        }

        Publish("JSChoiceFile", new Tuple<List<string>, List<string>>(names, urls));
    }

    #endregion
}
