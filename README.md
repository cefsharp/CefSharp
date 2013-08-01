# CefSharp - Embedded Chromium for .Net

This project contains .Net CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt. The bindings are written in C++/CLI but can be used from any CLR language e.g. C# or VB. CefSharp provides both WinForms and WPF web browser control implementations. See the CefSharp.WinForms.Example or CefSharp.Wpf.Example projects for example web browsers built using this library.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed.

# Documentation

Work-in-progress documentation can be found [here](https://github.com/cefsharp/CefSharp/wiki). Feel free to suggest changes to the
wiki pages, and also make changes yourself if you find something wrong/missing. Editing is open for all registered GitHub users.

# Binary Release

Binary releases contain everything needed to embed Chromium in your CLR application. You will need the [7-Zip archiver](http://www.7-zip.org/ "7-Zip") to extract.

**Stable (older)**: [CefSharp-1.25.0](http://sourceforge.net/projects/cefsharp/files/CefSharp-1.25.0.7z/download "Download")
Chromium 25.0.1364.152  
**Stable**: CefSharp-1.25.2-perlun.0 - no binary release yet; compile from source using the code at
https://github.com/cefsharp/CefSharp (look for the a tag named v1.25.2-perlun.0)

See the [ChangeLog](https://github.com/ataranto/CefSharp/blob/master/ChangeLog.md) for update information, or [Downloads](https://sourceforge.net/projects/cefsharp/files/) for older releases.

# Forks

- [The cefsharp project page](https://github.com/cefsharp/CefSharp) is the recommended starting place. This is the "official"
  CefSharp fork, as maintained by the CefSharp community.
- You can also view [the entire network of public forks/branches](https://github.com/cefsharp/CefSharp/network).

# Contact

Please use the [CefSharp Google Group](https://groups.google.com/forum/#!forum/cefsharp) for discussions of CefSharp usage.

# Links

- [CefGlue](https://bitbucket.org/fddima/cefglue/wiki/Home): An alternative .NET CEF wrapper build using P/Invoke.
