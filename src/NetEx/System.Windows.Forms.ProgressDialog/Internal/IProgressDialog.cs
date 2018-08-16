using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
    [ComImport]
    [Guid("EBBC7C04-315E-11d2-B62F-006097DF5BD4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal interface IProgressDialog
    {
        /// <summary>
        /// Starts the progress dialog box.
        /// </summary>
        /// <param name="hwndParent">A handle to the dialog box's parent window.</param>
        /// <param name="punkEnableModless">Reserved. Set to null.</param>
        /// <param name="dwFlags">Flags that control the operation of the progress dialog box. </param>
        /// <param name="pvResevered">Reserved. Set to IntPtr.Zero</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775262(v=vs.85).aspx"/>
        [PreserveSig]
        uint StartProgressDialog(
            IntPtr hwndParent, //HWND
            [MarshalAs(UnmanagedType.IUnknown)] object punkEnableModless, //IUnknown
            uint dwFlags,  //DWORD
            IntPtr pvResevered //LPCVOID
            );
        /// <summary>
        /// Stops the progress dialog box and removes it from the screen.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775264(v=vs.85).aspx"/>
        void StopProgressDialog();
        /// <summary>
        /// Sets the title of the progress dialog box.
        /// </summary>
        /// <param name="pwzTitle">A pointer to a null-terminated Unicode string that contains the dialog box title.</param>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775260(v=vs.85).aspx"/>
        void SetTitle(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzTitle //LPCWSTR
            );
        /// <summary>
        /// Specifies an Audio-Video Interleaved (AVI) clip that runs in the dialog box. Note: This method is not supported in Windows Vista or later versions.
        /// </summary>
        /// <param name="hInstAnimation">An instance handle to the module from which the AVI resource should be loaded.</param>
        /// <param name="idAnimation">An AVI resource identifier. To create this value, use the MAKEINTRESOURCE macro. The control loads the AVI resource from the module specified by hInstAnimation.</param>
        /// <remarks><see cref="SetAnimation"/> cannot be called before the progress dialog is visible. Until it is displayed, the progress dialog does not have a valid HWND. The existance of that HWND can be used to determine whether <see cref="SetAnimation"/> can be called.
        /// <para>This method takes the instance handle specified by hInstAnimation and uses an animation control to open and run a silent AVI clip.There are several restrictions as to what types of AVI clips can be used, including the following:</para>
        /// <list type="bullet">
        /// <item>
        /// <description>Clips cannot include sound.</description>
        /// </item>
        /// <item>
        /// <description>The size of the AVI clip cannot exceed 272 by 60 pixels.Smaller rectangles can be used, but they might not be properly centered.</description>
        /// </item>
        /// <item>
        /// <description>AVI clips must either be uncompressed or compressed with run-length (BI_RLE8) encoding. If you attempt to use an unsupported compression type, no animation is displayed.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775250(v=vs.85).aspx"/>
        void SetAnimation(
            IntPtr hInstAnimation, //HINSTANCE
            ushort idAnimation //UINT
            );
        /// <summary>
        /// Checks whether the user has canceled the operation.
        /// </summary>
        /// <returns>TRUE if the user has cancelled the operation; otherwise, FALSE.</returns>
        /// <remarks>The system does not send a message to the application when the user clicks the Cancel button. You must periodically use this function to poll the progress dialog box object to determine whether the operation has been canceled.</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775246(v=vs.85).aspx"/>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool HasUserCancelled();
        /// <summary>
        /// Updates the progress dialog box with the current state of the operation.
        /// </summary>
        /// <param name="dwCompleted">An application-defined value that indicates what proportion of the operation has been completed at the time the method was called.</param>
        /// <param name="dwTotal">An application-defined value that specifies what value dwCompleted will have when the operation is complete.</param>
        /// <remarks>Use any convenient numerical measure of the progress of the operation. To use values larger than 4 gigabytes (GB), you can instead call <see cref="SetProgress64"/>.</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775256(v=vs.85).aspx"/>
        void SetProgress(
            uint dwCompleted, //DWORD
            uint dwTotal //DWORD
            );
        /// <summary>
        /// Updates the progress dialog box with the current state of the operation.
        /// </summary>
        /// <param name="ullCompleted">An application-defined value that indicates what proportion of the operation has been completed at the time the method was called.</param>
        /// <param name="ullTotal">An application-defined value that specifies what value ullCompleted will have when the operation is complete.</param>
        /// <remarks>This method has exactly the same function as <see cref="SetProgress"/> but allows you to use values larger than one DWORD (4 GB).</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775258(v=vs.85).aspx"/>
        void SetProgress64(
            ulong ullCompleted, //ULONGLONG
            ulong ullTotal //ULONGLONG
            );
        /// <summary>
        /// Displays a message in the progress dialog.
        /// </summary>
        /// <param name="dwLineNum">The line number on which the text is to be displayed. Currently there are three lines — 1, 2, and 3. If the PROGDLG_AUTOTIME flag was included in the dwFlags parameter when IProgressDialog::StartProgressDialog was called, only lines 1 and 2 can be used. The estimated time will be displayed on line 3.</param>
        /// <param name="pwzString">A null-terminated Unicode string that contains the text.</param>
        /// <param name="fCompactPath">TRUE to have path strings compacted if they are too large to fit on a line. The paths are compacted with PathCompactPath.</param>
        /// <param name="pvResevered"> Reserved. Set to IntPtr.Zero.</param>
        /// <remarks>This function is typically used to display a message such as "Item XXX is now being processed." typically, messages are displayed on lines 1 and 2, with line 3 reserved for the estimated time.</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775254(v=vs.85).aspx"/>
        void SetLine(
            uint dwLineNum, //DWORD
            [MarshalAs(UnmanagedType.LPWStr)] string pwzString, //LPCWSTR
            [MarshalAs(UnmanagedType.VariantBool)] bool fCompactPath, //BOOL
            IntPtr pvResevered //LPCVOID
            );
        /// <summary>
        /// Sets a message to be displayed if the user cancels the operation.
        /// </summary>
        /// <param name="pwzCancelMsg">A pointer to a null-terminated Unicode string that contains the message to be displayed.</param>
        /// <param name="pvResevered">Reserved. Set to NULL.</param>
        /// <remarks>Even though the user clicks Cancel, the application cannot immediately call <see cref="StopProgressDialog"/> to close the dialog box. The application must wait until the next time it calls <see cref="HasUserCancelled"/> to discover that the user has canceled the operation. Since this delay might be significant, the progress dialog box provides the user with immediate feedback by clearing text lines 1 and 2 and displaying the cancel message on line 3. The message is intended to let the user know that the delay is normal and that the progress dialog box will be closed shortly. It is typically is set to something like "Please wait while ...".</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775252(v=vs.85).aspx"/>
        void SetCancelMsg(
            [MarshalAs(UnmanagedType.LPWStr)] string pwzCancelMsg, //LPCWSTR
            object pvResevered //LPCVOID
            );
        /// <summary>
        /// Resets the progress dialog box timer to zero.
        /// </summary>
        /// <param name = "dwTimerAction" > Flags that indicate the action to be taken by the timer.</param>
        /// <param name = "pvResevered" > Reserved.Set to NULL.</param>
        /// <remarks>The timer is used to estimate the remaining time. It is started when your application calls <see cref="StartProgressDialog"/>. Unless your application will start immediately, it should call <see cref="Timer"/> just before starting the operation. This practice ensures that the time estimates will be as accurate as possible. This method should not be called after the first call to <see cref="SetProgress"/>.</remarks>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb775266(v=vs.85).aspx"/>
        void Timer(
            uint dwTimerAction, //DWORD
            object pvResevered //LPCVOID
            );
    }
}