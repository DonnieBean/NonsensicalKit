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
       private Dictionary<string, IState<T>> states = new Dictionary<string, IState<T>>();

        /// <summary>
        /// 当前状态
        /// </summary>
        private IState<T> crtState;

        /// <summary>
        /// 注册状态
        /// </summary>
        protected void Register(string stateName, IState<T>i)
        {
            if (states.ContainsKey(stateName)==false)
            {
                states.Add(stateName, i);
            }
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="targetState"></param>
        protected void ChangeState(string targetState)
        {
            if (states.ContainsKey(targetState) == false)
            {
                return;
            }

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
