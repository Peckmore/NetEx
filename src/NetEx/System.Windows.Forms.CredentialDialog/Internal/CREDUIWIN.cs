using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms.Internal
{
    /// <summary>
    /// Flags that control the operation of a credential dialog box called using CredUIPromptForWindowsCredentials.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-gb/windows/desktop/api/wincred/nf-wincred-creduipromptforwindowscredentialsw"/>
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum CREDUIWIN
    {
        /// <summary>
        /// The caller is requesting that the credential provider return the user name and password in plain text.
        /// <para>This value cannot be combined with SECURE_PROMPT.</para>
        /// </summary>
        CREDUIWIN_GENERIC = 0x1,
        /// <summary>
        /// The Save check box is displayed in the dialog box. 
        /// </summary>
        CREDUIWIN_CHECKBOX = 0x2,
        /// <summary>
        /// Only credential providers that support the authentication package specified by the pulAuthPackage parameter should be enumerated. 
        /// <para>This value cannot be combined with CREDUIWIN_IN_CRED_ONLY.</para>
        /// </summary>
        CREDUIWIN_AUTHPACKAGE_ONLY = 0x10,
        /// <summary>
        /// Only the credentials specified by the pvInAuthBuffer parameter for the authentication package specified by the pulAuthPackage parameter should be enumerated. 
        /// <para>If this flag is set, and the pvInAuthBuffer parameter is NULL, the function fails.</para>
        /// <para>This value cannot be combined with CREDUIWIN_AUTHPACKAGE_ONLY.</para>
        /// </summary>
        CREDUIWIN_IN_CRED_ONLY = 0x20,
        /// <summary>
        /// Credential providers should enumerate only administrators. This value is intended for User Account Control (UAC) purposes only. We recommend that external callers not set this flag. 
        /// </summary>
        CREDUIWIN_ENUMERATE_ADMINS = 0x100,
        /// <summary>
        /// Only the incoming credentials for the authentication package specified by the pulAuthPackage parameter should be enumerated. 
        /// </summary>
        CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
        /// <summary>
        /// The credential dialog box should be displayed on the secure desktop. This value cannot be combined with CREDUIWIN_GENERIC. 
        /// <para>Windows Vista:  This value is supported beginning with Windows Vista with SP1.</para>
        /// </summary>
        CREDUIWIN_SECURE_PROMPT = 0x1000,
        /// <summary>
        /// The credential dialog box is invoked by the SspiPromptForCredentials function, and the client is prompted before a prior handshake. If SSPIPFC_NO_CHECKBOX is passed in the pvInAuthBuffer parameter, then the credential provider should not display the check box. 
        /// <para>Windows Vista:  This value is supported beginning with Windows Vista with SP1.</para>
        /// </summary>
        CREDUIWIN_PREPROMPTING = 0x2000,
        /// <summary>
        /// The credential provider should align the credential BLOB pointed to by the ppvOutAuthBuffer parameter to a 32-bit boundary, even if the provider is running on a 64-bit system. 
        /// </summary>
        CREDUIWIN_PACK_32_WOW = 0x10000000
    }
}