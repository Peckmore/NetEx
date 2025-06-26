<div align="center">

<img src="resources/images/icon.png" alt="NetEx.WinForms.ProgressDialog" width="75" />

# NetEx

This repo is a collection of .NET controls and libraries that people might (hopefully!) find useful. :relaxed:

</div>

## 📦 Projects

This repo contains the following projects:

### NetEx.Dialogs.WinForms
[![NetEx.Dialogs.WinForms)](https://img.shields.io/nuget/v/NetEx.Dialogs.WinForms.svg)](https://www.nuget.org/packages/NetEx.Dialogs.WinForms/) [![.NET](https://img.shields.io/badge/.net%20-5.0+-8A2BE2)](https://dotnet.microsoft.com/download) [![.NET Framework](https://img.shields.io/badge/.net%20framework-2.0+-8A2BE2)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

A managed implementation of the standard Windows credential and progress dialogs, for WinForms.

#### [CredentialDialog](dialogs.winforms/credentialdialog.md)
Displays a standard dialog box that prompts the user to enter credentials.

![Credential Dialog)](resources/images/credential-dialog-both.png)

#### [ProgressDialog](dialogs.winforms/progressdialog.md)
Displays a standard dialog box that informs the user of the progress of an action.

![Progress Dialog)](resources/images/progress-dialog-new.png)

### NetEx.Hooks
[![NetEx.Hooks)](https://img.shields.io/nuget/v/NetEx.Hooks.svg)](https://www.nuget.org/packages/NetEx.Hooks/) [![.NET](https://img.shields.io/badge/.net%20-5.0+-8A2BE2)](https://dotnet.microsoft.com/download) [![.NET Framework](https://img.shields.io/badge/.net%20framework-2.0+-8A2BE2)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

Provides global hooks for capturing keyboard, mouse, and clipboard events, and simulators for keyboard and mouse events.

#### [ClipboardHook](hooks/clipboardhook.md)
Provides a mechanism for hooking all clipboard events within the operating system.

#### [KeyboardHook](hooks/keyboardhook.md)
Provides a mechanism for hooking all keyboard events within the operating system.
#### [KeyboardSimulator](hooks/keyboardsimulator.md)
A keyboard event simulator, which can simulate `KeyDown`, `KeyUp`, and `KeyPress` events.
#### [MouseHook](hooks/mousehook.md)
Provides a mechanism for hooking all mouse events within the operating system.

#### [MouseSimulator](hooks/mousesimulator.md)
A mouse event simulator, which can simulate `MouseClick`, `MouseDoubleClick`, `MouseDown`, `MouseUp`, `MouseMove`, and `MouseWheel` events.

### NetEx.IO
[![NetEx.IO)](https://img.shields.io/nuget/v/NetEx.IO.svg)](https://www.nuget.org/packages/NetEx.IO/) [![.NET](https://img.shields.io/badge/.net%20-5.0+-8A2BE2)](https://dotnet.microsoft.com/download) [![.NET Framework](https://img.shields.io/badge/.net%20framework-2.0+-8A2BE2)](https://dotnet.microsoft.com/en-us/download/dotnet-framework) [![.NET Standard](https://img.shields.io/badge/.net%20standard-2.0+-8A2BE2)](https://dotnet.microsoft.com/en-us/platform/dotnet-standard)

Provides additional input and output (I/O) types, that allow reading and/or writing to data streams.

#### [MultiStream](io/multistream.md)
Creates a wrapper around multiple `Stream` instances, and presents them as a single, read-only stream.
#### [OnDisposeStream](io/ondisposestream.md)
Creates a wrapper around a `Stream` that can be used to perform additional cleanup when the underlying stream is disposed.

## 🙌 Usage

To use, simply install the required package from NuGet:

```powershell
# NetEx.Dialogs.WinForms
Install-Package NetEx.Dialogs.WinForms

# NetEx.Hooks
Install-Package NetEx.Hooks

# NetEx.IO
Install-Package NetEx.IO
```

## 📖 Documentation

Documentation and example code is available [here](https://peckmore.github.io/NetEx/overview/overview.html).

Full API documentation can be found [here](https://peckmore.github.io/NetEx/api/NetEx.Dialogs.WinForms.html).

## 🚀 Releases

A full list of all releases is available on the [Releases](https://github.com/Peckmore/NetEx/releases) tab on GitHub.

## 🔢 Versioning

**NetEx** use [Semantic Versioning](https://semver.org) for all packages.

## 📄 License

The code is licensed under the [MIT license](https://github.com/Peckmore/NetEx?tab=readme-ov-file#MIT-1-ov-file).