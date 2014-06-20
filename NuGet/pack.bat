set version=31.0.0-pre1a
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x86
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x64
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x86
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x64
rem NuGet pack CefSharp.WinForms.nuspec -NoPackageAnalysis -Version %version%
