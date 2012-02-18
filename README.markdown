CefSharp - Embedded Chromium for .Net
=====================================

This project contains .Net CLR bindings for The [Chromium Embedded Framework (CEF)](http://code.google.com/p/chromiumembedded/ "Google Code") by Marshall A. Greenblatt.  The bindings are written in C++/CLI but can be used from any CLR language e.g. C# or VB.  See the CefTest project for an example web browser built using this library.

This project is [BSD](http://www.opensource.org/licenses/bsd-license.php "BSD License") licensed.

NEWS
====

2010/12/20 - 0.3 Released
-------------------------
- [Download 0.3.1 here](https://github.com/downloads/chillitom/CefSharp/CefSharp-0.3.1.7z "src, bins and examples")
- Improvements to CLR object binding (big thanks _fddima_)
- Updated to CEF r152 / Chromium r69409
- Calling _CefBrowserControl.Focus()_ now works.
- New API for _IBeforeResourceLoad_ to avoid _ref_ parameters
- _Reload()_ and _Reload(bool ignoreCache)_ methods
- Streams now closed explicitly after resource load.
- Fix bug, Browser creation fails without registered JS objects

All feedback is welcomed gladly. Enjoy. T.

_UPDATE_ : bounced zip to 0.3.1 to fix a build issue, now includes Debug and Release lib files.

2010/12/15 - 0.2 Released
-------------------------
- [Download 0.2 here](https://github.com/downloads/chillitom/CefSharp/CefSharp-0.2.7z "src, bins and examples")
- ISchemeHandler lets you introduce your own scheme/protocol handlers
- JavaScript console messages are now exposed on the ConsoleMessage event
- Basic support for binding CLR objects into the browser's window DOM object.
- Renamed BrowserControl to CefWebBrowser, now derives from Control instead of UserControl
- The control now checks it is initialized be accepting commands
- Fixes including resize issues and incorrect address events.
- New unit tests and examples.

2010/12/03 - 0.1 Released
-------------------------
- [Download 0.1 here](https://github.com/downloads/chillitom/CefSharp/CefSharp-0.1.7z "src, bins and examples")
- BrowserControl user control
- Load, Forward, Back, Stop, Go
- INotifyPropertyChanged notification for navigation events CanGoForward, CanGoBack etc.
- ExecuteJavascript, works but needs polish
- Interception of resource loading, loads from System.IO.Stream

Enough to build a poor man's web browser and call a bit of JavaScript. Coming soon, more features including the ability to bind .Net object straight into the browser's V8 JavaScript context.





