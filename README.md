[![CefSharp Logo](logo.png)](https://cefsharp.github.io/ "CefSharp - Embedded Chromium for .NET")

[![Build status](https://ci.appveyor.com/api/projects/status/9g4mcuqruc283g66/branch/master?svg=true)](https://ci.appveyor.com/project/cefsharp/cefsharp/branch/master)
[![CefSharp.WinForms](https://img.shields.io/nuget/v/CefSharp.WinForms.svg?style=flat&label=WinForms)](https://www.nuget.org/packages/CefSharp.WinForms/)
[![CefSharp.Wpf](https://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat&label=Wpf)](https://www.nuget.org/packages/CefSharp.Wpf/)
[![CefSharp.Wpf.HwndHost](https://img.shields.io/nuget/v/CefSharp.Wpf.HwndHost.svg?style=flat&label=Wpf.HwndHost)](https://www.nuget.org/packages/CefSharp.Wpf.HwndHost/)
[![CefSharp.OffScreen](https://img.shields.io/nuget/v/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](https://www.nuget.org/packages/CefSharp.OffScreen/)

Got a quick question? [Discussions](https://github.com/cefsharp/CefSharp/discussions) here on `GitHub` is the preferred place to ask!

[CefSharp](https://cefsharp.github.io/) lets you embed Chromium in .NET apps. It is a lightweight .NET wrapper around the [Chromium Embedded Framework (CEF)](https://bitbucket.org/chromiumembedded/cef) by Marshall A. Greenblatt. About 30% of the bindings are written in C++/CLI with the majority of code here is C#. It can be used from C# or VB, or any other CLR language. CefSharp provides both WPF and WinForms web browser control implementations.

CefSharp is [BSD](https://opensource.org/licenses/BSD-3-Clause "BSD License") licensed, so it can be used in both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file. 

If you like and use CefSharp please consider signing up for a small monthly donation, even $25 can help tremendously. See [Financial Support](README.md#Financial-Support) for more details.

## Releases

Stable binaries are released on NuGet, and contain everything you need to embed Chromium in your .Net/CLR application. For usage see the [Quick Start](https://github.com/cefsharp/CefSharp/wiki/Quick-Start) guide or checkout [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) project for basic demos using the CefSharp NuGet packages.

- [CefSharp.WinForms](https://www.nuget.org/packages/CefSharp.WinForms/)
- [CefSharp.Wpf](https://www.nuget.org/packages/CefSharp.Wpf/)
- [CefSharp.OffScreen](https://www.nuget.org/packages/CefSharp.OffScreen/)
- [CefSharp.Wpf.HwndHost](https://github.com/cefsharp/CefSharp.Wpf.HwndHost/) (A [HwndHost](https://docs.microsoft.com/en-us/dotnet/api/system.windows.interop.hwndhost) based WPF implementation, similar to hosting the WinForms version in WPF, supports data binding, airspace issues apply).

## Documentation

* See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built with CefSharp. They demo most of the available features.
* See the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) project for a basic demo of using the CefSharp NuGet packages.
* See the [General Usage Guide](https://github.com/cefsharp/CefSharp/wiki/General-Usage) in help getting started/dealing with common scenarios.
* See the [Wiki](https://github.com/cefsharp/CefSharp/wiki) for work-in-progress documentation
* See the [FAQ](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions) for help with common issues
* Upgrading from an earlier version of CefSharp? See the [ChangeLog](https://github.com/cefsharp/CefSharp/wiki/ChangeLog) for breaking changes and upgrade tips.
* [CefSharp API](https://cefsharp.github.io/api/) generated from the source code comments.

## Contact

Please keep the `Issue Tracker` for **Bugs** only please! Before submitting a `PR` please read [CONTRIBUTING](https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md).

- [CefSharp Discussions](https://github.com/cefsharp/CefSharp/discussions) is generally where `CefSharp` specific questions should be asked, please search before posting, thanks!
- [Stackoverflow](https://stackoverflow.com/questions/tagged/cefsharp) is where generic html/javascript/C# questions can be asked.
- [Chromium Embedded Framework(CEF) Forum](https://magpcss.org/ceforum/viewforum.php?f=18)

## Branches & Forks

This is the `official` CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

Development is done in the `master` branch. New features are preferably added in feature branches, if the changes are more than trivial. New `PR's` should be targeted against `master`.

When a new release is imminent a `release` branch is created. We try to avoid making public facing `API` changes in `release` branches (Adding new features is fine, just not breaking changes).

### Releases

**CI Builds**<br/>
Every commit on `master` produces a `Nuget` package. Use at your own risk!


- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.WinForms.svg?style=flat&label=WinForms)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.WinForms)
- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.Wpf.svg?style=flat&label=Wpf)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.Wpf)
- [![MyGet Pre Release](https://img.shields.io/myget/cefsharp/v/CefSharp.OffScreen.svg?style=flat&label=OffScreen)](https://www.myget.org/feed/cefsharp/package/nuget/CefSharp.OffScreen)

**Pre-release**<br>

- [![CefSharp.WinForms](http://img.shields.io/nuget/vpre/CefSharp.WinForms.svg?style=flat&label=CefSharp.WinForms)](http://www.nuget.org/packages/CefSharp.WinForms/)
- [![CefSharp.Wpf](http://img.shields.io/nuget/vpre/CefSharp.Wpf.svg?style=flat&label=CefSharp.Wpf)](http://www.nuget.org/packages/CefSharp.Wpf/)
- [![CefSharp.OffScreen](http://img.shields.io/nuget/vpre/CefSharp.OffScreen.svg?style=flat&label=CefSharp.OffScreen)](http://www.nuget.org/packages/CefSharp.OffScreen/)

- [![CefSharp.WinForms.NETCore](http://img.shields.io/nuget/vpre/CefSharp.WinForms.NETCore.svg?style=flat&label=CefSharp.WinForms.NETCore)](http://www.nuget.org/packages/CefSharp.WinForms.NETCore/)
- [![CefSharp.Wpf.NETCore](http://img.shields.io/nuget/vpre/CefSharp.Wpf.NETCore.svg?style=flat&label=CefSharp.Wpf.NETCore)](http://www.nuget.org/packages/CefSharp.Wpf.NETCore/)
- [![CefSharp.OffScreen.NETCore](http://img.shields.io/nuget/vpre/CefSharp.OffScreen.NETCore.svg?style=flat&label=CefSharp.OffScreen.NETCore)](http://www.nuget.org/packages/CefSharp.OffScreen.NETCore/)

**Stable**<br>
- [![CefSharp.WinForms](http://img.shields.io/nuget/v/CefSharp.WinForms.svg?style=flat&label=CefSharp.WinForms)](http://www.nuget.org/packages/CefSharp.WinForms/)
- [![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat&label=CefSharp.Wpf)](http://www.nuget.org/packages/CefSharp.Wpf/)
- [![CefSharp.OffScreen](http://img.shields.io/nuget/v/CefSharp.OffScreen.svg?style=flat&label=CefSharp.OffScreen)](http://www.nuget.org/packages/CefSharp.OffScreen/)

- [![CefSharp.WinForms.NETCore](http://img.shields.io/nuget/v/CefSharp.WinForms.NETCore.svg?style=flat&label=CefSharp.WinForms.NETCore)](http://www.nuget.org/packages/CefSharp.WinForms.NETCore/)
- [![CefSharp.Wpf.NETCore](http://img.shields.io/nuget/v/CefSharp.Wpf.NETCore.svg?style=flat&label=CefSharp.Wpf.NETCore)](http://www.nuget.org/packages/CefSharp.Wpf.NETCore/)
- [![CefSharp.OffScreen.NETCore](http://img.shields.io/nuget/v/CefSharp.OffScreen.NETCore.svg?style=flat&label=CefSharp.OffScreen.NETCore)](http://www.nuget.org/packages/CefSharp.OffScreen.NETCore/)

### Release Branches

With each release a new branch is created, for example the `92.0.260` release corresponds to the [cefsharp/92](https://github.com/cefsharp/CefSharp/tree/cefsharp/92) branch.
If you're new to `CefSharp` and are downloading the source to check it out, please use a **Release** branch.

**&ast;** VC++ 2022 is required starting with version 138<br/>
**&ast;&ast;** For NetCore packages .Net 6 or greater is required.

| Branch                                                                | CEF Version  | VC++ Version | .Net Version | Status |
|-----------------------------------------------------------------------|------|-------|---------|-----------------|
| [master](https://github.com/cefsharp/CefSharp/)                       | 7258 | 2022* | 4.6.2** | Development     |
| [cefsharp/139](https://github.com/cefsharp/CefSharp/tree/cefsharp/139)| 7258 | 2022* | 4.6.2** | **Release**     |
| [cefsharp/138](https://github.com/cefsharp/CefSharp/tree/cefsharp/138)| 7204 | 2022* | 4.6.2** | Unsupported     |
| [cefsharp/137](https://github.com/cefsharp/CefSharp/tree/cefsharp/137)| 7151 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/136](https://github.com/cefsharp/CefSharp/tree/cefsharp/136)| 7103 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/135](https://github.com/cefsharp/CefSharp/tree/cefsharp/135)| 7049 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/134](https://github.com/cefsharp/CefSharp/tree/cefsharp/134)| 6998 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/133](https://github.com/cefsharp/CefSharp/tree/cefsharp/133)| 6943 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/132](https://github.com/cefsharp/CefSharp/tree/cefsharp/132)| 6834 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/131](https://github.com/cefsharp/CefSharp/tree/cefsharp/131)| 6778 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/130](https://github.com/cefsharp/CefSharp/tree/cefsharp/130)| 6723 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/129](https://github.com/cefsharp/CefSharp/tree/cefsharp/129)| 6668 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/128](https://github.com/cefsharp/CefSharp/tree/cefsharp/128)| 6613 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/127](https://github.com/cefsharp/CefSharp/tree/cefsharp/127)| 6533 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/126](https://github.com/cefsharp/CefSharp/tree/cefsharp/126)| 6478 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/125](https://github.com/cefsharp/CefSharp/tree/cefsharp/125)| 6422 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/124](https://github.com/cefsharp/CefSharp/tree/cefsharp/124)| 6367 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/123](https://github.com/cefsharp/CefSharp/tree/cefsharp/123)| 6312 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/122](https://github.com/cefsharp/CefSharp/tree/cefsharp/122)| 6261 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/121](https://github.com/cefsharp/CefSharp/tree/cefsharp/121)| 6167 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/120](https://github.com/cefsharp/CefSharp/tree/cefsharp/120)| 6099 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/119](https://github.com/cefsharp/CefSharp/tree/cefsharp/119)| 6045 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/118](https://github.com/cefsharp/CefSharp/tree/cefsharp/118)| 5993 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/117](https://github.com/cefsharp/CefSharp/tree/cefsharp/117)| 5938 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/116](https://github.com/cefsharp/CefSharp/tree/cefsharp/116)| 5845 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/115](https://github.com/cefsharp/CefSharp/tree/cefsharp/115)| 5790 | 2019* | 4.6.2** | Unsupported     |
| [cefsharp/114](https://github.com/cefsharp/CefSharp/tree/cefsharp/114)| 5735 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/113](https://github.com/cefsharp/CefSharp/tree/cefsharp/113)| 5615 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/112](https://github.com/cefsharp/CefSharp/tree/cefsharp/112)| 5615 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/111](https://github.com/cefsharp/CefSharp/tree/cefsharp/111)| 5563 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/110](https://github.com/cefsharp/CefSharp/tree/cefsharp/110)| 5481 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/109](https://github.com/cefsharp/CefSharp/tree/cefsharp/109)| 5414 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/108](https://github.com/cefsharp/CefSharp/tree/cefsharp/108)| 5359 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/107](https://github.com/cefsharp/CefSharp/tree/cefsharp/107)| 5304 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/106](https://github.com/cefsharp/CefSharp/tree/cefsharp/106)| 5249 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/105](https://github.com/cefsharp/CefSharp/tree/cefsharp/105)| 5195 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/104](https://github.com/cefsharp/CefSharp/tree/cefsharp/104)| 5112 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/103](https://github.com/cefsharp/CefSharp/tree/cefsharp/103)| 5060 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/102](https://github.com/cefsharp/CefSharp/tree/cefsharp/102)| 5005 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/101](https://github.com/cefsharp/CefSharp/tree/cefsharp/101)| 4951 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/100](https://github.com/cefsharp/CefSharp/tree/cefsharp/100)| 4896 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/99](https://github.com/cefsharp/CefSharp/tree/cefsharp/99)  | 4844 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/98](https://github.com/cefsharp/CefSharp/tree/cefsharp/98)  | 4758 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/97](https://github.com/cefsharp/CefSharp/tree/cefsharp/97)  | 4692 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/96](https://github.com/cefsharp/CefSharp/tree/cefsharp/96)  | 4664 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/95](https://github.com/cefsharp/CefSharp/tree/cefsharp/95)  | 4638 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/94](https://github.com/cefsharp/CefSharp/tree/cefsharp/94)  | 4606 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/93](https://github.com/cefsharp/CefSharp/tree/cefsharp/93)  | 4577 | 2019* | 4.5.2** | Unsupported     |
| [cefsharp/92](https://github.com/cefsharp/CefSharp/tree/cefsharp/92)  | 4515 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/91](https://github.com/cefsharp/CefSharp/tree/cefsharp/91)  | 4472 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/90](https://github.com/cefsharp/CefSharp/tree/cefsharp/90)  | 4430 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/89](https://github.com/cefsharp/CefSharp/tree/cefsharp/89)  | 4389 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/88](https://github.com/cefsharp/CefSharp/tree/cefsharp/88)  | 4324 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/87](https://github.com/cefsharp/CefSharp/tree/cefsharp/87)  | 4280 | 2015* | 4.5.2** | Unsupported     |
| [cefsharp/86](https://github.com/cefsharp/CefSharp/tree/cefsharp/86)  | 4240 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/85](https://github.com/cefsharp/CefSharp/tree/cefsharp/85)  | 4183 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/84](https://github.com/cefsharp/CefSharp/tree/cefsharp/84)  | 4147 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/83](https://github.com/cefsharp/CefSharp/tree/cefsharp/83)  | 4103 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/81](https://github.com/cefsharp/CefSharp/tree/cefsharp/81)  | 4044 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/79](https://github.com/cefsharp/CefSharp/tree/cefsharp/79)  | 3945 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/77](https://github.com/cefsharp/CefSharp/tree/cefsharp/77)  | 3865 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/75](https://github.com/cefsharp/CefSharp/tree/cefsharp/75)  | 3770 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/73](https://github.com/cefsharp/CefSharp/tree/cefsharp/73)  | 3683 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/71](https://github.com/cefsharp/CefSharp/tree/cefsharp/71)  | 3578 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/69](https://github.com/cefsharp/CefSharp/tree/cefsharp/69)  | 3497 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/67](https://github.com/cefsharp/CefSharp/tree/cefsharp/67)  | 3396 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/65](https://github.com/cefsharp/CefSharp/tree/cefsharp/65)  | 3325 | 2015  | 4.5.2   | Unsupported     |
| [cefsharp/63](https://github.com/cefsharp/CefSharp/tree/cefsharp/63)  | 3239 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/62](https://github.com/cefsharp/CefSharp/tree/cefsharp/62)  | 3202 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/57](https://github.com/cefsharp/CefSharp/tree/cefsharp/57)  | 2987 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/55](https://github.com/cefsharp/CefSharp/tree/cefsharp/55)  | 2883 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/53](https://github.com/cefsharp/CefSharp/tree/cefsharp/53)  | 2785 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/51](https://github.com/cefsharp/CefSharp/tree/cefsharp/51)  | 2704 | 2013  | 4.5.2   | Unsupported     |
| [cefsharp/49](https://github.com/cefsharp/CefSharp/tree/cefsharp/49)  | 2623 | 2013  | 4.0     | Unsupported     |
| [cefsharp/47](https://github.com/cefsharp/CefSharp/tree/cefsharp/47)  | 2526 | 2013  | 4.0     | Unsupported     |
| [cefsharp/45](https://github.com/cefsharp/CefSharp/tree/cefsharp/45)  | 2454 | 2013  | 4.0     | Unsupported     |
| [cefsharp/43](https://github.com/cefsharp/CefSharp/tree/cefsharp/43)  | 2357 | 2012  | 4.0     | Unsupported     |
| [cefsharp/41](https://github.com/cefsharp/CefSharp/tree/cefsharp/41)  | 2272 | 2012  | 4.0     | Unsupported     |
| [cefsharp/39](https://github.com/cefsharp/CefSharp/tree/cefsharp/39)  | 2171 | 2012  | 4.0     | Unsupported     |
| [cefsharp/37](https://github.com/cefsharp/CefSharp/tree/cefsharp/37)  | 2062 | 2012  | 4.0     | Unsupported     |

**&ast;** VC++ 2022 is required starting with version 138<br/>
**&ast;&ast;** For NetCore packages .Net 6 or greater is required.

## Financial Support

Is your company making money thanks to `CefSharp`? Do you rely on regular updates to the project? [Alex Maitland](https://github.com/amaitland) needs your support! Signup to [GitHub Sponsors](https://github.com/sponsors/amaitland).

One-Time or Recurring contributions can be made through [GitHub Sponsors](https://github.com/sponsors/amaitland) it only takes a GitHub account and a credit card.  You can also make a One-Time contribution via [PayPal](https://paypal.me/AlexMaitland).

As a stay at home dad I ([@amaitland](https://github.com/amaitland)) rely on your contributions to help support my family.

## Links

- [CefGlue](https://gitlab.com/xiliumhq/chromiumembedded/cefglue): An alternative .NET CEF wrapper built using P/Invoke.
- [CEF GitHub Project](https://github.com/chromiumembedded/cef) : The official CEF issue tracker
- [CEF Forum](http://magpcss.org/ceforum/) : The official CEF Forum
- [CEF API Docs](http://magpcss.org/ceforum/apidocs3/index-all.html) : Well worth a read if you are implementing a new feature
- [CefSharp API Doc](http://cefsharp.github.io/api/)

## Projects using CefSharp
- [HtmlView](https://github.com/ramon-mendes/HtmlView) : Visual Studio extension bringing CefSharp for showing HTML pages inside VS.
- [SharpBrowser](https://github.com/sharpbrowser/SharpBrowser) : The fastest web browser for C# with tabbed browsing and HTML5/CSS3.
- [Chromely CefSharp](https://github.com/chromelyapps/CefSharp) : Build HTML Desktop Apps on .NET/.NET Core 3/.NET 5 using native GUI, HTML5/CSS.
