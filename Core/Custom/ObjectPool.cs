using System;
using System.Collections.Generic;

namespace NonsensicalKit.Custom
{
    public class ObjectPool<T> where T : class, new()
    {
        private Queue<T> objectQueue;
        private Action<T> resetAction;
        private Action<T> onetimeInitAction;

        public ObjectPool(int initialBufferSize, Action<T>
            resetAction = null, Action<T> onetimeInitAction = null)
        {
            objectQueue = new Queue<T>();
            this.resetAction = resetAction;
            this.onetimeInitAction = onetimeInitAction;
            for (int i = 0; i < initialBufferSize; i++)
            {
                Store(New());
            }
        }

        public T New()
        {
            if (objectQueue.Count > 0)
            {
                T t = objectQueue.Dequeue();

                return t;
            }
            else
            {
                T t = new T();
                onetimeInitAction?.Invoke(t);

                return t;
            }
        }

        public void Store(T obj)
        {
            resetAction?.Invoke(obj);
            objectQueue.Enqueue(obj);
        }

        public void Clear()
        {
            objectQueue.Clear();
        }
    }
}
