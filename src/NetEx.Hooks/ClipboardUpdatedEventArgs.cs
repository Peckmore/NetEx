using System;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides data for the <see cref="ClipboardHook.ClipboardUpdated" /> event.
    /// </summary>
    public sealed class ClipboardUpdatedEventArgs : EventArgs
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardUpdatedEventArgs"/> class.
        /// </summary>
        public ClipboardUpdatedEventArgs()
        { }

        #endregion
    }
}