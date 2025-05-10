using NetEx.Hooks.Interop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides a mechanism for hooking all mouse events within the operating system.
    /// </summary>
    public static class MouseHook
    {
        #region Variables

        #region Private Static

        private static int _clicks;
        private static MouseButtons _depressedButtons;
        private static Rectangle _doubleClickArea;
        private static SafeWindowsHookHandle? _hookProcedureHandle;
        private static LowLevelMouseProc? _hookProcedure;
        private static MouseButtons _lastButtonDown = MouseButtons.None;
        private static uint _lastButtonDownTime;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a mouse button is clicked.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseClick;
        /// <summary>
        /// Occurs when a mouse button is double clicked.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseDoubleClick;
        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseDown;
        /// <summary>
        /// Occurs when the mouse pointer is moved.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseMove;
        /// <summary>
        /// Occurs a mouse button is released.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseUp;
        /// <summary>
        /// Occurs when the mouse wheel moves.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event MouseEventHandler? MouseWheel;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether <seealso cref="MouseHook"/> has been installed and is capturing mouse events.
        /// </summary>
        /// <returns><see langword="true"/> if the hook is installed and valid; otherwise <see langword="false"/>.</returns>
        public static bool IsInstalled => _hookProcedureHandle is { IsInvalid: false };

        #endregion

        #region Methods

        #region Private

        private static IntPtr HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= NativeMethods.HC_ACTION)
            {
                // We have a mouse event, so we can begin to parse it.
                var mouseHookStruct = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT));

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
            return NativeMethods.CallNextHookEx(_hookProcedureHandle!, nCode, (IntPtr)wParam, lParam);
        }
        private static void InternalMouseClick(MOUSEINPUT mouseInfo, MouseButtons button)
        {
            if (button == _lastButtonDown)
            {
                if (_clicks == 1)
                {
                    MouseClick?.Invoke(new MouseEventArgs(button, 1, mouseInfo.pt));
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

        /// <summary>
        /// Installs the mouse hook, capturing all global mouse events.
        /// </summary>
        /// <remarks>
        /// <para>This hook is called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.</para>
        /// <para>The hook procedure should process a message in less time than the data entry specified in the <c>LowLevelHooksTimeout</c> value in the following registry key:</para>
        /// <code>HKEY_CURRENT_USER\Control Panel\Desktop</code>
        /// <para>The value is in milliseconds.If the hook procedure times out, the system passes the message to the next hook. However, on Windows 7 and later, the hook is silently removed without being called. There is no way for the application to know whether the hook is removed.</para>
        /// <para><b>Windows 10 version 1709 and later</b> The maximum timeout value the system allows is 1000 milliseconds(1 second). The system will default to using a 1000 millisecond timeout if the <c>LowLevelHooksTimeout</c> value is set to a value larger than 1000.</para>
        /// </remarks>
        /// <exception cref="Win32Exception">The hook could not be installed.</exception>
        /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc"/>
        public static void Install()
        {
            // Check we haven't already installed the hook.
            if (_hookProcedureHandle == null)
            {
                // Get the current process.
                using (var currentProcess = Process.GetCurrentProcess())
                {
                    // Check that we can get a module name (which confirms that process, module, and name are not null).
                    if (currentProcess.MainModule?.ModuleName != null)
                    {
                        // Get the current module.
                        using (var currentModule = currentProcess.MainModule)
                        {
                            // Install our mouse hook.
                            _hookProcedure = HookCallback;
                            _hookProcedureHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL,
                                                                                  _hookProcedure,
                                                                                  NativeMethods.GetModuleHandle(currentModule.ModuleName),
                                                                                  0);

                            // Check whether the hook was successfully installed.
                            if (_hookProcedureHandle.IsInvalid)
                            {
                                // The hook failed to install, so we'll try to cleanup.

                                // First we grab the error code from attempting to install - we need to do this as uninstall could clear the
                                // value if it also fails.
                                var errorCode = Marshal.GetLastWin32Error();

                                // Now we call Uninstall() to try and cleanup.
                                Uninstall();

                                // Finally, we throw an exception using the error code to indicate that the hook failed to install.
                                throw new Win32Exception(errorCode);
                            }

                            // The hook installed, so we'll grab the allowed "double-click area" from the operating system so we can use it to
                            // check for double-click events later on.
                            _doubleClickArea = NativeMethods.GetDoubleClickArea();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Uninstalls the mouse hook, and stops further mouse events from being captured.
        /// </summary>
        /// <exception cref="Win32Exception">The hook could not be uninstalled.</exception>
        public static void Uninstall()
        {
            // Check whether the hook has been installed before we attempt to uninstall it.
            if (_hookProcedureHandle != null)
            {
                try
                {
                    // If the hook is valid, we need to unhook it. If it isn't, then it was never installed so we don't need to worry about
                    // unhooking it.
                    if (!_hookProcedureHandle.IsInvalid)
                    {
                        // We need to get the handle in order to unhook, so call `DangerousGetHandle` and unhook.
                        if (NativeMethods.UnhookWindowsHookEx(_hookProcedureHandle.DangerousGetHandle()))
                        {
                            // We successfully unhooked, so we'll now mark this handle as invalid as it is no longer in use.
                            _hookProcedureHandle.SetHandleAsInvalid();
                        }
                        else
                        {
                            // The hook failed to uninstall, so we throw an exception using the error code.
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                finally
                {
                    // Dispose of/release our safe handle, and clear our variables.
                    _hookProcedureHandle.Dispose();
                    _hookProcedureHandle = null;
                    _hookProcedure = null;
                }
            }
        }

        #endregion

        #endregion
    }
}