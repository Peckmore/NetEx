using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms.Internal
{
    /// <summary>
    /// Flags that control the operation of a credential dialog box called using CredUIPromptForCredentials.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-gb/windows/desktop/api/wincred/nf-wincred-creduipromptforcredentialsw"/>
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum CREDUI_FLAGS
    {
        /// <summary>
        /// Specifies that a user interface will be shown even if the credentials can be returned from an existing credential in credential manager. This flag is permitted only if CREDUI_FLAGS_GENERIC_CREDENTIALS is also specified. 
        /// </summary>
        CREDUI_FLAGS_ALWAYS_SHOW_UI = 0x00080,
        /// <summary>
        /// Populate the combo box with the prompt for a user name.
        /// </summary>
        CREDUI_FLAGS_COMPLETE_USERNAME = 0x00800,
        /// <summary>
        /// Do not store credentials or display check boxes. You can pass CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX with this flag to display the Save check box only, and the result is returned in the pfSave output parameter. 
        /// </summary>
        CREDUI_FLAGS_DO_NOT_PERSIST = 0x00002,
        /// <summary>
        /// Populate the combo box with user name/password only. Do not display certificates or smart cards in the combo box.
        /// </summary>
        CREDUI_FLAGS_EXCLUDE_CERTIFICATES = 0x00008,
        /// <summary>
        /// Specifies that the caller will call CredUIConfirmCredentials after checking to determine whether the returned credentials are actually valid. This mechanism ensures that credentials that are not valid are not saved to the credential manager. Specify this flag in all cases unless CREDUI_FLAGS_DO_NOT_PERSIST is specified.
        /// </summary>
        CREDUI_FLAGS_EXPECT_CONFIRMATION = 0x20000,
        /// <summary>
        /// Consider the credentials entered by the user to be generic credentials.
        /// </summary>
        CREDUI_FLAGS_GENERIC_CREDENTIALS = 0x40000,
        /// <summary>
        /// Notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip. 
        /// </summary>
        CREDUI_FLAGS_INCORRECT_PASSWORD = 0x00001,
        /// <summary>
        /// Do not allow the user to change the supplied user name.
        /// </summary>
        CREDUI_FLAGS_KEEP_USERNAME = 0x100000,
        /// <summary>
        /// Populate the combo box with the password only. Do not allow a user name to be entered.
        /// </summary>
        CREDUI_FLAGS_PASSWORD_ONLY_OK = 0x00200,
        /// <summary>
        /// Do not show the Save check box, but the credential is saved as though the box were shown and selected.
        /// </summary>
        CREDUI_FLAGS_PERSIST = 0x01000,
        /// <summary>
        /// Populate the combo box with local administrators only. Windows XP Home Edition: This flag will filter out the well-known Administrator account.
        /// </summary>
        CREDUI_FLAGS_REQUEST_ADMINISTRATOR = 0x00004,
        /// <summary>
        /// Populate the combo box with certificates and smart cards only. Do not allow a user name to be entered.
        /// </summary>
        CREDUI_FLAGS_REQUIRE_CERTIFICATE = 0x00010,
        /// <summary>
        /// Populate the combo box with certificates or smart cards only. Do not allow a user name to be entered.
        /// </summary>
        CREDUI_FLAGS_REQUIRE_SMARTCARD = 0x00100,
        /// <summary>
        /// This flag is meaningful only in locating a matching credential to prefill the dialog box, should authentication fail. When this flag is specified, wildcard credentials will not be matched. It has no effect when writing a credential. CredUI does not create credentials that contain wildcard characters.Any found were either created explicitly by the user or created programmatically, as happens when a RAS connection is made.
        /// </summary>
        CREDUI_FLAGS_SERVER_CREDENTIAL = 0x04000,
        /// <summary>
        /// If the check box is selected, show the Save check box and return TRUE in the pfSave output parameter, otherwise, return FALSE.Check box uses the value in pfSave by default. 
        /// </summary>
        CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX = 0x00040,
        /// <summary>
        /// The credential is a "runas" credential. The TargetName parameter specifies the name of the command or program being run. It is used for prompting purposes only.
        /// </summary>
        CREDUI_FLAGS_USERNAME_TARGET_CREDENTIALS = 0x80000,
        /// <summary>
        /// Check that the user name is valid.
        /// </summary>
        CREDUI_FLAGS_VALIDATE_USERNAME = 0x00400,
    }
}