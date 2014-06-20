set version=31.0.0-pre1a
rem NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.%Platform%;Platform=%Platform%
rem NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
rem NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x86
rem NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x64
rem NuGet pack CefSharp.WinForms.nuspec -NoPackageAnalysis -Version %version%
