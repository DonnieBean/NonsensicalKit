using System;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public class ConfirmInfo
    {
        public string message;
        public string leftButtonText;
        public string rightButtonText;
        public Action leftButtonClick;
        public Action rightButtonClick;

        public ConfirmInfo(string message, string leftButtonText, string rightButtonText, Action leftButtonClick, Action rightButtonClick)
        {
            this.message = message;
            this.leftButtonText = leftButtonText;
            this.rightButtonText = rightButtonText;
            this.leftButtonClick = leftButtonClick;
            this.rightButtonClick = rightButtonClick;
        }
    }

    public class ConfirmWindow : NonsensicalUI
    {
        public Text txt_Message;
        public Text txt_LeftButton;
        public Text txt_RightButton;

        private ConfirmInfo crtConfirmInfo;

        protected override void Awake()
        {
            base.Awake();
            Subscribe<ConfirmInfo>((uint)UIEnum.OpenConfirmPanel, OpenConfirmWindow);
        }
        private void OpenConfirmWindow(ConfirmInfo confirmInfo)
        {
            crtConfirmInfo = confirmInfo;

            txt_Message.text = confirmInfo.message;
            txt_LeftButton.text = confirmInfo.leftButtonText;
            txt_RightButton.text = confirmInfo.rightButtonText;
            OpenSelf(false);
        }

        public void LeftButtonClick()
        {
            CloseSelf();
            crtConfirmInfo.leftButtonClick?.Invoke();
        }

        public void RightButtonClick()
        {
            CloseSelf();
            crtConfirmInfo.rightButtonClick?.Invoke();

        }
    }
}
