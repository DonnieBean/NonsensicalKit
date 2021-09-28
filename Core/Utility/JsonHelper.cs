#if USE_NEWTONSOFTJSON
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NonsensicalKit.Utility
{
    public static class JsonHelper
    {
#if USE_NEWTONSOFTJSON
        public static MethodInfo deserializeMethod = typeof(JsonConvert).GetMethods().FirstOrDefault(
                        p => p.IsStatic == true && p.IsPublic == true && p.Name == "DeserializeObject" && p.ContainsGenericParameters == true);
#else
        public static MethodInfo deserializeMethod = typeof(JsonUtility).GetMethods().FirstOrDefault(
                        p => p.IsStatic == true && p.IsPublic == true && p.Name == "FromJson" && p.ContainsGenericParameters == true);
#endif

#if USE_NEWTONSOFTJSON
        /// <summary>
        /// 动态生成json(使用递归)
        /// 输入：JsonData jd=test(new string[]{"1","2","3"},10086);
        /// 结果：Debug.Log(jd["0"]["1"]["2"]);    //=10086
        /// 从litjson转为NewtonsoftJson,待测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static JObject Test(string[] a, int b, int pos = 0)
        {

            if (pos == a.Length)
            {
                return new JObject(b);
            }
            else
            {
                JObject jd = new JObject();

                jd[a[(int)pos]] = Test(a, b, pos + 1);

                return jd;
            }
        }

        /// <summary>
        /// 动态生成json(不使用递归)
        /// 从litjson转为NewtonsoftJson,待测试
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static JObject Test2(string[] a, int b)
        {
            JObject jd = new JObject();

            int pos = a.Length - 1;

            while (pos >= 0)
            {
                if (pos == a.Length - 1)
                {
                    jd[a[pos]] = b;
                }
                else
                {
                    //直接使用jd[a[pos]] = temp;会导致堆栈溢出异常
                    JObject temp = jd;
                    jd = new JObject();
                    jd[a[pos]] = temp;
                }

                pos--;
            }

            return jd;
        }
#endif


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void SaveFile<T>(string fileName, T data)
        {
            string dataJson = SerializeObject(data);

            FileHelper.EnsureDir(Path.Combine(Application.streamingAssetsPath, "SaveJsonFiles"));

            FileHelper.WriteTxt(Path.Combine(Application.streamingAssetsPath, "SaveJsonFiles", fileName + ".json"), dataJson);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T LoadFile<T>(string fileName)
        {

            string fullPath = Path.Combine(Application.streamingAssetsPath, "SaveJsonFiles", fileName + ".json");

            string dataJson = FileHelper.ReadAllText(fullPath);
            if (dataJson == null)
            {
                return default(T);
            }

            T data = DeserializeObject<T>(dataJson);

            return data;
        }
        public static string SerializeObject(object obj)
        {
#if USE_NEWTONSOFTJSON
            return JsonConvert.SerializeObject(obj);
#else
            return JsonUtility.ToJson(obj);
#endif
        }
        public static T DeserializeObject<T>(string str)
        {
#if USE_NEWTONSOFTJSON
            str = str.TrimBOM();
            return JsonConvert.DeserializeObject<T>(str);
#else
            return JsonUtility.FromJson<T>(str);
#endif
        }
    }
}
