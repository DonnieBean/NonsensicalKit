using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NonsensicalKit.FiniteStateMachine
{
    /// <summary>
    /// ����״̬��������
    /// </summary>
    public class FSMManager<T> : NonsensicalMono 
    {
        /// <summary>
        /// ����״̬
        /// </summary>
       private Dictionary<string, IState<T>> states = new Dictionary<string, IState<T>>();

        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        private IState<T> crtState;

        /// <summary>
        /// ע��״̬
        /// </summary>
        protected void Register(string stateName, IState<T>i)
        {
            if (states.ContainsKey(stateName)==false)
            {
                states.Add(stateName, i);
            }
        }

        /// <summary>
        /// �ı�״̬
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
