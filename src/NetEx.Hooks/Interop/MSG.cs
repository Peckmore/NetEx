using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetEx.Hooks.Interop
{
    /// <summary>
    /// Contains message information from a thread's message queue.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg"/>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        /// <summary>
        /// A handle to the window whose window procedure receives the message. This member is NULL when the message is a thread message.
        /// </summary>
        public IntPtr hwnd;
        /// <summary>
        /// Additional information about the message. The exact meaning depends on the value of the message member.
        /// </summary>
        public uint message;
        /// <summary>
        /// Additional information about the message. The exact meaning depends on the value of the message member.
        /// </summary>
        public IntPtr wParam;
        /// <summary>
        /// Additional information about the message. The exact meaning depends on the value of the message member.
        /// </summary>
        public IntPtr lParam;
        /// <summary>
        /// The time at which the message was posted.
        /// </summary>
        public uint time;
        /// <summary>
        /// The cursor position, in screen coordinates, when the message was posted.
        /// </summary>
        public Point pt;
    }
}