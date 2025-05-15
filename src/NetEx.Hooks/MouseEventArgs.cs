using System;
using System.Drawing;

namespace NetEx.Hooks
{
    /// <summary>
    ///  Provides data for the <see cref="MouseHook.MouseUp"/>, <see cref="MouseHook.MouseDown"/> and <see cref="MouseHook.MouseMove"/> events.
    /// </summary>
    /// <remarks>This code was taken from the .Net Framework.</remarks>
    /// <seealso href="https://github.com/dotnet/winforms/blob/release/9.0/src/System.Windows.Forms/src/System/Windows/Forms/Input/MouseEventArgs.cs"/>
    public sealed class MouseEventArgs : EventArgs
    {
        #region Construction

        /// <summary>
        ///  Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        internal MouseEventArgs(MouseButtons button, int clicks, Point location, int delta = 0)
            : this(button, clicks, location.X, location.Y, delta)
        { }
        /// <summary>
        ///  Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
        }

        #endregion

        #region Properties

        /// <summary>
        ///  Gets which mouse button was pressed.
        /// </summary>
        public MouseButtons Button { get; }
        /// <summary>
        ///  Gets the number of times the mouse button was pressed and released.
        /// </summary>
        public int Clicks { get; }
        /// <summary>
        ///  Gets a signed count of the number of detents the mouse wheel has rotated.
        /// </summary>
        public int Delta { get; }
        /// <summary>
        ///  Gets the location of the mouse during MouseEvent.
        /// </summary>
        public Point Location => new(X, Y);
        /// <summary>
        ///  Gets the x-coordinate of a mouse click.
        /// </summary>
        public int X { get; }
        /// <summary>
        ///  Gets the y-coordinate of a mouse click.
        /// </summary>
        public int Y { get; }

        #endregion
    }
}