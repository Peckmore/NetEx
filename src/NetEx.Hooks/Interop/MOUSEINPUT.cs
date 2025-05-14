using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetEx.Hooks.Interop
{
    /// <summary>
    /// Contains information about a simulated mouse event.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput"/>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        #region Fields

        /// <summary>
        /// The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member.
        /// </summary>
        public Point pt;
        /// <summary>
        /// <para>If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        /// <para>Windows Vista: If dwFlags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel movement.A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        /// <para>If dwFlags does not contain MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, then mouseData should be zero.</para>
        /// <para>If dwFlags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons were pressed or released. This value may be any combination of the following flags.</para>
        /// </summary>
        public uint mouseData;
        /// <summary>
        /// <para>A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values.</para>
        /// <para>The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions.Similarly MOUSEEVENTF_LEFTUP is set only when the button is first released.</para>
        /// <para>You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field.</para>
        /// </summary>
        public uint dwFlags;
        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public uint time;
        /// <summary>
        /// An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public IntPtr dwExtraInfo;

        #endregion
    }
}