using TMPro;
using UnityEngine.UI;

namespace NonsensicalKit.UI
{
    

    public class MessageWindowPro : NonsensicalUI
    {
        public TextMeshProUGUI txt_Message;

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
