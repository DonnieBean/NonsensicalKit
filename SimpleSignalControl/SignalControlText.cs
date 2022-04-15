using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit 
{ 
    [RequireComponent(typeof(Text))]
public class SignalControlText : NonsensicalMono
{
        [SerializeField] private string signal;

        private Text txt_self;

        protected override void Awake()
        {
            base.Awake();
            txt_self = GetComponent<Text>();
            Subscribe<string>(signal, OnReceive);
        }

        private void OnReceive(string str)
        {
            txt_self.text = str;
        }
    }
}