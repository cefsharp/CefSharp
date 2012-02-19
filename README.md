# CefSharp - Embedded Chromium for .Net

This project contains .Net CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt.  The bindings are written in C++/CLI but can be used from any CLR language e.g. C# or VB.  See the CefSharp.WinFormsExample or CefSharp.WpfExample projects example web browsers built using this library.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed.

# Binary Releases

Binary release builds contains everything that you need to embed Chromium in your CLR application. You will need the [7-Zip archiver](http://www.7-zip.org/ 7-Zip) to extract.

- [CefSharp 0.9](https://github.com/downloads/ataranto/CefSharp/CefSharp-0.9-bin.7z "Download"): Feb 18 2012, Chromium 17.0.963.15

# Quick Start

CefSharp requires the following files to be present in the same directory as either CefSharp.WinForms.dll or CefSharp.Wpf.dll:

- libcef.dll
- icudt.dll
- chrome.pak
- locales/

Additional DLLs may be needed for additional chromium functionality (audio, video, WebGL, etc).

# Links

- [CefGlue](https://bitbucket.org/fddima/cefglue/wiki/Home)
