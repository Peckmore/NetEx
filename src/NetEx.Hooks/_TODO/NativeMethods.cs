using NetEx.Hooks.Internal;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetEx.Hooks
{
    internal static class NativeMethods
    {
        #region Constants

        public const int HC_ACTION = 0;

        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 0x1;
        public const int INPUT_HARDWARE = 0x2;

        public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int KEYEVENTF_UNICODE = 0x4;
        public const int KEYEVENTF_SCANCODE = 0x8;

        public const int MOUSEEVENTF_MOVE = 1;
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_LEFTUP = 0x4;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;
        public const int MOUSEEVENTF_XDOWN = 0x80;
        public const int MOUSEEVENTF_XUP = 0x100;
        public const int MOUSEEVENTF_WHEEL = 0x800;
        public const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        public const int WH_KEYBOARD_LL = 0xD;
        public const int WH_MOUSE_LL = 0xE;

        public const int WM_DESTROY = 0x0002;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MOUSEHWHEEL = 0x20E;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_NCXBUTTONDOWN = 0xAB;
        public const int WM_NCXBUTTONUP = 0xAC;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_XBUTTONDOWN = 0x20B;
        public const int WM_XBUTTONUP = 0x20C;
        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_CHANGECBCHAIN = 0x30D;

        public const int XBUTTON1 = 0x1;
        public const int XBUTTON2 = 0x2;

        #endregion

        #region Methods

        #region kernel32.dll

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        #region user32.dll

        [DllImport("user32.dll", EntryPoint = "CallNextHookEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(SafeWindowsHookHandle hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "ChangeClipboardChain", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetDoubleClickTime();

        //[DllImport("user32.dll", EntryPoint = "GetSystemMetrics", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern Int32 GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", EntryPoint = "SendInput", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint SendInput(uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);

        public static uint SendInput(INPUT pInput, int cbSize)
        {
            return SendInput(1, new[] { pInput}, cbSize);
        }
        
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetClipboardViewer", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeWindowsHookHandle SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr WindowFromPoint(Point pt);

        #endregion

        #endregion
    }
}