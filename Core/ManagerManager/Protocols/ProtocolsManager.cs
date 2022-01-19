using NonsensicalKit;
using NonsensicalKit.Manager;
using NonsensicalKit.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 通过类的名字为键，使用接口快速对接数据源
    /// </summary>
    public class ProtocolsManager : NonsensicalManagerBase<ProtocolsManager>
    {
        private Dictionary<string, Type> keyClassPair = new Dictionary<string, Type>();

        private Queue<Tuple<string, string>> buffer = new Queue<Tuple<string, string>>();

        protected override void Awake()
        {
            base.Awake();

            Subscribe<string, string>((uint)NonsensicalManagerEnum.ReceviedProtocolsMessage, OnReceivedProtocolsMessage);
            Subscribe<string, string>("ReceviedProtocolsMessage", OnReceivedProtocolsMessage);

            InitSubscribe(0, OnInitStart);
        }

        private void Update()
        {
            //通过这种方法传输的数据有可能是从子线程中的socket中传输过来的，需要缓存到下一个Update中执行防止出现错误
            while (buffer.Count > 0)
            {
                var crt = buffer.Dequeue();

                PublishMessage(crt.Item1, crt.Item2);
            }
        }

        private void  OnInitStart()
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
    }
}
