
[![CefSharp Logo](https://github.com/cefsharp/CefSharp/raw/master/logo.png)](https://cefsharp.github.io/ "CefSharp - Embedded Chromium for .NET")

# CefSharp.Wpf.HwndHost
[![CefSharp.Wpf.HwndHost Build Status](https://img.shields.io/appveyor/build/cefsharp/cefsharp-wpf-hwndhost)](https://ci.appveyor.com/project/cefsharp/cefsharp-wpf-hwndhost)
[![CefSharp.Wpf](https://img.shields.io/nuget/v/CefSharp.Wpf.HwndHost.svg?style=flat&label=CefSharp.Wpf.HwndHost)](https://www.nuget.org/packages/CefSharp.Wpf.HwndHost/)

Designed as a drop in replacement for [CefSharp.Wpf.ChromiumWebBrowser](http://nuget.org/packages/CefSharp.Wpf/) for those who want the native Win32 based implementation (For IME support and better performance).

The control uses a [HwndHost](https://docs.microsoft.com/en-us/dotnet/api/system.windows.interop.hwndhost?view=netframework-4.6.2) to host the native `CefBrowser` instance.

As with any HwndHost based control standard `AirSpace` issues apply.
