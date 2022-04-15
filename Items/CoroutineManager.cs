using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// 以Transform为键管理协程
    /// </summary>
    public class CoroutineManager
    {
        private Dictionary<Transform, CoroutineInfo> coroutines = new Dictionary<Transform, CoroutineInfo>();

        public bool CheckPlaying(Transform target)
        {
            if (coroutines.ContainsKey(target))
            {
                return coroutines[target].isPlaying;
            }
            else
            {
                return false;
            }
        }

        public void PlayCoroutine(Transform target, IEnumerator coroutine)
        {
            if (coroutines.ContainsKey(target)==false)
            {
                CoroutineInfo ci = new CoroutineInfo();
                coroutines.Add(target, ci);
            }
          var v=  NonsensicalUnityInstance.Instance.StartCoroutine( RunIt(coroutines[target], coroutine));
            coroutines[target].Coroutine = v;
        }

        private IEnumerator RunIt(CoroutineInfo ci ,IEnumerator coroutine)
        {
            ci.isPlaying = true;

            yield return coroutine;

            ci.isPlaying = false;
        }

        public void Stop(Transform target)
        {
            if (coroutines.ContainsKey(target) && coroutines[target].isPlaying==true)
            {
                coroutines[target].isPlaying = false;
                NonsensicalUnityInstance.Instance.StopCoroutine(coroutines[target].Coroutine);
            }
        }
    }

    public class CoroutineInfo
    {
        public Coroutine Coroutine { get; set; }
        public bool isPlaying=false;
    }
}
