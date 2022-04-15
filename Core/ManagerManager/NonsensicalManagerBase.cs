using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类应继承此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NonsensicalManagerBase<T> : NonsensicalMono where T : class
    {
        public static T Instance;

        private Dictionary<int, Action> actions = new Dictionary<int, Action>();
        private Dictionary<int, IEnumerator> coroutines = new Dictionary<int, IEnumerator>();

        protected override void Awake()
        {
            base.Awake();

            Instance = this as T;


            Subscribe<int>((int)NonsensicalManagerEnum.InitStart, OnInitStart);
        }

        protected void InitSubscribe(int index, Action action)
        {
            if (actions.ContainsKey(index) == false)
            {
                Publish((int)NonsensicalManagerEnum.InitSubscribe, index);
                actions.Add(index, action);
            }
        }

        protected void InitSubscribe(int index, IEnumerator coroutine)
        {
            if (coroutines.ContainsKey(index) == false)
            {
                Publish((int)NonsensicalManagerEnum.InitSubscribe, index);
                coroutines.Add(index, coroutine);
            }
        }

        private void OnInitStart(int index)
        {
            if (actions.ContainsKey(index))
            {
                Init(index, actions[index]);
            }
            
            if (coroutines.ContainsKey(index))
            {
                StartCoroutine(Init(index, coroutines[index]));
            }
        }

        private void Init(int index, Action func)
        {
            func.Invoke();
            Debug.Log("Manager Load Complete:"+ GetType() );
            Publish((int)NonsensicalManagerEnum.InitComleted, index);
        }

        private IEnumerator Init(int index, IEnumerator coroutine)
        {
            yield return StartCoroutine(coroutine);
            Debug.Log("Manager Load Complete:" + GetType());
            Publish((int)NonsensicalManagerEnum.InitComleted, index);
        }
    }
}
