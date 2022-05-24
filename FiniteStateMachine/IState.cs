using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.FiniteStateMachine
{
    /// <summary>
    /// ����״̬����״̬
    /// </summary>
    /// <typeparam name="T">״̬�������Ķ�����</typeparam>
    public interface IState<T>
    {
        public void Init(T t);

        public void OnEnter();

        public void OnUpdate();

        public void OnExit();
    }

}
