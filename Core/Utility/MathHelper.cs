using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper 
{
    /// <summary>
    /// �ж��Ƿ�Ϊ����
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool  isPrime(int num)
    {
        if (num <= 3)
        {
            return num > 1;
        }
        // ����6�ı��������һ����������
        if (num % 6 != 1 && num % 6 != 5)
        {
            return false;
        }

        int sqrt = (int)Math.Sqrt(num);
        for (int i = 5; i <= sqrt; i += 6)
        {
            if (num % i == 0 || num % (i + 2) == 0)
            {
                return false;
            }
        }
        return true;
    }
}
