using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public abstract class PreivewElementBase<ElementData> : ListTableElement<ElementData> where ElementData : class
    {
        /// <summary>
        /// 是否可以点击编辑
        /// </summary>
        public bool CanEditName;

        public CanvasGroup cg_IpfName;
        public Button btn_EditName;
        public Button btn_Preview;
        public Image img_Preview;
        public InputField ipf_Name;

        public Text txt_Button;

        protected override void Awake()
        {
            base.Awake();

            btn_EditName.onClick.AddListener(OnEditNameClick);
            btn_Preview.onClick.AddListener(OnPreviewClick);
            ipf_Name.onEndEdit.AddListener(NameEditEnd);
        }

        protected override void Start()
        {
            base.Start();
            cg_IpfName.alpha = 0;
            cg_IpfName.blocksRaycasts = false;
            btn_EditName.interactable = true;
        }

        protected abstract void OnPreviewClick();

        protected virtual void SetPreview(Sprite sprite)
        {
            img_Preview.sprite = sprite;
        }
        protected virtual void SetPreview(Texture2D texture2D)
        {
            if (img_Preview.sprite != null)
            {
                Destroy(img_Preview.sprite);
            }
            img_Preview.sprite = Sprite.Create(texture2D, new Rect(0, 0, 300, 300), new Vector2(0.5f, 0.5f));
        }

        protected void OnEditNameClick()
        {
            if (!CanEditName)
            {
                return;
            }

            cg_IpfName.alpha = 1;
            cg_IpfName.blocksRaycasts = true;
            btn_EditName.interactable = false;
        }

        protected virtual void NameEditEnd(string newName)
        {
            cg_IpfName.alpha = 0;
            cg_IpfName.blocksRaycasts = false;
            btn_EditName.interactable = true;

            txt_Button.text = newName;
        }
    }

}
