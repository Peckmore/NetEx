using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms.Internal;

#if NETFRAMEWORK
using System.Security.Permissions;
#endif

namespace System.Windows.Forms
{
    /// <summary>
    /// Displays a standard dialog box that prompts the user to enter credentials. This class cannot be inherited.
    /// </summary>
    [DefaultEvent("HelpRequest")]
    [DefaultProperty("Username")]
    [Description("Displays a dialog box that prompts the user to enter credentials.")]
    [ToolboxBitmap(typeof(CredentialDialog), "ToolboxBitmap.png")]
    public sealed class CredentialDialog : CommonDialog
    {
        #region Constants

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_BANNER_HEIGHT = 60;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_BANNER_WIDTH = 320;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_MAX_CAPTION_LENGTH = 128;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_MAX_DOMAIN_TARGET_LENGTH = 337;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_MAX_MESSAGE_LENGTH = 32767;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_MAX_PASSWORD_LENGTH = 256;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int CREDUI_MAX_USERNAME_LENGTH = 513;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int ERROR_CANCELLED = 1223;
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private const int ERROR_SUCCESS = 0;

        #endregion

        #region Fields

        private string _domain;
        private Image? _image;
        private string _message;
        private SecureString? _password;
        private bool _saveChecked;
        private string _title;
        private string _username;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialDialog" /> class.
        /// </summary>
        public CredentialDialog()
        {
            _domain = string.Empty;
            _message = string.Empty;
            _title = string.Empty;
            _username = string.Empty;

            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating a Windows error code to be displayed in the dialog box. This only applies to Windows Vista or later, and only when <see cref="AutoUpgradeEnabled"/> is set to <see langword="true"/>.
        /// </summary>
        /// <value>The Windows error code for the corresponding error message to be formatted and displayed in the dialog box.</value>
        [Category("Behaviour")]
        [DefaultValue(0)]
        [Description("Controls what Windows error message to show in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public int AuthenticationError { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box should automatically upgrade appearance and behavior when running on Windows Vista or later.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box should automatically upgrade appearance and behavior when running on Windows Vista or later; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>If this property is <see langword="false"/> the dialog box will have a Windows XP-style appearance and behavior on Windows Vista. On Windows XP and Windows Server 2003 this property does not have any effect.</remarks>
        [DefaultValue(true)]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool AutoUpgradeEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating the types of credentials that will be shown in the dialog box when running on Windows XP and Windows Server 2003, or when <see cref="AutoUpgradeEnabled"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value>One of the <see cref="CredentialDialogCredentialFilter"/> values. The default value is <see cref="CredentialDialogCredentialFilter.AllCredentials"/>.</value>
        /// <remarks>This property is only applicable on Windows XP and Windows Server 2003, or on later versions of Windows when using the dialog box with <see cref="AutoUpgradeEnabled"/> set to <see langword="false"/>.</remarks>
        [Category("Behaviour")]
        [DefaultValue(CredentialDialogCredentialFilter.AllCredentials)]
        [Description("The types of credentials to display in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public CredentialDialogCredentialFilter CredentialFilter { get; set; }
        /// <summary>
        /// Gets or sets the domain entered in the dialog box.
        /// </summary>
        /// <value>The domain entered in the dialog box. The default value is an empty string ("").</value>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> exceeds the maximum length for a domain.
        /// </exception>
        [Category("Data")]
        [DefaultValue("")]
        [Description("The domain entered in the dialog box.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public string Domain
        {
            get => _domain;
            set
            {
                if (value.Length > CREDUI_MAX_DOMAIN_TARGET_LENGTH)
                {
                    throw new ArgumentException($"The domain name has a maximum length of {CREDUI_MAX_DOMAIN_TARGET_LENGTH} characters.", nameof(value));
                }

                _domain = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box should prevent the user from changing the supplied username when running on Windows XP and Windows Server 2003, or when <see cref="AutoUpgradeEnabled"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box should prevent the user from changing the supplied username; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>This property is only applicable on Windows XP and Windows Server 2003, or on later versions of Windows when using the dialog box with <see cref="AutoUpgradeEnabled"/> set to <see langword="false"/>.</remarks>
        [Category("Behaviour")]
        [DefaultValue(false)]
        [Description("Controls whether the dialog box should prevent the user from changing the username.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool DisableUsername { get; set; }
        /// <summary>
        /// Gets or sets the image that is displayed in the dialog box when running on Windows XP and Windows Server 2003, or when <see cref="AutoUpgradeEnabled"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value>The <see cref="System.Drawing.Image"/> to display.</value>
        /// <remarks>
        /// If this member is NULL, a default bitmap is used. The bitmap size is limited to 320x60 pixels.
        /// <para>This property is only applicable on Windows XP and Windows Server 2003, or on later versions of Windows when using the dialog box with <see cref="AutoUpgradeEnabled"/> set to <see langword="false"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not have the correct height or width.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The image to display in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Image? Image
        {
            get => _image;
            set
            {
                if (value != null)
                {
                    if (value.Height != CREDUI_BANNER_HEIGHT)
                    {
                        throw new ArgumentException($"The banner image height must be {CREDUI_BANNER_HEIGHT} pixels.", nameof(value));
                    }

                    if (value.Width != CREDUI_BANNER_WIDTH)
                    {
                        throw new ArgumentException($"The banner image width must be {CREDUI_BANNER_WIDTH} pixels.", nameof(value));
                    }
                }

                _image = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box should automatically notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip. This only applies when running on Windows XP and Windows Server 2003, or when <see cref="AutoUpgradeEnabled"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box should automatically notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>This property is only applicable on Windows XP and Windows Server 2003, or on later versions of Windows when using the dialog box with <see cref="AutoUpgradeEnabled"/> set to <see langword="false"/>.</remarks>
        [Category("Behaviour")]
        [DefaultValue(false)]
        [Description("Controls whether the dialog box should automatically notify the user of insufficient credentials by displaying the \"Logon unsuccessful\" balloon tip.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool IncorrectPasswordPrompt { get; set; }
        /// <summary>
        /// Gets or sets the text to display in the dialog box.
        /// </summary>
        /// <value>The text to display in the dialog box. The default value is an empty string ("").</value>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> exceeds the maximum length for the message.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The text to display in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public string Message
        {
            get => _message;
            set
            {
                if (value is { Length: > CREDUI_MAX_MESSAGE_LENGTH })
                {
                    throw new ArgumentException($"The message has a maximum length of {CREDUI_MAX_MESSAGE_LENGTH} characters.", nameof(value));
                }

                _message = value;
            }
        }
        /// <summary>
        /// Gets or sets a <see cref="SecureString"/> containing the password entered in the dialog box.
        /// </summary>
        /// <value>The password entered in the dialog box. The default value is null.</value>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> exceeds the maximum length for a password.
        /// </exception>
        [Category("Data")]
        [DefaultValue(null)]
        [Description("The password entered in the dialog box.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public SecureString? Password
        {
            get => _password;
            set
            {
                if (value is { Length: > CREDUI_MAX_PASSWORD_LENGTH })
                {
                    throw new ArgumentException($"The password has a maximum length of {CREDUI_MAX_PASSWORD_LENGTH} characters.", nameof(value));
                }

                _password = value;
            }
        }
#if DEBUG
        /// <summary>
        /// Gets a <see cref="string"/> containing the password entered in the dialog box.
        /// </summary>
        /// <value>The value of <see cref="Password"/> converted to a <see cref="string"/>.</value>
        /// <remarks>This property is provided for debugging purposes only, and is only included in debug builds.</remarks>
        [Category("Data")]
        [Description("The password entered in the dialog box. This property is provided for debugging purposes only, and is only included in debug builds.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public string PasswordString
        {
            get
            {
                var unmanagedPasswordPointer = IntPtr.Zero;
                try
                {
                    unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password ?? new());
                    return Marshal.PtrToStringUni(unmanagedPasswordPointer) ?? string.Empty;
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
                }
            }
        }
#endif
        /// <summary>
        /// Gets or sets a value indicating whether the save check box is selected.
        /// </summary>
        /// <value><see langword="true"/> if the save check box is selected; otherwise, <see langword="false"/>. The default value is <see langword="false"/>.</value>
        /// <remarks>The <see cref="ShowSave"/> property must be set before in order for the save check box to appear in the dialog box.</remarks>
        [Category("Behaviour")]
        [DefaultValue(false)]
        [Description("The state of the save check box in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public bool SaveChecked
        {
            get => _saveChecked;
            set => _saveChecked = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box contains a save check box.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box contains a save check box; otherwise, <see langword="false"/>. The default value is <see langword="true"/>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Controls whether to show the save check box in the dialog.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool ShowSave { get; set; }
        /// <summary>
        /// Gets or sets the dialog box title.
        /// </summary>
        /// <value>The dialog box title. The default value is an empty string ("").</value>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> exceeds the maximum length for the title.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The string to display in the title bar of the dialog box.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public string Title
        {
            get => _title;
            set
            {
                if (value.Length > CREDUI_MAX_CAPTION_LENGTH)
                {
                    throw new ArgumentException($"The caption has a maximum length of {CREDUI_MAX_CAPTION_LENGTH} characters.", nameof(value));
                }

                _title = value;
            }
        }
        /// <summary>
        /// Gets or sets the username entered in the dialog box.
        /// </summary>
        /// <value>The username entered in the dialog box. The default value is an empty string ("").</value>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> exceeds the maximum length for a username.
        /// </exception>
        [Category("Data")]
        [DefaultValue("")]
        [Description("The username entered in the dialog box.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public string Username
        {
            get => _username;
            set
            {
                if (value is { Length: > CREDUI_MAX_USERNAME_LENGTH })
                {
                    throw new ArgumentException($"The user name has a maximum length of {CREDUI_MAX_USERNAME_LENGTH} characters.", nameof(value));
                }

                _username = value;
            }
        }

        #endregion

        #region Methods

        #region Private

        private void ConvertUnmanagedPasswordToSecureString(IntPtr unmanagedPasswordPointer)
        {
            // Create a new SecureString to store the parsed password in.
            var newPassword = new SecureString();

            // Indicate that the following block contains unsafe code
            unsafe
            {
                // Get the pointer to the memory where the user's password has been stored
                // and then create a char array on that memory so that we can iterate through
                var managedPasswordCharacters = (char*)unmanagedPasswordPointer.ToPointer();
                {
                    // Loop through the char array up to the maximum length allowed for a password
                    for (var x = 0; x < CREDUI_MAX_PASSWORD_LENGTH; x++)
                    {
                        // If the current char is a null-character then we have reached the end
                        // of the string in memory so break out of the loop
                        if (managedPasswordCharacters[x] == '\0')
                        {
                            break;
                        }

                        // Append the current char to the password SecureString
                        newPassword.AppendChar(managedPasswordCharacters[x]);

                        // Zero the memory where the current char was stored for additional security
                        managedPasswordCharacters[x] = '0';
                    }
                }
            }

            // Make the password SecureString readonly to prevent tampering and provide
            // additional security
            newPassword.MakeReadOnly();

            // Dispose of the previous password object, and set the password to our newly parsed password.
            _password?.Dispose();
            _password = newPassword;
        }
        private void ParseUsername(StringBuilder username)
        {
            // Create objects to store the parsed username and domain
            var parsedUsername = new StringBuilder(CREDUI_MAX_USERNAME_LENGTH);
            var parsedDomain = new StringBuilder(CREDUI_MAX_DOMAIN_TARGET_LENGTH);

            // Attempt to parse the username using a native method
            if (NativeMethods.CredUIParseUserName(username.ToString(), parsedUsername, CREDUI_MAX_USERNAME_LENGTH, parsedDomain, CREDUI_MAX_DOMAIN_TARGET_LENGTH) == ERROR_SUCCESS)
            {
                // Assign the parsed username and domain to the backing variables for our properties
                _username = parsedUsername.ToString();
                _domain = parsedDomain.ToString();
            }
            else
            {
                // If the native method failed to parse the username and domain it most likely
                // is either malformed or doesn't contain a domain name. As a fallback we'll just
                // assume that the entire string represents the username and that there is no
                // domain name.
                _username = username.ToString().TrimStart('\\');
                _domain = string.Empty;
            }
        }
        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
        private int RunDialogOld(CREDUI_INFO credUiInfo)
        {
            // Begin setting the flags to be used to display the classic credential dialog. We know
            // that we do not want the credentials entered to be stored in the Credential Manager so
            // we can immediately set this flag.
            var flags = CREDUI_FLAGS.CREDUI_FLAGS_DO_NOT_PERSIST;

            // Set the flag to determine whether to show the "Incorrect Password" prompt to the user
            if (IncorrectPasswordPrompt)
            {
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_INCORRECT_PASSWORD;
            }

            // Set the flag to determine whether the "Save Password" checkbox is shown
            if (ShowSave)
            {
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX;
            }

            // Set the flag to determine whether the username within the dialog is read-only
            if (DisableUsername)
            {
                flags |= CREDUI_FLAGS.CREDUI_FLAGS_KEEP_USERNAME;
            }

            // Switch based on our CredentialFilter enum and set the flag to display only the
            // usernames requested by the user
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

            // Create an int to store the result code from showing the dialog
            int resultCode;

            // Get the username that we will pass to the dialog - this object will also get the returned
            // username and domain
            var username = new StringBuilder(_username) { Capacity = CREDUI_MAX_USERNAME_LENGTH };

            // Marshal the password from a SecureString into memory and get a pointer that we can
            // pass to the credential dialog API
            var unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password ?? new());

            try
            {
                // Show the credential dialog and store the result code
                resultCode = NativeMethods.CredUIPromptForCredentials(ref credUiInfo, _domain, IntPtr.Zero, AuthenticationError, username, CREDUI_MAX_USERNAME_LENGTH, unmanagedPasswordPointer, CREDUI_MAX_PASSWORD_LENGTH, ref _saveChecked, (int)flags);

                // If the dialog returned ERROR_SUCCESS then parse the username and password that were entered
                if (resultCode == ERROR_SUCCESS)
                {
                    // Parse the username returned to also get the domain name
                    ParseUsername(username);

                    // The password entered into the dialog has been placed into memory, so we need to
                    // grab this and put it back into our SecureString
                    ConvertUnmanagedPasswordToSecureString(unmanagedPasswordPointer);
                }
            }
            finally
            {
                // Free the memory allocated from marshalling the password into memory
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
            }

            // Return the result code that was returned from the dialog itself
            return resultCode;
        }
        private int RunDialogVista(CREDUI_INFO credUiInfo)
        {
            // Create a variable to store our calculated flags.
            CREDUIWIN flags = 0;

            // Set the flag to determine whether the "Save Password" checkbox is shown.
            if (ShowSave)
            {
                flags |= CREDUIWIN.CREDUIWIN_CHECKBOX;
            }

            // Check the value of our CredentialFilter enum and set the flag to display only the
            // usernames requested by the user.
            if (CredentialFilter == CredentialDialogCredentialFilter.AdministratorsOnly)
            {
                flags |= CREDUIWIN.CREDUIWIN_ENUMERATE_ADMINS;
            }

            // A variable for storing the result code from showing the dialog.
            int resultCode;

            // Create a pointer to the packed credential object we will pass to the dialog, along
            // with it's maximum size.
            var packedInputCredential = IntPtr.Zero;
            var packedInputCredentialSize = 128;

            // Create a pointer to the packed credential object we will receive from the dialog, along
            // with an Int32 to store the object size.
            IntPtr packedOutputCredential;
            int packedOutputCredentialSize;

            // Specify which authentication package we will use with the dialog. This functionality is
            // unsupported.
            uint authenticationPackage = 0;

            // Marshal the password from a SecureString into memory and get a pointer that we can
            // pass to the credential dialog API.
            var unmanagedPasswordPointer = Marshal.SecureStringToGlobalAllocUnicode(_password ?? new());

            // Get the username that we will pass to the dialog - this object will also get the
            // returned username and domain.
            var username = new StringBuilder(_username) { Capacity = CREDUI_MAX_USERNAME_LENGTH };

            // If the user has also specified a domain then insert that into the username.
#if NET40_OR_GREATER || NET
            if (!string.IsNullOrWhiteSpace(_domain))
#else
            if (!string.IsNullOrEmpty(_domain.Trim()))
#endif
            {
                username.Insert(0, _domain + "\\");
            }

            try
            {
                // Allocate memory for our packed credential
                packedInputCredential = Marshal.AllocCoTaskMem(packedInputCredentialSize);

                // Attempt to pack the credential. If this fails, it can often be because the buffer
                // size provided is too small. If the buffer is not of sufficient size, then
                // 'packedInputCredentialSize' will be set to the required size, in bytes, of the
                // packed credentials.
                if (!NativeMethods.CredPackAuthenticationBuffer(0, username, unmanagedPasswordPointer, packedInputCredential, ref packedInputCredentialSize))
                {
                    // If the attempt to pack the credentials failed it was probably due to
                    // buffer size, so we attempt the operation again using the size specified
                    // during the previous attempt.

                    // Firstly we free the memory previously allocated as it is of the incorrect size.
                    Marshal.FreeCoTaskMem(packedInputCredential);

                    // Now we allocate memory for our packed credential.
                    packedInputCredential = Marshal.AllocCoTaskMem(packedInputCredentialSize);

                    // Now we attempt to pack the credential again. If it fails this time then it is
                    // for some other reason, so we return the Win32 error (as this is what RunDialog
                    // expects).
                    if (!NativeMethods.CredPackAuthenticationBuffer(0, username, unmanagedPasswordPointer, packedInputCredential, ref packedInputCredentialSize))
                    {
                        return Marshal.GetLastWin32Error();
                    }
                }

                // Display the dialog box using the previously packed credentials.
                resultCode = NativeMethods.CredUIPromptForWindowsCredentials(ref credUiInfo, AuthenticationError, ref authenticationPackage, packedInputCredential, packedInputCredentialSize, out packedOutputCredential, out packedOutputCredentialSize, ref _saveChecked, flags);
            }
            finally
            {
                // Free the memory allocated from marshalling the password into memory.
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);

                // Free the memory allocated to the packed credential we passed to the dialog.
                Marshal.FreeCoTaskMem(packedInputCredential);
            }

            try
            {
                // If the dialog box result was OK then unpack the entered credentials.
                if (resultCode == ERROR_SUCCESS)
                {
                    // Create variables for unpacking the credential object into.
                    var internalDomain = new StringBuilder(CREDUI_MAX_DOMAIN_TARGET_LENGTH);
                    var unpackedUsernameBufferSize = CREDUI_MAX_USERNAME_LENGTH;
                    var unpackedDomainBufferSize = CREDUI_MAX_DOMAIN_TARGET_LENGTH;
                    var unpackedPasswordBufferSize = CREDUI_MAX_PASSWORD_LENGTH;
                    unmanagedPasswordPointer = Marshal.AllocHGlobal(CREDUI_MAX_PASSWORD_LENGTH);

                    try
                    {
                        // Try and unpack the packed credential we received from the dialog.
                        if (!NativeMethods.CredUnPackAuthenticationBuffer(1, packedOutputCredential, packedOutputCredentialSize, username, ref unpackedUsernameBufferSize, internalDomain, ref unpackedDomainBufferSize, unmanagedPasswordPointer, ref unpackedPasswordBufferSize))
                        {
                            return Marshal.GetLastWin32Error(); // Return the Win32 error the dialog returned - this is expected by DialogBase.
                        }
                        else
                        {
                            // The credential was successfully unpacked so we can now call our methods
                            // to make the information available.

                            // Parse the username returned to also get the domain name
                            ParseUsername(username);

                            // The password entered into the dialog has been placed into memory, so we need to
                            // grab this and put it back into our SecureString
                            ConvertUnmanagedPasswordToSecureString(unmanagedPasswordPointer);
                        }
                    }
                    finally
                    {
                        // Free the memory allocated from marshalling the password into memory
                        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPasswordPointer);
                    }
                }

                // Return the result code that was returned from the dialog itself
                return resultCode;
            }
            finally
            {
                // Free the memory allocated to the packed credential we received from the dialog.
                Marshal.FreeCoTaskMem(packedOutputCredential);
            }
        }
        private bool UseVistaDialogInternal()
        {
            // This code is based on the FileDialog class, included as part of the .Net Framework

            // Check that we are running on Windows NT
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return false;
            }

            // Check that we are running on Windows Vista or later
            if (Environment.OSVersion.Version < new Version(6, 0)) // Vista identifies as Windows version 6.0
            {
                return false;
            }

            // If we are on Vista or later check whether the user has chosen to use the newer credential dialog
            if (!AutoUpgradeEnabled)
            {
                return false;
            }

#if NETFRAMEWORK
            // Code access security (CAS) is an unsupported, legacy technology. The infrastructure to enable CAS, which exists only
            // in .NET Framework 2.x - 4.x, is deprecated and not receiving servicing or security fixes.

            // Assert EnvironmentPermission in order to determine the system BootMode
            new EnvironmentPermission(PermissionState.Unrestricted).Assert();

            try
            {
#endif
                // If we are running on Vista or later, check that we are running in normal boot mode,
                // and not any other mode (e.g., Safe Mode)
                return SystemInformation.BootMode == BootMode.Normal;
#if NETFRAMEWORK
            }
            finally
            {
                // Code access security (CAS) is an unsupported, legacy technology. The infrastructure to enable CAS, which exists only
                // in .NET Framework 2.x - 4.x, is deprecated and not receiving servicing or security fixes.

                // Revert our previously asserted access permission
                CodeAccessPermission.RevertAssert();
            }
#endif
        }

#endregion

        #region Protected

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_image != null)
                {
                    _image.Dispose();
                    _image = null;
                }

                if (_password != null)
                {
                    _password.Dispose();
                    _password = null;
                }
            }

            base.Dispose(disposing);
        }
        /// <inheritdoc/>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            // Check we are on at least Windows XP/Server 2003 as the Credential APIs require it
            if (Environment.OSVersion.Version.Major < 5)
            {
                throw new PlatformNotSupportedException("The Credential Management API requires Windows XP/Windows Server 2003 or higher.");
            }

            // Check for illegal cross thread calls
            if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != 0)
            {
                throw new ThreadStateException("Current thread must be set to single thread apartment(STA) mode before OLE calls can be made. Ensure that your Main function has STAThreadAttribute marked on it. This exception is only raised if a debugger is attached to the process.");
            }

            // Create our CREDUI_INFO object, which will be used by either the new or old dialog
            var credUiInfo = new CREDUI_INFO
            {
                hwndParent = hwndOwner,
                pszCaptionText = _title,
                pszMessageText = _message
            };

            // Check which style of dialog we are using and get the image if the user has specified
            // one and we are using the classic dialog,
            var gdiBitmap = IntPtr.Zero;
            if (_image != null && !UseVistaDialogInternal())
            {
                gdiBitmap = ((Bitmap)_image).GetHbitmap();
            }

            // Add the image to the CREDUI_INFO object
            credUiInfo.hbmBanner = gdiBitmap;

            // Update the size of the CREDUI_INFO object
            credUiInfo.cbSize = Marshal.SizeOf(credUiInfo);

            try
            {
                // Show the credential dialog using the appropriate API
                var resultCode = UseVistaDialogInternal() ? RunDialogVista(credUiInfo) : RunDialogOld(credUiInfo);

                // Parse the result code to determine our return value, or throw a Win32Exception
                // if an error occurred.
                switch (resultCode)
                {
                    case ERROR_SUCCESS:
                        return true;
                    case ERROR_CANCELLED:
                        return false;
                    default:
                        var win32Exception = new Win32Exception(resultCode);
                        throw new InvalidOperationException(win32Exception.Message, win32Exception);
                }
            }
            finally
            {
                // If an image was used free the memory used by the GDI bitmap object
                if (gdiBitmap != IntPtr.Zero)
                {
                    NativeMethods.DeleteObject(gdiBitmap);
                }
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset()
        {
            // All configurable properties have a DefaultValueAttribute associated
            // with them. So for easy consistency we just grab each property, grab
            // the default value, then assign that value to the property.
            foreach (var property in GetType().GetProperties())
            {
                var dvList = property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (dvList.Length > 0 && dvList[0] is DefaultValueAttribute attribute)
                {
                    property.SetValue(this, attribute.Value, null);
                }
            }
        }

        #endregion

        #endregion
    }
}