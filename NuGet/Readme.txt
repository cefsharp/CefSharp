CefSharp Nuget Package

Background:
  CefSharp is a .Net wrapping library for CEF (Chromium Embedded Framework) https://bitbucket.org/chromiumembedded/cef
  CEF is a C/C++ library that allows developers to embed the HTML content rendering strengths of Google's Chrome open source WebKit engine (Chromium).

Post Installation:
  - Read the release notes for your version https://github.com/cefsharp/CefSharp/releases (Any known issues will be listed here)
  - Read the `Need to know/limitations` section of the General usage guide (https://github.com/cefsharp/CefSharp/wiki/General-Usage#need-to-knowlimitations)
  - For `x86` or x64` set your projects PlatformTarget architecture to `x86` or `x64`.
  - `AnyCPU` target is supported though requires additional code/changes see https://github.com/cefsharp/CefSharp/issues/1714 for details.
  - Check your output `\bin` directory to make sure the appropriate references have been copied.
  - Add an app.manifest to your exe if you don't already have one, it's required for Windows 10 compatability, HighDPI support and tooltips. The   https://github.com/cefsharp/CefSharp.MinimalExample project contains an example app.manifest file in the root of the WPF/WinForms/OffScreen examples. 
  
Deployment:
  - Make sure a minimum of `Visual C++ 2015` is installed (`x86` or x64` depending on your build) or package the runtime dlls with your application, see the FAQ for details.
  
What's New:
  See https://github.com/cefsharp/CefSharp/wiki/ChangeLog
  IMPORTANT NOTE - Visual C++ 2015 is now required
  IMPORTANT NOTE - .NET Framework 4.5.2 is now required.  
  IMPORTANT NOTE - Chromium has removed support for Windows XP/2003 and Windows Vista/Server 2008 (non R2).

Basic Troubleshooting:
  - Minimum of .Net 4.5.2
  - Minimum of `Visual C++ 2015 Redist` is installed (either `x86` or `x64` depending on your application). VC++ 2017/2019 are backwards compatible. 
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
    NOTE: CefSharp does not currently support CEF sandboxing.
  - By default `CEF` has it's own log file, `Debug.log` which is located in your executing folder. e.g. `bin`

For further help please read the following content:
  - General Usage Guide https://github.com/cefsharp/CefSharp/wiki/General-Usage
  - Minimal Example Projects showing the browser in action (https://github.com/cefsharp/CefSharp.MinimalExample)
  - CefSharp GitHub https://github.com/cefsharp/CefSharp
  - CefSharp's Wiki on github (https://github.com/cefsharp/CefSharp/wiki)
  - FAQ: https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions
  - Troubleshooting guide (https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting)
  - Google Groups (https://groups.google.com/forum/#!forum/cefsharp) - Historic reference only
  - CefSharp vs Cef (https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md#cefsharp-vs-cef)
  - Join the active community and ask your question on Gitter Chat (https://gitter.im/cefsharp/CefSharp)
  - If you have a reproducible bug then please open an issue on `GitHub`

Please consider giving back, it's only with your help will this project to continue.

Regards,
CefSharp Team
