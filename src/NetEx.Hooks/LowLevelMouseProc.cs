using System;

namespace NetEx.Hooks
{
    /// <summary>
    /// <para>An application-defined or library-defined callback function used with the SetWindowsHookExA/SetWindowsHookExW function. The system calls this function every time a new mouse input event is about to be posted into a thread input queue.</para>
    /// <para>The HOOKPROC type defines a pointer to this callback function. LowLevelMouseProc is a placeholder for the application-defined or library-defined function name.</para>
    /// <para>LowLevelMouseProc is a placeholder for the application-defined or library-defined function name.</para>
    /// </summary>
    /// <param name="nCode">
    /// <para>A code the hook procedure uses to determine how to process the message.</para>
    /// <para>If nCode is less than zero, the hook procedure must pass the message to the CallNextHookEx function without further processing and should return the value returned by CallNextHookEx.</para>
    /// </param>
    /// <param name="wParam">
    /// <para>The identifier of the mouse message.</para>
    /// <para>This parameter can be one of the following messages: WM_LBUTTONDOWN, WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_RBUTTONDOWN or WM_RBUTTONUP.</para>
    /// </param>
    /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
    /// <returns>
    /// <para>If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.</para>
    /// <para>If nCode is greater than or equal to zero, and the hook procedure did not process the message, it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and may behave incorrectly as a result.</para>
    /// <para>If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the rest of the hook chain or the target window procedure.</para>
    /// </returns>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc"/>
    internal delegate IntPtr LowLevelMouseProc(int nCode, int wParam, IntPtr lParam);
}