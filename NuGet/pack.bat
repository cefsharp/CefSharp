set version=31.0.0-pre1b
REM set platform=x86
rem NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=

if "%platform%"=="x64" (
	copy ..\x64\Release\CefSharp.Core.dll
	set DotPlatform=.x64
) else (
	copy ..\Win32\Release\CefSharp.Core.dll
	set Platform=x86
	set DotPlatform=
)

NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=%DotPlatform%;Platform=%Platform%
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=Wpf;DotPlatform=%DotPlatform%;Platform=%Platform%
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=WinForms;DotPlatform=%DotPlatform%;Platform=%Platform%
