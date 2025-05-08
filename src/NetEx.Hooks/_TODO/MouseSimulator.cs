using NetEx.Hooks.Internal;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace NetEx.Hooks
{
    public static class MouseSimulator
    {
        #region Methods

        public static void MouseClick(MouseButtons mouseButton)
        {
            MouseDown(mouseButton);
            Thread.Sleep(1);
            MouseUp(mouseButton);
        }
        public static void MouseDoubleClick(MouseButtons mouseButton)
        {
            MouseClick(mouseButton);
            Thread.Sleep(NativeMethods.GetDoubleClickTime() - 1);
            MouseClick(mouseButton);
        }
        public static void MouseDown(MouseButtons mouseButton)
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

                if (NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
        public static void MouseMove(Point location)
        {
            MouseMove(location, MouseCoordinateMapping.PrimaryMonitor);
        }
        public static void MouseMove(Point location, MouseCoordinateMapping mouseCoordinateMapping)
        {
            var input = new INPUT(NativeMethods.INPUT_MOUSE);
            input.input.mi.dwFlags = (NativeMethods.MOUSEEVENTF_MOVE | (uint)mouseCoordinateMapping);

            switch (mouseCoordinateMapping)
            {
                case MouseCoordinateMapping.Relative:
                    input.input.mi.pt = location;
                    break;
                case MouseCoordinateMapping.PrimaryMonitor:
                    input.input.mi.pt.X = (int)Math.Ceiling((location.X * 65535d) / 1920) + 1; // SystemInformation.PrimaryMonitorSize.Width
                    input.input.mi.pt.Y = (int)Math.Ceiling((location.Y * 65535d) / 1080) + 1; // SystemInformation.PrimaryMonitorSize.Height
                    break;
                case MouseCoordinateMapping.VirtualDesktop:
                    input.input.mi.pt.X = (int)Math.Ceiling(Math.Ceiling((double)(location.X * 65535)) / 1920) + 1; // SystemInformation.VirtualScreen.Width
                    input.input.mi.pt.Y = (int)Math.Ceiling(Math.Ceiling((double)(location.Y * 65535)) / 1080) + 1; // SystemInformation.VirtualScreen.Height
                    break;
            }

            var ret = NativeMethods.SendInput(input, Marshal.SizeOf(input));
            if (ret == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        public static void MouseUp(MouseButtons mouseButton)
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

                if (NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
        public static void MouseWheel(int delta)
        {
            if (delta > 0)
            {
                var input = new INPUT(NativeMethods.INPUT_MOUSE);
                input.input.mi.dwFlags = NativeMethods.MOUSEEVENTF_WHEEL;
                input.input.mi.mouseData = (uint)delta;

                if (NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        #endregion
    }
}