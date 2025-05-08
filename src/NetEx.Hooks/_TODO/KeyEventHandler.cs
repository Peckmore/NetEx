namespace NetEx.Hooks
{
    /// <summary>
    /// Represents the method that will handle the <see cref="KeyboardHook.KeyUp" /> or <see cref="KeyboardHook.KeyDown" /> event of <see cref="KeyboardHook" />.
    /// </summary>
    /// <param name="e">A <see cref="KeyEventArgs" /> that contains the event data.</param>
    public delegate void KeyEventHandler(KeyEventArgs e);
}