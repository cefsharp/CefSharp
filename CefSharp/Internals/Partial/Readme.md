The ChromiumWebBrowser class in this folder is not directly included in CefSharp.dll
it is shared between the WPF, OffScreen and WinForms implementations with #if def's
to allow for the partial class to be used between the three implementations.
