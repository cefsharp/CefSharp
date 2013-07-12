WIP
###### 1.25.2-perlun.1

- Add support for parameters of type 'Object' accepting any type of value. [@EMonk72]
- Fix object binding to allow multiple methods with same name. [@EMonk72]
- Reimplemented Javascript-to-.NET binding to support a range of options including C#-style param arrays and optional parameters
  with default values. [@EMonk72]
- Added support for accessing the body of the request in a custom scheme handler. [@kpko]
- Added support for overriding the window.alert(), window.confirm() and window.prompt() methods by using the new IJsDialogHandler
  interface + the new IWebBrowser.JsDialogHandler property, which is presumed to be your own implementation of this interface.
  [@KristinaKoeva]
- Fixed a null pointer exception that occurs when WebView is loaded. [@joaompneves]
- Fixed crash on window deactivate. [@joaompneves]

June 17, 2013, Chromium 25.0.1364.1123
###### 1.25.2-perlun.0

- Updated to compile correctly with VS2010 in Release mode. [@plundberg]

###### 1.25.1-perlun.0
June 7, 2013, Chromium 25.0.1364.1123

- Upgraded the project to be compiled with VS2010 SP1 and/or VS2012, instead of VS2008. This means that you will now only be able to consume this version of CefSharp from .NET 4.0 code (3.5 and 2.0 is no longer supported). [@plundberg, @mwisnicki]
- Added support for exposing CLR properties to Javascript code. [@plundberg]
- Breaking change: Changed CLR methods and properties to use camelCase (rather than just exposing them as-is, which was the previous case), to make them act more like regular Javascript methods and properties. [@plundberg]

###### 1.25.0
March 11, 2013, Chromium 25.0.1364.68

- Add support for file downloads using IRequestHandler.GetDownloadHandler() [@gbrhaz, @jclement]
- Add support for HTTP Basic Authentication using GetAuthCredentials [@rotem925]
- Add OnBeforeClose() binding [@intninety]
- Raise LoadCompleted event when OnFrameLoadEnd() is called [@kppullin]
- Allow referencing CefSharp.WinForms.dll and CefSharp.Wpf.dll from the same project [@mwisnicki]
- Fix a bug that prevented Wpf.WebView from working after being moved from one window to another [@dvarchev]
- Add a LoadHtml() overload with a url parameter [@flowcoder]
- Fix poup dimensions when using Wpf.WebView [@KristinaKoeva]
- Make RequestResponse object to allow custom status code, status text and headers [@dvarchev]
- Correct dimentions of Wpf.WebView when running in non default DPI settings, prevents web content from appearing cut off [@dvarchev]
- Don't generate debug info for release vcproj configurations [@oconnor663]
- Fix a GC bug when removing a WPF web view from the view hierarchy [@MasonOfWords]

###### 0.12
August 1, 2012, Chromium 21.0.1180.0

- Updated all extension point interfaces to match their CEF names (ILoadHandler, IRequestHandler, etc).
- Add per-browser RegisterJSObject() method.
- WPF: WebView supports popups.
- WPF: remove source hook on dispose.
- Expose all CefSetCookie() parameters.
- Added VisitAllCookies() and VisitUrlCookies().

###### 0.11
April 10, 2012, Chromium 20.0.1092.0

- Fix OnBeforeBrowser [@kppullin]
- Wrap SetCookie(), DeleteCookies(), and SetCookiePath() [@davybrion]
- Transform WPF control size to device coordinates [@xpaulbettsx]

###### 0.10
March 18, 2012, Chromium 17.0.963.15

- Bulid has been split into CefSharp.dll (shared code), CefSharp.WinForms.dll (WinForms implementation) and CefSharp.Wpf.dll (WPF implementation).
- Renamed controls to `WebView` to prevent naming conflicts with Microsoft WebBrowser types.
- added ExecuteScript() method to execute JavaScript without returning a value.
- EvaluateScript() returns object, JavaScript evaluation result will be converted to the appropriate CLR type when possible (#32).
- CloseBrowser() is now called from Dispose().
- ScriptCore: Fixed r6025 error.
- Improved init/teardown checks (TryGetCefBrowser() method)
- WPF: ToolTips.
- WPF: Added ContentsWidth and ContentsHeight properties.
- WPF: Tab focus support.
- WPF: Improved mouse and keyboard handling.


###### 0.9
Feb 18 2012, Chromium 17.0.963.15
