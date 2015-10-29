CefSharp Nuget Package

Background:
  CefSharp is a .Net wrapping library for CEF (Chromium Embedded Framework) https://bitbucket.org/chromiumembedded/cef
  CEF is a C/C++ library that allows developers to embed the HTML content rendering strengths of Google's Chrome open source WebKit engine (Chromium).

Post Installation:
  - Make sure you set either `x86` or x64`. (Won't work with `AnyCpu`)
  - After installing the `Nuget` package we recommend closing Visual Studio completely and then reopening (This ensures your references show up and you have full intellisense).
  - Check your output `\bin` directory to make sure the appropriate references have been copied.
  - Build fails even though packages are installed. Short term rebuild again and everything should be find. Long term we recommend reading http://www.xavierdecoster.com/migrate-away-from-msbuild-based-nuget-package-restore
  
What's New:
  See https://github.com/cefsharp/CefSharp/wiki/ChangeLog
  IMPORTANT NOTE - VC++ 2013 Redist is now required.

Basic Troubleshooting:
  - Minimum of .Net 4.0 Client Profile
  - Make sure `VC++ 2013 Redist` is installed (either `x86` or `x64` depending on your application)
  - Please ensure your binaries directory contains these required dependencies:
    * libcef.dll (CEF code)
    * icudtl.dat (Unicode Support data)
    * CefSharp.Core.dll, CefSharp.dll, 
      CefSharp.BrowserSubprocess.exe, CefSharp.BrowserSubProcess.Core.dll
        - These are required CefSharp binaries that are the common core logic binaries of CefSharp.
    * One of the following UI presentation approaches:
        * CefSharp.WinForms.dll
        * CefSharp.Wpf.dll
        * CefSharp.OffScreen.dll
  - Additional optional CEF files are described at: https://github.com/cefsharp/cef-binary/blob/master/README.txt#L82
    NOTE: CefSharp does not currently support CEF sandboxing so wow_helper.exe is not currently useful.

For further help please read the following content:
  - CefSharp Tutorials https://github.com/cefsharp/CefSharp.Tutorial
  - CefSharp GitHub https://github.com/cefsharp/CefSharp
  - CefSharp's Wiki on github (https://github.com/cefsharp/CefSharp/wiki)
  - Minimal Example Projects showing the browser in action (https://github.com/cefsharp/CefSharp.MinimalExample)
  - FAQ: https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions
  - Troubleshooting guide (https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting)
  - Google Groups (https://groups.google.com/forum/#!forum/cefsharp)
  - CefSharp vs Cef (https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md#cefsharp-vs-cef)
  - Only when you've exhausted all other options then open an issue on `GitHub`

Please consider giving back, it's only with your help will this project to continue.

Regards,
CefSharp Team
