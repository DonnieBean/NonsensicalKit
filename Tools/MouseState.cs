using UnityEngine;

namespace NonsensicalKit.Tools
{

    /// <summary>
    /// 鼠标当前状态，防止交互冲突
    /// </summary>
    public class MouseState : MonoBehaviour
    {
        public static bool IsApplicationFocused { get; private set; } = true;   //是否拥有程序焦点

        public static bool IncludedByWindow { get; private set; } = true;     //鼠标是否在视窗中

        public static bool FocusedAndIncluded { get { return IsApplicationFocused && IncludedByWindow; } }      //拥有程序焦点且鼠标在视窗中

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
