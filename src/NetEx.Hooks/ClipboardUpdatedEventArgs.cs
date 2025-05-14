using System;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides data for the <see cref="ClipboardHook.ClipboardUpdated" /> event.
    /// </summary>
    public class ClipboardUpdatedEventArgs : EventArgs
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardUpdatedEventArgs"/> class.
        /// </summary>
        public ClipboardUpdatedEventArgs(string[]? dataFormats = null)
        {
            DataFormats = dataFormats;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An array of <c>DataFormats</c> names representing the types of the data placed on the clipboard.
        /// </summary>
        public string[] DataFormats { get; }

        #endregion
    }
}