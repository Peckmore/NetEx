using Microsoft.Win32.SafeHandles;
using NetEx.Hooks.Interop;

namespace NetEx.Hooks
{
    /// <summary>
    /// A safe handle implementation for Windows hooks.
    /// </summary>
    internal class SafeWindowsHookHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Construction

        private SafeWindowsHookHandle()
            : base(true)
        { }

        #endregion

        #region Properties

        protected override bool ReleaseHandle()
        {
            // If the handle is valid, attempt to unhook and return whether the unhook was successful.
            return IsInvalid || NativeMethods.UnhookWindowsHookEx(handle);
        }

        #endregion
    }
}