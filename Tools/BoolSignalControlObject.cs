using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NonsensicalKit
{
    public class BoolSignalControlObject : NonsensicalMono
    {
        [SerializeField] private GameObject target;
        [SerializeField] private string signal;

        protected override void Awake()
        {
            base.Awake();

            Subscribe<bool>(signal, OnChangeTarget);
        }

        private void OnChangeTarget(bool state)
        {
            target.SetActive(state);
        }
    }

}
