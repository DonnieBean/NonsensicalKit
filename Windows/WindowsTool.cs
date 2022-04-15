using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NonsensicalKit.WindowsTool
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
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);
    }
}