namespace NetEx.Hooks
{
    /// <summary>
    /// Represents the method that will handle the <see cref="MouseHook.MouseDown" />, <see cref="MouseHook.MouseUp" />, or <see cref="MouseHook.MouseMove" /> event of a <see cref="MouseHook"/>.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    /// <remarks>This code was taken from the .NET Framework.</remarks>
    /// <seealso href="https://github.com/dotnet/winforms/blob/release/9.0/src/System.Windows.Forms/src/System/Windows/Forms/Input/MouseEventHandler.cs"/>
    public delegate void MouseEventHandler(MouseEventArgs e);
}