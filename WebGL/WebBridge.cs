using NonsensicalKit;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebBridge : NonsensicalMono
{
    [DllImport("__Internal")]
    private static extern void sendMessageToJs(string key, string[] values);

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
            
            Publish(v[0], v);
        }
    }

    public void SendMessageToJS(string key, string[] values)
    {
        sendMessageToJs(key, values);
    }

    public void SendMessageToUnity(string str)
    {
        Debug.Log("webglÊý¾Ý  "+str);
        string[] values = str.Split('|');
        switch (values[0])
        {
            case "choiceFiles":
                ChoiceFiles(values[1]);
                break;
            case "urlQuery":
                Publish("urlQuery", values[1]);
                break;
            default:
                buffer.Enqueue(values);
                break;
        }
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
}
