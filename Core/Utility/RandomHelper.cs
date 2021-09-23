using System;
using System.Security.Cryptography;
using UnityEngine;

namespace NonsensicalKit.Utility
{
    public class RandomHelper
    {
        /// <summary>
        /// 创建一个足够随机的token
        /// </summary>
        /// <returns></returns>
        public static string CreateToken()
        {
            var deviceId = SystemInfo.deviceUniqueIdentifier;
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            var last2DigitsofYear = DateTime.Now.Year % 100;
            //依照规则可以添加其他字段，确保足够复杂
            var source = ((day * 10) + (month * 100) + (last2DigitsofYear) * 1000) + deviceId;
            //创建md5
            using (var md5Hash = MD5.Create())
            {
                return MD5Helper.GetMd5Hash(md5Hash, source);
            }
        }

        /// <summary>
        /// 获取使用Guid作为种子返回的随机数
        /// </summary>
        /// <param name="_max">返回值的绝对值小于max</param>
        /// <returns></returns>
        public static int GetRandomInt(int _max)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            System.Random random = new System.Random(iSeed);
            int temp = random.Next(_max * 2 - 1);
            temp = temp - _max + 1;

            return temp;
        }

        public static float GetRandomFloat(float _max)
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            System.Random random = new System.Random(iSeed);
            float temp = (float) random.NextDouble()*_max;
            return temp;
        }

        /// <summary>
        /// 获取通过csp加密返回的随机数
        /// </summary>
        /// <param name="_max">返回值的绝对值小于max</param>
        /// <returns></returns>
        public static int GetRandomIntPrime(int _max)
        {
            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            Int32 result = BitConverter.ToInt32(randomBytes, 0);
            result %= _max;

            return result;
        }
    }
}
