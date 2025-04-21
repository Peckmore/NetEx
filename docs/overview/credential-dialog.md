# Credential Dialog

A managed implementation of the standard Windows credential dialog, for WinForms.

## Usage

Install the package from NuGet:

```powershell
Install-Package NetEx.WinForms.CredentialDialog
```

## Overview

This project implements a standard Windows credential dialog using the WinForms `CommonDialog` as its base. The project aims to match standard .Net Framework and WinForms behaviour as closely as possible.

![A credential dialog with upgraded appearance.](../images/credential-dialog-new.png)

![A credential dialog with classic appearance.](../images/credential-dialog-old.png)

`CredentialDialog` supports the standard `ShowDialog()` method of invocation, as is standard across all dialogs using `CommonDialog`.

`ProgressDialog` is supported on all versions of Windows starting with *Windows XP*.

*Windows Vista* introduced new visual styling for many controls and UI elements, including the Windows progress dialog. As a result, the appearance of the dialog is different between *Windows XP* and later Windows versions.

## Applies to

| Product                   | Versions              |
|---------------------------|-----------------------|
| **.Net**                  | 5.0+ *(Windows only)* |
| **.Net Framework**        | 2.0 to 4.8.1          |