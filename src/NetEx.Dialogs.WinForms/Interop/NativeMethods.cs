using System.Runtime.InteropServices;
using System.Text;

namespace NetEx.Dialogs.WinForms.Interop
{
    internal static class NativeMethods
    {
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CredPackAuthenticationBuffer(int dwFlags, StringBuilder pszUserName, nint pszPassword, nint pPackedCredentials, ref int pcbPackedCredentials);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIParseUserNameW")]
        public static extern int CredUIParseUserName(string pszUserName, StringBuilder pszUser, int ulUserMaxChars, StringBuilder pszDomain, int ulDomainMaxChars);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIPromptForCredentialsW")]
        public static extern int CredUIPromptForCredentials(ref CREDUI_INFO pUiInfo, string pszTargetName, nint reserved, int dwAuthError, StringBuilder pszUserName, int ulUserNameMaxChars, nint pszPassword, int ulPasswordMaxChars, [MarshalAs(UnmanagedType.Bool)] ref bool pfSave, int dwFlags);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "CredUIPromptForWindowsCredentialsW")]
        public static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO pUiInfo, int dwAuthError, ref uint pulAuthPackage, nint pvInAuthBuffer, int ulInAuthBufferSize, out nint ppvOutAuthBuffer, out int pulOutAuthBufferSize, ref bool pfSave, CREDUIWIN dwFlags);
        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, nint pAuthBuffer, int cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainName, nint pszPassword, ref int pcchMaxPassword);
        
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DeleteObject(nint hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(nint hModule);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern nint LoadLibraryEx(string lpFileName, nint hFile, uint dwFlags);
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(nint hWnd);
    }
}