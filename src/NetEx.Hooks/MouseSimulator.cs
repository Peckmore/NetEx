using NetEx.Hooks.Interop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

#if NET462_OR_GREATER || NETCOREAPP
using System.Threading.Tasks;
#endif

namespace NetEx.Hooks
{
    /// <summary>
    /// A mouse event simulator, which can simulate <c>MouseClick</c>, <c>MouseDoubleClick</c>, <c>MouseDown</c>, <c>MouseUp</c>, <c>MouseMove</c>, and <c>MouseWheel</c> events.
    /// </summary>
    public static class MouseSimulator
    {
        #region Methods

        /// <summary>
        /// Simulates a <c>MouseClick</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>A <c>MouseClick</c> event consists of a <c>MouseDown</c> event, followed by a short delay, then a <c>MouseUp</c> event.</remarks>
        public static bool MouseClick(MouseButtons mouseButton)
        {
            if (MouseDown(mouseButton))
            {
                Thread.Sleep(1);
                return MouseUp(mouseButton);
            }

            return false;
        }
#if NET462_OR_GREATER || NETCOREAPP
        /// <summary>
        /// Simulates a <c>MouseClick</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>
        /// <para>A <c>MouseClick</c> event consists of a <c>MouseDown</c> event, followed by a short delay, then a <c>MouseUp</c> event.</para>
        /// <para>This method uses an awaitable <see cref="Task"/> to create the delay between <c>MouseDown</c> and <c>MouseUp</c> events.</para>
        /// </remarks>
        public static async Task<bool> MouseClickAsync(MouseButtons mouseButton)
        {
            if (MouseDown(mouseButton))
            {
                await Task.Delay(10);
                return MouseUp(mouseButton);
            }

            return false;
        }
#endif
        /// <summary>
        /// Simulates a <c>MouseDoubleClick</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>A <c>MouseDoubleClick</c> event consists of a <c>MouseClick</c> event, followed by a delay 1ms shorter than the allowed double-click time (as defined by the operating system), then another <c>MouseClick</c> event.</remarks>
        public static bool MouseDoubleClick(MouseButtons mouseButton)
        {
            if (MouseClick(mouseButton))
            {
                Thread.Sleep(NativeMethods.GetDoubleClickTime() - 1);
                return MouseClick(mouseButton);
            }

            return false;
        }
#if NET462_OR_GREATER || NETCOREAPP
        /// <summary>
        /// Simulates a <c>MouseDoubleClick</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>
        /// <para>A <c>MouseDoubleClick</c> event consists of a <c>MouseClick</c> event, followed by a delay 1ms shorter than the allowed double-click time (as defined by the operating system), then another <c>MouseClick</c> event.</para>
        /// <para>This method uses an awaitable <see cref="Task"/> to create the delay between <c>MouseClick</c> events.</para>
        /// </remarks>
        public static async Task<bool> MouseDoubleClickAsync(MouseButtons mouseButton)
        {
            if (await MouseClickAsync(mouseButton))
            {
                await Task.Delay(NativeMethods.GetDoubleClickTime() - 1);
                return await MouseClickAsync(mouseButton);
            }

            return false;
        }
#endif
        /// <summary>
        /// Simulates a <c>MouseDown</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool MouseDown(MouseButtons mouseButton)
        {
            if (mouseButton > MouseButtons.None)
            {
                var input = new INPUT(NativeMethods.INPUT_MOUSE);

                switch (mouseButton)
                {
                    case MouseButtons.Left:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_LEFTDOWN;
                        break;
                    case MouseButtons.Right:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_RIGHTDOWN;
                        break;
                    case MouseButtons.Middle:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_MIDDLEDOWN;
                        break;
                    case MouseButtons.XButton1:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_XDOWN;
                        input.input.mi.mouseData = NativeMethods.XBUTTON1;
                        break;
                    case MouseButtons.XButton2:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_XDOWN;
                        input.input.mi.mouseData = NativeMethods.XBUTTON2;
                        break;
                }

                var success = NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 1;
                Debug.Assert(success, new Win32Exception(Marshal.GetLastWin32Error()).Message); // Lazy way to get the error message.
                return success;
            }

            return false;
        }
        /// <summary>
        /// Simulates a <c>MouseMove</c> event using <see cref="MouseCoordinateMapping.Absolute"/> coordinate mapping.
        /// </summary>
        /// <param name="location">The location to simulator moving the mouse cursor to.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool MouseMove(Point location)
        {
            return MouseMove(location, MouseCoordinateMapping.Absolute);
        }
        /// <summary>
        /// Simulates a <c>MouseMove</c> event..
        /// </summary>
        /// <param name="location">The location to simulator moving the mouse cursor to.</param>
        /// <param name="mouseCoordinateMapping">Defines how the specified location is applied relative to the current mouse cursor location.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool MouseMove(Point location, MouseCoordinateMapping mouseCoordinateMapping)
        {
            var input = new INPUT(NativeMethods.INPUT_MOUSE);
            input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_MOVE | (uint)mouseCoordinateMapping;

            switch (mouseCoordinateMapping)
            {
                case MouseCoordinateMapping.Relative:
                    input.input.mi.pt = location;
                    break;
                case MouseCoordinateMapping.Absolute:
                    var primaryMonitorSize = NativeMethods.GetPrimaryMonitorSize();
                    input.input.mi.pt.X = (int)Math.Ceiling(location.X * 65535d / primaryMonitorSize.Width) + 1;
                    input.input.mi.pt.Y = (int)Math.Ceiling(location.Y * 65535d / primaryMonitorSize.Height) + 1;
                    break;
                case MouseCoordinateMapping.VirtualDesktop:
                    var virtualScreen = NativeMethods.GetVirtualScreen();
                    input.input.mi.pt.X = (int)Math.Ceiling(location.X * 65535d / virtualScreen.Width) + 1;
                    input.input.mi.pt.Y = (int)Math.Ceiling(location.Y * 65535d / virtualScreen.Height) + 1;
                    break;
            }

            var success = NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 1;
            Debug.Assert(success, new Win32Exception(Marshal.GetLastWin32Error()).Message); // Lazy way to get the error message.
            return success;
        }
        /// <summary>
        /// Simulates a <c>MouseUp</c> event..
        /// </summary>
        /// <param name="mouseButton">The mouse button to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool MouseUp(MouseButtons mouseButton)
        {
            if (mouseButton > MouseButtons.None)
            {
                var input = new INPUT(NativeMethods.INPUT_MOUSE);

                switch (mouseButton)
                {
                    case MouseButtons.Left:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_LEFTUP;
                        break;
                    case MouseButtons.Right:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_RIGHTUP;
                        break;
                    case MouseButtons.Middle:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_MIDDLEUP;
                        break;
                    case MouseButtons.XButton1:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_XUP;
                        input.input.mi.mouseData = NativeMethods.XBUTTON1;
                        break;
                    case MouseButtons.XButton2:
                        input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_XUP;
                        input.input.mi.mouseData = NativeMethods.XBUTTON2;
                        break;
                }

                var success = NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 1;
                Debug.Assert(success, new Win32Exception(Marshal.GetLastWin32Error()).Message); // Lazy way to get the error message.
                return success;
            }

            return false;
        }
        /// <summary>
        /// Simulates a <c>MouseWheel</c> event..
        /// </summary>
        /// <param name="delta">The mouse wheel increment to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool MouseWheel(int delta)
        {
            if (delta > 0)
            {
                var input = new INPUT(NativeMethods.INPUT_MOUSE);
                input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_WHEEL;
                input.input.mi.mouseData = (uint)delta;

                var success = NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 1;
                Debug.Assert(success, new Win32Exception(Marshal.GetLastWin32Error()).Message); // Lazy way to get the error message.
                return success;
            }

            return false;
        }

        #endregion
    }
}