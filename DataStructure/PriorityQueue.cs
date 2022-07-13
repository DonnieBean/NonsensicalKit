using NonsensicalKit.Utility;
using System;
using System.Text;
using UnityEngine;


namespace NonsensicalKit
{
    /// <summary>
    /// 自由判断的优先队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        public delegate bool CompareEventHandler(T t1, T t2);

        public event CompareEventHandler Compare;

        //优先队列中元素个数，同时是队尾索引
        private int N;
        private T[] queue;
        private int size;

        /// <summary>
        /// 当前容量
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// 判断队列是否为空
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// 返回队列元素个数
        /// </summary>
        public int Count { get { return N; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compare">堆顶的条件</param>
        /// <param name="size">初始尺寸</param>
        public PriorityQueue(CompareEventHandler compare, int size = 55)
        {
            Compare = compare;
            this.size = size;
            queue = new T[size + 1];
            /* 主体从1开始索引
             * 从0开始索引时，子节点为2*n+1和2*n+2，父节点为(n-1)/2
             * 从1开始索引时，子节点为2*n和2*n+1，父节点为n/2
             * 从1开始索引可以使每次找父子节点操作少一个加减操作
             */
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="val"></param>
        public void Push(T val)
        {
            if (N + 1 > size)
            {   //容量不够时翻倍
                T[] newqueue = new T[size * 2 + 1];
                queue.CopyTo(newqueue, 0);
                queue = newqueue;
                size *= 2;
            }
            queue[++N] = val;
            //将队尾元素上浮到合适位置
            Swim(N);
        }

        /// <summary>
        /// 取出最大元素
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            //队首元素就是最大值
            T max = queue[1];
            //将队尾元素放入队首
            Swap(1, N);
            //删除队尾元素
            N--;
            //恢复堆的有序性
            Sink(1);
            return max;
        }

        /// <summary>
        /// 获取数组对象
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] values = new T[N];
            Array.Copy(queue, 1, values, 0, N);

            return values;
        }

        /// <summary>
        /// 获取排序好的数组
        /// </summary>
        /// <returns></returns>
        public T[] ToSortedArray()
        {
            T[] values = new T[N];
            Array.Copy(queue, 1, values, 0, N);

            for (int i = N - 1; i > 0; i--)
            {
                NumHelper.Swap(values, 0, i);

                int index = 0;
                while (2 * index + 1 < i)
                {
                    int child = 2 * index + 1;

                    if (child + 1 < i)
                    {
                        if (Compare(values[index], values[child]) && Compare(values[index], values[child + 1]))
                            break;
                        if (Compare(values[child + 1], values[child])) child++;
                        NumHelper.Swap(values, index, child);
                        index = child;
                    }
                    else
                    {
                        if (Compare(values[index], values[child]))
                            break;
                        NumHelper.Swap(values, index, child);
                        index = child;
                    }
                }
            }
            return values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= N; i++)
            {
                sb.Append(queue[i]).Append(" ");
            }
            return sb.ToString();
        }

        //下沉操作
        private void Sink(int k)
        {
            //左子节点是否存在
            while (2 * k <= N)
            {
                int child = 2 * k;

                //判断是否需要下沉
                if (CompareValue(k, child) && CompareValue(k, child + 1))
                    break;
                //找出子节点的较大值
                if (CompareValue(child + 1, child)) child++;
                //交换位置
                Swap(k, child);
                k = child;
            }
        }

        //进行判断
        private bool CompareValue(int i1, int i2)
        {
            if (i1 > N)
            {
                return false;
            }
            else if (i2 > N)
            {
                return true;
            }
            else
            {
                return Compare(queue[i1], queue[i2]);
            }
        }

        //上浮操作
        private void Swim(int k)
        {
            //当前节点大于父节点，则交换，直至堆恢复有序
            while (k > 1 && CompareValue(k, k / 2))
            {

                Swap(k, k / 2);
                k = k / 2;
            }
        }

        //交换元素
        private void Swap(int i1, int i2)
        {
            T tmp = queue[i1];
            queue[i1] = queue[i2];
            queue[i2] = tmp;
        }
    }

    /// <summary>
    /// 大顶堆优先队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxHeapPriorityQueue<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// 当前容量
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// 判断队列是否为空
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// 返回队列元素个数
        /// </summary>
        public int Count { get { return N; } }

        private int N;
        private T[] queue;
        private int size;


        public MaxHeapPriorityQueue(int size = 55)
        {
            this.size = size;
            queue = new T[size + 1];
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="val"></param>
        public void Push(T val)
        {
            if (N + 1 > size)
            {
                T[] newqueue = new T[size * 2 + 1];
                queue.CopyTo(newqueue, 0);
                queue = newqueue;
                size *= 2;
            }
            queue[++N] = val;
            Swim(N);
        }

        /// <summary>
        /// 取出最大元素
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T max = queue[1];
            Swap(1, N);
            N--;
            Sink(1);
            return max;
        }

        public T[] ToArray()
        {
            T[] values = new T[N];
            Array.Copy(queue, 1, values, 0, N);

            return values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= N; i++)
            {
                sb.Append(queue[i]).Append(" ");
            }
            return sb.ToString();
        }

        private void Sink(int k)
        {
            while (2 * k <= N)
            {
                int child = 2 * k;

                if (Less(child, k) && Less(child + 1, k))
                    break;
                if (Less(child, child + 1)) child++;
                Swap(k, child);
                k = child;
            }
        }

        private bool Less(int i1, int i2)
        {
            if (i1 > N)
            {
                return true;
            }
            else if (i2 > N)
            {
                return false;
            }
            else
            {
                return queue[i1].CompareTo(queue[i2]) < 0;
            }
        }

        private void Swim(int k)
        {
            while (k > 1 && Less(k / 2, k))
            {

                Swap(k, k / 2);
                k = k / 2;
            }
        }

        private void Swap(int i1, int i2)
        {
            T tmp = queue[i1];
            queue[i1] = queue[i2];
            queue[i2] = tmp;
        }
    }

    /// <summary>
    /// 小顶堆优先队列
    /// 和大顶堆基本一致，仅在判断大小时相反
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HeapPriorityQueue<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// 当前容量
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// 判断队列是否为空
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// 返回队列元素个数
        /// </summary>
        public int Count { get { return N; } }

        private int N;
        private T[] queue;
        private int size;

        public HeapPriorityQueue(int size = 55)
        {
            this.size = size;
            queue = new T[size + 1];
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="val"></param>
        public void Push(T val)
        {
            if (N + 1 > size)
            {
                T[] newqueue = new T[size * 2 + 1];
                queue.CopyTo(newqueue, 0);
                queue = newqueue;
                size *= 2;
            }
            queue[++N] = val;
            Swim(N);
        }

        /// <summary>
        /// 取出最小元素
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T max = queue[1];
            Swap(1, N);
            N--;
            Sink(1);
            return max;
        }

        public T[] ToArray()
        {
            T[] values = new T[N];
            Array.Copy(queue, 1, values, 0, N);

            return values;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= N; i++)
            {
                sb.Append(queue[i]).Append(" ");
            }
            return sb.ToString();
        }

        private void Sink(int k)
        {
            while (2 * k <= N)
            {
                int child = 2 * k;

                if (Greater(child, k) && Greater(child + 1, k))
                    break;
                if (Greater(child, child + 1)) child++;
                Swap(k, child);
                k = child;
            }
        }


        private bool Greater(int i1, int i2)
        {
            if (i1 > N)
            {
                return true;
            }
            else if (i2 > N)
            {
                return false;
            }
            else
            {
                return queue[i1].CompareTo(queue[i2]) > 0;
            }
        }

        private void Swim(int k)
        {
            while (k > 1 && Greater(k / 2, k))
            {
                Swap(k, k / 2);
                k = k / 2;
            }
        }

        private void Swap(int i1, int i2)
        {
            T tmp = queue[i1];
            queue[i1] = queue[i2];
            queue[i2] = tmp;
        }

    }
}
