using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper 
{
    /// <summary>
    /// 判断是否为质数
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool  isPrime(int num)
    {
        if (num <= 3)
        {
            return num > 1;
        }
        // 不在6的倍数两侧的一定不是质数
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
