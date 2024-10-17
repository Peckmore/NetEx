using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NetEx.Windows.Forms
{
    internal static class NativeMethods
    {
        /* CA5122 has been suppressed because the warning is bogus in this scenario. It appears
         * that when assemblies target < .Net 4.0 this warning is generated erroneously. As this
         * library targets .Net 2.0 this warning can be safely ignored.
         * 
         * https://stackoverflow.com/a/13517193/2678851
         */

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [SuppressMessage("Microsoft.Security", "CA5122:PInvokesShouldNotBeSafeCriticalFxCopRule")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}