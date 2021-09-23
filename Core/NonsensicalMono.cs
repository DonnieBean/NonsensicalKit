using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 省略消息的注销，同时可以自动注册继承了ICustomEventHandler的接口的方法
    /// </summary>
    public abstract class NonsensicalMono : MonoBehaviour
    {
        /// <summary>
        /// 放于构造函数时可以使用
        /// 但无法确认放在构造函数中是否存在问题
        /// </summary>
        //protected NonsensicalMono()
        //{
        //    InitInterfaceHandler();
        //}

        private List<ListenerInfo> listenerInfos = new List<ListenerInfo>();

        public Action DestroyAction;

        protected virtual void Awake()
        {
            InitCustomEventHandler();
        }

        protected virtual void OnDestroy()
        {
            foreach (var listener in listenerInfos)
            {
                bool isUint = listener.UseUint;
                Type[] types = listener.Types;
                Type messageAggregator;
                object instance;
                Type messageHandler;
                switch (types.Length)
                {
                   case  0:
                        {
                            messageAggregator = typeof(MessageAggregator);
                            instance = messageAggregator.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                            messageHandler = typeof(MessageHandler);
                        }
                        break;
                    case 1:
                        {
                            messageAggregator = typeof(MessageAggregator<>).MakeGenericType(types);
                            instance = messageAggregator.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                            messageHandler = typeof(MessageHandler<>).MakeGenericType(types);
                        }
                        break;
                    case 2:
                        {
                            messageAggregator = typeof(MessageAggregator<,>).MakeGenericType(types);
                            instance = messageAggregator.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                            messageHandler = typeof(MessageHandler<,>).MakeGenericType(types);
                        }
                        break;
                    case 3:
                        {
                            messageAggregator = typeof(MessageAggregator<,,>).MakeGenericType(types);
                            instance = messageAggregator.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                            messageHandler = typeof(MessageHandler<,,>).MakeGenericType(types);
                        }
                        break;
                    default:
                        continue;
                }
                if (isUint)
                {
                    MethodInfo unsubMethod = messageAggregator.GetMethod("Unsubscribe", new Type[] { typeof(uint), messageHandler });
                    unsubMethod.Invoke(instance, new object[] { listener.Index, listener.Func });
                }
                else
                {
                    MethodInfo unsubMethod = messageAggregator.GetMethod("Unsubscribe", new Type[] { typeof(string), messageHandler });
                    unsubMethod.Invoke(instance, new object[] { listener.Str, listener.Func });
                }
            }

            DestroyAction?.Invoke();
        }


        #region Subscribe
        protected void Subscribe<T1, T2, T3>(uint index, MessageHandler<T1, T2, T3> func)
        {
            MessageAggregator<T1, T2, T3>.Instance.Subscribe(index, func);

            ListenerInfo temp = new ListenerInfo(index, func, typeof(T1), typeof(T2), typeof(T3));
            listenerInfos.Add(temp);
        }
        protected void Subscribe<T1, T2>(uint index, MessageHandler<T1, T2> func)
        {
            MessageAggregator<T1, T2>.Instance.Subscribe(index, func);

            ListenerInfo temp = new ListenerInfo(index, func, typeof(T1), typeof(T2));
            listenerInfos.Add(temp);
        }
        protected void Subscribe<T>(uint index, MessageHandler<T> func)
        {
            MessageAggregator<T>.Instance.Subscribe(index, func);

            ListenerInfo temp = new ListenerInfo(index, func, typeof(T));
            listenerInfos.Add(temp);
        }
        protected void Subscribe(uint index, MessageHandler func)
        {
            MessageAggregator.Instance.Subscribe(index, func);

            ListenerInfo temp = new ListenerInfo(index, func);
            listenerInfos.Add(temp);
        }
        protected void Subscribe<T1, T2, T3>(string str, MessageHandler<T1, T2, T3> func)
        {
            MessageAggregator<T1, T2, T3>.Instance.Subscribe(str, func);

            ListenerInfo temp = new ListenerInfo(str, func, typeof(T1), typeof(T2), typeof(T3));
            listenerInfos.Add(temp);
        }
        protected void Subscribe<T1, T2>(string str, MessageHandler<T1, T2> func)
        {
            MessageAggregator<T1, T2>.Instance.Subscribe(str, func);

            ListenerInfo temp = new ListenerInfo(str, func, typeof(T1), typeof(T2));
            listenerInfos.Add(temp);
        }
        protected void Subscribe<T>(string str, MessageHandler<T> func)
        {
            MessageAggregator<T>.Instance.Subscribe(str, func);

            ListenerInfo temp = new ListenerInfo(str, func, typeof(T));
            listenerInfos.Add(temp);
        }
        protected void Subscribe(string str, MessageHandler func)
        {
            MessageAggregator.Instance.Subscribe(str, func);

            ListenerInfo temp = new ListenerInfo(str, func);
            listenerInfos.Add(temp);
        }
        #endregion

        #region Publish
        protected void Publish<T1, T2, T3>(uint index, T1 data1, T2 data2, T3 data3)
        {
            MessageAggregator<T1, T2, T3>.Instance.Publish(index, data1, data2, data3);
        }
        protected void Publish<T1, T2>(uint index, T1 data1, T2 data2)
        {
            MessageAggregator<T1, T2>.Instance.Publish(index, data1, data2);
        }
        protected void Publish<T>(uint index, T data)
        {
            MessageAggregator<T>.Instance.Publish(index, data);
        }
        protected void Publish(uint index)
        {
            MessageAggregator.Instance.Publish(index);
        }
        protected void Publish<T1, T2, T3>(string str, T1 data1, T2 data2, T3 data3)
        {
            MessageAggregator<T1, T2, T3>.Instance.Publish(str, data1, data2, data3);
        }
        protected void Publish<T1, T2>(string str, T1 data1, T2 data2)
        {
            MessageAggregator<T1, T2>.Instance.Publish(str, data1, data2);
        }
        protected void Publish<T>(string str, T data)
        {
            MessageAggregator<T>.Instance.Publish(str, data);
        }
        protected void Publish(string str)
        {
            MessageAggregator.Instance.Publish(str);
        }
        #endregion

        private void InitCustomEventHandler()
        {
            var interfaces = this.GetType().GetInterfaces();
            foreach (var crtInterface in interfaces)
            {
                if (crtInterface.GetInterface(nameof(ICustomEventHandler)) == null)
                {
                    continue;
                }

                MethodInfo[] MIs = crtInterface.GetMethods();

                foreach (var crtMI in MIs)
                {
                    ParameterInfo[] PIs = crtMI.GetParameters();
                    Type ma;
                    Type mh;
                    Type[] types = new Type[0];



                    switch (PIs.Length)
                    {
                        case 0:
                            {
                                ma = typeof(MessageAggregator);
                                mh = typeof(MessageHandler);
                                types = new Type[0];
                            }
                            break;
                        case 1:
                            {
                                Type pt = PIs[0].ParameterType;
                                ma = typeof(MessageAggregator<>).MakeGenericType(pt);
                                mh = typeof(MessageHandler<>).MakeGenericType(pt);
                                types = new Type[1] { pt };
                            }
                            break;
                        case 2:
                            {
                                Type pt1 = PIs[0].ParameterType;
                                Type pt2 = PIs[1].ParameterType;
                                ma = typeof(MessageAggregator<,>).MakeGenericType(pt1, pt2);
                                mh = typeof(MessageHandler<,>).MakeGenericType(pt1, pt2);
                                types = new Type[2] { pt1,pt2 };
                            }
                            break;
                        case 3:
                            {
                                Type pt1 = PIs[0].ParameterType;
                                Type pt2 = PIs[1].ParameterType;
                                Type pt3 = PIs[2].ParameterType;
                                ma = typeof(MessageAggregator<,,>).MakeGenericType(pt1, pt2, pt3);
                                mh = typeof(MessageHandler<,,>).MakeGenericType(pt1, pt2, pt3);
                                types = new Type[3] { pt1, pt2, pt3 };
                            }
                            break;
                        default:
                            //Debug.Log("不支持超过三个参数的方法");
                            continue;
                    }
                    MethodInfo subMethod = ma.GetMethod("Subscribe", new Type[] { typeof(string), mh });
                    object instance = ma.GetField("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    var d = Delegate.CreateDelegate(mh, this, crtMI);

                    subMethod.Invoke(instance, new object[] { $"{crtInterface.Name}+{crtMI.Name}", d });



                    ListenerInfo temp = new ListenerInfo($"{crtInterface.Name}+{crtMI.Name}", d, types);
                    listenerInfos.Add(temp);

                }
            }
        }

        private struct ListenerInfo
        {
            public bool UseUint;
            public Type[] Types;
            public uint Index;
            public string Str;
            public object Func;

            public ListenerInfo(uint index, object func, params Type[] types)
            {
                UseUint = true;
                Types = types;
                Index = index;
                Func = func;
                Str = null;
            }

            public ListenerInfo(string str, object func, params Type[] types)
            {
                UseUint = false;
                Types = types;
                Str = str;
                Func = func;
                Index = 0;
            }
        }
    }


    public interface ICustomEventHandler
    {
        /*
         继承接口应当只有
        [无返回值且有零到三个传参]
        的方法
         */
    }
}
