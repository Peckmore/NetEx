using System;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides data for the <see cref="KeyboardHook.KeyDown" /> or <see cref="KeyboardHook.KeyUp" /> event.
    /// </summary>
    /// <remarks>This code was taken from the .Net Framework.</remarks>
    /// <seealso href="https://github.com/dotnet/winforms/blob/release/9.0/src/System.Windows.Forms/src/System/Windows/Forms/Input/KeyEventArgs.cs"/>
    public class KeyEventArgs : EventArgs
    {
        #region Fields

        private bool _suppressKeyPress;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        public KeyEventArgs(Keys keyData)
        {
            KeyData = keyData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the ALT key was pressed.
        /// </summary>
        public virtual bool Alt => (KeyData & Keys.Alt) == Keys.Alt;
        /// <summary>
        /// Gets a value indicating whether the CTRL key was pressed.
        /// </summary>
        public bool Control => (KeyData & Keys.Control) == Keys.Control;
        /// <summary>
        /// Gets or sets a value indicating whether the event was handled.
        /// </summary>
        public bool Handled { get; set; }
        /// <summary>
        /// Gets the keyboard code for a <see cref="KeyboardHook.KeyDown"/> or <see cref="KeyboardHook.KeyUp"/> event.
        /// </summary>
        public Keys KeyCode
        {
            get
            {
                var keyGenerated = KeyData & Keys.KeyCode;
                if (!Enum.IsDefined(typeof(Keys), (int)keyGenerated))
                {
                    return Keys.None;
                }

                return keyGenerated;
            }
        }
        /// <summary>
        /// Gets the keyboard value for a <see cref="KeyboardHook.KeyDown"/> or <see cref="KeyboardHook.KeyUp"/> event.
        /// </summary>
        public int KeyValue => (int)(KeyData & Keys.KeyCode);
        /// <summary>
        /// Gets the key data for a <see cref="KeyboardHook.KeyDown"/> or <see cref="KeyboardHook.KeyUp"/> event.
        /// </summary>
        public Keys KeyData { get; }
        /// <summary>
        /// Gets the modifier flags for a <see cref="KeyboardHook.KeyDown"/> or <see cref="KeyboardHook.KeyUp"/> event. This indicates which modifier keys (CTRL, SHIFT, and/or ALT) were pressed.
        /// </summary>
        public Keys Modifiers => KeyData & Keys.Modifiers;
        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed.
        /// </summary>
        public virtual bool Shift => (KeyData & Keys.Shift) == Keys.Shift;
        /// <summary>
        /// Gets or sets a value indicating whether the key event should be passed on to the next listener.</summary>
        /// <returns><see langword="true" /> if the key event should not be sent to the control; otherwise, <see langword="false" />.</returns>
        public bool SuppressKeyPress
        {
            get => _suppressKeyPress;
            set
            {
                _suppressKeyPress = value;
                Handled = value;
            }
        }

        #endregion
    }
}