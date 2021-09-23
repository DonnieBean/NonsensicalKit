using NonsensicalKit;
using NonsensicalKit.Manager;
using NonsensicalKit.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ProtocolsManager : NonsensicalManagerBase<ProtocolsManager>
{
    private Dictionary<string, Type> keyClassPair = new Dictionary<string, Type>();

    private Queue<Tuple<string, string>> buffer = new Queue<Tuple<string, string>>();

    MethodInfo deserializeMethod;
    protected override void Awake()
    {
        base.Awake();

        Subscribe<string, string>((uint)NonsensicalManagerEnum.ReceviedProtocolsMessage, OnReceivedProtocolsMessage);
        Subscribe<string, string>("ReceviedProtocolsMessage", OnReceivedProtocolsMessage);
    }

    private void Update()
    {
        while (buffer.Count > 0)
        {
            var crt = buffer.Dequeue();

            PublishMessage(crt.Item1, crt.Item2);
        }
    }

    private void OnReceivedProtocolsMessage(string key, string value)
    {
        buffer.Enqueue(new Tuple<string, string>(key, value));
    }

    private void PublishMessage(string key, string value)
    {
        if (keyClassPair.ContainsKey(key) == false)
        {
            Debug.LogWarning($"没有与键{key}对应的类");
            return;
        }
        Type pt = keyClassPair[key];

        MethodInfo deserializeMethod2 = JsonHelper.deserializeMethod.MakeGenericMethod(new Type[] { pt });
        object deserializeData = null;

        try
        {
            deserializeData = deserializeMethod2.Invoke(null, new object[] { value });
        }
        catch (Exception)
        {

        }

        if (deserializeData == null)
        {
            Debug.LogWarning($"无法反序列化字符串，类型：{pt}，字符串：{value}");
            return;
        }

        Type ma = typeof(MessageAggregator<>).MakeGenericType(pt);

        MethodInfo pubMethod = ma.GetMethod("Publish", new Type[] { typeof(string), pt });
        object instance = ma.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);

        pubMethod.Invoke(instance, new object[] { "IUseProtocols`1+OnReceivedMessage", deserializeData });
    }

    protected override void InitStart()
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var item in types)
        {
            var v = item.GetCustomAttribute<ProtocolsClassAttribute>();

            if (v != null)
            {
                if (keyClassPair.ContainsKey(v.key))
                {
                    Debug.LogWarning($"存在使用同一个键{v.key}的类{item}和{keyClassPair[v.key]}");
                    break;
                }
                keyClassPair.Add(v.key, item);
            }
        }

        InitComplete();
    }

    protected override void LateInitStart()
    {
        LateInitComplete();
    }
}

public class ProtocolsClassAttribute : Attribute
{
    public string key;
    public ProtocolsClassAttribute(string key)
    {
        this.key = key;
    }
}

public interface IUseProtocols<T> : ICustomEventHandler
{
    public void OnReceivedMessage(T value);
}
