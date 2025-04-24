using NetEx.Windows.Forms.Internal;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace NetEx.Windows.Forms
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal static class NativeMethods
    {
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CredPackAuthenticationBuffer(int dwFlags, StringBuilder pszUserName, IntPtr pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIParseUserNameW")]
        public static extern int CredUIParseUserName(string pszUserName, StringBuilder pszUser, int ulUserMaxChars, StringBuilder pszDomain, int ulDomainMaxChars);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIPromptForCredentialsW")]
        public static extern int CredUIPromptForCredentials(ref CREDUI_INFO pUiInfo, string pszTargetName, IntPtr reserved, int dwAuthError, StringBuilder pszUserName, int ulUserNameMaxChars, IntPtr pszPassword, int ulPasswordMaxChars, [MarshalAs(UnmanagedType.Bool)] ref bool pfSave, int dwFlags);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIPromptForWindowsCredentialsW")]
        public static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO pUiInfo, int dwAuthError, ref uint pulAuthPackage, IntPtr pvInAuthBuffer, int ulInAuthBufferSize, out IntPtr ppvOutAuthBuffer, out int pulOutAuthBufferSize, ref bool pfSave, CREDUIWIN dwFlags);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, int cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainName, IntPtr pszPassword, ref int pcchMaxPassword);
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}