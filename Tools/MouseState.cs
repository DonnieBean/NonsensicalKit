using UnityEngine;

namespace NonsensicalKit.Tools
{

    /// <summary>
    /// ��굱ǰ״̬����ֹ������ͻ
    /// </summary>
    public class MouseState : MonoBehaviour
    {
        public static bool IsApplicationFocused { get; private set; } = true;   //�Ƿ�ӵ�г��򽹵�

        public static bool IncludedByWindow { get; private set; } = true;     //����Ƿ����Ӵ���

        public static bool FocusedAndIncluded { get { return IsApplicationFocused && IncludedByWindow; } }      //ӵ�г��򽹵���������Ӵ���

        private void Awake()
        {
            IsApplicationFocused = Application.isFocused;
        }

        private void OnApplicationFocus(bool focus)
        {
            IsApplicationFocused = focus;
        }

        private void Update()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x < 0 || mousePos.x > screenWidth || mousePos.y < 0 || mousePos.y > screenHeight)
            {
                IncludedByWindow = false;
            }
            else
            {
                IncludedByWindow = true;
            }
        }
    }

}
