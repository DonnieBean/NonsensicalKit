using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    [RequireComponent(typeof(Button))]
    public class SendBoolSignalButton : NonsensicalMono
    {
        [SerializeField] private string signal;

        [SerializeField] private bool initState;

        private Button btn_Self;
        private bool crtState;


        protected override void Awake()
        {
            base.Awake();
            crtState = initState;

            btn_Self = GetComponent<Button>();
            if (btn_Self != null)
            {
                btn_Self.onClick.AddListener(SendSignal);
            }
        }

        private void SendSignal()
        {
            crtState = !crtState;
            Publish(signal, crtState);
        }
    }
}