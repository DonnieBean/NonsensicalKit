using System;
using System.Collections;
using System.Collections.Generic;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// ������Ӧ�̳д���
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


            Subscribe<int>((uint)NonsensicalManagerEnum.InitStart, OnInitStart);
        }

        protected void InitSubscribe(int index, Action action)
        {
            if (actions.ContainsKey(index) == false)
            {
                Publish((uint)NonsensicalManagerEnum.InitSubscribe, index);
                actions.Add(index, action);
            }
        }

        protected void InitSubscribe(int index, IEnumerator coroutine)
        {
            if (coroutines.ContainsKey(index) == false)
            {
                Publish((uint)NonsensicalManagerEnum.InitSubscribe, index);
                coroutines.Add(index, coroutine);
            }
        }

        private void OnInitStart(int index)
        {
            if (actions.ContainsKey(index))
            {
                Init(index, actions[index]);
            }
            else if (coroutines.ContainsKey(index))
            {
                StartCoroutine(Init(index, coroutines[index]));
            }
        }

        private void Init(int index, Action func)
        {
            func.Invoke();

            Publish((uint)NonsensicalManagerEnum.InitComleted, index);
        }

        private IEnumerator Init(int index, IEnumerator coroutine)
        {
            yield return StartCoroutine(coroutine);

            Publish((uint)NonsensicalManagerEnum.InitComleted, index);
        }
    }
}
