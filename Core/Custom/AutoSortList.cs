using System;
using System.Collections.Generic;

namespace NonsensicalKit
{
    /// <summary>
    /// 添加时会在排序后再插入的链表，可以保证随时都是排序好的状态
    /// 性能较差
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class AutoSortList<T> where T : IComparable
    {
        private List<T> values = new List<T>();

        public T this[int index] { get { return values[index]; } set { values[index] = value; } }

        public int Count => values.Count;

        public void Add(T item)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].CompareTo(item) > 0)
                {
                    values.Insert(i, item);
                    return;
                }
            }
            values.Add(item);
        }

        public int CheckPos(T item)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].CompareTo(item) > 0)
                {
                    return i;
                }
            }
            return values.Count;
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool Contains(T item)
        {
            return values.Contains(item);
        }

        public bool Remove(T item)
        {
            return values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            values.RemoveAt(index);
        }

        public T[] ToArray()
        {
            return values.ToArray();
        }
    }
}
