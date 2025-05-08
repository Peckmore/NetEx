namespace NetEx.Hooks.Internal
{
    /// <summary>
    /// SystemMetric values.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics#parameters"/>
    internal enum SystemMetric : int
    {
        /// <summary>
        /// <para>The width of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle that is defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click. The two clicks must also occur within a specified time.</para>
        /// <para>To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.</para>
        /// </summary>
        SM_CXDOUBLECLK = 36,

        /// <summary>
        /// <para>The height of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click. The two clicks must also occur within a specified time.</para>
        /// <para>To set the height of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKHEIGHT.</para>
        /// </summary>
        SM_CYDOUBLECLK = 37
    }
}