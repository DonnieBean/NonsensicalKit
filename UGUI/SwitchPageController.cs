using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public class SwitchPageController : NonsensicalUI
    {
        [SerializeField] protected Button[] buttons;

        [SerializeField] protected GameObject[] targets;

        [SerializeField] private int initSelect = 0;

        private GameObject[] selectedImages;

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
                selectedImages = new GameObject[targets.Length];

                for (int i = 0; i < selectedImages.Length; i++)
                {
                    selectedImages[i] = buttons[i].transform.Find("img_selected").gameObject;
                }

                Switch(initSelect);
            }

           
        }

        protected virtual void Switch(int index)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (index == i)
                {
                    targets[i].SetActive(true);
                    selectedImages[i].SetActive(true);
                }
                else
                {
                    targets[i].SetActive(false);
                    selectedImages[i].SetActive(false);
                }
            }
        }
    }
}
