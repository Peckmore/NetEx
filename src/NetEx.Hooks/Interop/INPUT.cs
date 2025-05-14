namespace NetEx.Hooks.Interop
{
    /// <summary>
    /// Used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input"/>
    internal struct INPUT
    {
        #region Fields

        /// <summary>
        /// The type of the input event.
        /// </summary>
        public int type;
        /// <summary>
        /// A union of the information about a simulated event.
        /// </summary>
        public INPUTUNION input;

        #endregion

        #region Construction

        public INPUT(int inputType)
        {
            type = inputType;
            input = new();
        }

        #endregion
    }
}