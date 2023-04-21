CefSharp Nuget Package

Background:
  CefSharp is a .Net wrapping library for CEF (Chromium Embedded Framework) https://bitbucket.org/chromiumembedded/cef
  CEF is a C/C++ library that allows developers to embed the HTML content rendering strengths of Google's Chrome open source WebKit engine (Chromium).

Post Installation:
  - Add an app.manifest to your exe if you don't already have one, it's required for Windows 10 compatability, GPU detection, HighDPI support and tooltips. The https://github.com/cefsharp/CefSharp.MinimalExample project contains an example app.manifest file in the root of the WPF/WinForms/OffScreen examples. 
  - For `x86` or x64` set your projects PlatformTarget architecture to `x86` or `x64`.
  - `AnyCPU` target is supported though requires additional code/changes see https://github.com/cefsharp/CefSharp/issues/1714 for details.
  - Read the release notes for your version https://github.com/cefsharp/CefSharp/releases (Any known issues will be listed here)
  - Read the `Need to know/limitations` section of the General usage guide (https://github.com/cefsharp/CefSharp/wiki/General-Usage#need-to-knowlimitations)
  - Check your output `\bin` directory to make sure the appropriate references have been copied.
  
Deployment:
  - Make sure a minimum of `Visual C++ 2019` is installed (`x86` or x64` depending on your build) or package the runtime dlls with your application, see the FAQ for details.
  
What's New:
  See https://github.com/cefsharp/CefSharp/releases
  IMPORTANT NOTE - Visual C++ 2019 or greater is required
  IMPORTANT NOTE - .NET Framework 4.6.2 or greater is required.
  IMPORTANT NOTE - Chromium support for Windows 7/8/8.1 ends with version 109, starting with version 110 a minimum of Windows 10 is required.

Basic Troubleshooting:
  - Minimum of .Net 4.6.2
  - Minimum of `Visual C++ 2019 Redist` is installed (either `x86` or `x64` depending on your application).
  - Please ensure your binaries directory contains these required dependencies:
    * libcef.dll (Chromium Embedded Framework Core library)
    * icudtl.dat (Unicode Support data)
	* chrome_elf.dll(Crash reporting library)
	* snapshot_blob.bin, v8_context_snapshot.bin (V8 snapshot data)
	* locales\en-US.pak, chrome_100_percent.pak, chrome_200_percent.pak, resources.pak, d3dcompiler_47.dll, libEGL.dll, libGLESv2.dll
	  - Whilst these are technically listed as optional, the browser is unlikely to function without these files.
	  - See https://github.com/cefsharp/CefSharp/wiki/Output-files-description-table-%28Redistribution%29 for details
    * CefSharp.Core.dll, CefSharp.dll, CefSharp.Core.Runtime.dll
      CefSharp.BrowserSubprocess.exe, CefSharp.BrowserSubProcess.Core.dll
        - These are required CefSharp binaries that are the common core logic binaries of CefSharp.
    * One of the following UI presentation libraries:
        * CefSharp.WinForms.dll
        * CefSharp.Wpf.dll
        * CefSharp.OffScreen.dll
  - Additional CEF files are described at: https://github.com/cefsharp/CefSharp/wiki/Output-files-description-table-%28Redistribution%29
  - NOTE: CefSharp does not currently support CEF sandboxing.
  - By default `CEF` has it's own log file, `Debug.log` which is located in your executing folder. e.g. `bin`

For further help please read the following content:
  - Quick Start https://github.com/cefsharp/CefSharp/wiki/Quick-Start
  - General Usage Guide https://github.com/cefsharp/CefSharp/wiki/General-Usage
  - Minimal Example Projects showing the browser in action (https://github.com/cefsharp/CefSharp.MinimalExample)
  - CefSharp GitHub https://github.com/cefsharp/CefSharp
  - CefSharp's Wiki on github (https://github.com/cefsharp/CefSharp/wiki)
  - FAQ: https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions
  - Troubleshooting guide (https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting)
  - CefSharp vs Cef (https://github.com/cefsharp/CefSharp/blob/master/CONTRIBUTING.md#cefsharp-vs-cef)
  - Got a question? Ask it on GitHub Discussions (https://github.com/cefsharp/CefSharp/discussions)
  - If you have a reproducible bug then please open an issue on `GitHub` making sure to complete the bug report template.

Please consider giving back, it's only with your help will this project to continue.
Sponsor the project via GitHub sponsors (https://github.com/sponsors/amaitland)

Regards,
Alex Maitland
