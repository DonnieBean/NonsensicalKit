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
    /// </summary>
    public class AppConfigManager : NonsensicalManagerBase<AppConfigManager>
    {
        public NonsensicalConfigDataBase[] configDatas;
        public ConfigDataBase[] datas;

        protected override void Awake()
        {
            base.Awake();
            datas = new ConfigDataBase[configDatas.Length];
            InitSubscribe(0, OnInitStart());
        }

        /// <summary>
        /// 用于编辑器环境中的反向读取json文件
        /// </summary>
        public void LoadJson()
        {
            for (int i = 0; i < configDatas.Length; i++)
            {
                var data = configDatas[i].GetData();
                string path = GetFilePath(data);
                string str = FileHelper.ReadAllText(path);
                MethodInfo deserializeMethod = JsonHelper.deserializeMethod.MakeGenericMethod(new Type[] { data.GetType() });
                object deserializeData = null;
                try
                {
                    deserializeData = deserializeMethod.Invoke(null, new object[] { str });
                }
                catch (Exception e)
                {
                    Debug.LogError("NonsensicalAppConfig文件反序列化出错" + path + "\r\n" + e.ToString());
                }

                configDatas[i].SetData((ConfigDataBase)deserializeData); 
                configDatas[i].OnSetDataEnd();
            }
        }

        /// <summary>
        /// 使用类型来获取配置信息，返回匹配的第一个
        /// </summary>
        /// <typeparam name="T">配置类的类型</typeparam>
        /// <param name="t">out值，获取不到时为默认值</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetConfig<T>(out T t) where T : ConfigDataBase
        {
            t = default(T);
            foreach (var configData in datas)
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
        public bool TryGetConfig<T>(string ID, out T t) where T : ConfigDataBase
        {
            t = default(T);
            foreach (var configData in datas)
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
        public bool TryGetConfigValue<Config, T>(string filedName, out T t) where Config : ConfigDataBase
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

        protected IEnumerator OnInitStart()
        {
            if (!PlatformInfo.Instance.isEditor)
            {
                yield return StartCoroutine(LoadAppConfig());
            }
            else
            {
                Dictionary<Type, HashSet<string>> IDTable = new Dictionary<Type, HashSet<string>>();

                for (int i = 0; i < configDatas.Length; i++)
                {
                    var data = configDatas[i].GetData();
                    datas[i] = data;
                    Type t = data.GetType();
                    if (IDTable.ContainsKey(t) == false)
                    {
                        IDTable.Add(t, new HashSet<string>());
                    }

                    string crtID = data.ConfigID;
                    if (!IDTable[t].Contains(crtID))
                    {
                        FileHelper.WriteTxt(GetFilePath(data), JsonHelper.SerializeObject(data));
                        IDTable[t].Add(crtID);
                    }
                    else
                    {
                        Debug.LogError("相同类型配置的ID重复:" + crtID);
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

                ConfigDataBase crtData = configDatas[j].GetData();
                StartCoroutine(HttpHelper.Get(GetFilePath(crtData), null, (unityWebRequest) =>
                {
                    if (unityWebRequest != null && unityWebRequest.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                    {
                        MethodInfo deserializeMethod = JsonHelper.deserializeMethod.MakeGenericMethod(new Type[] { crtData.GetType() });
                        object deserializeData = null;
                        try
                        {
                            deserializeData = deserializeMethod.Invoke(null, new object[] { unityWebRequest.downloadHandler.text });
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("NonsensicalAppConfig文件反序列化出错\r\n" + e.ToString());
                        }

                        configDatas[j] .SetData( (ConfigDataBase)deserializeData);
                        configDatas[j].OnSetDataEnd();
                    }
                    count--;

                    datas[i] = configDatas[j].GetData();

                }, null));
            }

            while (count != 0)
            {
                yield return null;
            }
        }

        private string GetFilePath(ConfigDataBase configData)
        {
            string configFilePath = Path.Combine(Application.streamingAssetsPath, "Configs", configData.GetType().ToString() + "_" + configData.ConfigID + ".json");
            return configFilePath;
        }
    }
}
