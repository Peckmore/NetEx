using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NetEx.Hooks.Interop
{
    internal static class NativeMethods
    {
        #region Constants

        public const int HC_ACTION = 0;

        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 0x1;
        //public const int INPUT_HARDWARE = 0x2;

        //public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;
        //public const int KEYEVENTF_UNICODE = 0x4;
        //public const int KEYEVENTF_SCANCODE = 0x8;

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

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        #region user32.dll

        [DllImport("user32.dll", EntryPoint = "CallNextHookEx", SetLastError = true)]
        public static extern IntPtr CallNextHookEx(SafeWindowsHookHandle hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "ChangeClipboardChain", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", EntryPoint = "CreateWindowExW", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int exStyle, string className, string windowName, uint style, int x, int y, int width, int height, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "DispatchMessage", SetLastError = true)]
        public static extern IntPtr DispatchMessage(ref MSG lpMsg);

        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime", SetLastError = true)]
        public static extern int GetDoubleClickTime();

        [DllImport("user32.dll", EntryPoint = "GetMessage", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics", SetLastError = true)]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport("user32.dll", EntryPoint = "IsWindow", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "PostMessageW", SetLastError = true)]
        public static extern ushort PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "PostQuitMessage", SetLastError = true)]
        public static extern ushort PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true)]
        public static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);
        
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetClipboardViewer", SetLastError = true)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
        public static extern SafeWindowsHookHandle SetWindowsHookEx(int idHook, Delegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", EntryPoint = "TranslateMessage", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", EntryPoint = "UnregisterClassW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        #endregion

        #region Public

        public static Rectangle GetDoubleClickArea()
        {
            return new Rectangle(0,
                                 0,
                                 GetSystemMetrics(SystemMetric.SM_CXDOUBLECLK),
                                 GetSystemMetrics(SystemMetric.SM_CYDOUBLECLK));
        }
        public static Size GetPrimaryMonitorSize()
        {
            return new(GetSystemMetrics(SystemMetric.SM_CXSCREEN), GetSystemMetrics(SystemMetric.SM_CYSCREEN));
        }
        public static Rectangle GetVirtualScreen()
        {
            // Borrowed from .Net/WinForms
            // https://github.com/dotnet/winforms/blob/release/9.0/src/System.Windows.Forms/src/System/Windows/Forms/SystemInformation.cs#L449

            if (GetSystemMetrics(SystemMetric.SM_CMONITORS) != 0)
            {
                return new(GetSystemMetrics(SystemMetric.SM_XVIRTUALSCREEN),
                                    GetSystemMetrics(SystemMetric.SM_YVIRTUALSCREEN),
                                    GetSystemMetrics(SystemMetric.SM_CXVIRTUALSCREEN),
                                    GetSystemMetrics(SystemMetric.SM_CYVIRTUALSCREEN));
            }

            var size = GetPrimaryMonitorSize();
            return new Rectangle(0, 0, size.Width, size.Height);
        }
        public static void PostDestroyMessage(IntPtr hWnd)
        {
            PostMessage(hWnd, WM_DESTROY, IntPtr.Zero, IntPtr.Zero);
        }
        public static uint SendInput(INPUT pInput, int cbSize)
        {
            return SendInput(1, new[] { pInput }, cbSize);
        }

        #endregion

        #endregion
    }
}