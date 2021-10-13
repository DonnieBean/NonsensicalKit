using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit
{
    [RequireComponent(typeof(Button))]
    public class SendSignalButton : NonsensicalMono
    {
        [SerializeField] private string signal;
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

        private void SendSignal()
        {
            Publish(signal);
        }
    }
}