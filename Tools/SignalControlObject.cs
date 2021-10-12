using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public class SignalControlObject : NonsensicalMono
    {
        [SerializeField] private GameObject target;
        [SerializeField] private string signal;

        protected override void Awake()
        {
            base.Awake();

            Subscribe<bool>(signal, OnShowTarget);
        }

        private void OnShowTarget(bool state)
        {
            target.SetActive(state);
        }
    }

}
