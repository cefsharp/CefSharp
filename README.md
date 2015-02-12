# CefSharp - Embedded Chromium for .NET

This project contains .NET CLR bindings for The
[Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt. The
bindings are written in C++/CLI but can be used from any CLR language, e.g. C# or VB. CefSharp provides both WinForms and WPF web
browser control implementations. See the CefSharp.WinForms.Example or CefSharp.Wpf.Example projects for example web browsers built
using this library; they are (at this moment) the best "documentation" of how the control can actually be used.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed, which means that it can be used from both proprietary and free/open source applications. For the full details, see the [LICENSE](LICENSE) file.

# Documentation & mailing list

Work-in-progress documentation can be found [here](https://github.com/cefsharp/CefSharp/wiki). If something is missing/incomplete,
please don't hesitate to ask at the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp). You might also find
help by searching the archive to that same Google Group for previous questions.

# Binary Release

Binary releases contain everything needed to embed Chromium in your CLR application.

* **Stable** (requires .NET 4): CefSharp-1.25.8 - [binaries](https://github.com/cefsharp/CefSharp/releases/download/v1.25.8/CefSharp-v1.25.8-binaries.zip),
[source](https://github.com/cefsharp/CefSharp/archive/v1.25.8.zip). Based on Chromium 25.0.1364.152
* **Stable** (older release, works with .NET 2. Requires [7-Zip](http://www.7-zip.org/) to extract.): [CefSharp-1.25.0](http://sourceforge.net/projects/cefsharp/files/CefSharp-1.25.0.7z/download).
Based on Chromium 25.0.1364.152

See [Downloads](https://sourceforge.net/projects/cefsharp/files/) page for older releases. Please note that the latest Stable
release is the only version officially supported; it's very unlikely that we will fix bugs in older releases.

# Forks

- [The cefsharp project page](https://github.com/cefsharp/CefSharp) is the recommended starting place. This is the "official"
  CefSharp fork, as maintained by the CefSharp community.
- You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

# Links

- [CefGlue](https://bitbucket.org/fddima/cefglue/wiki/Home): An alternative .NET CEF wrapper build using P/Invoke.
