###### 1.19.0
August 1, 2012, Chromium 19.0.1084.9

- Updated all extension point interfaces to match their CEF names (ILoadHandler, IRequestHandler, etc).
- Add per-browser RegisterJSObject() method.
- WPF: WebView supports popups.
- WPF: remove source hook on dispose.
- Expose all CefSetCookie() parameters.
- Added VisitAllCookies() and VisitUrlCookies().

###### 0.11
April 10, 2012, Chromium 20.0.1092.0

- Fix OnBeforeBrowser [kppullin]
- Wrap SetCookie(), DeleteCookies(), and SetCookiePath() [davybrion]
- Transform WPF control size to device coordinates [xpaulbettsx]

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
