﻿using System.Collections;
using System.IO;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 运行时的管理类管理类，对管理类统一管理初始化
    /// </summary>
    public class NonsensicalRuntimeManager : MonoSingleton<NonsensicalRuntimeManager>
    {
        private int _initCount;
        private int _lateInitCount;
        private int _finalInitCount;

        public bool loadCompleted { get;private set; }

        protected override void Awake()
        {
            base.Awake();
            Subscribe((uint)NonsensicalManagerEnum.ManagerSubscribe, InitSubscribe);
            Subscribe((uint)NonsensicalManagerEnum.InitComlete, InitComplete);
            Subscribe((uint)NonsensicalManagerEnum.LateInitComlete, LateInitComplete);
            Subscribe((uint)NonsensicalManagerEnum.FinalInitComlete, FinalInitComplete);
        }

        private void Start()
        {
            string logLock = Path.Combine(Application.streamingAssetsPath, "Nonsensical");

            if (File.Exists(logLock))
            {
                File.Delete(logLock);
                gameObject.AddComponent<DebugConsole>();
            }
            StartCoroutine(Init());
        }

        private void InitSubscribe()
        {
            _initCount++;
            _lateInitCount++;
            _finalInitCount++;
        }

        private IEnumerator Init()
        {
            yield return null;

            if (_initCount == 0)
            {
                loadCompleted = true;
                MessageAggregator.Instance.Publish((uint)NonsensicalManagerEnum.AllInitComplete);
            }
            else
            {
                InitStart();
            }
        }
        private void InitStart()
        {
            Publish((uint)NonsensicalManagerEnum.InitStart);
        }

        private void InitComplete()
        {
            _initCount--;

            if (_initCount == 0)
            {
                LateInitStart();
            }
        }

        private void LateInitStart()
        {
            Publish((uint)NonsensicalManagerEnum.LateInitStart);
        }

        private void LateInitComplete()
        {
            _lateInitCount--;

            if (_lateInitCount == 0)
            {
                FinalInitStart();
            }
        }
        private void FinalInitStart()
        {
            Publish((uint)NonsensicalManagerEnum.FinalInitStart);
        }
        private void FinalInitComplete()
        {
            _finalInitCount--;

            if (_finalInitCount == 0)
            {
                loadCompleted = true;
                MessageAggregator.Instance.Publish((uint)NonsensicalManagerEnum.AllInitComplete);
            }
        }

    }
}
