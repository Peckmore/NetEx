namespace NetEx.Hooks
{
    /// <summary>
    /// Represents the method that will handle the <see cref="KeyboardHook.KeyUp" /> or <see cref="KeyboardHook.KeyDown" /> event of <see cref="KeyboardHook" />.
    /// </summary>
    /// <param name="e">A <see cref="KeyEventArgs" /> that contains the event data.</param>
    /// <remarks>This code was taken from the .NET Framework.</remarks>
    /// <seealso href="https://github.com/dotnet/winforms/blob/release/9.0/src/System.Windows.Forms/src/System/Windows/Forms/Input/KeyEventHandler.cs"/>
    public delegate void KeyEventHandler(KeyEventArgs e);
}