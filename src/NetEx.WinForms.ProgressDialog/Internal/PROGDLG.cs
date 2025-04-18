using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms.Internal
{
    /// <summary>
    /// Flags that control the operation of the progress dialog box.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775262(v=vs.85).aspx"/>
    [Flags]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum PROGDLG : uint
    {
        /// <summary>
        /// Normal progress dialog box behavior.
        /// </summary>
        PROGDLG_NORMAL = 0x00000000,
        /// <summary>
        /// The progress dialog box will be modal to the window specified by hwndParent. By default, a progress dialog box is modeless.
        /// </summary>
        [SuppressMessage("ReSharper", "CommentTypo")]
        PROGDLG_MODAL = 0x00000001,
        /// <summary>
        /// Automatically estimate the remaining time and display the estimate on line 3. If this flag is set, IProgressDialog::SetLine can be used only to display text on lines 1 and 2.
        /// </summary>
        PROGDLG_AUTOTIME = 0x00000002,
        /// <summary>
        /// Do not show the "time remaining" text.
        /// </summary>
        PROGDLG_NOTIME = 0x00000004,
        /// <summary>
        /// Do not display a minimize button on the dialog box's caption bar.
        /// </summary>
        PROGDLG_NOMINIMIZE = 0x00000008,
        /// <summary>
        /// Do not display a progress bar. Typically, an application can quantitatively determine how much of the operation remains and periodically pass that value to IProgressDialog::SetProgress. The progress dialog box uses this information to update its progress bar. This flag is typically set when the calling application must wait for an operation to finish, but does not have any quantitative information it can use to update the dialog box.
        /// </summary>
        PROGDLG_NOPROGRESSBAR = 0x00000010,
        /// <summary>
        /// Windows Vista and later. Sets the progress bar to marquee mode. This causes the progress bar to scroll horizontally, similar to a marquee display. Use this when you wish to indicate that progress is being made, but the time required for the operation is unknown.
        /// </summary>
        PROGDLG_MARQUEEPROGRESS = 0x00000020,
        /// <summary>
        /// Windows Vista and later. Do not display a cancel button. The operation cannot be canceled. Use this only when absolutely necessary.
        /// </summary>
        PROGDLG_NOCANCEL = 0x00000040,
    }
}