namespace NonsensicalKit
{
    /// <summary>
    /// 使用一维数组实现三维数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Array3<T>
    {
        private readonly T[] array3;

        public readonly int length0;
        public readonly int length1;
        public readonly int length2;

        private readonly int step0;
        private readonly int step1;

        public Array3(int _length0, int _length1, int _length2)
        {
            array3 = new T[_length0 * _length1 * _length2];
            length0 = _length0;
            length1 = _length1;
            length2 = _length2;

            step0 = _length1 * _length2;
            step1 = _length2;
        }

        public void Reset(T state)
        {
            for (int i = 0; i < array3.Length; i++)
            {
                array3[i] = state;
            }
        }

        public T this[int index0, int index1, int index2]
        {
            get
            {
                    return array3[index0 * step0 + index1 * step1 + index2];
            }
            set
            {
                array3[index0 * step0 + index1 * step1 + index2] = value;

            }
        }

        public T this[Int3 int3]
        {
            get
            {
                return array3[int3.I1 * step0 + int3.I2 * step1 + int3.I3];
            }
            set
            {
                array3[int3.I1 * step0 + int3.I2 * step1 + int3.I3] = value;

            }
        }
    }
}
