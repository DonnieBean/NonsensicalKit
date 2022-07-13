using NonsensicalKit.Utility;
using System;
using System.Text;
using UnityEngine;


namespace NonsensicalKit
{
    /// <summary>
    /// �����жϵ����ȶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        public delegate bool CompareEventHandler(T t1, T t2);

        public event CompareEventHandler Compare;

        //���ȶ�����Ԫ�ظ�����ͬʱ�Ƕ�β����
        private int N;
        private T[] queue;
        private int size;

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// �ж϶����Ƿ�Ϊ��
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// ���ض���Ԫ�ظ���
        /// </summary>
        public int Count { get { return N; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compare">�Ѷ�������</param>
        /// <param name="size">��ʼ�ߴ�</param>
        public PriorityQueue(CompareEventHandler compare, int size = 55)
        {
            Compare = compare;
            this.size = size;
            queue = new T[size + 1];
            /* �����1��ʼ����
             * ��0��ʼ����ʱ���ӽڵ�Ϊ2*n+1��2*n+2�����ڵ�Ϊ(n-1)/2
             * ��1��ʼ����ʱ���ӽڵ�Ϊ2*n��2*n+1�����ڵ�Ϊn/2
             * ��1��ʼ��������ʹÿ���Ҹ��ӽڵ������һ���Ӽ�����
             */
        }

        /// <summary>
        /// ���Ԫ��
        /// </summary>
        /// <param name="val"></param>
        public void Push(T val)
        {
            if (N + 1 > size)
            {   //��������ʱ����
                T[] newqueue = new T[size * 2 + 1];
                queue.CopyTo(newqueue, 0);
                queue = newqueue;
                size *= 2;
            }
            queue[++N] = val;
            //����βԪ���ϸ�������λ��
            Swim(N);
        }

        /// <summary>
        /// ȡ�����Ԫ��
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            //����Ԫ�ؾ������ֵ
            T max = queue[1];
            //����βԪ�ط������
            Swap(1, N);
            //ɾ����βԪ��
            N--;
            //�ָ��ѵ�������
            Sink(1);
            return max;
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] values = new T[N];
            Array.Copy(queue, 1, values, 0, N);

            return values;
        }

        /// <summary>
        /// ��ȡ����õ�����
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

        //�³�����
        private void Sink(int k)
        {
            //���ӽڵ��Ƿ����
            while (2 * k <= N)
            {
                int child = 2 * k;

                //�ж��Ƿ���Ҫ�³�
                if (CompareValue(k, child) && CompareValue(k, child + 1))
                    break;
                //�ҳ��ӽڵ�Ľϴ�ֵ
                if (CompareValue(child + 1, child)) child++;
                //����λ��
                Swap(k, child);
                k = child;
            }
        }

        //�����ж�
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

        //�ϸ�����
        private void Swim(int k)
        {
            //��ǰ�ڵ���ڸ��ڵ㣬�򽻻���ֱ���ѻָ�����
            while (k > 1 && CompareValue(k, k / 2))
            {

                Swap(k, k / 2);
                k = k / 2;
            }
        }

        //����Ԫ��
        private void Swap(int i1, int i2)
        {
            T tmp = queue[i1];
            queue[i1] = queue[i2];
            queue[i2] = tmp;
        }
    }

    /// <summary>
    /// �󶥶����ȶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxHeapPriorityQueue<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// �ж϶����Ƿ�Ϊ��
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// ���ض���Ԫ�ظ���
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
        /// ���Ԫ��
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
        /// ȡ�����Ԫ��
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
    /// С�������ȶ���
    /// �ʹ󶥶ѻ���һ�£������жϴ�Сʱ�෴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HeapPriorityQueue<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public int Size { get { return size; } }
        /// <summary>
        /// �ж϶����Ƿ�Ϊ��
        /// </summary>
        public bool isEmpty { get { return N == 0; } }
        /// <summary>
        /// ���ض���Ԫ�ظ���
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
        /// ���Ԫ��
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
        /// ȡ����СԪ��
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
