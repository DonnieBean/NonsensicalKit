using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.FiniteStateMachine
{
    /// <summary>
    /// 有限状态机的状态
    /// </summary>
    /// <typeparam name="T">状态机操作的对象类</typeparam>
    public interface IState<T>
    {
        public void Init(T t);

        public void OnEnter();

        public void OnUpdate();

        public void OnExit();
    }

}
