using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
    /// <summary>
    /// Displays a standard dialog box that informs the user of the progress of an action. This class cannot be inherited.
    /// </summary> 
    [DefaultEvent("Closed")]
    [DefaultProperty("Value")]
    [Description("Displays a dialog box to inform the user of the progress of an action.")]
    [ToolboxBitmap(typeof(ProgressDialog), "ToolboxBitmap.png")]
    public sealed class ProgressDialog : CommonDialog
    {
        #region Fields

        private bool _dialogResponse;
        private bool _hasCanceled;
        private bool _hasClosed = true;
        private IProgressDialog? _iProgressDialog;
        private readonly string[] _lines;
        private bool _invokedModal = true;
        private bool _modal;
        private IntPtr _parentHandle = IntPtr.Zero;
        private ProgressBarStyle _progressBarStyle;
        private readonly AutoResetEvent _threadCompleted;
        private readonly AutoResetEvent _updated;
        private ulong _value;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user clicks the cancel button on the <see cref="ProgressDialog"/>. The event is not raised if the dialog box was shown using <see cref="CommonDialog.ShowDialog()"/> or one of its overloads.
        /// </summary> 
        public event EventHandler? Canceled;
        /// <summary>
        /// Occurs when the <see cref="ProgressDialog"/> is closed. The event is not raised if the dialog box was shown using <see cref="CommonDialog.ShowDialog()"/> or one of its overloads.
        /// </summary>
        public event EventHandler? Closed;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog" /> class.
        /// </summary>
        public ProgressDialog()
        {
            _lines = new string[3];
            _threadCompleted = new(false);
            _updated = new(false);

            CancelMessage = string.Empty;
            Title = string.Empty;
            
            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the resource that contains an Audio-Video Interleaved (AVI) clip to run in the dialog box. Not supported in Windows Vista and later.
        /// </summary>
        /// <value>An <see cref="AnimationResource"/> which points to a file and the index of the animation resource within that file. The default value is null.</value>
        [Browsable(false)]
        [DefaultValue(null)]
        public AnimationResource? Animation { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the dialog box should automatically close upon cancellation or completion.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box should automatically close upon cancellation or completion; otherwise <see langword="false"/>.</value>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box will automatically close upon cancellation or completion.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool AutoClose { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the dialog box calculates the time remaining based on its active time and progress. If this property is set to <see langword="true"/>, text can only be displayed on lines 1 and 2.
        /// </summary>
        /// <value><see langword="true"/> if the dialog box automatically estimate the remaining time; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box will automatically calculate the time remaining.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool AutoTime { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Cancel button is displayed in the client area of the dialog box. If set to <see langword="false"/> the operation cannot be canceled. Use this only when absolutely necessary. Only applies to Windows Vista and later.
        /// </summary>
        /// <value><see langword="true"/> to display a Cancel button for the dialog box; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box has a cancel button. Only applies to Windows Vista and later.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool CancelButton { get; set; }
        /// <summary>
        /// Gets or sets a message to be displayed if the user cancels the operation.
        /// </summary>
        /// <value>The progress dialog cancel message. The default value is an empty string ("").</value>
        /// <remarks>Even though the user clicks Cancel, the application may not immediately call <see cref="Close"/> to close the dialog box. Since this delay might be significant, the progress dialog box provides the user with immediate feedback by clearing text lines 1 and 2 and displaying the cancel message on line 3. The message is intended to let the user know that the delay is normal and that the progress dialog box will be closed shortly. It is typically is set to something like "Please wait while ...".</remarks>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The string to display if the user cancels the operation.")]
        [Localizable(true)]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
        public string CancelMessage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to truncate path strings for text on lines 1, 2 and 3 of the dialog box. If set to <see langword="true"/> paths are compacted with PathCompactPath.
        /// </summary>
        /// <value><see langword="true"/> to have path strings compacted if they are too large to fit on a line; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box truncates path strings if they are too large to fit on a line.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool CompactPath { get; set; }
        /// <summary>
        /// Gets or sets the maximum value of the range of the progress bar within the dialog box. This property has no effect if <see cref="ShowProgressBar"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value>The maximum value of the range of the progress bar within the dialog box. The default is 100.</value>
        [Category("Behavior")]
        [DefaultValue(typeof(ulong), "100")]
        [Description("The upper bound of the range the dialog box is working with.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public ulong Maximum { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Minimize button is displayed in the caption bar of the dialog box.
        /// </summary>
        /// <value><see langword="true"/> to display a Minimize button for the dialog box; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Determines whether the dialog box has a minimize box in the upper-right corner of its caption bar.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool MinimizeBox { get; set; }
        /// <summary>
        /// Gets or sets the manner in which progress should be indicated on the progress bar within the dialog box. This property has no effect if <see cref="ShowProgressBar"/> is set to <see langword="false"/>. Only applies to Windows Vista and later.
        /// </summary>
        /// <value>One of the <see cref="ProgressBarStyle"/> values. The default value is <see cref="ProgressBarStyle.Continuous"/>.</value>
        /// <remarks>Setting this value to <see cref="ProgressBarStyle.Blocks"/> will have no effect, and the appearance of the progress bar will be the same as if set to <see cref="ProgressBarStyle.Continuous"/>.</remarks>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="value"/> is not a member of the <see cref="ProgressBarStyle"/> enumeration.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue(typeof(ProgressBarStyle), "Continuous")]
        [Description("This property allows the user to set the style of the progress bar within the dialog box. Only applies to Windows Vista and later.")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public ProgressBarStyle ProgressBarStyle
        {
            get => _progressBarStyle;
            set
            {
                if (!Enum.IsDefined(typeof(ProgressBarStyle), value))
                {
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ProgressBarStyle));
                }

                _progressBarStyle = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether a progress bar is displayed in the client area of the dialog box.
        /// </summary>
        /// <value><see langword="true"/> to display a progress bar for the dialog box; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        /// <remarks>Typically, an application can quantitatively determine how much of the operation remains and periodically pass that value to the progress dialog box. The progress dialog box then uses this information to update its progress bar. You would typically set this property to <see langword="false"/> if the calling application must wait for an operation to finish but does not have any quantitative information it can use to update the dialog box.</remarks>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box has a progress bar.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool ShowProgressBar { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether time remaining is displayed in the client area of the dialog box.
        /// </summary>
        /// <value><see langword="true"/> to display the time remaining for the dialog box; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Controls whether the dialog box shows \"time remaining\" information.")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool ShowRemainingTime { get; set; }
        /// <summary>
        /// Gets or sets the dialog box title.
        /// </summary>
        /// <value>The dialog box title. The default value is an empty string ("").</value>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The string to display in the title bar of the dialog box.")]
        [Localizable(true)]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the current position of the progress bar within the dialog box. This property has no effect if <see cref="ShowProgressBar"/> is set to <see langword="false"/>.
        /// </summary>
        /// <value>The position within the range of the progress bar. The default is 0.</value>
        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(typeof(ulong), "0")]
        [Description("The current value for the dialog box, in the range specified between 0 and the maximum property.")]
        public ulong Value
        {
            get => _value;
            set
            {
                if (value > Maximum)
                {
                    value = Maximum;
                }

                // Update the value.
                _value = value;

                // Trigger our polling loop to update the dialog.
                _updated.Set();
            }
        }

        #endregion

        #region Methods

        #region Private

        private void CloseComDialog()
        {
            // If the COM dialog exists then close it and clean up
            if (_iProgressDialog != null)
            {
                // Stop the progress dialog and release it's resources
                _iProgressDialog.StopProgressDialog();
                Marshal.ReleaseComObject(_iProgressDialog);
                _iProgressDialog = null;
            }
        }
        private void OnClosed(bool waitForThread)
        {
            // Set a flag to indicate that the dialog has been closed - this also
            // tells our thread to stop
            _hasClosed = true;

            // Wait for the dialog thread to stop if requested
            if (waitForThread)
            {
                _threadCompleted.WaitOne();
            }

            // Set the owner of the dialog (the parent window) as the
            // foreground window. This should make for a nicer user
            // experience as when the dialog closes it will return
            // focus to the window that called it.
            NativeMethods.SetForegroundWindow(_parentHandle);

            // If we are modeless then invoke the Closed event.
            if (!_modal)
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Protected

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Call our "Close" method to do the initial cleanup
                Close();

                // Now dispose of any managed resources

                // Check to make sure that the COM dialog object has been cleaned up
                CloseComDialog();

                // Close our reset event objects
                _threadCompleted.Close();
                _updated.Close();
            }

            base.Dispose(disposing);
        }
        /// <inheritdoc/>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            if (_iProgressDialog != null)
            {
                return false;
            }

            // Copy the modal state, then reset the variable so a possible
            // ShowDialog call in the future will work correctly.
            _modal = _invokedModal;
            _invokedModal = true;

            // Set our flags
            _hasCanceled = false;
            _hasClosed = false;

            // Keep a reference to the owner for if we are running modeless.
            _parentHandle = hwndOwner;

            // Dialog thread
            var runDialogThread = new Thread(delegate ()
            {
                // Clear the thread completed AutoResetEvent to indicate that the thread has started
                _threadCompleted.Reset();

                // Copy the settings for the dialog box instance
                var autoTimeCopy = AutoTime;
                var compactPathCopy = CompactPath;

                // Create an IntPtr for if we load an AnimationResource. We create
                // the variable here so we can use it in the finally block to free
                // up any resources.
                var animationModuleHandle = IntPtr.Zero;

                // We run the following in a try block so that if something unexpected
                // goes wrong we can still clean up the COM object for the dialog
                try
                {
                    // Create a new IProgressDialog
                    _iProgressDialog = (IProgressDialog)new WindowsProgressDialog();

                    // Build the progress dialog flags based on the properties the user has set
                    var flags = (uint)PROGDLG.PROGDLG_NORMAL;
                    if (_modal)
                    {
                        flags += (uint)PROGDLG.PROGDLG_MODAL;
                    }

                    if (!ShowRemainingTime)
                    {
                        flags += (uint)PROGDLG.PROGDLG_NOTIME;
                    }

                    if (AutoTime)
                    {
                        flags += (uint)PROGDLG.PROGDLG_AUTOTIME;
                    }

                    if (!MinimizeBox)
                    {
                        flags += (uint)PROGDLG.PROGDLG_NOMINIMIZE;
                    }

                    if (!ShowProgressBar)
                    {
                        flags += (uint)PROGDLG.PROGDLG_NOPROGRESSBAR;
                    }

                    if (ProgressBarStyle == ProgressBarStyle.Marquee)
                    {
                        flags += (uint)PROGDLG.PROGDLG_MARQUEEPROGRESS;
                    }

                    if (!CancelButton)
                    {
                        flags += (uint)PROGDLG.PROGDLG_NOCANCEL;
                    }

                    // We do a check on CancelMessage before we apply it and replace it
                    // with a blank space if the user has set it to null or empty. This
                    // prevents a UI issue with the dialog which causes its contents to
                    // shrink and leave an "unpainted" strip along the bottom of the
                    // dialog if CancelMessage is null or empty when user presses Cancel.
                    _iProgressDialog.SetCancelMsg(CancelMessage, null);

                    // Apply the remaining properties
                    _iProgressDialog.SetProgress64(Value, Maximum);
                    _iProgressDialog.SetTitle(Title);

                    // Set the dialog animation
                    // Note: Animations only apply to Windows versions earlier than Vista, so if we
                    // detect that we are running on Vista or later we skip setting the animation
                    // as it wouldn't do anything anyway.
                    if (Environment.OSVersion.Version.Major < 6 && Animation != null)
                    {
                        // ReSharper disable once CommentTypo
                        animationModuleHandle = NativeMethods.LoadLibraryEx(string.Format(CultureInfo.InvariantCulture, "{0}\0", Animation.FileName), IntPtr.Zero, 0x03); // Win32: DONT_RESOLVE_DLL_REFERENCES | LOAD_LIBRARY_AS_DATAFILE
                        if (animationModuleHandle != IntPtr.Zero)
                        {
                            _iProgressDialog.SetAnimation(animationModuleHandle, Animation.ResourceIndex);
                        }
                    }

                    // Show the progress dialog
                    var hResult = _iProgressDialog.StartProgressDialog(_parentHandle, null, flags, IntPtr.Zero);
                    if (hResult != 0)
                    {
                        // ReSharper disable once StringLiteralTypo
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unable to start the progress dialog. HRESULT: {0}", hResult));
                    }

                    // Reset the timer dialog to begin time remaining calculations
                    // ReSharper disable once CommentTypo
                    _iProgressDialog.Timer(0x01, null); // Win32: PDTIMER_RESET

                    // Monitor for property updates and/or the user pressing cancel
                    while (!_hasClosed)
                    {
                        if (_iProgressDialog.HasUserCancelled() && !_hasCanceled)
                        {
                            // The user has pressed 'Cancel' on the dialog and we haven't
                            // already processed it.

                            // We use this flag to indicate that we have already processed
                            // the user cancelling the dialog. This prevents us from raising
                            // the Canceled event multiple times.
                            _hasCanceled = true;

                            // Set the dialog response to false as the dialog was cancelled.
                            _dialogResponse = false;

                            // If we are modeless raise the Canceled event.
                            if (!_modal)
                            {
                                Canceled?.BeginInvoke(this, EventArgs.Empty, null, null);
                            }

                            // If AutoClose is set to true then exit the loop.
                            if (AutoClose)
                            {
                                break;
                            }
                        }
                        else if (Value >= Maximum && AutoClose)
                        {
                            // The user has set the Value for the progress of the dialog to
                            // greater than or equal to the Maximum value (in which case the
                            // dialog has 'Completed'). If AutoClose is set to true then exit
                            // the loop.

                            // Set the dialog response to true as the dialog has 'completed".
                            _dialogResponse = true;

                            // Exit the loop.
                            break;
                        }
                        else if (!_hasCanceled)
                        {
                            // We only carry on updating if the dialog hasn't been canceled.
                            // This allows the Cancel message to be correctly displayed.

                            // Update the text lines of our dialog, accounting for whether
                            // AutoTime is enabled.
                            for (uint x = 0; x < (autoTimeCopy ? 2 : 3); x++)
                                _iProgressDialog.SetLine(x + 1, _lines[x], compactPathCopy, IntPtr.Zero);

                            // Set the Progress and Title of our dialog. To simplify the code we
                            // always use SetProgress64.
                            _iProgressDialog.SetTitle(Title);
                            _iProgressDialog.SetProgress64(Value, Maximum);
                        }

                        // We use a reset event here as we need to continuously poll the
                        // HasUserCancelled to check whether the user has pressed Cancel.
                        // We set this polling rate fairly low to minimize the impact on
                        // system performance, but high enough to keep it responsive. But
                        // the user may wish to send updates to the value of the progress
                        // bar or the lines of text at a greater rate. By using the reset
                        // event we can poll and update at a fixed rate, but also trigger
                        // an immediate update whenever Value or SetLine() are updated.
                        _updated.WaitOne(250, true);
                    }
                }
                finally
                {
                    // Close the dialog COM object.
                    CloseComDialog();

                    // Release any resources used loading an AnimationResource.
                    if (animationModuleHandle != IntPtr.Zero)
                    {
                        NativeMethods.FreeLibrary(animationModuleHandle);
                    }

                    // Indicate that the thread has completed.
                    _threadCompleted.Set();

                    // Call the OnClosed() method to indicate that the dialog is closed and to fire
                    // off the Closed event.
                    OnClosed(false);
                }
            });

            // Run the dialog thread, which will show the dialog, update it
            // and monitor for completion/cancellation.
            runDialogThread.Start();

            // If we are running modal then we need to block the calling
            // thread and wait for the dialog to close.
            if (_modal)
            {
                runDialogThread.Join();
            }

            // Return the dialog response (only applicable for ShowDialog() calls).
            return _dialogResponse;
        }

        #endregion

        #region Public

        /// <summary>
        /// Closes the dialog box.
        /// </summary>
        public void Close()
        {
            // If the dialog has already been closed then return immediately.
            if (_hasClosed)
            {
                return;
            }

            // Call our method to close the dialog and fire off the Closed event.
            OnClosed(true);
        }
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
        /// <summary>
        /// Displays a message in the dialog.
        /// </summary>
        /// <param name="line">The line number on which the text is to be displayed. Currently there are three lines — 1, 2, and 3. If <see cref="AutoTime"/> is set to <see langword="true"/> only lines 1 and 2 can be used. The estimated time will be displayed on line 3.</param>
        /// <param name="message">The message to be displayed on the line specified.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="line"/> is less than 1 or greater than 3.
        /// </exception>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void SetLine(int line, string message)
        {
            // Check that the line parameter does not fall outside the possible range.
            if (line is < 1 or > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(line), string.Format(CultureInfo.InvariantCulture, "{0} must be a value of 1, 2 or 3.", nameof(line)));
            }

            // Set the specified line to the message supplied.
            _lines[line - 1] = message;

            // Trigger our polling loop to update the dialog.
            _updated.Set();
        }
        /// <summary>
        /// Runs a common dialog box with a default owner in a non-modal fashion.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
        public void Show()
        {
            Show(null);
        }
        /// <summary>
        /// Runs a common dialog box with the specified owner in a non-modal fashion.
        /// </summary>
        /// <param name="owner">Any object that implements <see cref="IWin32Window"/> that represents the top-level window that will own the modal dialog box.</param>
        public void Show(IWin32Window? owner)
        {
            // There is already a dialog shown so return immediately.
            if (_iProgressDialog != null)
            {
                return;
            }

            // The ShowDialog command does a lot of checks to make sure
            // it is possible to show a dialog, and to get a window handle
            // if the user doesn't supply one. Rather than replicate that code
            // here we can just set a flag to tell RunDialog to show the dialog
            // in a modeless fashion, and then just call the base ShowDialog
            // method to benefit from all of those checks.
            _invokedModal = false;
            ShowDialog(owner);
        }

        #endregion

        #endregion
    }
}