set version=3.29.0-pre0
NuGet pack CefSharp.Core.nuspec -NoPackageAnalysis -Version %version%
NuGet pack CefSharp.Wpf.nuspec -NoPackageAnalysis -Version %version%
