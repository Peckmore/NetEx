using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
    /// <summary>Prompts the user for credentials. This class cannot be inherited.</summary>
    [DefaultEvent("HelpRequest")]
    [DefaultProperty("Username")]
    [Description("Displays a dialog box that prompts the user to enter credentials.")]
    [ToolboxBitmap(typeof(CredentialDialog), "CredentialDialogToolboxBitmap.png")]
    [ToolboxItem(true)]
    public sealed class CredentialDialog : CommonDialog
    {
        #region Constants

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_BANNER_HEIGHT = 60;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_BANNER_WIDTH = 320;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_MAX_CAPTION_LENGTH = 128;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_MAX_DOMAIN_TARGET_LENGTH = 337;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_MAX_MESSAGE_LENGTH = 32767;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_MAX_PASSWORD_LENGTH = 256;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int CREDUI_MAX_USERNAME_LENGTH = 513;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int ERROR_CANCELLED = 1223;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public const int ERROR_SUCCESS = 0;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes an instance of the <see cref="CredentialDialog" /> class.
        /// </summary>
        public CredentialDialog() => Reset();

        #endregion


        #region Methods

        #region Private

        private bool UseVistaDialogInternal
        {
            get
            {
                // Check that we are running on Windows NT
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    return false;

                // Check that we are running on Windows Vista or later
                if (Environment.OSVersion.Version < new Version(6, 0)) // Vista identifies as Windows version 6.0
                    return false;


                // Assert EnvironmentPermission in order to determine the system BootMode
                new EnvironmentPermission(PermissionState.Unrestricted).Assert();

                try
                {
                    // If we are running on Vista or later, check that we are running in normal boot mode,
                    // and not any other mode (e.g., Safe Mode)
                    return SystemInformation.BootMode == BootMode.Normal;
                }
                finally
                {
                    // Revert our previously asserted access permission
                    CodeAccessPermission.RevertAssert();
                }
            }
        }

        #endregion

        #region Protected

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Banner != null)
                {
                    _banner.Dispose();
                    _banner = null;
                }

                if (_password != null)
                {
                    _password.Dispose();
                    _password = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion 

        #region Public

        /// <summary>
        /// Resets all properties to their default values.
        /// </summary>
        [SuppressMessage("ReSharper", "InheritdocConsiderUsage")]
        public override void Reset()
        {
            // All configurable properties have a DefaultValueAttribute associated
            // with them. So for easy consistency we just grab each property, grab
            // the default value, then assign that value to the property.
            foreach (var property in GetType().GetProperties())
            {
                var dvList = property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (dvList.Length > 0 && dvList[0] is DefaultValueAttribute attribute)
                    property.SetValue(this, attribute.Value, null);
            }
        }

        #endregion

        #endregion



















        #region Fields

        private Image _banner;
        private string _domain;
        private StringBuilder _internalDomain = new StringBuilder(CREDUI_MAX_DOMAIN_TARGET_LENGTH);
        private StringBuilder _internalUsername = new StringBuilder(CREDUI_MAX_USERNAME_LENGTH);
        private string _message;
        private SecureString _password = new SecureString();
        private bool _saveChecked;
        private string _title;

        #endregion

        #region Properties



        #region Public

        /// <summary>Gets or sets a value indicating whether this <see cref="CredentialDialog"/> instance should automatically upgrade appearance and behavior when running on Windows Vista.</summary>
        /// <value>true if this <see cref="CredentialDialog"/> instance should automatically upgrade appearance and behavior when running on Windows Vista; otherwise, false. The default is true.</value>
        /// <remarks>
        /// If this property is false, the <see cref="CredentialDialog"/> class will have a Windows XP-style appearance and behavior on Windows Vista.
        /// <para>On Windows XP, this property does not have any effect.</para>
        /// </remarks>
        [DefaultValue(true)]
        public bool AutoUpgradeEnabled { get; set; }

        /// <summary>Gets or sets the banner image displayed in the credential dialog box.</summary>
        /// <remarks>
        /// If this member is NULL, a default bitmap is used. The bitmap size is limited to 320x60 pixels.
        /// <para>This property is only applicable on Windows XP and Windows Server 2003, or on later version of Windows when using the <see cref="CredentialDialog"/> with <see cref="AutoUpgradeEnabled"/> set to false.</para>
        /// </remarks>
        [Category("Appearance"), Description("The banner image displayed in the credential dialog box.")]
        [DefaultValue(null)]
        public Image Banner
        {
            get => _banner;
            set
            {
                if (value != null)
                {
                    if (value.Width != CREDUI_BANNER_WIDTH)
                        throw new ArgumentException($"The banner image width must be {CREDUI_BANNER_WIDTH} pixels.", nameof(value));

                    if (value.Height != CREDUI_BANNER_HEIGHT)
                        throw new ArgumentException($"The banner image height must be {CREDUI_BANNER_HEIGHT} pixels.", nameof(value));
                }

                _banner = value;
            }
        }
        /// <summary>Gets or sets a value indicating the types of credentials that will be shwon in the credential dialog box.</summary>
        [Category("Behaviour"), Description("Indicates the types of credentials that will be shwon in the credential dialog box.")]
        [DefaultValue(CredentialDialogCredentialFilter.AllCredentials)]
        public CredentialDialogCredentialFilter CredentialFilter { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the credential dialog box should prevent the user from changing the supplied user name.
        /// </summary>
        [Category("Behaviour"), Description("Indicates whether the credential dialog box should prevent the user from changing the supplied user name.")]
        [DefaultValue(false)]
        public bool DisableUsername { get; set; }
        /// <summary>Gets or sets a string containing the domain entered in the credential dialog box.</summary>
        /// <value>The domain entered in the credential dialog box. The default value is an empty string ("").</value>
        [Category("Data"), Description("The domain entered in the credential dialog box.")]
        [DefaultValue("")]
        public string Domain
        {
            get => _domain;
            set
            {
                if (value != null)
                    if (value.Length > CREDUI_MAX_DOMAIN_TARGET_LENGTH)
                        throw new ArgumentException(
                            $"The domain name has a maximum length of {CREDUI_MAX_DOMAIN_TARGET_LENGTH} characters.", nameof(value));

                _domain = value;
            }
        }
        /// <summary>Gets or sets a value indicating why the credential dialog box is needed.</summary>
        /// <remarks>On Windows Vista the corresponding error message is formatted and displayed in the dialog box unless <see cref="AutoUpgradeEnabled"/> is set to false.</remarks>
        [Category("Behaviour"), Description("A value indicating why the credential dialog box is needed.")]
        [DefaultValue(0)]
        public int ErrorCode { get; set; }
        /// <summary>Gets or sets a value indicating whether the credential dialog box should automatically notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip.</summary>
        /// <value>true if the credential dialog box should automatically notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip; otherwise, false. The default is false.</value>
        /// <remarks>This property is only applicable on Windows XP and Windows Server 2003, or on later version of Windows when using the <see cref="CredentialDialog"/> with <see cref="AutoUpgradeEnabled"/> set to false.</remarks>
        [Category("Behaviour"), Description("Indicates whether the credential dialog box should automatically notify the user of insufficient credentials by displaying the \"Logon unsuccessful\" balloon tip.")]
        [DefaultValue(false)]
        public bool IncorrectPasswordPrompt { get; set; }
        /// <summary>Gets or sets the text to display in the credential dialog box.</summary>
        [Category("Appearance"), Description("The text to display in the credential dialog box.")]
        [DefaultValue("")]
        public string Message
        {
            get => _message;
            set
            {
                if (value != null)
                    if (value.Length > CREDUI_MAX_MESSAGE_LENGTH)
                        throw new ArgumentException($"The message has a maximum length of {CREDUI_MAX_MESSAGE_LENGTH} characters.", nameof(value));

                _message = value;
            }
        }
        /// <summary>Gets or sets a <see cref="SecureString"/> containing the password entered in the credential dialog box.</summary>
        [Category("Data"), Description("The password entered in the credential dialog box.")]
        [DefaultValue(null)]
        public SecureString Password
        {
            get => _password;
            set
            {
                if (value != null)
                    if (value.Length > CREDUI_MAX_PASSWORD_LENGTH)
                        throw new ArgumentException($"The password has a maximum length of {CREDUI_MAX_PASSWORD_LENGTH} characters.", nameof(value));

                _password = value;
            }
        }
#if DEBUG
        /// <summary>Gets or sets a string containing the password entered in the credential dialog box.</summary>
        /// <remarks>This method is provided for debugging purposes only and should not be used in a production environment.</remarks>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        [Category("Data"), Description("The password entered in the credential dialog box.")]
        public string PasswordString
        {
            get
            {
                if (_password == null)
                    return string.Empty;

                IntPtr unmanagedPasswordPointer = IntPtr.Zero;
                try
                {
                    unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password);
                    return Marshal.PtrToStringUni(unmanagedPasswordPointer);
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
                }
            }
        }
#endif
        /// <summary>Gets or sets a value indicating whether the save check box is selected.</summary>
        /// <value>true if the save check box is selected; otherwise, false. The default value is false.</value>
        /// <remarks>The <see cref="ShowSave"/> property must be set before in order for the save check box to appear in the dialog box.</remarks>
        [Category("Behaviour"), Description("The state of the save check box in the dialog.")]
        [DefaultValue(false)]
        public bool SaveChecked
        {
            get => _saveChecked;
            set => _saveChecked = value;
        }
        /// <summary>Gets or sets a value indicating whether the dialog box contains a save check box.</summary>
        /// <value>true if the dialog box contains a save check box; otherwise, false. The default value is true.</value>
        [Category("Appearance"), Description("Controls whether to show the save check box in the dialog.")]
        [DefaultValue(true)]
        public bool ShowSave { get; set; }

        /// <summary>Gets or sets the string to display as the caption of the credential dialog box.</summary>
        [Category("Appearance"), Description("The string to display as the caption of the credential dialog box.")]
        [DefaultValue("")]
        public string Title
        {
            get => _title;
            set
            {
                if (value != null)
                    if (value.Length > CREDUI_MAX_CAPTION_LENGTH)
                        throw new ArgumentException($"The caption has a maximum length of {CREDUI_MAX_CAPTION_LENGTH} characters.", nameof(value));

                _title = value;
            }
        }
        /// <summary>Gets or sets a string containing the username entered in the credential dialog box.</summary>
        /// <value>The username entered in the credential dialog box. The default value is an empty string ("").</value>
        [Category("Data"), Description("The username entered in the credential dialog box.")]
        [DefaultValue("")]
        public string Username
        {
            get => _internalUsername.ToString();
            set
            {
                if (value != null)
                    if (value.Length > CREDUI_MAX_USERNAME_LENGTH)
                        throw new ArgumentException($"The user name has a maximum length of {CREDUI_MAX_USERNAME_LENGTH} characters.", nameof(value));

                _internalUsername = new StringBuilder(value) { Capacity = CREDUI_MAX_USERNAME_LENGTH };
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private

        private unsafe void ConvertUnmanagedPasswordToSecureString(IntPtr unmanagedPasswordPointer)
        {
            string managedPassword = Marshal.PtrToStringUni(unmanagedPasswordPointer);

            fixed (char* managedPasswordCharacters = managedPassword)
            {
                _password = new SecureString();
                for (int x = 0; x < managedPassword.Length; x++)
                {
                    _password.AppendChar(managedPasswordCharacters[x]);
                    managedPasswordCharacters[x] = '0';
                }
            }

            _password.MakeReadOnly();
        }
        private void ParseUsername()
        {
            StringBuilder parsedUsername = new StringBuilder(CREDUI_MAX_USERNAME_LENGTH);
            StringBuilder parsedDomain = new StringBuilder(CREDUI_MAX_DOMAIN_TARGET_LENGTH);

            if (NativeMethods.CredUIParseUserName(_internalUsername.ToString(), parsedUsername, CREDUI_MAX_USERNAME_LENGTH, parsedDomain, CREDUI_MAX_DOMAIN_TARGET_LENGTH) == ERROR_SUCCESS)
            {
                _internalUsername = new StringBuilder(parsedUsername.ToString());
                _domain = parsedDomain.ToString();
            }
            else
            {
                _internalUsername = new StringBuilder(_internalUsername.ToString().TrimStart('\\'));
                _domain = string.Empty;
            }

            _internalUsername.Capacity = CREDUI_MAX_USERNAME_LENGTH;
        }
        private int RunDialogOld(CREDUI_INFO credUiInfo)
        {
            CREDUI_FLAGS flags = CREDUI_FLAGS.CREDUI_FLAGS_DO_NOT_PERSIST;

            if (IncorrectPasswordPrompt)
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_INCORRECT_PASSWORD;

            if (ShowSave)
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX;

            if (DisableUsername)
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_KEEP_USERNAME;

            switch (CredentialFilter)
            {
                case CredentialDialogCredentialFilter.ExcludeCertificates:
                    flags |= CREDUI_FLAGS.CREDUI_FLAGS_EXCLUDE_CERTIFICATES;
                    break;
                case CredentialDialogCredentialFilter.AdministratorsOnly:
                    flags |= CREDUI_FLAGS.CREDUI_FLAGS_REQUEST_ADMINISTRATOR;
                    break;
                case CredentialDialogCredentialFilter.RequireCertificates:
                    flags |= CREDUI_FLAGS.CREDUI_FLAGS_REQUIRE_SMARTCARD;
                    break;
            }

            int resultCode;
            IntPtr unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password);

            try
            {
                resultCode = NativeMethods.CredUIPromptForCredentials(ref credUiInfo, _internalDomain.ToString(), IntPtr.Zero, ErrorCode, _internalUsername, CREDUI_MAX_USERNAME_LENGTH, unmanagedPasswordPointer, CREDUI_MAX_PASSWORD_LENGTH, ref _saveChecked, (int)flags);
                ConvertUnmanagedPasswordToSecureString(unmanagedPasswordPointer);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
            }

            if (resultCode == ERROR_SUCCESS)
                ParseUsername();

            return resultCode;
        }
        private int RunDialogVista(CREDUI_INFO credUiInfo)
        {
            CREDUIWIN flags = 0;

            if (ShowSave)
                flags = flags | CREDUIWIN.CREDUIWIN_CHECKBOX;

            switch (CredentialFilter)
            {
                case CredentialDialogCredentialFilter.AdministratorsOnly:
                    flags = flags | CREDUIWIN.CREDUIWIN_ENUMERATE_ADMINS;
                    break;
            }

            int resultCode;
            IntPtr packedInputcredential = IntPtr.Zero;
            IntPtr packedOutputcredential;
            int packedInputcredentialSize = 128;
            int packedOutputcredentialSize;
            uint authenticationPackage = 0; // This functionality is unsupported
            IntPtr unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password);

            try
            {
                // Pack the input credential for use in the dialog box
                if (_internalUsername != null)
                {
                    packedInputcredential = Marshal.AllocCoTaskMem(packedInputcredentialSize);
                    if (!NativeMethods.CredPackAuthenticationBuffer(0, _internalUsername, unmanagedPasswordPointer, packedInputcredential, ref packedInputcredentialSize))
                    {
                        Marshal.FreeCoTaskMem(packedInputcredential);
                        packedInputcredential = Marshal.AllocCoTaskMem(packedInputcredentialSize);
                        if (!NativeMethods.CredPackAuthenticationBuffer(0, _internalUsername, unmanagedPasswordPointer, packedInputcredential, ref packedInputcredentialSize))
                            return Marshal.GetLastWin32Error();
                    }
                }

                // Display the dialog box using the previously packed credentials
                resultCode = NativeMethods.CredUIPromptForWindowsCredentials(ref credUiInfo, ErrorCode, ref authenticationPackage, packedInputcredential, packedInputcredentialSize, out packedOutputcredential, out packedOutputcredentialSize, ref _saveChecked, flags);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
                Marshal.FreeCoTaskMem(packedInputcredential);
                _password.Dispose();
                _password = null;
            }

            try
            {
                // If the dialog box result was OK then unpack the entered credentials
                if (resultCode == ERROR_SUCCESS)
                {
                    _internalDomain = new StringBuilder(CREDUI_MAX_DOMAIN_TARGET_LENGTH);
                    int unpackedUsernameBufferSize = CREDUI_MAX_USERNAME_LENGTH;
                    int unpackedDomainBufferSize = CREDUI_MAX_DOMAIN_TARGET_LENGTH;
                    int unpackedPasswordBufferSize = CREDUI_MAX_PASSWORD_LENGTH;
                    unmanagedPasswordPointer = Marshal.AllocHGlobal(CREDUI_MAX_PASSWORD_LENGTH);

                    try
                    {
                        if (!NativeMethods.CredUnPackAuthenticationBuffer(1, packedOutputcredential, packedOutputcredentialSize, _internalUsername, ref unpackedUsernameBufferSize, _internalDomain, ref unpackedDomainBufferSize, unmanagedPasswordPointer, ref unpackedPasswordBufferSize))
                            return Marshal.GetLastWin32Error();
                        else
                        {
                            ConvertUnmanagedPasswordToSecureString(unmanagedPasswordPointer);
                            ParseUsername();
                        }
                    }
                    finally
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
                    }
                }
                return resultCode;
            }
            finally
            {
                Marshal.FreeCoTaskMem(packedOutputcredential);
            }
        }

        #endregion

        #region Protected

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            if (Environment.OSVersion.Version.Major < 5)
                throw new PlatformNotSupportedException("The Credential Management API requires Windows XP/Windows Server 2003 or higher.");

            if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != 0)
                throw new ThreadStateException("error"); // TODO:

            CREDUI_INFO credUiInfo = new CREDUI_INFO
            {
                hwndParent = hwndOwner,
                pszCaptionText = _title,
                pszMessageText = _message
            };
            if (_banner == null || (UseVistaDialogInternal && AutoUpgradeEnabled))
                credUiInfo.hbmBanner = IntPtr.Zero;
            else
                credUiInfo.hbmBanner = ((Bitmap)_banner).GetHbitmap();
            credUiInfo.cbSize = Marshal.SizeOf(credUiInfo);

            if (!string.IsNullOrEmpty(_domain))
                _internalUsername.Insert(0, _domain + "\\");

            if (_password == null)
                _password = new SecureString();

            try
            {
                int resultCode;
                if (UseVistaDialogInternal && AutoUpgradeEnabled)
                    resultCode = RunDialogVista(credUiInfo);
                else
                    resultCode = RunDialogOld(credUiInfo);

                switch (resultCode)
                {
                    case ERROR_SUCCESS:
                        return true;
                    case ERROR_CANCELLED:
                        return false;
                    default:
                        Win32Exception win32Exception = new Win32Exception(resultCode);
                        throw new InvalidOperationException(win32Exception.Message, win32Exception);
                }
            }
            catch
            { throw; }
            finally
            {
                NativeMethods.DeleteObject(credUiInfo.hbmBanner);
            }
        }

        #endregion

        #endregion

    }
}