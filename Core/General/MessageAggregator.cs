using System.Collections.Generic;

namespace NonsensicalKit
{
    /* 经简单测试，循环十万次调用单一方法时，publish的时间消耗是直接引用调用的20倍，但平均每次调用时间仍在接受范围内
     * 消息聚合器应当只用于模块之间的通信，且当通信过于频繁时不应使用，模块内部使用应直接引用的方式进行值的传递
     */

    public delegate void MessageHandler<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void MessageHandler<T1, T2>(T1 arg1, T2 arg2);
    public delegate void MessageHandler<T>(T arg);
    public delegate void MessageHandler();

    /// <summary>
    /// 消息聚合器，最多支持三个参数的泛型
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class MessageAggregator<T1, T2, T3>
    {
        public static MessageAggregator<T1, T2, T3> Instance = new MessageAggregator<T1, T2, T3>();

        private readonly Dictionary<int, MessageHandler<T1, T2, T3>> _messages = new Dictionary<int, MessageHandler<T1, T2, T3>>();
        private readonly Dictionary<string, MessageHandler<T1, T2, T3>> _strMessages = new Dictionary<string, MessageHandler<T1, T2, T3>>();

        private MessageAggregator()
        {

        }

        public void Subscribe(int name, MessageHandler<T1, T2, T3> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                _messages.Add(name, handler);
            }
            else
            {
                _messages[name] += handler;
            }
        }

        public void Unsubscribe(int name, MessageHandler<T1, T2, T3> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _messages[name] -= handler;

                if (_messages[name] == null)
                {
                    _messages.Remove(name);
                }
            }
        }

        public void Publish(int name, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_messages.ContainsKey(name))
            {
                _messages[name](arg1, arg2, arg3);
            }
        }
        public bool Check(int value)
        {
            return _messages.ContainsKey(value);
        }

        public void Subscribe(string name, MessageHandler<T1, T2, T3> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                _strMessages.Add(name, handler);
            }
            else
            {
                _strMessages[name] += handler;
            }
        }

        public void Unsubscribe(string name, MessageHandler<T1, T2, T3> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _strMessages[name] -= handler;

                if (_strMessages[name] == null)
                {
                    _strMessages.Remove(name);
                }
            }
        }

        public void Publish(string name, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_strMessages.ContainsKey(name))
            {
                _strMessages[name](arg1, arg2, arg3);
            }
        }

        public bool Check(string value)
        {
            return _strMessages.ContainsKey(value);
        }

    }

    public class MessageAggregator<T1, T2>
    {
        public static MessageAggregator<T1, T2> Instance = new MessageAggregator<T1, T2>();

        private readonly Dictionary<int, MessageHandler<T1, T2>> _messages = new Dictionary<int, MessageHandler<T1, T2>>();
        private readonly Dictionary<string, MessageHandler<T1, T2>> _strMessages = new Dictionary<string, MessageHandler<T1, T2>>();

        private MessageAggregator()
        {

        }

        public void Subscribe(int name, MessageHandler<T1, T2> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                _messages.Add(name, handler);
            }
            else
            {
                _messages[name] += handler;
            }
        }

        public void Unsubscribe(int name, MessageHandler<T1, T2> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _messages[name] -= handler;

                if (_messages[name] == null)
                {
                    _messages.Remove(name);
                }
            }
        }

        public void Publish(int name, T1 arg1, T2 arg2)
        {
            if (_messages.ContainsKey(name))
            {
                _messages[name](arg1, arg2);
            }
        }
        public bool Check(int value)
        {
            return _messages.ContainsKey(value);
        }

        public void Subscribe(string name, MessageHandler<T1, T2> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                _strMessages.Add(name, handler);
            }
            else
            {
                _strMessages[name] += handler;
            }
        }

        public void Unsubscribe(string name, MessageHandler<T1, T2> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _strMessages[name] -= handler;

                if (_strMessages[name] == null)
                {
                    _strMessages.Remove(name);
                }
            }
        }

        public void Publish(string name, T1 arg1, T2 arg2)
        {
            if (_strMessages.ContainsKey(name))
            {
                _strMessages[name](arg1, arg2);
            }
        }
        public bool Check(string value)
        {
            return _strMessages.ContainsKey(value);
        }
    }

    public class MessageAggregator<T>
    {
        public static MessageAggregator<T> Instance = new MessageAggregator<T>();

        private readonly Dictionary<int, MessageHandler<T>> _messages = new Dictionary<int, MessageHandler<T>>();
        private readonly Dictionary<string, MessageHandler<T>> _strMessages = new Dictionary<string, MessageHandler<T>>();

        private MessageAggregator()
        {

        }

        public void Subscribe(int name, MessageHandler<T> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                _messages.Add(name, handler);
            }
            else
            {
                _messages[name] += handler;
            }
        }

        public void Unsubscribe(int name, MessageHandler<T> handler)
        {
            if (!_messages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _messages[name] -= handler;

                if (_messages[name] == null)
                {
                    _messages.Remove(name);
                }
            }
        }

        public void Publish(int name, T args)
        {
            if (_messages.ContainsKey(name))
            {
                _messages[name](args);
            }
        }

        public bool Check(int value)
        {
            return _messages.ContainsKey(value);
        }

        public void Subscribe(string name, MessageHandler<T> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                _strMessages.Add(name, handler);
            }
            else
            {
                _strMessages[name] += handler;
            }
        }

        public void Unsubscribe(string name, MessageHandler<T> handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _strMessages[name] -= handler;

                if (_strMessages[name] == null)
                {
                    _strMessages.Remove(name);
                }
            }
        }

        public void Publish(string name, T args)
        {
            if (_strMessages.ContainsKey(name))
            {
                _strMessages[name](args);
            }
        }
        public bool Check(string value)
        {
            return _strMessages.ContainsKey(value);
        }
    }

    public class MessageAggregator
    {
        // 不能使用单例基类，会使构造函数暴露
        public static MessageAggregator Instance = new MessageAggregator();

        private readonly Dictionary<int, MessageHandler> _messages = new Dictionary<int, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _strMessages = new Dictionary<string, MessageHandler>();

        private MessageAggregator()
        {

        }

        public void Subscribe(int name, MessageHandler handler)
        {
            if (!_messages.ContainsKey(name))
            {
                _messages.Add(name, handler);
            }
            else
            {
                _messages[name] += handler;
            }
        }

        public void Unsubscribe(int name, MessageHandler handler)
        {
            if (!_messages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _messages[name] -= handler;

                if (_messages[name] == null)
                {
                    _messages.Remove(name);
                }
            }
        }

        public void Publish(int name)
        {
            if (_messages.ContainsKey(name))
            {
                _messages[name]();
            }
        }

        public bool Check(int value)
        {
            return _messages.ContainsKey(value);
        }

        public void Subscribe(string name, MessageHandler handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                _strMessages.Add(name, handler);
            }
            else
            {
                _strMessages[name] += handler;
            }
        }

        public void Unsubscribe(string name, MessageHandler handler)
        {
            if (!_strMessages.ContainsKey(name))
            {
                return;
            }
            else
            {
                _strMessages[name] -= handler;

                if (_strMessages[name] == null)
                {
                    _strMessages.Remove(name);
                }
            }
        }

        public void Publish(string name)
        {
            if (_strMessages.ContainsKey(name))
            {
                _strMessages[name]();
            }
        }

        public bool Check(string value)
        {
            return _strMessages.ContainsKey(value);
        }
    }
}
