using System;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    /// <summary>
    /// GameObject�����
    /// </summary>
    public class GameObjectPool 
    {
        private GameObject prefab;  //Ԥ����
        private Queue<GameObject> queue;    //��ʹ�õĶ���
        private Action<GameObject> resetAction; //���س��к����
        private Action<GameObject> initAction;  //ȡ��ʱ����
        private Action<GameObject> firstInitAction; //�״�����ʱ����

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
        /// ȡ������
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
        /// �Żض���
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
