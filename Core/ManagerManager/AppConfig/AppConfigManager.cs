using NonsensicalKit.Utility;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 配置管理类
    /// 编辑器中初始化时会将configDatas中所有的类序列化保存成json文件，发布后运行时读取json文件而不是直接使用configDatas中的数据，这样发布后就可以通过修改json文件来修改配置
    /// 
    /// 目前会导致一个报警，因为会new序列化对象类，可以把数据用一个数据类包起来，通过抽象方法获取Object，再通过getType获取数据类类型，序列化和反序列化使用数据类来消除报警，但修改会导致当前项目存储的信息失效，所以暂时不处理
    /// new对象只是为了获取其中的数据，并不会进行操作，所以不会有逻辑上的问题（但可能有内存上的问题）
    /// </summary>
    public class AppConfigManager : NonsensicalManagerBase<AppConfigManager>
    {
        public NonsensicalConfigDataBase[] configDatas;

        protected override void Awake()
        {
            base.Awake();
            InitSubscribe(0, OnInitStart());
        }

        /// <summary>
        /// 用于编辑器环境中的反向读取json文件
        /// </summary>
        public void LoadJson()
        {
            for (int i = 0; i < configDatas.Length; i++)
            {
                string path = GetFilePath(configDatas[i]);
                string str = FileHelper.ReadAllText(path);
                MethodInfo deserializeMethod = JsonHelper.deserializeMethod.MakeGenericMethod(new Type[] { configDatas[i].GetType() });
                object deserializeData = null;
                try
                {
                    deserializeData = deserializeMethod.Invoke(null, new object[] { str });
                }
                catch (Exception e)
                {
                    Debug.LogError("NonsensicalAppConfig文件反序列化出错"+ path + "\r\n"+ e.ToString());
                }

                configDatas[i].CopyForm<NonsensicalConfigDataBase>((NonsensicalConfigDataBase)deserializeData) ;
            }
        }

        /// <summary>
        /// 使用类型来获取配置信息，返回匹配的第一个
        /// </summary>
        /// <typeparam name="T">配置类的类型</typeparam>
        /// <param name="t">out值，获取不到时为默认值</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetConfig<T>(out T t) where T : NonsensicalConfigDataBase
        {
            t = default(T);
            foreach (var configData in configDatas)
            {
                if (configData.GetType() == typeof(T))
                {
                    t = configData as T;
                    break;
                }
            }
            return t != null;
        }

        /// <summary>
        /// 同时使用类型和ID来获取配置信息，返回匹配的第一个
        /// </summary>
        /// <typeparam name="T">配置类的类型</typeparam>
        /// <param name="ID">想要获取的ID</param>
        /// <param name="t">out值，获取不到时为默认值</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetConfig<T>(string ID, out T t) where T : NonsensicalConfigDataBase
        {
            t = default(T);
            foreach (var configData in configDatas)
            {
                if (configData.ConfigID == ID && configData.GetType() == typeof(T))
                {
                    t = configData as T;
                    break;
                }
            }
            return t != null;
        }

        /// <summary>
        /// 获取某个配置类的某个字段的值
        /// </summary>
        /// <typeparam name="Config">配置类的类型</typeparam>
        /// <typeparam name="T">字段的类型</typeparam>
        /// <param name="filedName">字段的名称</param>
        /// <param name="t">out值，获取不到时为默认值</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetConfigValue<Config, T>(string filedName, out T t) where Config : NonsensicalConfigDataBase
        {
            t = default(T);
            if (TryGetConfig(out Config config))
            {
                Type type = typeof(Config);

                var f = type.GetField(filedName);    //unity只会序列化字段，不会序列化属性
                if (f != null)
                {
                    var v = f.GetValue(config);
                    if (v.GetType() == typeof(T))
                    {
                        t = (T)v;
                        return true;
                    }
                }
            }
            return false;
        }

        protected IEnumerator  OnInitStart()
        {
                if (!PlatformInfo.Instance.isEditor)
                {
                   yield return  StartCoroutine(LoadAppConfig());
                }
                else
                {
                    Dictionary<Type, HashSet<string>> IDTable = new Dictionary<Type, HashSet<string>>();

                    foreach (var configData in configDatas)
                    {
                        Type t = configData.GetType();
                        if (IDTable.ContainsKey(t) == false)
                        {
                            IDTable.Add(t, new HashSet<string>());
                        }

                        if (!IDTable[t].Contains(configData.ConfigID))
                        {
                            FileHelper.WriteTxt(GetFilePath(configData), JsonHelper.SerializeObject(configData));
                            IDTable[t].Add(configData.ConfigID);
                        }
                        else
                        {
                            Debug.LogError("相同类型配置的ID重复:" + configData.ConfigID);
                            break;
                        }
                    }
                }
            
        }

        private IEnumerator LoadAppConfig()
        {
            int count = configDatas.Length;
            for (int i = 0; i < configDatas.Length; i++)
            {
                int j = i;
                StartCoroutine(HttpHelper.Get(GetFilePath(configDatas[i]), null, (unityWebRequest) =>
                {
                    if (unityWebRequest != null && unityWebRequest.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                    {
                        MethodInfo deserializeMethod = JsonHelper.deserializeMethod.MakeGenericMethod(new Type[] { configDatas[j].GetType() });
                        object deserializeData = null;
                        try
                        {
                            deserializeData = deserializeMethod.Invoke(null, new object[] { unityWebRequest.downloadHandler.text });
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("NonsensicalAppConfig文件反序列化出错\r\n" + e.ToString());
                        }

                        configDatas[j] = (NonsensicalConfigDataBase)deserializeData;
                    }
                    count--;

                }, null));
            }

            while (count != 0)
            {
                yield return null;
            }
        }


        private string GetFilePath(NonsensicalConfigDataBase configData)
        {
            string configFilePath = Path.Combine(Application.streamingAssetsPath, "Configs", configData.GetType().ToString() + "_" + configData.ConfigID + ".json");
            return configFilePath;
        }

    }
}
