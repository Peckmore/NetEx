using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
    /// <summary>
    /// The CREDUI_INFO structure is used to pass information to the CredUIPromptForCredentials function that creates a dialog box used to obtain credentials information.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-gb/windows/desktop/api/wincred/ns-wincred-credui_infow"/>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct CREDUI_INFO
    {
        /// <summary>
        /// Set to the size of the CREDUI_INFO structure.
        /// </summary>
        public int cbSize;
        /// <summary>
        /// Specifies the handle to the parent window of the dialog box. The dialog box is modal with respect to the parent window. If this member is NULL, the desktop is the parent window of the dialog box.
        /// </summary>
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public IntPtr hwndParent;
        /// <summary>
        /// Pointer to a string containing a brief message to display in the dialog box. The length of this string should not exceed CREDUI_MAX_MESSAGE_LENGTH.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszMessageText;
        /// <summary>
        /// Pointer to a string containing the title for the dialog box. The length of this string should not exceed CREDUI_MAX_CAPTION_LENGTH.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszCaptionText;
        /// <summary>
        /// Bitmap to display in the dialog box. If this member is NULL, a default bitmap is used. The bitmap size is limited to 320x60 pixels.
        /// </summary>
        public IntPtr hbmBanner;
    }
}