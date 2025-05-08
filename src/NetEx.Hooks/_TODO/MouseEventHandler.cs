namespace NetEx.Hooks
{
    /// <summary>Represents the method that will handle the <see langword="MouseDown" />, <see langword="MouseUp" />, or <see langword="MouseMove" /> event of a form, control, or other component.</summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    public delegate void MouseEventHandler(MouseEventArgs e);
}