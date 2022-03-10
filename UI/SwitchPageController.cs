using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public class SwitchPageController : NonsensicalUI
    {
        public Button[] buttons;

        public GameObject[] targets;

        protected override void Awake()
        {
            base.Awake();

            if (buttons.Length != targets.Length)
            {
                Debug.LogWarning("数量不正确");
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    int index = i;
                    buttons[i].onClick.AddListener(() => { Switch(index); });
                }
                Switch(0);
            }
        }

        protected virtual void Switch(int index)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (index == i)
                {
                    targets[i].gameObject.SetActive(true);
                }
                else
                {
                    targets[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
