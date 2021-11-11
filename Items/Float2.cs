using System;
using UnityEngine;

namespace NonsensicalKit
{
    [Serializable]
    public struct Float2
    {
        public readonly static Float2 zero = new Float2(0, 0);
        public readonly static Float2 one = new Float2(1, 1);

        [SerializeField] private float f1;
        [SerializeField] private float f2;
        public float F1 { get { return f1; } set { f1 = value; } }

        public float F2 { get { return f2; } set { f2 = value; } }

        public float X => F1;
        public float Y => F2;

        public Float2(float _f1, float _f2)
        {
            f1 = _f1;
            f2 = _f2;
        }

        public Float2(Vector2 _vector2)
        {
            f1 = _vector2.x;
            f2 = _vector2.y;
        }

        public Vector3 ToVector3()
        {
            return new Vector2(F1, F2);
        }

        public static Float2 operator *(Float2 a, float b)
        {
            Float2 c = new Float2
            {
                F1 = a.F1 * b,
                F2 = a.F2 * b
            };
            return c;
        }

        public static Float2 operator /(Float2 a, float b)
        {
            Float2 c = new Float2
            {
                F1 = a.F1 / b,
                F2 = a.F2 / b
            };
            return c;
        }

        public static Float2 operator +(Float2 a, Float2 b)
        {
            Float2 c = new Float2
            {
                F1 = a.F1 + b.F1,
                F2 = a.F2 + b.F2
            };
            return c;
        }
        public static Float2 operator -(Float2 a, Float2 b)
        {
            Float2 c = new Float2
            {
                F1 = a.F1 - b.F1,
                F2 = a.F2 - b.F2
            };
            return c;
        }
        public static Float2 operator -(Float2 a)
        {
            Float2 c = new Float2
            {
                F1 = -a.F1,
                F2 = -a.F2,
            };
            return c;
        }

        public override string ToString()
        {
            return $"({F1},{F2})";
        }

        public static explicit operator Float2(Vector2 v)
        {
            return new Float2(v.x, v.y);
        }

        public static float Distance(Float2 pos1, Float2 pos2)
        {
            float f1Offset = pos1.F1 - pos2.F1;
            float f2Offset = pos1.F2 - pos2.F2;
            float temp = (f1Offset) * (f1Offset) + (f2Offset) * (f2Offset);
            return Mathf.Sqrt(temp);
        }
    }

}