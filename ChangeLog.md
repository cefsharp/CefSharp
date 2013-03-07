###### 0.13
Unreleased, Chromium 25.0.1364.68

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
