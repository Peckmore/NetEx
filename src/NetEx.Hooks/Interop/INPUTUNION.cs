using System.Runtime.InteropServices;

namespace NetEx.Hooks.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct INPUTUNION
    {
        #region Fields

        /// <summary>
        /// The information about a simulated mouse event.
        /// </summary>
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        /// <summary>
        /// The information about a simulated keyboard event.
        /// </summary>
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        /// <summary>
        /// The information about a simulated hardware event.
        /// </summary>
        [FieldOffset(0)]
        public HARDWAREINPUT hi;

        #endregion

        #region Construction

        public INPUTUNION()
        {
            mi = new();
            ki = new();
        }

        #endregion
    }
}