if "%APPVEYOR_BUILD_NUMBER%"=="" (
	set APPVEYOR_BUILD_NUMBER=10001
)

set version=31.0.0.%APPVEYOR_BUILD_NUMBER%-CI
REM set platform=x86

if "%platform%"=="x64" (
	set DotPlatform=.x64
	set CPlatform=x64
) else (
	set Platform=x86
	set DotPlatform=.x86
	set CPlatform=Win32
)

NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version% -Properties DotPlatform=%DotPlatform%;Platform=%Platform%;CPlatform=%CPlatform%
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=Wpf;DotPlatform=%DotPlatform%;Platform=%Platform%
NuGet pack CefSharp.WpfOrWinForms.nuspec -NoPackageAnalysis -Version %version% -Properties ControlType=WinForms;DotPlatform=%DotPlatform%;Platform=%Platform%
