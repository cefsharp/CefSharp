set version=33.0.0-pre0
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version%
rem NuGet pack CefSharp.WinForms.nuspec -NoPackageAnalysis -Version %version%
