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
    public class AppConfigManager : NonsensicalManagerBase<AppConfigManager>
    {
        public NonsensicalConfigDataBase[] configDatas;

        private bool isEditor;

        /// <summary>
        /// 使用类型来获取配置信息，返回匹配的第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool TryGetConfig<T>(out T t) where T : class
        {
            t = null;
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
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool TryGetConfig<T>(string ID, out T t) where T : class
        {
            t = null;
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

        protected override void Awake()
        {
            base.Awake();

            isEditor = Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.OSXEditor
                || Application.platform == RuntimePlatform.LinuxEditor;
        }

        protected override void InitStart()
        {
            if (!isEditor)
            {
                StartCoroutine(LoadAppConfig());
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

                InitComplete();
            }
        }

        private IEnumerator LoadAppConfig()
        {
            int count = configDatas.Length;
            for (int i = 0; i < configDatas.Length; i++)
            {
                int j = i;
                Debug.Log(GetFilePath(configDatas[i]));
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
            InitComplete();
        }

        protected override void LateInitStart()
        {
            LateInitComplete();
        }

        private string GetFilePath(NonsensicalConfigDataBase configData)
        {
            string configFilePath = Path.Combine(Application.streamingAssetsPath, "Configs", configData.GetType().ToString() + "_" + configData.ConfigID + ".json");
            return configFilePath;
        }
    }
}
