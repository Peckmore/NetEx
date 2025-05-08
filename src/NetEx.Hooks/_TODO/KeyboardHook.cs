using NetEx.Hooks.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NetEx.Hooks
{
    public static class KeyboardHook
    {
        #region Fields

        private static SafeWindowsHookHandle? _hookProcedureHandle;
        private static Keys _modifiers = 0;
        private static LowLevelKeyboardProc? _hookProcedure;

        #endregion

        #region Events

        public static event KeyEventHandler? KeyDown;
        public static event KeyEventHandler? KeyUp;

        #endregion

        #region Properties

        public static bool Active => _hookProcedureHandle?.IsInvalid == false;

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

            return NativeMethods.CallNextHookEx(_hookProcedureHandle, nCode, (IntPtr)wParam, lParam);
        }

        #endregion

        #region Public Static

        public static void Install()
        {
            if (_hookProcedureHandle == null)
            {
                using (var currentProcess = Process.GetCurrentProcess())
                {
                    using (var currentModule = currentProcess.MainModule)
                    {
                        _hookProcedure = HookCallback;
                        _hookProcedureHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL,
                                                                              _hookProcedure,
                                                                              NativeMethods.GetModuleHandle(currentModule.ModuleName),
                                                                              0);
                        if (_hookProcedureHandle.IsInvalid)
                        {
                            Uninstall();
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                        Debug.WriteLine("Keyboard hooked");
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
                            Debug.WriteLine("Keyboard unhooked");
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
        }

        #endregion

        #endregion
    }
}