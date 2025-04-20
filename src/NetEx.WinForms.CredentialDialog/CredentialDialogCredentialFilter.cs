namespace System.Windows.Forms
{
    /// <summary>
    /// Specifies the credentials to display in a <see cref="CredentialDialog"/>.
    /// </summary>
    public enum CredentialDialogCredentialFilter
    {
        /// <summary>
        /// Populate the <see cref="CredentialDialog"/> with all available credentials.
        /// </summary>
        AllCredentials = 0,
        /// <summary>
        /// Populate the <see cref="CredentialDialog"/> with local administrators only.
        /// </summary>
        /// <remarks>On Windows XP Home Edition this flag will filter out the well-known Administrator account. On Windows Vista and later this value is intended for User Account Control (UAC) purposes only. We recommend that external callers not set this flag.</remarks>
        AdministratorsOnly = 1,
        /// <summary>
        /// Populate the <see cref="CredentialDialog"/> with usernames only. This option will not display certificates or smart cards in the <see cref="CredentialDialog"/>.
        /// </summary>
        /// <remarks>This option is only applicable on Windows XP and Windows Server 2003, or on later version of Windows when using the <see cref="CredentialDialog"/> with <see cref="CredentialDialog.AutoUpgradeEnabled"/> set to false.</remarks>
        ExcludeCertificates = 2,
        /// <summary>
        /// Populate the <see cref="CredentialDialog"/> with certificates and smart cards only. This option will not allow a user name to be entered.
        /// </summary>
        /// <remarks>This option is only applicable on Windows XP and Windows Server 2003, or on later version of Windows when using the <see cref="CredentialDialog"/> with <see cref="CredentialDialog.AutoUpgradeEnabled"/> set to false.</remarks>
        RequireCertificates = 3
    }
}