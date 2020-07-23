# CefSharp.Wpf.HwndHost

Designed as a drop in replacement for [CefSharp.Wpf.ChromiumWebBrowser](http://nuget.org/packages/CefSharp.Wpf/) for those who want the native Win32 based implementation (For IME support and better performance).

The control uses a [HwndHost](https://docs.microsoft.com/en-us/dotnet/api/system.windows.interop.hwndhost?view=netframework-4.6.2) to host the native `CefBrowser` instance.

As with any HwndHost based control standard `AirSpace` issues apply.
