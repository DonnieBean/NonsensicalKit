using System;
using System.Collections.Generic;
using System.Reflection;

namespace NonsensicalKit
{
    public abstract class ContextCatcher<T> : NonsensicalMono, IUseProtocols<T>
    {
        private Dictionary<FieldInfo, Action<object>> fieldCatchs = new Dictionary<FieldInfo, Action<object>>();

        private Dictionary<PropertyInfo, Action<object>> propertyCatchs = new Dictionary<PropertyInfo, Action<object>>();

        Type t = typeof(T);

        public void OnReceivedMessage(T value)
        {
            foreach (var item in fieldCatchs)
            {
                item.Value(item.Key.GetValue(value));
            }
            foreach (var item in propertyCatchs)
            {
                item.Value(item.Key.GetValue(value));
            }
        }

        public void SetFieldCatch(NonsensicalMono user, string fieldName, Action<object> callback)
        {
            var v = t.GetField(fieldName);
            if (v != null)
            {
                fieldCatchs.Add(v, callback);
                user.DestroyAction += () => { fieldCatchs.Remove(v); };
            }
        }
        public void SetPropertyCatch(NonsensicalMono user, string propertyName, Action<object> callback)
        {
            var v = t.GetProperty(propertyName);
            if (v != null)
            {
                propertyCatchs.Add(v, callback);
                user.DestroyAction += () => { propertyCatchs.Remove(v); };
            }
        }
    }
}