using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public abstract class TreeNodeTableElementBase<ElementData> : NonsensicalUI where ElementData : ITreeNodeClass<ElementData>
    {
        public Text txt_NodeName;

        public RectTransform rt_Box;
        public Button btn_Fold;
        public Button btn_Visible;
        public Button btn_Name;
        public Image img_FoldState;
        public Image img_VisibleState;

        public Sprite sp_Fold;
        public Sprite sp_Unfold;
        public Sprite sp_Visible;
        public Sprite sp_Invisible;

        //每一级子节点往右移动多少距离（单位：像素）
        public int levelDistance = 25;

        public int Level { get; set; }

        public ElementData elementData;

        public bool IsFold { get { return elementData.IsFold; } set { elementData.IsFold = value; } }

        public bool IsVisible { get { return elementData.IsVisible; } set { elementData.IsVisible = value; } }

        /// <summary>
        /// box一开始坐标值，作为位移的基准值
        /// </summary>
        private Vector2 basePosition;

        protected override void Awake()
        {
            base.Awake();
            basePosition = rt_Box.anchoredPosition;

            btn_Fold.onClick.AddListener(OnFoldButtonClick);
            btn_Visible.onClick.AddListener(OnVisibleButtonClick);
            btn_Name.onClick.AddListener(OnNameClick);
        }

        public virtual void SetValue(ElementData elementData, int level, bool isFold = true, bool isVisible = true)
        {
            this.Level = level;
            this.elementData = elementData;
            rt_Box.anchoredPosition = new Vector2(basePosition.x + level * levelDistance, basePosition.y);
            btn_Fold.gameObject.SetActive(elementData.GetChild() != null && elementData.GetChild().Count != 0);
            img_FoldState.sprite = sp_Fold;

            ChangeFold(isFold);
            ChangeVisible(isVisible);
        }

        protected virtual void OnFoldButtonClick()
        {
            ChangeFold(!IsFold);
        }

        protected virtual void OnVisibleButtonClick()
        {
            ChangeVisible(!IsVisible);
        }

        public virtual void ChangeFold(bool isFold)
        {
            IsFold = isFold;
            if (IsFold)
            {
                img_FoldState.sprite = sp_Fold;
            }
            else
            {
                img_FoldState.sprite = sp_Unfold;
            }
        }

        public virtual void ChangeVisible(bool isVisible)
        {
            IsVisible = isVisible;
            if (IsVisible)
            {
                img_VisibleState.sprite = sp_Visible;
            }
            else
            {
                img_VisibleState.sprite = sp_Invisible;
            }
        }

        protected abstract void OnNameClick();
    }

}
