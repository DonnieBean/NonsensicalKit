using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public class SignalControlObject : NonsensicalMono
    {
        [SerializeField] private GameObject target;
        [SerializeField] private string showSignal;
        [SerializeField] private string hideSignal;

        protected override void Awake()
        {
            base.Awake();

            Subscribe(showSignal, OnShowTarget);
            Subscribe(hideSignal, OnHideTarget);
        }

        private void OnShowTarget()
        {
            target.SetActive(true);
        }
        private void OnHideTarget()
        {
            target.SetActive(false);
        }
    }

}
