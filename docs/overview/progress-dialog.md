# Progress Dialog

A managed implementation of the standard Windows progress dialog, for WinForms.

## Usage

Install the package from NuGet:

```powershell
Install-Package NetEx.WinForms.ProgressDialog
```

## Overview

This project implements a standard Windows progress dialog using the WinForms `CommonDialog` as its base. The project aims to match standard .Net Framework and WinForms behaviour as closely as possible.

![A progress dialog with upgraded appearance.](../images/progress-dialog-new.png)

`ProgressDialog` supports the standard `ShowDialog()` method of invocation, as is standard across all dialogs using `CommonDialog`. However, because the dialog is typically expected to be shown in a non-modal fashion, it is recommended to use the `Show()` method instead.

`ProgressDialog` is supported on all versions of Windows starting with *Windows XP*.

*Windows Vista* introduced new visual styling for many controls and UI elements, including the Windows progress dialog. As a result, the appearance of the dialog is different between *Windows XP* and later Windows versions.

*Windows 7* introduced the ability to display progress indicators on a taskbar icon. The Windows progress dialog, and subsequently this `ProgressDialog` implementation, will therefore automatically display progress notification in the host application's taskbar icon on *Windows 7* or later.

## Applies to

| Product                   | Versions              |
|---------------------------|-----------------------|
| **.Net**                  | 5.0+ *(Windows only)* |
| **.Net Framework**        | 2.0 to 4.8.1          |