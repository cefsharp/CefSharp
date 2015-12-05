# CefSharp - Embedded Chromium for .NET

## [CefSharp Website](http://cefsharp.github.io/)
## [CefSharp Wiki](https://github.com/cefsharp/CefSharp/wiki)

This project contains .NET CLR bindings for The [Chromium Embedded Framework (CEF)](https://bitbucket.org/chromiumembedded/cef) by Marshall A. Greenblatt. A small [Core](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Core) of the bindings are written in C++/CLI but the majority of code here is C#. It can of course be used from any CLR language, e.g. C# or VB.

CefSharp provides both WPF and WinForms web browser control implementations. See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or  [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built using this library; they are (at this moment) the best "documentation" of features. In addition see the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) repo for how CefSharp can actually be used via NuGet packages.

This is the `official` CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed, which means that it can be used from both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

# Documentation, Contact etc.

Apart from code samples mentioned above work-in-progress documentation can be found [in the wiki](https://github.com/cefsharp/CefSharp/wiki). If something is missing/incomplete, please don't hesitate to ask on the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp). You might also find help by searching the archive to that same Google Group for previous questions. Before opening an issue or submitting a `PR` please read [Contributing.md](https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md)

[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cefsharp/CefSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# NuGet Packages

Binary releases contain everything needed to embed Chromium in your .Net/CLR application. For usage see [FAQ #8](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#CefSharp_binaries) item.

Upgrading from an earlier version of CefSharp?  See the [ChangeLog](https://github.com/cefsharp/CefSharp/wiki/ChangeLog) for breaking changes and upgrade tips.

**CI Builds**<br/>
Every commit on `master` produces a `Nuget` package. Use at your own risk! [CefSharp MyGet Feed](https://www.myget.org/F/cefsharp/)

**Pre-release**<br>
![CefSharp.Wpf](http://img.shields.io/nuget/vpre/CefSharp.Wpf.svg?style=flat)

**Stable**<br> 
![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat) for either  [WPF](http://www.nuget.org/packages/CefSharp.Wpf/) or 
[WinForms](http://www.nuget.org/packages/CefSharp.WinForms/) or 
[OffScreen](http://www.nuget.org/packages/CefSharp.OffScreen/) now **including** JS Binding.

# Branches & Forks

![](http://img.shields.io/appveyor/ci/cefsharp/cefsharp.svg)

* Development is done in the `master` branch. New features are preferably added in feature branches, if the changes are more than trivial.
* When a new release is imminent a `version` branch is created. We try to avoid making public facing `API` changes in release branches (Adding new features is fine, just not breaking changes).

### Version Branches

| Branch | CEF Version | VC++ Version | .Net Version | Status |
|--------|-------------|--------------|--------------|--------|
| [master](https://github.com/cefsharp/CefSharp/) | 2526 | 2013 | 4.0 | Development |
| [cefsharp/45](https://github.com/cefsharp/CefSharp/tree/cefsharp/45) | 2454 | 2013 | 4.0 | **Release** |
| [cefsharp/43](https://github.com/cefsharp/CefSharp/tree/cefsharp/43) | 2357 | 2012 | 4.0 | **Release** |
| [cefsharp/41](https://github.com/cefsharp/CefSharp/tree/cefsharp/41) | 2272 | 2012 | 4.0 | Unsupported |
| [cefsharp/39](https://github.com/cefsharp/CefSharp/tree/cefsharp/39) | 2171 | 2012 | 4.0 | Unsupported |
| [cefsharp/37](https://github.com/cefsharp/CefSharp/tree/cefsharp/37) | 2062 | 2012 | 4.0 | Unsupported |

#### CefSharp1

**Ultra stable/LTS** Read "He's dead Jim", based on Chromium 25. See the [CefSharp1](https://github.com/cefsharp/CefSharp/tree/CefSharp1#binary-release) branch README for CefSharp1 info. Please note that this version is no longer developed or supported.

# Links

- [CefGlue](https://bitbucket.org/xilium/xilium.cefglue/): An alternative .NET CEF wrapper built using P/Invoke.
