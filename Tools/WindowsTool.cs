using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NonsensicalKit.Tools
{
    public struct RECT
    {
        public int Left;        //最左坐标
        public int Top;         //最上坐标
        public int Right;       //最右坐标
        public int Down;        //最下坐标
    }

    public struct Point
    {
        public int x;
        public int y;
    }

    public class WindowsTool : MonoBehaviour
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);

        //设置边缘时的鼠标位置
        public void SetSursor()
        {
            IntPtr hWnd = GetForegroundWindow();    //获取当前窗口句柄
            RECT screenRect = new RECT();
            GetWindowRect(hWnd, ref screenRect);
            Point p;
            GetCursorPos(out p);
            if (p.x < screenRect.Left + 9)
            {
                SetCursorPos(screenRect.Right - 10, p.y);
            }
            if (p.x > screenRect.Right - 9)
            {
                SetCursorPos(screenRect.Left + 10, p.y);
            }
            if (p.y < screenRect.Top + 9)
            {
                SetCursorPos(p.x, screenRect.Down - 10);
            }
            if (p.y > screenRect.Down - 9)
            {
                SetCursorPos(p.x, screenRect.Top + 10);
            }
        }
    }
}