## [3.0.1] - 2025-08-21


### 🐛 Bug Fixes

- Correct package project url for all packages
- *(credentialdialog)* `CredentialDialog` crashes when shown on a system with Windows Hello #8



### ⚙️ Build

- Automate build process using conventional commits



### 📚 Documentation

- *(readme)* Update readme introduction
- Correct missing punctuation in package readme files

## [3.0.0] - 2025-06-27

A brand new release which features a complete overhaul of the `NetEx` packages.

- `CredentialDialog` and `ProgressDialog` have been merged into a single package (`NetEx.Dialogs.WinForms`)
- A new `NetEx.Hooks` package has been added with various mouse, keyboard, and clipboard hooks
- A new `NetEx.IO` package has been added with two `Stream` implementations
- Namespaces moved from `System` to `NetEx`
- Much larger framework support, with everything from .Net 2.0 -> .Net 9 support, and .Net Standard support (where applicable)
- NuGet package prefix has been updated from `NetEx-` -> `NetEx.`
- Older NuGet packages will now be deprecated

Breaking Change: Namespaces have been moved from `System` to `NetEx`, so any references will need updating.


## [2.0.2] - 2018-10-02

Large update to both `CredentialDialog` and `ProgressDialog`:

- Update code, comments XMLdoc and Attributes for consistency.
- Fix for deadlock which occurs if `Close()` is called in the event handler of the `Closed` or `Canceled` events.
- Remove 'Completed' event from `ProgressDialog` as the event did not make sense.
- Rename `CloseDialog()` -> `CloseComDialog()` to better describe what the method does.

Breaking Change: The `Completed` event has been removed from the `ProgressDialog` class. This event did not make sense as the dialog is always told what percentage completed the activity is, and therefore the calling application would always know whether the activity had completed. This also allows the dialog to function correctly when used in scenarios whereby the an activity is being performed which carries out several tasks, but shows the progress for each task individually.


## [1.1.1] - 2018-09-19

Update to add the `CredentialDialog` library.


## [1.0.13] - 2018-09-04

Initial release of the `ProgressDialog` library.


