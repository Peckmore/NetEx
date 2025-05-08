using System;

namespace NetEx.Hooks
{
    /// <summary>
    /// Specifies constants that define which mouse button was pressed.
    /// </summary>
    [Flags]
    public enum MouseButtons
    {
        /// <summary>
        /// The left mouse button was pressed.
        /// </summary>
        Left = 0x00100000,

        /// <summary>
        /// No mouse button was pressed.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The right mouse button was pressed.
        /// </summary>
        Right = 0x00200000,

        /// <summary>
        /// The middle mouse button was pressed.
        /// </summary>
        Middle = 0x00400000,

        /// <summary>
        /// The first XButton (XBUTTON1) on Microsoft IntelliMouse Explorer was pressed.
        /// </summary>
        XButton1 = 0x00800000,

        /// <summary>
        /// The second XButton (XBUTTON2) on Microsoft IntelliMouse Explorer was pressed.
        /// </summary>
        XButton2 = 0x01000000,
    }
}