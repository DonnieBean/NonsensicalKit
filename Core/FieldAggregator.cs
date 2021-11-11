using System.Collections.Generic;

namespace NonsensicalKit
{
    public delegate void FieldChangedHandler();

    /// <summary>
    /// ×Ö¶Î¾ÛºÏÆ÷
    /// </summary>
    public class FieldAggregator<T>
    {
        public static FieldAggregator<T> Instance = new FieldAggregator<T>();

        private readonly Dictionary<uint, T> _fields = new Dictionary<uint, T>();
        private readonly Dictionary<uint, FieldChangedHandler> _fieldListeners = new Dictionary<uint, FieldChangedHandler>();
        private readonly Dictionary<string, T> _strFields = new Dictionary<string, T>();
        private readonly Dictionary<string, FieldChangedHandler> _strFieldListeners = new Dictionary<string, FieldChangedHandler>();

        private FieldAggregator()
        {

        }

        public void Subscribe(uint name, FieldChangedHandler handler)
        {
            if (!_fieldListeners.ContainsKey(name))
            {
                _fieldListeners.Add(name, handler);
            }
            else
            {
                _fieldListeners[name] = handler;
            }
        }

        public void Unsubscribe(uint name, FieldChangedHandler handler)
        {
            if (!_fieldListeners.ContainsKey(name))
            {
                return;
            }
            else
            {
                _fieldListeners[name] -= handler;

                if (_fieldListeners[name] == null)
                {
                    _fieldListeners.Remove(name);
                }
            }
        }

        public void Set(uint name, T value)
        {
            if (!_fields.ContainsKey(name))
            {
                _fields.Add(name, value);
            }
            else
            {
                _fields[name] = value;
            }
            if (_fieldListeners.ContainsKey(name))
            {
                _fieldListeners[name]?.Invoke();
            }
        }

        public bool TryGet(uint name, out T value)
        {
            value = default(T);
            if (_fields.ContainsKey(name) && _fields[name] != null)
            {
                value = _fields[name];
                return true;
            }
            else
            {
                return false;
            }
        }

        public T Get(uint name)
        {
            if (_fields.ContainsKey(name) && _fields[name] != null)
            {
                return _fields[name];
            }
            else
            {
                return default;
            }
        }

        public void Subscribe(string name, FieldChangedHandler handler)
        {
            if (!_strFieldListeners.ContainsKey(name))
            {
                _strFieldListeners.Add(name, handler);
            }
            else
            {
                _strFieldListeners[name] = handler;
            }
        }

        public void Unsubscribe(string name, FieldChangedHandler handler)
        {
            if (!_strFieldListeners.ContainsKey(name))
            {
                return;
            }
            else
            {
                _strFieldListeners[name] -= handler;

                if (_strFieldListeners[name] == null)
                {
                    _strFieldListeners.Remove(name);
                }
            }
        }

        public void Set(string name, T value)
        {
            if (!_strFields.ContainsKey(name))
            {
                _strFields.Add(name, value);
            }
            else
            {
                _strFields[name] = value;
            }
            if (_strFieldListeners.ContainsKey(name))
            {
                _strFieldListeners[name]?.Invoke();
            }
        }

        public bool TryGet(string name,out T value)
        {
            value = default(T);
            if (_strFields.ContainsKey(name) && _strFields[name] != null)
            {
                value = _strFields[name];
                return true ;
            }
            else
            {
                return false;
            }
        }

        public T Get(string name)
        {
            if (_strFields.ContainsKey(name) && _strFields[name] != null)
            {
                return _strFields[name];
            }
            else
            {
                return default;
            }
        }
    }
}