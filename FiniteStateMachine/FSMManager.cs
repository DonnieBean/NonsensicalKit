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
        private Dictionary<string, State<T>> states = new Dictionary<string, State<T>>();

        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        private State<T> crtState;

        public string State { get; private set; }

        private T t;

        public void Init(T t)
        {
            this.t = t;
        }

        /// <summary>
        /// ע��״̬
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
        /// �ı�״̬
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
