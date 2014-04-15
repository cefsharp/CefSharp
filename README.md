# CefSharp - Embedded Chromium for .NET

This project contains .NET CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt. A small [Core](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Core) of the bindings are written in C++/CLI but the majority of code here is C#. It can of course be used from any CLR language, e.g. C# or VB. 

CefSharp provides both WPF and WinForms web browser control implementations. See the [CefSharp.Wpf.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.Wpf.Example) or  [CefSharp.WinForms.Example](https://github.com/cefsharp/CefSharp/tree/master/CefSharp.WinForms.Example) projects for example web browsers built using this library; they are (at this moment) the best "documentation" of features. In addition see the [CefSharp.MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample/) repo for how CefSharp can actually be used via NuGet packages.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed, which means that it can be used from both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

# Documentation & mailing list

Apart from code samples mentioned above work-in-progress documentation can be found [in the wiki](https://github.com/cefsharp/CefSharp/wiki). If something is missing/incomplete, please don't hesitate to ask at the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp). You might also find help by searching the archive to that same Google Group for previous questions.

# Binary Release

Binary releases contain everything needed to embed Chromium in your CLR application.

- **Pre-release** - CefSharp-3.29.0-pre.0 Currently only available as a WPF [NuGet](http://www.nuget.org/packages/CefSharp.Wpf/3.29.0-pre0). NuGet is CefSharp's new primary (only?) binary delivery mechanism. 
* **Stable** (.NET 4): CefSharp-1.25.7 - 
[binaries](https://github.com/cefsharp/CefSharp/releases/download/v1.25.7/CefSharp-v1.25.7-binaries.zip),
[source](https://github.com/cefsharp/CefSharp/archive/v1.25.7.zip), + [WPF](http://www.nuget.org/packages/CefSharp.Wpf/) or 
[WinForms](http://www.nuget.org/packages/CefSharp.WinForms/) NuGet's. Based on Chromium 25.0.1364.152
* **Legacy** (older release, works with .NET 2. Requires [7-Zip](http://www.7-zip.org/) to extract.): 
[CefSharp-1.25.0](http://sourceforge.net/projects/cefsharp/files/CefSharp-1.25.0.7z/download).
Based on Chromium 25.0.1364.152

See the [SF Downloads](https://sourceforge.net/projects/cefsharp/files/) page for even older releases. Please note that the latest Stable release, *or preferably the new CEF3 Pre-release getting out of beta real-soon-now*, is the only version officially supported; it's very unlikely that we will fix bugs in older releases.

# Branches & Forks

* Development is done in the `master` branch, which *used to be called* `CefSharp3` during the development cycle. New features are preferably added in feature branches, if the changes are more than trivial.
* Ultra-stable/LTS work is done in `CefSharp1`.
* [The CefSharp Authors page](https://github.com/cefsharp/CefSharp) is the recommended starting place. This is the "official" CefSharp fork, as maintained by the CefSharp community. You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

# Links

- [CefGlue](https://bitbucket.org/fddima/cefglue/wiki/Home): An alternative .NET CEF wrapper built using P/Invoke.
