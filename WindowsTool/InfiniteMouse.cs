using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NonsensicalKit.WindowsTool
{
    public class InfiniteMouse : MonoBehaviour
    {
        [Serializable]
        public class MouseJumpEvent : UnityEvent<Vector2> { }

        public bool isOn = false;

        [FormerlySerializedAs("onMouseJump")]
        [SerializeField]
        private MouseJumpEvent m_OnMouseJump = new MouseJumpEvent();

        public void Update()
        {
            if (isOn)
            {
                SetSursor();
            }
        }

        public MouseJumpEvent OnMouseJump
        {
            get { return m_OnMouseJump; }
            set { m_OnMouseJump = value; }
        }

        //设置边缘时的鼠标位置
        public void SetSursor()
        {
            IntPtr hWnd = WindowsTool.GetForegroundWindow();    //获取当前窗口句柄
            RECT screenRect = new RECT();
            WindowsTool.GetWindowRect(hWnd, ref screenRect);
            Point p;
            WindowsTool.GetCursorPos(out p);
            if (p.x < screenRect.Left + 9)
            {
                m_OnMouseJump?.Invoke(new Vector2(-Screen.width,0));
                WindowsTool.SetCursorPos(screenRect.Right - 10, p.y);
            }
            if (p.x > screenRect.Right - 9)
            {
                m_OnMouseJump?.Invoke(new Vector2(Screen.width, 0));
                WindowsTool.SetCursorPos(screenRect.Left + 10, p.y);
            }
            if (p.y < screenRect.Top + 9)
            {
                m_OnMouseJump?.Invoke(new Vector2(0, Screen.height));
                WindowsTool.SetCursorPos(p.x, screenRect.Down - 10);
            }
            if (p.y > screenRect.Down - 9)
            {
                m_OnMouseJump?.Invoke(new Vector2(0, -Screen.height));
                WindowsTool.SetCursorPos(p.x, screenRect.Top + 10);
            }
        }
    }
}
