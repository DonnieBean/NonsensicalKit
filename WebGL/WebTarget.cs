using NonsensicalKit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WebTarget : NonsensicalMono
{
    private Queue<string[]> buffer = new Queue<string[]>();

    private void Update()
    {
        while (buffer.Count > 0)
        {
            var v = buffer.Dequeue();

            Publish(v[0], v);
        }
    }

    public void SendMessageToUnity(string[] values)
    {
        switch (values[0])
        {
            case "ChoiceFiles":
                ChoiceFiles(values[1]);
                break;
            default:
                buffer.Enqueue(values);
                break;
        }
    }

    public void ChoiceFiles(string nameWithUrl)
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

        Publish("WebGLChoiceFile", new Tuple<List<string>, List<string>>(names, urls));
    }
}
