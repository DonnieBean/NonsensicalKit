using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// GameObject对象池
    /// </summary>
    public class GameObjectPool 
    {
        private GameObject prefab;  //预制体
        private Queue<GameObject> queue;    //待使用的对象
        private Action<GameObject> resetAction; //返回池中后调用
        private Action<GameObject> initAction;  //取出时调用
        private Action<GameObject> firstInitAction; //首次生成时调用

        public GameObjectPool( GameObject prefab, Action<GameObject>
            ResetAction , Action<GameObject> InitAction = null, Action<GameObject> FirstInitAction = null)
        {
            this.prefab = prefab;
            queue = new Queue<GameObject>();
            resetAction = ResetAction;
            initAction = InitAction;
            firstInitAction = FirstInitAction;
        }

        /// <summary>
        /// 取出对象
        /// </summary>
        /// <returns></returns>
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
                firstInitAction?.Invoke(t);
                initAction?.Invoke(t);

                return t;
            }
        }

        /// <summary>
        /// 放回对象
        /// </summary>
        /// <param name="obj"></param>
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
