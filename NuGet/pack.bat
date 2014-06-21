set version=31.0.0-pre1a
REM set platform=x86
rem NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.%Platform%;Platform=%Platform%
rem NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=
rem NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=.x86
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=Wpf;DotPlatform=.%Platform%;Platform=%Platform%
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=WinForms;DotPlatform=.%Platform%;Platform=%Platform%
