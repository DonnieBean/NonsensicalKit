using NonsensicalKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WebTarget : NonsensicalMono
{
    private Queue<string> buffer = new Queue<string>();

    private void Update()
    {
        while (buffer.Count > 0)
        {
            string str = buffer.Dequeue();
            Publish("ReceviedSocketIOMessage", str);
        }
    }

    public void GetMessage(string msg)
    {
        //Debug.Log("socketIOÊý¾Ý" + msg);
        string str = msg.Replace("\\", "");
        buffer.Enqueue(str);
    }

    public void ChoiceFile(string nameWithUrl)
    {
        string[] ss = nameWithUrl.Split('|');

        if ((ss.Length & 1) == 1)
        {
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

            Publish("WebGLChoiceFile", new Tuple<List<string>, List<string>>(names, urls));
        }
    }
}
