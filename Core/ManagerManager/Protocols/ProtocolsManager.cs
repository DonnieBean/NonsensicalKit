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
    /// ͨ���������Ϊ����ʹ�ýӿڿ��ٶԽ�����Դ
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
            //ͨ�����ַ�������������п����Ǵ����߳��е�socket�д�������ģ���Ҫ���浽��һ��Update��ִ�з�ֹ���ִ���
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
                        Debug.LogWarning($"����ʹ��ͬһ����{v.key}����{item}��{keyClassPair[v.key]}");
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
                Debug.LogWarning($"û�����{key}��Ӧ����");
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
                Debug.LogWarning($"�޷������л��ַ��������ͣ�{pt}���ַ�����{value}");
                return;
            }

            Type ma = typeof(MessageAggregator<>).MakeGenericType(pt); 

             MethodInfo pubMethod = ma.GetMethod("Publish", new Type[] { typeof(string), pt });
            object instance = ma.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);

            pubMethod.Invoke(instance, new object[] { "IUseProtocols`1+OnReceivedMessage", deserializeData });
        }
    }
}
