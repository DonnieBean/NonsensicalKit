﻿using System.Collections;
using System.IO;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    public class NonsensicalRuntimeManager : MonoSingleton<NonsensicalRuntimeManager>
    {
        private int _initCount;
        private int _lateInitCount;

        protected override void Awake()
        {
            Subscribe((uint)NonsensicalManagerEnum.InitSubscribe, InitSubscribe);
            Subscribe((uint)NonsensicalManagerEnum.InitComlete, InitComplete);
            Subscribe((uint)NonsensicalManagerEnum.LateInitComlete, LateInitComplete);
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
                MessageAggregator.Instance.Publish((uint)NonsensicalManagerEnum.AllInitComplete);
            }
        }

        private IEnumerator Init()
        {
            yield return null;

            if (_initCount == 0)
            {

                MessageAggregator.Instance.Publish((uint)NonsensicalManagerEnum.AllInitComplete);
            }
            else
            {
                InitStart();
            }
        }
    }
}
