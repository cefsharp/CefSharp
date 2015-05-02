# CefSharp - Embedded Chromium for .NET

This project contains .NET CLR bindings for The [Chromium Embedded Framework (CEF)](https://bitbucket.org/chromiumembedded/cef) by Marshall A. Greenblatt. A small [Core](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Core) of the bindings are written in C++/CLI but the majority of code here is C#. It can of course be used from any CLR language, e.g. C# or VB. 

CefSharp provides both WPF and WinForms web browser control implementations. See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or  [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built using this library; they are (at this moment) the best "documentation" of features. In addition see the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) repo for how CefSharp can actually be used via NuGet packages.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed, which means that it can be used from both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

# Documentation, Contact etc.

Apart from code samples mentioned above work-in-progress documentation can be found [in the wiki](https://github.com/cefsharp/CefSharp/wiki). If something is missing/incomplete, please don't hesitate to ask on the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp). You might also find help by searching the archive to that same Google Group for previous questions.

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

**Ultra stable/LTS** Read "He's dead Jim", based on Chromium 25. See the [CefSharp1](https://github.com/cefsharp/CefSharp/tree/CefSharp1#binary-release) branch README for CefSharp1 info. Please note that this version is no longer supported.

# Branches & Forks

![](http://img.shields.io/appveyor/ci/cefsharp/cefsharp.svg)

* Development is done in the `master` branch, which *used to be called* `CefSharp3` during the development cycle. New features are preferably added in feature branches, if the changes are more than trivial.
* Ultra-stable/LTS sources are in `CefSharp1`.
* [The CefSharp GitHub project page](https://github.com/cefsharp/CefSharp) is the recommended starting place. This is the "official" CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

# Links

- [CefGlue](https://bitbucket.org/xilium/xilium.cefglue/): An alternative .NET CEF wrapper built using P/Invoke.
