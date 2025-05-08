using Microsoft.Win32.SafeHandles;

namespace NetEx.Hooks
{
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
            return IsInvalid || NativeMethods.UnhookWindowsHookEx(handle);
        }

        #endregion
    }
}