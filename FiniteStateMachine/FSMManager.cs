using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NonsensicalKit.FiniteStateMachine
{
    /// <summary>
    /// 有限状态机管理类
    /// </summary>
    public class FSMManager<T> : NonsensicalMono 
    {
        /// <summary>
        /// 所有状态
        /// </summary>
        private Dictionary<string, State<T>> states = new Dictionary<string, State<T>>();

        /// <summary>
        /// 当前状态
        /// </summary>
        private State<T> crtState;

        public string State { get; private set; }

        private T t;

        public void Init(T t)
        {
            this.t = t;
        }

        /// <summary>
        /// 注册状态
        /// </summary>
        protected void Register(string stateName, State<T>i,float autoRunTime=-1)
        {
            if (states.ContainsKey(stateName)==false)
            {
                states.Add(stateName, i);
                i.Init(this,t, autoRunTime);
            }
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="targetState"></param>
        public void ChangeState(string targetState)
        {
            if (states.ContainsKey(targetState) == false)
            {
                return;
            }
            State = targetState;
            if (crtState!=null)
            {
                crtState.OnExit();
            }

            crtState = states[targetState];

            crtState.OnEnter();
        }

        protected virtual void Update()
        {
            crtState?.OnUpdate();
        }
    }
}
