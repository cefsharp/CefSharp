# CefSharp for .NET

CefSharp is a .NET library for embedding [Chromium Embedded Framework](https://github.com/chromiumembedded/cef) in WPF, WinForms, and offscreen applications.

## Quick start

For .NET 6 or newer, install one of the implementation packages:

- `CefSharp.WinForms.NETCore`
- `CefSharp.Wpf.NETCore`
- `CefSharp.OffScreen.NETCore`

### Set a runtime identifier

A [RuntimeIdentifier](https://learn.microsoft.com/dotnet/core/tools/csproj#runtimeidentifier) tells Visual Studio and MSBuild which architecture-specific files to copy. Without one, the x86, x64, and ARM64 files are all copied. A runtime identifier is required when publishing.

Add one of the following to the project file:

```xml
<RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-x86</RuntimeIdentifier>
<!-- or -->
<RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-x64</RuntimeIdentifier>
<!-- or -->
<RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-arm64</RuntimeIdentifier>
```

You can also use the SDK runtime identifier:

```xml
<RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">$(NETCoreSdkRuntimeIdentifier)</RuntimeIdentifier>
<SelfContained Condition="'$(SelfContained)' == ''">false</SelfContained>
```

For a project with multiple platforms, select the runtime identifier by `PlatformTarget`:

```xml
<PropertyGroup Condition="'$(PlatformTarget)' == 'x86'">
  <RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-x86</RuntimeIdentifier>
</PropertyGroup>

<PropertyGroup Condition="'$(PlatformTarget)' == 'x64'">
  <RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-x64</RuntimeIdentifier>
</PropertyGroup>

<PropertyGroup Condition="'$(PlatformTarget)' == 'arm64'">
  <RuntimeIdentifier Condition="'$(RuntimeIdentifier)' == ''">win-arm64</RuntimeIdentifier>
</PropertyGroup>
```

For more information, see [issue #3284](https://github.com/cefsharp/CefSharp/issues/3284#issuecomment-772132523).

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
```

#### OffScreen

```csharp
using CefSharp;
using CefSharp.OffScreen;

var browser = new ChromiumWebBrowser("https://www.google.com");
```

### Execute JavaScript

```csharp
using CefSharp;

var loadResponse = await browser.WaitForInitialLoadAsync();
if (loadResponse.Success)
{
    var response = await browser.EvaluateScriptAsync("1 + 1");
    if (response.Success)
    {
        var result = response.Result;
    }
}
```

See the [JavaScript integration guide](https://github.com/cefsharp/CefSharp/wiki/General-Usage#javascript-integration) for more details.

### Publish

```powershell
dotnet publish -c Release -f net6.0-windows -r win-x86
dotnet publish -c Release -f net6.0-windows -r win-x64
dotnet publish -c Release -f net6.0-windows -r win-arm64
```

For self-contained single-file publishing (`SelfContained=true` and `PublishSingleFile=true`), the browser subprocess must be self-hosted by the main application executable. See [issue #3407](https://github.com/cefsharp/CefSharp/issues/3407).

See the full [.NET quick start](https://github.com/cefsharp/CefSharp/wiki/Quick-Start-For-MS-.Net-5.0-or-greater).

## Post-installation

- Read the [release notes](https://github.com/cefsharp/CefSharp/releases) for the installed version.
- Read the [limitations and important notes](https://github.com/cefsharp/CefSharp/wiki/General-Usage#need-to-knowlimitations).
- Verify that the required files have been copied to the `bin` directory.

## Deployment and troubleshooting

Install the Visual C++ 2022 Redistributable, or package the required Visual C++ runtime DLLs with the application. The architecture (`x86`, `x64`, or `arm64`) must match the application.

The output directory must contain the CEF files, `Ijwhost.dll`, the CefSharp common binaries, and the selected UI implementation library. Refer to the [output files description](https://github.com/cefsharp/CefSharp/wiki/Output-files-description-table-%28Redistribution%29) for the complete list.

CefSharp does not support CEF sandboxing. By default, CEF writes `Debug.log` in the application directory.

## More information

- [General usage guide](https://github.com/cefsharp/CefSharp/wiki/General-Usage)
- [Minimal example projects](https://github.com/cefsharp/CefSharp.MinimalExample)
- [API documentation](https://cefsharp.github.io/api/)
- [Frequently asked questions](https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions)
- [Troubleshooting guide](https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting)
- [GitHub Discussions](https://github.com/cefsharp/CefSharp/discussions)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/cefsharp)
