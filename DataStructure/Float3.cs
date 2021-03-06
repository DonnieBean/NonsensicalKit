using UnityEngine;
using System;

namespace NonsensicalKit
{
    [Serializable]
    public struct Float3
    {
        public readonly static Float3 zero = new Float3(0, 0, 0);
        public readonly static Float3 one = new Float3(1, 1, 1);

        [SerializeField] private float f1;
        [SerializeField] private float f2;
        [SerializeField] private float f3;
        public float F1 { get { return f1; } set { f1 = value; } }
        public float F2 { get { return f2; } set { f2 = value; } }
        public float F3 { get { return f3; } set { f3 = value; } }

        public float X { get { return f1; } set { f2 = value; } }
        public float Y => F2;
        public float Z => F3;

        public Float3(float _f1, float _f2, float _f3)
        {
            f1 = _f1;
            f2 = _f2;
            f3 = _f3;
        }

        public Float3(Vector3 _vector3)
        {
            f1 = _vector3.x;
            f2 = _vector3.y;
            f3 = _vector3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(F1, F2, F3);
        }

        public static Float3 operator *(Float3 a, float b)
        {
            Float3 c = new Float3
            {
                F1 = a.F1 * b,
                F2 = a.F2 * b,
                F3 = a.F3 * b
            };
            return c;
        }

        public static Float3 operator /(Float3 a, float b)
        {
            Float3 c = new Float3
            {
                F1 = a.F1 / b,
                F2 = a.F2 / b,
                F3 = a.F3 / b
            };
            return c;
        }

        public static Float3 operator +(Float3 a, Float3 b)
        {
            Float3 c = new Float3
            {
                F1 = a.F1 + b.F1,
                F2 = a.F2 + b.F2,
                F3 = a.F3 + b.F3
            };
            return c;
        }
        public static Float3 operator -(Float3 a, Float3 b)
        {
            Float3 c = new Float3
            {
                F1 = a.F1 - b.F1,
                F2 = a.F2 - b.F2,
                F3 = a.F3 - b.F3
            };
            return c;
        }
        public static Float3 operator -(Float3 a)
        {
            Float3 c = new Float3
            {
                F1 = -a.F1,
                F2 = -a.F2,
                F3 = -a.F3
            };
            return c;
        }

        public override string ToString()
        {
            return $"({F1},{F2},{F3})";
        }

        public static explicit operator Float3(Vector3 v)
        {
            return new Float3(v.x, v.y, v.z);
        }

        public static float Distance(Float3 pos1, Float3 pos2)
        {
            float f1Offset = pos1.F1 - pos2.F1;
            float f2Offset = pos1.F2 - pos2.F2;
            float f3Offset = pos1.F3 - pos2.F3;
            float temp = (f1Offset) * (f1Offset) + (f2Offset) * (f2Offset) + (f3Offset) * (f3Offset);
            return Mathf.Sqrt(temp);
        }
    }
}