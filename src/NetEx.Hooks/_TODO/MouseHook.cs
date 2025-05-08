using NetEx.Hooks.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace NetEx.Hooks
{
    public static class MouseHook
    {

        #region Variables

        #region Private Static

        private static IntPtr _clickedControl;
        private static int _clicks;
        private static MouseButtons _depressedButtons;
        private static Rectangle _doubleClickArea;
        private static SafeWindowsHookHandle? _hookProcedureHandle;
        private static LowLevelMouseProc _hookProcedure;
        private static MouseButtons _lastButtonDown = MouseButtons.None;
        private static uint _lastButtonDownTime;
        private static IntPtr _lastClickedControl;

        #endregion

        #endregion

        #region Events

        public static event MouseEventHandler? MouseDown;
        public static event MouseEventHandler? MouseMove;
        public static event MouseEventHandler? MouseUp;
        public static event MouseEventHandler? MouseWheel;
        public static event MouseEventHandler? MouseClick;
        public static event MouseEventHandler? MouseDoubleClick;

        #endregion

        #region Properties

        #region Public

        public static bool Active
        {
            get
            {
                if (_hookProcedureHandle == null)
                {
                    return false;
                }

                return !_hookProcedureHandle.IsInvalid;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private

        private static IntPtr HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= NativeMethods.HC_ACTION)
            {
                var mouseHookStruct = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));
                _clickedControl = NativeMethods.WindowFromPoint(mouseHookStruct.pt);

                switch (wParam)
                {
                    case NativeMethods.WM_MOUSEMOVE:
                        MouseMove?.Invoke(new MouseEventArgs(_depressedButtons, 0, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, 0));
                        break;
                    case NativeMethods.WM_LBUTTONDOWN:
                        InternalMouseDown(mouseHookStruct, MouseButtons.Left);
                        break;
                    case NativeMethods.WM_RBUTTONDOWN:
                        InternalMouseDown(mouseHookStruct, MouseButtons.Right);
                        break;
                    case NativeMethods.WM_MBUTTONDOWN:
                        InternalMouseDown(mouseHookStruct, MouseButtons.Middle);
                        break;
                    case NativeMethods.WM_XBUTTONDOWN:
                    case NativeMethods.WM_NCXBUTTONDOWN:
                        var tempDownButton = (mouseHookStruct.mouseData >> 16) switch
                        {
                            1 => MouseButtons.XButton1,
                            2 => MouseButtons.XButton2,
                            _ => MouseButtons.None
                        };
                        InternalMouseDown(mouseHookStruct, tempDownButton);
                        break;
                    case NativeMethods.WM_LBUTTONUP:
                        InternalMouseClick(mouseHookStruct, MouseButtons.Left);
                        InternalMouseUp(mouseHookStruct, MouseButtons.Left);
                        break;
                    case NativeMethods.WM_RBUTTONUP:
                        InternalMouseClick(mouseHookStruct, MouseButtons.Right);
                        InternalMouseUp(mouseHookStruct, MouseButtons.Right);
                        break;
                    case NativeMethods.WM_MBUTTONUP:
                        InternalMouseClick(mouseHookStruct, MouseButtons.Middle);
                        InternalMouseUp(mouseHookStruct, MouseButtons.Middle);
                        break;
                    case NativeMethods.WM_XBUTTONUP:
                    case NativeMethods.WM_NCXBUTTONUP:
                        var tempUpButton = (mouseHookStruct.mouseData >> 16) switch
                        {
                            1 => MouseButtons.XButton1,
                            2 => MouseButtons.XButton2,
                            _ => MouseButtons.None
                        };
                        InternalMouseClick(mouseHookStruct, tempUpButton);
                        InternalMouseUp(mouseHookStruct, tempUpButton);
                        break;
                    case NativeMethods.WM_MOUSEWHEEL:
                    case NativeMethods.WM_MOUSEHWHEEL:
                        var delta = (short)(mouseHookStruct.mouseData >> 16);
                        MouseWheel?.Invoke(new MouseEventArgs(MouseButtons.None, 0, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, delta));
                        break;
                }
            }
            return NativeMethods.CallNextHookEx(_hookProcedureHandle, nCode, (IntPtr)wParam, lParam);
        }
        private static void InternalMouseClick(MOUSEINPUT mouseInfo, MouseButtons button)
        {
            if (button == _lastButtonDown && _clickedControl == _lastClickedControl)
            {
                if (_clicks == 1)
                {
                    MouseClick?.Invoke(new MouseEventArgs(button, 1, mouseInfo.pt.X, mouseInfo.pt.Y, 0));
                }
                else if (_clicks == 2)
                {
                    MouseDoubleClick?.Invoke(new MouseEventArgs(button, 2, mouseInfo.pt.X, mouseInfo.pt.Y, 0));
                    _clicks = 0;
                }
            }
        }
        private static void InternalMouseDown(MOUSEINPUT mouseInfo, MouseButtons button)
        {
            if (_clicks == 1
                && button == _lastButtonDown
                && _clickedControl == _lastClickedControl
                && mouseInfo.time - _lastButtonDownTime <= NativeMethods.GetDoubleClickTime()
                && _doubleClickArea.Contains(mouseInfo.pt.X, mouseInfo.pt.Y))
            {
                _clicks = 2;
            }
            else
            {
                _clicks = 1;
            }

            var e = new MouseEventArgs(button, _clicks, mouseInfo.pt.X, mouseInfo.pt.Y, 0);

            _depressedButtons |= button;
            _lastButtonDown = button;
            _lastButtonDownTime = mouseInfo.time;
            _lastClickedControl = _clickedControl;
            _doubleClickArea.Location = new Point(mouseInfo.pt.X - _doubleClickArea.Width / 2, mouseInfo.pt.Y - _doubleClickArea.Height / 2);

            MouseDown?.Invoke(e);
        }
        private static void InternalMouseUp(MOUSEINPUT mouseInfo, MouseButtons button)
        {
            _depressedButtons &= ~button;
            _lastButtonDown = button;
            MouseUp?.Invoke(new MouseEventArgs(button, 1, mouseInfo.pt.X, mouseInfo.pt.Y, 0));
        }

        #endregion

        #region Public

        public static void Install()
        {
            if (_hookProcedureHandle == null)
            {
                using (var currentProcess = Process.GetCurrentProcess())
                {
                    using (var currentModule = currentProcess.MainModule)
                    {
                        _hookProcedure = HookCallback;
                        _hookProcedureHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, _hookProcedure, NativeMethods.GetModuleHandle(currentModule.ModuleName), 0);
                        if (_hookProcedureHandle.IsInvalid)
                        {
                            Uninstall();
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }

                        _doubleClickArea = new Rectangle(0, 0, 16, 16); //System.Windows.Forms.SystemInformation.DoubleClickSize.Width, System.Windows.Forms.SystemInformation.DoubleClickSize.Height);
                        Debug.WriteLine("Mouse hooked");
                    }
                }
            }
        }
        public static void Uninstall()
        {
            if (_hookProcedureHandle != null)
            {
                try
                {
                    if (!_hookProcedureHandle.IsInvalid)
                    {
                        if (NativeMethods.UnhookWindowsHookEx(_hookProcedureHandle.DangerousGetHandle()))
                        {
                            _hookProcedureHandle.SetHandleAsInvalid();
                            Debug.WriteLine("Mouse unhooked");
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                finally
                {
                    _hookProcedureHandle.Dispose();
                    _hookProcedureHandle = null;
                }
            }
            _hookProcedure = null;
        }

        #endregion

        #endregion
    }
}