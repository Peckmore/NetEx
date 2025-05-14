using NetEx.Hooks.Interop;

namespace NetEx.Hooks
{
    /// <summary>
    /// Defines how simulated mouse movements should be applied to the mouse position, relative to its current position.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput"/>
    public enum MouseCoordinateMapping
    {
        /// <summary>
        /// Maps the mouse coordinates relative to the previous mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up).
        /// </summary>
        /// <remarks>
        /// <para>Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet.</para>
        /// <para>The system applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance. If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.</para>
        /// </remarks>
        Relative = 0,
        /// <summary>The mouse coordinates map to normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right corner. In a multi-monitor system, the coordinates map to the primary monitor.</summary>
        Absolute = NativeMethods.MOUSEEVENTF_ABSOLUTE,
        /// <summary>The mouse coordinates map to the entire virtual desktop.</summary>
        VirtualDesktop = NativeMethods.MOUSEEVENTF_ABSOLUTE | NativeMethods.MOUSEEVENTF_VIRTUALDESK
    }
}