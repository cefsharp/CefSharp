[![CefSharp Logo](logo.png)](http://cefsharp.github.io/ "CefSharp - Embedded Chromium for .NET")

![Build Status](http://img.shields.io/appveyor/ci/cefsharp/cefsharp.svg)
[![CefSharp.WinForms](http://img.shields.io/nuget/v/CefSharp.WinForms.svg?style=flat)](http://www.nuget.org/packages/CefSharp.WinForms/)
[![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat)](http://www.nuget.org/packages/CefSharp.Wpf/)
[![CefSharp.OffScreen](http://img.shields.io/nuget/v/CefSharp.OffScreen.svg?style=flat)](http://www.nuget.org/packages/CefSharp.OffScreen/)

Got a quick question? Jump on [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cefsharp/CefSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[CefSharp](http://cefsharp.github.io/) lets you embed Chromium in .NET apps. It is a lightweight .NET wrapper around the [Chromium Embedded Framework (CEF)](https://bitbucket.org/chromiumembedded/cef) by Marshall A. Greenblatt. About 30% of the bindings are written in C++/CLI with the majority of code here is C#. It can be used from C# or VB, or any other CLR language. CefSharp provides both WPF and WinForms web browser control implementations.

CefSharp is [BSD](http://opensource.org/licenses/BSD-3-Clause "BSD License") licensed, so it can be used in both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

## Releases

Stable binaries are released on NuGet, and contain everything you need  to embed Chromium in your .Net/CLR application. For usage see the [Quick Start](https://github.com/cefsharp/CefSharp/wiki/Quick-Start) guide or [FAQ #8](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#CefSharp_binaries).

- [CefSharp.WinForms](http://www.nuget.org/packages/CefSharp.WinForms/)
- [CefSharp.Wpf](http://www.nuget.org/packages/CefSharp.Wpf/)
- [CefSharp.OffScreen](http://www.nuget.org/packages/CefSharp.OffScreen/)

## Documentation

* See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built with CefSharp. They demo most of the available features.
* See the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) project for a basic demo of using the CefSharp NuGet packages.
* See the [Wiki](https://github.com/cefsharp/CefSharp/wiki) for work-in-progress documentation
* See the [FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions) for help with common issues 
* Upgrading from an earlier version of CefSharp? See the [ChangeLog](https://github.com/cefsharp/CefSharp/wiki/ChangeLog) for breaking changes and upgrade tips.

## Contact

If you have a simple question please start by asking it on [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cefsharp/CefSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge). Please keep the `Issue Tracker` for **Bugs** only please! Before submitting a `PR` please read [CONTRIBUTING](https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md)

[Stackoverflow](http://stackoverflow.com/questions/tagged/cefsharp) as always is a useful resource, you can post your more complex issues here. The [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp) is **no longer active**. It is however a useful resource for archived questions/answers.

## Branches & Forks

This is the `official` CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

Development is done in the `master` branch. New features are preferably added in feature branches, if the changes are more than trivial. New `PR's` should be targeted against `master`.

When a new release is imminent a `release` branch is created. We try to avoid making public facing `API` changes in `release` branches (Adding new features is fine, just not breaking changes).

### Build Status

**CI Builds**<br/>
Every commit on `master` produces a `Nuget` package. Use at your own risk! [CefSharp MyGet Feed](https://www.myget.org/gallery/cefsharp)

**Pre-release**<br>
![CefSharp.Wpf](http://img.shields.io/nuget/vpre/CefSharp.Wpf.svg?style=flat)

**Stable**<br> 
![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat) for either  [WPF](http://www.nuget.org/packages/CefSharp.Wpf/) or 
[WinForms](http://www.nuget.org/packages/CefSharp.WinForms/) or 
[OffScreen](http://www.nuget.org/packages/CefSharp.OffScreen/).

### Release Branches

| Branch | CEF Version | VC++ Version | .Net Version | Status |
|--------|-------------|--------------|--------------|--------|
| [master](https://github.com/cefsharp/CefSharp/) | 2785 | 2013 | 4.5.2 | Development |
| [cefsharp/53](https://github.com/cefsharp/CefSharp/tree/cefsharp/53) | 2785 | 2013 | 4.5.2 | **Release** |
| [cefsharp/51](https://github.com/cefsharp/CefSharp/tree/cefsharp/51) | 2704 | 2013 | 4.5.2 | Unsupported |
| [cefsharp/49](https://github.com/cefsharp/CefSharp/tree/cefsharp/49) | 2623 | 2013 | 4.0   | Unsupported |
| [cefsharp/47](https://github.com/cefsharp/CefSharp/tree/cefsharp/47) | 2526 | 2013 | 4.0   | Unsupported |
| [cefsharp/45](https://github.com/cefsharp/CefSharp/tree/cefsharp/45) | 2454 | 2013 | 4.0   | Unsupported |
| [cefsharp/43](https://github.com/cefsharp/CefSharp/tree/cefsharp/43) | 2357 | 2012 | 4.0   | Unsupported |
| [cefsharp/41](https://github.com/cefsharp/CefSharp/tree/cefsharp/41) | 2272 | 2012 | 4.0   | Unsupported |
| [cefsharp/39](https://github.com/cefsharp/CefSharp/tree/cefsharp/39) | 2171 | 2012 | 4.0   | Unsupported |
| [cefsharp/37](https://github.com/cefsharp/CefSharp/tree/cefsharp/37) | 2062 | 2013 | 4.0   | Unsupported |

## Links

- [CefGlue](https://bitbucket.org/xilium/xilium.cefglue/): An alternative .NET CEF wrapper built using P/Invoke.
- [ChromiumFx](https://bitbucket.org/chromiumfx/chromiumfx) : Another P/Invoke .Net CEF wrapper
- [CEF Forum](http://magpcss.org/ceforum/) : The official CEF Forum
- [CEF API Docs] (http://magpcss.org/ceforum/apidocs3/index-all.html) : Well worth a read if your implementing a new feature

