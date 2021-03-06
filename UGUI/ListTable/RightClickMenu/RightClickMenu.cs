using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NonsensicalKit.UI
{
    public class RightClickMenu : ListTableManager<RightClickMenuElement, RightClickMenuItem>, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 此顶点应当是右键菜单的左上角
        /// </summary>
        [SerializeField] private RectTransform topNode;

        private bool isHover;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHover = false;
        }

        protected override void Awake()
        {
            base.Awake();

            Subscribe<List<RightClickMenuItem>>((int)UIEnum.OpenRightClickMenu, OnOpen);
            Subscribe((int)UIEnum.CloseRightClickMenu, OnCloseMenu);
        }

        private void Update()
        {
            if ((InputCenter.Instance.mouseLeftKeyDown || InputCenter.Instance.mouseRightKeyDown) && isHover == false)
            {
                CloseSelf();
            }
        }
        
        private void OnOpen(IEnumerable<RightClickMenuItem> datas)
        {

            OpenSelf();
            UpdateUI(datas);
        }

        private void OnCloseMenu()
        {
            CloseSelf();
        }

        protected override void UpdateUI(IEnumerable<RightClickMenuItem> datas)
        {
            base.UpdateUI(datas);
            topNode.position = InputCenter.Instance.mousePos;
        }
    }
}
