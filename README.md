# CefSharp - Embedded Chromium for .NET

This project contains .NET CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt. A small [Core](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Core) of the bindings are written in C++/CLI but the majority of code here is C#. It can of course be used from any CLR language, e.g. C# or VB. 

CefSharp provides both WPF and WinForms web browser control implementations. See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or  [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built using this library; they are (at this moment) the best "documentation" of features. In addition see the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) repo for how CefSharp can actually be used via NuGet packages.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed, which means that it can be used from both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

# Documentation, Contact etc.

Apart from code samples mentioned above work-in-progress documentation can be found [in the wiki](https://github.com/cefsharp/CefSharp/wiki). If something is missing/incomplete, please don't hesitate to ask on [StackOverflow](http://stackoverflow.com/questions/tagged/cefsharp) or in the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp). You might also find help by searching the archive to that same Google Group for previous questions.

# NuGet Packages

Binary releases contain everything needed to embed Chromium in your CLR application. For usage see [FAQ #8](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#CefSharp_binaries) item.

**Pre-release**<br>
![CefSharp.Wpf](http://img.shields.io/nuget/vpre/CefSharp.Wpf.svg?style=flat)
   Has the final feature that a lot of people want, JS Binding ... reportedly some people already have it in productive use it and will never look back.

**Stable**<br> 
![CefSharp.Wpf](http://img.shields.io/nuget/v/CefSharp.Wpf.svg?style=flat) for either  [WPF](http://www.nuget.org/packages/CefSharp.Wpf/) or 
[WinForms](http://www.nuget.org/packages/CefSharp.WinForms/) If you want mature battle tested code, and don't *need* the JS Binding (yet) go for this one.

**Ultra stable/LTS** Read "He's (almost) dead Jim", based on Chromium 25. See the [CefSharp1](https://github.com/jornh/CefSharp/tree/CefSharp1#binary-release) branch README for CefSharp1 info. We even date back to supporting .NET 2. Please note that the latest Stable release  is the only version officially supported; it's very unlikely that we will fix bugs in older releases.

# Branches & Forks

![](http://img.shields.io/appveyor/ci/cefsharp/cefsharp.svg)

* Development is done in the `master` branch, which *used to be called* `CefSharp3` during the development cycle. New features are preferably added in feature branches, if the changes are more than trivial.
* Ultra-stable/LTS sources are in `CefSharp1`.
* [The CefSharp GitHub project page](https://github.com/cefsharp/CefSharp) is the recommended starting place. This is the "official" CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

# Links

- [CefGlue](https://bitbucket.org/fddima/cefglue/wiki/Home): An alternative .NET CEF wrapper built using P/Invoke.
