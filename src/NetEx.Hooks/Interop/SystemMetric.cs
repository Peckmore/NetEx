namespace NetEx.Hooks.Interop
{
    /// <summary>
    /// A subset of SystemMetric values.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics#parameters"/>
    internal enum SystemMetric
    {
        /// <summary>
        /// The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
        /// </summary>
        SM_CXSCREEN = 0,

        /// <summary>
        /// The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
        /// </summary>
        SM_CYSCREEN = 1,

        /// <summary>
        /// <para>The width of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle that is defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click. The two clicks must also occur within a specified time.</para>
        /// <para>To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.</para>
        /// </summary>
        SM_CXDOUBLECLK = 36,

        /// <summary>
        /// <para>The height of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click. The two clicks must also occur within a specified time.</para>
        /// <para>To set the height of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKHEIGHT.</para>
        /// </summary>
        SM_CYDOUBLECLK = 37,

        /// <summary>
        /// The coordinates for the left side of the virtual screen. The virtual screen is the bounding rectangle of all display monitors. The SM_CXVIRTUALSCREEN metric is the width of the virtual screen.
        /// </summary>
        SM_XVIRTUALSCREEN = 76,

        /// <summary>
        /// The coordinates for the top of the virtual screen. The virtual screen is the bounding rectangle of all display monitors. The SM_CYVIRTUALSCREEN metric is the height of the virtual screen.
        /// </summary>
        SM_YVIRTUALSCREEN = 77,

        /// <summary>
        /// The width of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors. The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen.
        /// </summary>
        SM_CXVIRTUALSCREEN = 78,

        /// <summary>
        /// The height of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors. The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen.
        /// </summary>
        SM_CYVIRTUALSCREEN = 79,

        /// <summary>
        /// The number of display monitors on a desktop.
        /// </summary>
        SM_CMONITORS = 80
    }
}