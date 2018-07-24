CefSharp Nuget Package

Background:
  CefSharp is a .Net wrapping library for CEF (Chromium Embedded Framework) https://bitbucket.org/chromiumembedded/cef
  CEF is a C/C++ library that allows developers to embed the HTML content rendering strengths of Google's Chrome open source WebKit engine (Chromium).

Post Installation:
  - Read the release notes for your version https://github.com/cefsharp/CefSharp/releases (Any known issues will be listed here)
  - For `x86` or x64` set your solution target architecture to `x86` or `x64`, just changing the project is currently not enough (See https://msdn.microsoft.com/en-us/library/ms185328.aspx#Anchor_0 for details).
  - `AnyCPU` target is supported though requires additional code/changes see https://github.com/cefsharp/CefSharp/issues/1714 for details.
  - After installing the `Nuget` package we recommend closing Visual Studio completely and then reopening (This ensures your references show up and you have full intellisense).
  - Check your output `\bin` directory to make sure the appropriate references have been copied.
  - Build fails even though packages are installed. Short term rebuild again and everything should be find. Long term we recommend reading http://www.xavierdecoster.com/migrate-away-from-msbuild-based-nuget-package-restore
  - Minimal designer support was added in version `57.0.0 for both `WinForms` and `WPF`. For older versions there is no designer support (designer will throw an exception).
  
Deployment:
  - Make sure `Visual C++ 2015` is installed (`x86` or x64` depending on your build) or you package the runtime dlls with your application, see the FAQ for details.
  
What's New:
  See https://github.com/cefsharp/CefSharp/wiki/ChangeLog
  IMPORTANT NOTE - Visual C++ 2015 is now required
  IMPORTANT NOTE - .NET Framework 4.5.2 is now required.  
  IMPORTANT NOTE - Chromium has removed support for Windows XP/2003 and Windows Vista/Server 2008 (non R2).
  
  The Microsoft .NET Framework 4.5.2 Developer Pack for Visual Studio 2012 and Visual Studio 2013 is available here: 
  https://www.microsoft.com/en-gb/download/details.aspx?id=42637

Basic Troubleshooting:
  - Minimum of .Net 4.5.2
  - Make sure `VC++ 2015 Redist` is installed (either `x86` or `x64` depending on your application)
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
