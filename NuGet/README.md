# CefSharp for .NET Framework

CefSharp is a .NET library for embedding [Chromium Embedded Framework](https://github.com/chromiumembedded/cef) in WPF, WinForms, and offscreen applications.

## Quick start

For .NET Framework 4.6.x, 4.7.x, and 4.8 applications, install one of the implementation packages:

- `CefSharp.WinForms`
- `CefSharp.Wpf`
- `CefSharp.OffScreen`

.NET Framework 4.6.2 is the minimum supported version.

### Configure the platform target

For an `x86` or `x64` application, set the project's **Platform target** to match the application architecture.

`AnyCPU` is supported when `Prefer 32-bit` is enabled; the application is then 32-bit and uses the x86 binaries. For other `AnyCPU` configurations, refer to [issue #1714](https://github.com/cefsharp/CefSharp/issues/1714). The [MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample) project demonstrates `AnyCPU` support.

### Add an application manifest

Add an [application manifest](https://learn.microsoft.com/windows/win32/sbscs/application-manifests) to your executable if it does not already have one. The manifest is required for Windows 10/11 compatibility, GPU acceleration, high-DPI support, and tooltips.

Include the Windows 10/11 compatibility declaration:

```xml
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<assembly manifestVersion="1.0" xmlns="urn:schemas-microsoft-com:asm.v1">
  <assemblyIdentity
    type="win32"
    name="Contoso.ExampleApplication.ExampleBinary"
    version="1.2.3.4"
    processorArchitecture="x86" />
  <description>Contoso Example Application</description>
  <compatibility xmlns="urn:schemas-microsoft-com:compatibility.v1">
    <application>
      <!-- Windows 10/11 -->
      <supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}" />
    </application>
  </compatibility>
</assembly>
```

The [MinimalExample](https://github.com/cefsharp/CefSharp.MinimalExample) project contains example manifests for WPF, WinForms, and OffScreen applications.

### Add a browser

#### WPF

Use `CefSharp.Wpf.ChromiumWebBrowser`:

```xml
<!-- Add to the parent control:
xmlns:wpf="clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf" -->
<wpf:ChromiumWebBrowser x:Name="Browser" Address="https://www.google.com" />
```

#### WinForms

```csharp
using CefSharp;
using CefSharp.WinForms;

var browser = new ChromiumWebBrowser("https://www.google.com");
parent.Controls.Add(browser);

browser.LoadUrl("https://github.com");
```

#### OffScreen

```csharp
using CefSharp;
using CefSharp.OffScreen;

var browser = new ChromiumWebBrowser("https://www.google.com");
browser.LoadUrl("https://github.com");
```

### Execute JavaScript

```csharp
using CefSharp;

var response = await browser.EvaluateScriptAsync("1 + 1");
if (response.Success)
{
    var result = response.Result;
}
```

See the full [.NET Framework quick start](https://github.com/cefsharp/CefSharp/wiki/Quick-Start-For-MS-.Net-4.x).

## Post-installation

- Read the [release notes](https://github.com/cefsharp/CefSharp/releases) for the installed version.
- Read the [limitations and important notes](https://github.com/cefsharp/CefSharp/wiki/General-Usage#need-to-knowlimitations).
- Verify that the required files have been copied to the `bin` directory.
- Chromium 110 and later require Windows 10 or newer.

## Deployment and troubleshooting

Install the Visual C++ 2022 Redistributable, or package the required Visual C++ runtime DLLs with the application. The architecture (`x86`, `x64`, or `arm64`) must match the application.

The output directory must contain the CEF files, the CefSharp common binaries, and the selected UI implementation library. Refer to the [output files description](https://github.com/cefsharp/CefSharp/wiki/Output-files-description-table-%28Redistribution%29) for the complete list.

CefSharp does not support CEF sandboxing. By default, CEF writes `Debug.log` in the application directory.

## More information

- [General usage guide](https://github.com/cefsharp/CefSharp/wiki/General-Usage)
- [Minimal example projects](https://github.com/cefsharp/CefSharp.MinimalExample)
- [API documentation](https://cefsharp.github.io/api/)
- [Frequently asked questions](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions)
- [Troubleshooting guide](https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting)
- [GitHub Discussions](https://github.com/cefsharp/CefSharp/discussions)
