using NonsensicalKit.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public class RightClickMenuElement : ListTableElement<RightClickMenuItem>
    {
        [SerializeField] private Image img_Icon;
        [SerializeField] private Text txt_Text;
        [SerializeField] private Button btn_Element;

        public override void SetValue(RightClickMenuItem elementData)
        {
            base.SetValue(elementData);

            if (elementData.spriteName!=null)
            {
                if (SpriteManager.Instance.TyrGetSprite(this,elementData.spriteName,out var v))
                {
                    img_Icon.gameObject.SetActive(true);
                    img_Icon.sprite = v;
                }
            }
            else
            {
                img_Icon.gameObject.SetActive(false);
            }
            txt_Text.text = elementData.text;
            btn_Element.onClick.RemoveAllListeners();
            btn_Element.onClick.AddListener(()=>elementData.clickAction());
        }
    }
}
