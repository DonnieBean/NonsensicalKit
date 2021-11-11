namespace NonsensicalKit
{
    /// <summary>
    /// 使用一维数组实现四维数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Array4<T>
    {
        private readonly T[] array4;

        public readonly int length0;
        public readonly int length1;
        public readonly int length2;
        public readonly int length3;

        private readonly int step0;
        private readonly int step1;
        private readonly int step2;

        public Array4(int _length0, int _length1, int _length2, int _length3)
        {
            array4 = new T[_length0 * _length1 * _length2 * _length3];
            length0 = _length0;
            length1 = _length1;
            length2 = _length2;
            length3 = _length3;

            step0 = _length1 * _length2 * _length3;
            step1 = _length2 * _length3;
            step2 = _length3;
        }

        public T this[int index0, int index1, int index2, int index3]
        {
            get
            {
                return array4[index0 * step0 + index1 * step1 + index2 * step2 + index3];
            }
            set
            {
                array4[index0 * step0 + index1 * step1 + index2 * step2 + index3] = value;
            }
        }

        public T this[Int3 int3, int index3]
        {
            get
            {
                return array4[int3.I1 * step0 + int3.I2 * step1 + int3.I3 * step2 + index3];
            }
            set
            {
                array4[int3.I1 * step0 + int3.I2 * step1 + int3.I3 * step2 + index3] = value;
            }
        }
    }

}