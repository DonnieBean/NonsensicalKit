using NonsensicalKit;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

class InterceptKeys : MonoBehaviour
{
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private event LowLevelKeyboardProc OnKeyBoardKeyEnter;

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);


    public Action<int> keyDown;
    public Action<int> keyUp;
    public Action<KeyCode> keyDownC;
    public Action<KeyCode> keyUpC;

    private IntPtr _hookID = IntPtr.Zero;

    void Start()
    {
        OnKeyBoardKeyEnter += HookCallback;
        _hookID = SetHook(OnKeyBoardKeyEnter);
    }

    void OnDestroy()
    {
        UnhookWindowsHookEx(_hookID);
    }
 
    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (System.Diagnostics.Process curProcess = System.Diagnostics.Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //UnityEngine.Debug.Log("hookKeyDown:" + vkCode);
                keyDown?.Invoke(vkCode);
                keyDownC?.Invoke(Conversion(vkCode));
                MessageAggregator<int>.Instance.Publish("hookKeyDown", vkCode);
                MessageAggregator<KeyCode>.Instance.Publish("hookKeyDown", Conversion(vkCode));

            }
            else if (wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //UnityEngine.Debug.Log("hookKeyDown:" + vkCode);
                keyUp?.Invoke(vkCode);
                keyUpC?.Invoke(Conversion(vkCode));
                MessageAggregator<int>.Instance.Publish("hookKeyUp", vkCode);
                MessageAggregator<KeyCode>.Instance.Publish("hookKeyUp", Conversion(vkCode));
            }

        }
       
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private KeyCode Conversion(int code)
    {
        switch (code)
        {
            case 48: return KeyCode.Alpha0;
            case 49: return KeyCode.Alpha1;
            case 50: return KeyCode.Alpha2;
            case 51: return KeyCode.Alpha3;
            case 52: return KeyCode.Alpha4;
            case 53: return KeyCode.Alpha5;
            case 54: return KeyCode.Alpha6;
            case 55: return KeyCode.Alpha7;
            case 56: return KeyCode.Alpha8;
            case 57: return KeyCode.Alpha9;
            case 65: return KeyCode.A;
            case 66: return KeyCode.B;
            case 67: return KeyCode.C;
            case 68: return KeyCode.D;
            case 69: return KeyCode.E;
            case 70: return KeyCode.F;
            case 71: return KeyCode.G;
            case 72: return KeyCode.H;
            case 73: return KeyCode.I;
            case 74: return KeyCode.J;
            case 75: return KeyCode.K;
            case 76: return KeyCode.L;
            case 77: return KeyCode.M;
            case 78: return KeyCode.N;
            case 79: return KeyCode.O;
            case 80: return KeyCode.P;
            case 81: return KeyCode.Q;
            case 82: return KeyCode.R;
            case 83: return KeyCode.S;
            case 84: return KeyCode.T;
            case 85: return KeyCode.U;
            case 86: return KeyCode.V;
            case 87: return KeyCode.W;
            case 88: return KeyCode.X;
            case 89: return KeyCode.Y;
            case 90: return KeyCode.Z;
            case 96: return KeyCode.Keypad0;
            case 97: return KeyCode.Keypad1;
            case 98: return KeyCode.Keypad2;
            case 99: return KeyCode.Keypad3;
            case 100: return KeyCode.Keypad4;
            case 101: return KeyCode.Keypad5;
            case 102: return KeyCode.Keypad6;
            case 103: return KeyCode.Keypad7;
            case 104: return KeyCode.Keypad8;
            case 105: return KeyCode.Keypad9;
            case 112: return KeyCode.F1;
            case 113: return KeyCode.F2;
            case 114: return KeyCode.F3;
            case 115: return KeyCode.F4;
            case 116: return KeyCode.F5;
            case 117: return KeyCode.F6;
            case 118: return KeyCode.F7;
            case 119: return KeyCode.F8;
            case 120: return KeyCode.F9;
            case 121: return KeyCode.F10;
            case 122: return KeyCode.F11;
            case 123: return KeyCode.F12;
            case 192: return KeyCode.BackQuote;
            default: return KeyCode.None;
        }
    }
}