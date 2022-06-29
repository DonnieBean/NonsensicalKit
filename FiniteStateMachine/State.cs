using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit.FiniteStateMachine
{
    /// <summary>
    /// 有限状态机的状态
    /// </summary>
    /// <typeparam name="T">状态机操作的对象类</typeparam>
    public abstract class State<T>
    {
        protected T t;
        protected FSMManager<T> fsm;
        protected float timer;
        protected float autoRunTime;
        public virtual void Init(FSMManager<T> fsm, T t,float autoRunTime)
        {
            this.fsm = fsm;
            this.t = t;
            this.autoRunTime = autoRunTime;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }
}
