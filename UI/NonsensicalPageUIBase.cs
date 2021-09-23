using UnityEngine;

namespace NonsensicalKit.UI
{
    public abstract class NonsensicalPageUIBase : NonsensicalUI
    {

        protected override void OnOpen()
        {
            _rectTransform.anchoredPosition = Vector2.zero;
            base.OnOpen();
        }
        protected override void OnClose()
        {
            base.OnClose();
            _rectTransform.anchoredPosition = new Vector2(10000, 10000);
        }
    }
}

