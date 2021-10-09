using Newtonsoft.Json.Linq;
using NonsensicalKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTarget : NonsensicalMono
{
    public void GetMessage(string msg)
    {
        string str = msg.Replace("\\", "");
        try
        {
            JObject jo = JObject.Parse(str);

            if (jo.ContainsKey("DeviceCode") == false)
            {
                //Debug.Log("不包含DeviceCode字段");
                return;
            }
            var deviceCode = jo["DeviceCode"];
            NonsensicalKit.MessageAggregator<string, string>.Instance.Publish("ReceviedProtocolsMessage", deviceCode.ToString(), str);
        }
        catch (Exception e)
        {
            //Debug.Log("转换出现问题" + e.ToString());
        }
    }

    public void ChoiceFile(string nameWithUrl)
    {
        string[] ss = nameWithUrl.Split('|');
        
        if ((ss.Length&1)==1)
        {
            List<string> names = new List<string>();
            List<string> urls = new List<string>();
            for (int i = 0; i < ss.Length-1; i++)
            {
                if ((i&1)==0)
                {
                    names.Add(ss[i]);
                }
                else
                {
                    urls.Add(ss[i]);
                }
            }

            Publish("ChoiceFile", new Tuple<List<string>, List<string>>(names, urls));
        }
        
    }
}
