using System.Collections.Generic;

namespace NonsensicalKit
{
    public delegate void MessageHandler<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void MessageHandler<T1, T2>(T1 arg1, T2 arg2);
    public delegate void MessageHandler<T>(T arg);
    public delegate void MessageHandler();

    public class MessageAggregator<T1, T2, T3>
    {
        public static MessageAggregator<T1, T2, T3> Instance = new MessageAggregator<T1, T2, T3>();

        private readonly Dictionary<uint, MessageHandler<T1, T2, T3>> _messages = new Dictionary<uint, MessageHandler<T1, T2, T3>>();
        private readonly Dictionary<string, MessageHandler<T1, T2, T3>> _strMessages = new Dictionary<string, MessageHandler<T1, T2, T3>>();

        private MessageAggregator()
        {
         
        }

        public void Subscribe(uint name, MessageHandler<T1, T2, T3> handler)
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

        public void Unsubscribe(uint name, MessageHandler<T1, T2, T3> handler)
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

        public void Publish(uint name, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_messages.ContainsKey(name) && _messages[name] != null)
            {
                _messages[name](arg1, arg2, arg3);
            }
        }
        public bool Check(uint value)
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
            if (_strMessages.ContainsKey(name) && _strMessages[name] != null)
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

        private readonly Dictionary<uint, MessageHandler<T1, T2>> _messages = new Dictionary<uint, MessageHandler<T1, T2>>();
        private readonly Dictionary<string, MessageHandler<T1, T2>> _strMessages = new Dictionary<string, MessageHandler<T1, T2>>();

        private MessageAggregator()
        {

        }

        public void Subscribe(uint name, MessageHandler<T1, T2> handler)
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

        public void Unsubscribe(uint name, MessageHandler<T1, T2> handler)
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

        public void Publish(uint name, T1 arg1, T2 arg2)
        {
            if (_messages.ContainsKey(name) && _messages[name] != null)
            {
                _messages[name](arg1, arg2);
            }
        }
        public bool Check(uint value)
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
            if (_strMessages.ContainsKey(name) && _strMessages[name] != null)
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

        private readonly Dictionary<uint, MessageHandler<T>> _messages = new Dictionary<uint, MessageHandler<T>>();
        private readonly Dictionary<string, MessageHandler<T>> _strMessages = new Dictionary<string, MessageHandler<T>>();

        private MessageAggregator()
        {

        }

        public void Subscribe(uint name, MessageHandler<T> handler)
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

        public void Unsubscribe(uint name, MessageHandler<T> handler)
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

        public void Publish(uint name, T args)
        {
            if (_messages.ContainsKey(name) && _messages[name] != null)
            {
                _messages[name](args);
            }
        }

        public bool Check(uint value)
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
            if (_strMessages.ContainsKey(name) && _strMessages[name] != null)
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

        private readonly Dictionary<uint, MessageHandler> _messages = new Dictionary<uint, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _strMessages = new Dictionary<string, MessageHandler>();

        private MessageAggregator()
        {

        }

        public void Subscribe(uint name, MessageHandler handler)
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

        public void Unsubscribe(uint name, MessageHandler handler)
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

        public void Publish(uint name)
        {
            if (_messages.ContainsKey(name) && _messages[name] != null)
            {
                _messages[name]();
            }
        }

        public bool Check(uint value)
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
            if (_strMessages.ContainsKey(name) && _strMessages[name] != null)
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
