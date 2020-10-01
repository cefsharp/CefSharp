using System;
using System.Reflection;
using System.Runtime.InteropServices;
using CefSharp;

[assembly: AssemblyTitle("CefSharp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyCompany(AssemblyInfo.AssemblyCompany)]
[assembly: AssemblyProduct(AssemblyInfo.AssemblyProduct)]
[assembly: AssemblyCopyright(AssemblyInfo.AssemblyCopyright)]
[assembly: ComVisible(AssemblyInfo.ComVisible)]
[assembly: AssemblyVersion(AssemblyInfo.AssemblyVersion)]
[assembly: AssemblyFileVersion(AssemblyInfo.AssemblyFileVersion)]
[assembly: CLSCompliant(AssemblyInfo.ClsCompliant)]

namespace CefSharp
{
    /// <exclude />
    public static class AssemblyInfo
    {
        public const bool ClsCompliant = false;
        public const bool ComVisible = false;
        public const string AssemblyCompany = "The CefSharp Authors";
        public const string AssemblyProduct = "CefSharp";
        public const string AssemblyVersion = "84.4.0";
        public const string AssemblyFileVersion = "84.4.0.0";
        public const string AssemblyCopyright = "Copyright © 2020 The CefSharp Authors";
        public const string CefSharpCoreProject = "CefSharp.Core, PublicKey=" + PublicKey;
        public const string CefSharpBrowserSubprocessProject = "CefSharp.BrowserSubprocess, PublicKey=" + PublicKey;
        public const string CefSharpBrowserSubprocessCoreProject = "CefSharp.BrowserSubprocess.Core, PublicKey=" + PublicKey;
        public const string CefSharpWpfProject = "CefSharp.Wpf, PublicKey=" + PublicKey;
        public const string CefSharpWinFormsProject = "CefSharp.WinForms, PublicKey=" + PublicKey;
        public const string CefSharpOffScreenProject = "CefSharp.OffScreen, PublicKey=" + PublicKey;
        public const string CefSharpTestProject = "CefSharp.Test, PublicKey=" + PublicKey;

        // Use "%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn.exe" -Tp <assemblyname> to get PublicKey
        public const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100c5ddf5d063ca8e695d4b8b5ad76634f148db9a41badaed8850868b75f916e313f15abb62601d658ce2bed877d73314d5ed202019156c21033983fed80ce994a325b5d4d93b0f63a86a1d7db49800aa5638bb3fd98f4a33cceaf8b8ba1800b7d7bff67b72b90837271491b61c91ef6d667be0521ce171f77e114fc2bbcfd185d3";
    }
}
