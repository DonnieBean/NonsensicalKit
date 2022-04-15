using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    [RequireComponent(typeof(Button))]
    public class SendIntSignalButton : NonsensicalMono
    {
        [SerializeField] private string signal;
        [SerializeField] private int index;
        private Button btn_Self;

        protected override void Awake()
        {
            base.Awake();
            btn_Self = GetComponent<Button>();
            if (btn_Self != null)
            {
                btn_Self.onClick.AddListener(SendSignal);
            }
            
        }

        public void SetSignalAndMessage(string newSignal, int newIndex)
        {
            signal = newSignal;
            index = newIndex;
        }

        public void SetSignal(string newSignal)
        {
            signal = newSignal;
        }

        public void SetMessage(int newIndex)
        {
            index = newIndex;
        }

        private void SendSignal()
        {
            Publish(signal, index);
        }
    }
}

