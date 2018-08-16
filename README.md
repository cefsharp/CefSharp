[![CefSharp Logo](logo.png)](http://cefsharp.github.io/ "CefSharp - Embedded Chromium for .NET")

[![Build status](https://ci.appveyor.com/api/projects/status/9g4mcuqruc283g66/branch/master?svg=true)](https://ci.appveyor.com/project/cefsharp/cefsharp/branch/master)
[![CefSharp.WinForms](http://img.shields.io/nuget/v/CefSharp.WinForms.svg?style=flat&label=WinForms)](http://www.nuget.org/packages/CefSharp.WinForms/)
[![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat&label=Wpf)](http://www.nuget.org/packages/CefSharp.Wpf/)
[![CefSharp.OffScreen](http://img.shields.io/nuget/v/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](http://www.nuget.org/packages/CefSharp.OffScreen/)

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
* See the [General Usage Guide](https://github.com/cefsharp/CefSharp/wiki/General-Usage) in help getting started/dealing with common scenarios.
* See the [Wiki](https://github.com/cefsharp/CefSharp/wiki) for work-in-progress documentation
* See the [FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions) for help with common issues
* Upgrading from an earlier version of CefSharp? See the [ChangeLog](https://github.com/cefsharp/CefSharp/wiki/ChangeLog) for breaking changes and upgrade tips.
* [CefSharp API](http://cefsharp.github.io/api/55.0.0/) generated from the source code comments.

## Contact

If you have a simple question please start by asking it on [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cefsharp/CefSharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge). Please keep the `Issue Tracker` for **Bugs** only please! Before submitting a `PR` please read [CONTRIBUTING](https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md)

[Stackoverflow](http://stackoverflow.com/questions/tagged/cefsharp) as always is a useful resource, you can post your more complex issues here. The [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp) is **no longer active**. It is however a useful resource for archived questions/answers.

## Branches & Forks

This is the `official` CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/search?utf8=%E2%9C%93&q=cefsharp+fork%3Atrue&type=Repositories&ref=searchresults).

*Note* Due to so many Forks - Github can't process them through the `Network Graphs` Section so, if you need to do a search use the following: `cefsharp fork:true` and it will be able to process all of the forks.

Development is done in the `master` branch. New features are preferably added in feature branches, if the changes are more than trivial. New `PR's` should be targeted against `master`.

When a new release is imminent a `release` branch is created. We try to avoid making public facing `API` changes in `release` branches (Adding new features is fine, just not breaking changes).

### Releases

**CI Builds**<br/>
Every commit on `master` produces a `Nuget` package. Use at your own risk!


- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.WinForms.svg?style=flat&label=WinForms)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.WinForms)
- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.Wpf.svg?style=flat&label=Wpf)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.Wpf)
- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.OffScreen)

**Pre-release**<br>

- [![CefSharp.WinForms](http://img.shields.io/nuget/vpre/CefSharp.WinForms.svg?style=flat&label=WinForms)](http://www.nuget.org/packages/CefSharp.WinForms/)
- [![CefSharp.Wpf](http://img.shields.io/nuget/vpre/CefSharp.Wpf.svg?style=flat&label=Wpf)](http://www.nuget.org/packages/CefSharp.Wpf/)
- [![CefSharp.OffScreen](http://img.shields.io/nuget/vpre/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](http://www.nuget.org/packages/CefSharp.OffScreen/)

**Stable**<br>
- [![CefSharp.WinForms](http://img.shields.io/nuget/v/CefSharp.WinForms.svg?style=flat&label=WinForms)](http://www.nuget.org/packages/CefSharp.WinForms/)
- [![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat&label=Wpf)](http://www.nuget.org/packages/CefSharp.Wpf/)
- [![CefSharp.OffScreen](http://img.shields.io/nuget/v/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](http://www.nuget.org/packages/CefSharp.OffScreen/)

### Release Branches

With each release a new branch is created, for example the `53.0.1` release corresponds to the `cefsharp/53` branch.
If you're new to `CefSharp` and are downloading the source to check it out, please use a **Release** branch

| Branch                                                               | CEF Version | VC++ Version | .Net Version | Status |
|----------------------------------------------------------------------|------|------|-------|-----------------|
| [master](https://github.com/cefsharp/CefSharp/)                      | 3440 | 2015 | 4.5.2 | Development     |
| [cefsharp/67](https://github.com/cefsharp/CefSharp/tree/cefsharp/67) | 3396 | 2015 | 4.5.2 | **Pre-Release** |
| [cefsharp/65](https://github.com/cefsharp/CefSharp/tree/cefsharp/65) | 3325 | 2015 | 4.5.2 | **Release**     |
| [cefsharp/63](https://github.com/cefsharp/CefSharp/tree/cefsharp/63) | 3239 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/62](https://github.com/cefsharp/CefSharp/tree/cefsharp/62) | 3202 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/57](https://github.com/cefsharp/CefSharp/tree/cefsharp/57) | 2987 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/55](https://github.com/cefsharp/CefSharp/tree/cefsharp/55) | 2883 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/53](https://github.com/cefsharp/CefSharp/tree/cefsharp/53) | 2785 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/51](https://github.com/cefsharp/CefSharp/tree/cefsharp/51) | 2704 | 2013 | 4.5.2 | Unsupported     |
| [cefsharp/49](https://github.com/cefsharp/CefSharp/tree/cefsharp/49) | 2623 | 2013 | 4.0   | Unsupported     |
| [cefsharp/47](https://github.com/cefsharp/CefSharp/tree/cefsharp/47) | 2526 | 2013 | 4.0   | Unsupported     |
| [cefsharp/45](https://github.com/cefsharp/CefSharp/tree/cefsharp/45) | 2454 | 2013 | 4.0   | Unsupported     |
| [cefsharp/43](https://github.com/cefsharp/CefSharp/tree/cefsharp/43) | 2357 | 2012 | 4.0   | Unsupported     |
| [cefsharp/41](https://github.com/cefsharp/CefSharp/tree/cefsharp/41) | 2272 | 2012 | 4.0   | Unsupported     |
| [cefsharp/39](https://github.com/cefsharp/CefSharp/tree/cefsharp/39) | 2171 | 2012 | 4.0   | Unsupported     |
| [cefsharp/37](https://github.com/cefsharp/CefSharp/tree/cefsharp/37) | 2062 | 2012 | 4.0   | Unsupported     |


## Financial Support

To continue developing/supporting the project I (@amaitland) am asking for financial contributions. Donations of any size are greatly appreciated!

`67.0.0` Release: [![Bountysource](https://api.bountysource.com/badge/issue?issue_id=61407775)](https://www.bountysource.com/issues/61407775-funding-request-release-67-0-0?utm_source=61407775&utm_medium=shield&utm_campaign=ISSUE_BADGE)

Recurring contributions can be made through [BountySource Salt](https://salt.bountysource.com/) or contact me if you'd like to donate through `Paypal`.

Now that I (@amaitland) am a stay at home dad your contributions are the only reason I'm allowed to continue working on the project. Without continued funding the time I currently spend on the project will have to be put into finding other paid work.

## Links

- [CefGlue](https://bitbucket.org/xilium/xilium.cefglue/): An alternative .NET CEF wrapper built using P/Invoke.
- [ChromiumFx](https://bitbucket.org/chromiumfx/chromiumfx) : Another P/Invoke .Net CEF wrapper
- [CEF Bitbucket Project](https://bitbucket.org/chromiumembedded/cef/overview) : The official CEF issue tracker
- [CEF Forum](http://magpcss.org/ceforum/) : The official CEF Forum
- [CEF API Docs](http://magpcss.org/ceforum/apidocs3/index-all.html) : Well worth a read if you are implementing a new feature
- [CefSharp API Doc](http://cefsharp.github.io/api/)

## Projects using CefSharp

- [HtmlView](https://github.com/MISoftware/HtmlView) : Visual Studio extension bringing CefSharp for showing HTML pages inside VS.
- [Chromely](https://github.com/mattkol/Chromely) : Build .NET/.NET Core HTML5 desktop apps using cross-platform native GUI API.