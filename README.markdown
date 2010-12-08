CefSharp - Embedded Chromium for .Net
=====================================

This project contains .Net CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt.  The bindings are written in C++/CLI but can be used from any CLR language e.g. C# or VB.  See the CefTest project for an example web browser built using this library.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed.

NEWS
====

2010/12/03 - 0.1 Released 
-------------------------
- [Download 0.1 here](https://github.com/downloads/chillitom/CefSharp/CefSharp-0.1.7z "src, bins and examples")
- BrowserControl user control
- Load, Forward, Back, Stop, Go
- INotifyPropertyChanged notification for navigation events CanGoForward, CanGoBack etc.
- ExecuteJavascript, works but needs polish
- Interception of resource loading, loads from System.IO.Stream

Enough to build a poor man's web browser and call a bit of JavaScript. Coming soon, more features including the ability to bind .Net object straight into the browser's V8 JavaScript context.
			




