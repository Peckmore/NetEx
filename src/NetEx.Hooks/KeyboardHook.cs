using NetEx.Hooks.Interop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides a mechanism for hooking all keyboard events within the operating system.
    /// </summary>
    public static class KeyboardHook
    {
        #region Fields

        private static SafeWindowsHookHandle? _hookProcedureHandle;
        private static Keys _modifiers = 0;
        private static LowLevelKeyboardProc? _hookProcedure;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise keyboard performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event KeyEventHandler? KeyDown;
        /// <summary>
        /// Occurs a key is released.
        /// </summary>
        /// <remarks>Event handlers should aim to be as fast as possible, otherwise mouse performance may be impacted, or the hook removed by the operating system. See <see cref="Install"/> for more information.</remarks>
        public static event KeyEventHandler? KeyUp;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether <see cref="KeyboardHook"/> has been installed and is capturing keyboard events.
        /// </summary>
        /// <returns><see langword="true"/> if the hook is installed and valid; otherwise <see langword="false"/>.</returns>
        public static bool IsInstalled => _hookProcedureHandle is { IsInvalid: false };

        #endregion

        #region Methods

        #region Private Static

        private static IntPtr HookCallback(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= NativeMethods.HC_ACTION)
            {
                var keyboardHookStruct = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));
                var keyData = (Keys)keyboardHookStruct.wVk;
                Keys modifierKey = keyData switch
                {
                    Keys.LMenu => Keys.Alt,
                    Keys.RMenu => Keys.Alt,
                    Keys.LControlKey => Keys.Control,
                    Keys.RControlKey => Keys.Control,
                    Keys.LShiftKey => Keys.Shift,
                    Keys.RShiftKey => Keys.Shift,
                    _ => 0
                };

                KeyEventArgs? eventArgs = null;
                switch (wParam)
                {
                    case NativeMethods.WM_KEYDOWN:
                    case NativeMethods.WM_SYSKEYDOWN:
                        _modifiers |= modifierKey;
                        eventArgs = new KeyEventArgs(keyData | _modifiers);
                        KeyDown?.Invoke(eventArgs);
                        break;

                    case NativeMethods.WM_KEYUP:
                    case NativeMethods.WM_SYSKEYUP:
                        _modifiers &= ~modifierKey;
                        eventArgs = new KeyEventArgs(keyData | _modifiers);
                        KeyUp?.Invoke(eventArgs);
                        break;
                }

                if (eventArgs is { SuppressKeyPress: true })
                {
                    return (IntPtr)1;
                }
            }

            return NativeMethods.CallNextHookEx(_hookProcedureHandle!, nCode, (IntPtr)wParam, lParam);
        }

        #endregion

        #region Public Static

        /// <summary>
        /// Installs the keyboard hook, capturing all global keyboard events.
        /// </summary>
        /// <remarks>
        /// <para>This hook is called in the context of the thread that installed it. The call is made by sending a message to the thread that installed the hook. Therefore, the thread that installed the hook must have a message loop.</para>
        /// <para>The hook procedure should process a message in less time than the data entry specified in the <c>LowLevelHooksTimeout</c> value in the following registry key:</para>
        /// <code>HKEY_CURRENT_USER\Control Panel\Desktop</code>
        /// <para>The value is in milliseconds. If the hook procedure times out, the system passes the message to the next hook. However, on <b>Windows 7</b> and later, the hook is silently removed without being called. There is no way for the application to know whether the hook is removed.</para>
        /// <para><b>Windows 10 version 1709 and later</b> The maximum timeout value the system allows is 1000 milliseconds (1 second). The system will default to using a 1000 millisecond timeout if the <c>LowLevelHooksTimeout</c> value is set to a value larger than 1000.</para>
        /// </remarks>
        /// <exception cref="Win32Exception">The hook could not be installed.</exception>
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
                            // Install our keyboard hook.
                            _hookProcedure = HookCallback;
                            _hookProcedureHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL,
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
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Attempts to install the keyboard hook.
        /// </summary>
        /// <returns><see langword="true"/> if the hook was successfully installed; otherwise <see langword="false"/>.</returns>
        public static bool TryInstall()
        {
            try
            {
                Install();
                return true;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return false;
            }
        }
        /// <summary>
        /// Attempts to uninstall the keyboard hook.
        /// </summary>
        /// <returns><see langword="true"/> if the hook was successfully installed; otherwise <see langword="false"/>.</returns>
        public static bool TryUninstall()
        {
            try
            {
                Uninstall();
                return true;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return false;
            }
        }
        /// <summary>
        /// Uninstalls the keyboard hook, and stops further keyboard events from being captured.
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