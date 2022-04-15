using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    public class MessageInfo
    {
        public string message;
        public float surviceTime;

        public MessageInfo(string message, float surviceTime)
        {
            this.message = message;
            this.surviceTime = surviceTime;
        }
    }

    public class MessageWindow : NonsensicalUI
    {
        public Text txt_Message;

        private MessageInfo crtMessageInfo;

        protected override void Awake()
        {
            base.Awake();
            Subscribe<MessageInfo>((int)UIEnum.OpenMessagePanel, OpenMessageWindow);
        }

        private void OpenMessageWindow(MessageInfo messageInfo)
        {
            crtMessageInfo = messageInfo;

            txt_Message.text = messageInfo.message;

            OpenSelf(false);
            NonsensicalUnityInstance.Instance.DelayDoIt(messageInfo.surviceTime,()=> { CloseSelf(); });
        }
    }
}
