# Overview

<b>NetEx</b> is a collection of .Net controls and libraries I've written that I thought other people might also find useful.

The following topics give an overview of each library or control within the project, along with examples, and links to API documentation.

## NetEx.Dialogs.WinForms

Name | Description
-----|------------
[CredentialDialog](dialogs.winforms/credentialdialog.md) | Displays a standard dialog box that prompts the user to enter credentials.
[ProgressDialog](dialogs.winforms/progressdialog.md) | Displays a standard dialog box that informs the user of the progress of an action.

## NetEx.Hooks

Name | Description
-----|------------
[ClipboardHook](hooks/clipboardhook.md) | Provides a mechanism for hooking all clipboard events within the operating system.
[KeyboardHook](hooks/keyboardhook.md) | Provides a mechanism for hooking all keyboard events within the operating system.
[KeyboardSimulator](hooks/keyboardsimulator.md) | A keyboard event simulator, which can simulate `KeyDown`, `KeyUp`, and `KeyPress` events.
[MouseHook](hooks/mousehook.md) | Provides a mechanism for hooking all mouse events within the operating system.
[MouseSimulator](hooks/mousesimulator.md) | A mouse event simulator, which can simulate `MouseClick`, `MouseDoubleClick`, `MouseDown`, `MouseUp`, `MouseMove`, and `MouseWheel` events.

## NetEx.IO

Name | Description
-----|------------
[MultiStream](io/multistream.md) | Creates a wrapper around multiple `Stream` instances, and presents them as a single, read-only stream.
[OnDisposeStream](io/ondisposestream.md) | Creates a wrapper around a `Stream` that can be used to perform additional cleanup when the underlying stream is disposed.