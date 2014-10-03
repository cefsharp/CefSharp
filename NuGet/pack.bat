set version=33.0.0-pre2
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.WinForms.nuspec -NoPackageAnalysis -Version %version%
