using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 运行时的管理类管理类，对管理类统一管理初始化
    /// 执行顺序为 Init=>LateInit=>FinalInit
    /// </summary>
    public class NonsensicalRuntimeManager : MonoSingleton<NonsensicalRuntimeManager>
    {
        /// <summary>
        /// 是否全部管理类已初始化完成
        /// </summary>
        public bool allInitCompleted { get; private set; }

        /// <summary>
        /// 记录每个批次需要初始化的管理类个数
        /// </summary>
        private Dictionary<int, int> initCount = new Dictionary<int, int>();

        /// <summary>
        /// 最大批次
        /// </summary>
        private int maxBatch;

        protected override void Awake()
        {
            base.Awake();

            Subscribe<int>((uint)NonsensicalManagerEnum.InitSubscribe, InitSubscribe);
            Subscribe<int>((uint)NonsensicalManagerEnum.InitComleted, InitComplete);
        }

        private void Start()
        {
            string logLock = Path.Combine(Application.streamingAssetsPath, "Nonsensical");

            if (File.Exists(logLock))
            {
                File.Delete(logLock);
                gameObject.AddComponent<DebugConsole>();
            }
            StartCoroutine(InitStart(0));
        }

        private void InitSubscribe(int index)
        {
            if (index > maxBatch)
            {
                maxBatch = index;
            }

            if (initCount.ContainsKey(index) == false)
            {
                initCount.Add(index, 0);
            }

            initCount[index]++;
        }

        private IEnumerator InitStart(int crtBatch)
        {
            //管理类会在Start时开始注册，Start后等待一帧保证注册全部完成
            yield return null;

            Publish((uint)NonsensicalManagerEnum.InitStart, crtBatch);
        }

        private void InitComplete(int index)
        {
            initCount[index]--;
            if (initCount[index] == 0)
            {
                if (index == maxBatch)
                {
                    allInitCompleted = true;
                    MessageAggregator.Instance.Publish((uint)NonsensicalManagerEnum.AllInitComplete);
                }
                else
                {
                    while (true)
                    {
                        index++;
                        if (initCount.ContainsKey(index))
                        {
                            break;
                        }
                        if (index>maxBatch)
                        {
                            LogManager.Instance.LogFatal("管理类批次出现错误");
                        }
                    }
                    Publish((uint)NonsensicalManagerEnum.InitStart, index);
                }
            }
        }
    }
}
