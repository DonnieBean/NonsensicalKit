using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    [RequireComponent(typeof(Button))]
    public class SendStringSignalButton : NonsensicalMono
    {
        [SerializeField] private string signal;
        [SerializeField] private string message;
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

        public void SetSignalAndMessage(string newSignal, string newMessage)
        {
            signal = newSignal;
            message = newMessage;
        }

        public void SetSignal(string newSignal)
        {
            signal = newSignal;
        }

        public void SetMessage(string newMessage)
        {
            message = newMessage;
        }

        private void SendSignal()
        {
            Publish(signal, message);
        }
    }
}