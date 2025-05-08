namespace NetEx.Hooks
{
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
        /// <summary>Maps the absolute coordinates of the mouse on the screen. In a multimonitor system, the coordinates map to the primary monitor.</summary>
        PrimaryMonitor = NativeMethods.MOUSEEVENTF_ABSOLUTE,
        /// <summary>Maps coordinates to the entire desktop.</summary>
        VirtualDesktop = NativeMethods.MOUSEEVENTF_ABSOLUTE | NativeMethods.MOUSEEVENTF_VIRTUALDESK
    }
}