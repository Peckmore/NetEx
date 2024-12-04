# NetEx
Extensions for the .Net Framework

The **NetEx** project contains (hopefully!) useful extensions to the .Net Framework. This repository is the starting point for the project, with links to the various repositories that constitute the **NetEx** project, and aims to serve as a quick overview of the functionality contained within the project.

## Overview
The aim is for each project to target a single set of functionality (e.g., a progress dialog). As a result, each project has it's own repository and compiles into it's own library and NuGet package.

All projects target the lowest framework version or standard possible in order to try and maximise compatibility. Please note however that projects will not all target *the same* framework version or standard, as some rely on functionality introduced in later versions.

## Projects
**NetEx** currently consists of the following projects:

* **[netex.credentialdialog](https://github.com/Peckmore/netex.credentialdialog)**  
A managed implementation of the standard Windows credential dialog.

* **[netex.progresssdialog](https://github.com/Peckmore/netex.progressdialog)**  
A managed implementation of the standard Windows progress dialog.

## Nuget

All projects are available on NuGet. The packages currently available are:

* [NetEx.CredentialDialog](https://www.nuget.org/packages/NetEx-CredentialDialog/)
* [NetEx.ProgressDialog](https://www.nuget.org/packages/NetEx-ProgressDialog/)

##  License

All code is licensed under the [MIT license](https://github.com/Peckmore/NetEx/blob/master/LICENSE).
