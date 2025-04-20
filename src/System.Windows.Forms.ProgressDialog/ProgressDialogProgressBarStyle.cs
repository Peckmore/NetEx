using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms
{
    /// <summary>
    /// Specifies the style of progress bar that a <see cref="ProgressDialog"/> uses to indicate the progress of an operation.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum ProgressDialogProgressBarStyle
    {
        /// <summary>
        /// Indicates progress by increasing the size of a smooth, continuous bar in a <see cref="ProgressDialog"/>.
        /// </summary>
        Continuous,
        /// <summary>
        /// Indicates progress by continuously scrolling a block across a <see cref="ProgressDialog"/> in a marquee fashion.
        /// </summary>
        Marquee
    }
}