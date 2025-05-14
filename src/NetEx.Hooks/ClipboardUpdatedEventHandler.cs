namespace NetEx.Hooks
{
    /// <summary>
    /// Represents the method that will handle the <see cref="ClipboardHook.ClipboardUpdated"/> event of <see cref="ClipboardHook"/>.
    /// </summary>
    /// <param name="e">A <see cref="ClipboardUpdatedEventArgs" /> that contains the event data.</param>
    public delegate void ClipboardUpdatedEventHandler(); //ClipboardUpdatedEventArgs e);
}