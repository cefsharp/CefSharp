set version=33.1.0-pre01
NuGet pack CefSharp.Common.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.WinForms.nuspec -NoPackageAnalysis -Version %version%
