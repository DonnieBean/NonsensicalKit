using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public class GameObjectPool 
    {
        private GameObject prefab;
        private Queue<GameObject> queue;
        private Action<GameObject> resetAction;
        private Action<GameObject> initAction;
        private Action<GameObject> onetimeInitAction;

        public GameObjectPool( GameObject prefab, Action<GameObject>
            ResetAction , Action<GameObject> InitAction = null, Action<GameObject> OnetimeInitAction = null)
        {
            this.prefab = prefab;
            queue = new Queue<GameObject>();
            resetAction = ResetAction;
            initAction = InitAction;
            onetimeInitAction = OnetimeInitAction;
        }

        public GameObject New()
        {
            if (queue.Count > 0)
            {
                GameObject t = queue.Dequeue();
                initAction?.Invoke(t);
                return t;
            }
            else
            {
                GameObject t = GameObject.Instantiate(prefab);
                onetimeInitAction?.Invoke(t);
                initAction?.Invoke(t);

                return t;
            }
        }

        public void Store(GameObject obj)
        {
            if (queue.Contains(obj)==false)
            {
                resetAction(obj);
                queue.Enqueue(obj);
            }
        }
    }
}
