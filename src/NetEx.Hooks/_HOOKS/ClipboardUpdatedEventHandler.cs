using System;
using System.Diagnostics.CodeAnalysis;

namespace NetEx.Windows.Forms
{
    //// <summary>Represents the method that will handle the <see cref="E:System.RemoteAccess.Mdc.MdcConnection.ClipboardUpdated"/> event of an <see cref="T:System.RemoteAccess.Mdc.MdcConnection"/>.</summary>
    [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
    public delegate void ClipboardUpdatedEventHandler(object sender, ClipboardUpdatedEventArgs e);
}