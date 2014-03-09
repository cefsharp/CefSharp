using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CefSharp;

[assembly: AssemblyTitle("CefSharp")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The CefSharp Project")]
[assembly: AssemblyProduct("CefSharp")]
[assembly: AssemblyCopyright("Copyright © The CefSharp Authors 2010-2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("3.29.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: InternalsVisibleTo("CefSharp.Core, PublicKey=" + AssemblyInfo.PublicKey )]
[assembly: InternalsVisibleTo("CefSharp.BrowserSubprocess, PublicKey=" + AssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("CefSharp.Wpf, PublicKey=" + AssemblyInfo.PublicKey)]
[assembly: InternalsVisibleTo("CefSharp.WinForms, PublicKey=" + AssemblyInfo.PublicKey)]

namespace CefSharp
{
    public static class AssemblyInfo
    {
        //use "%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\sn.exe" -Tp <assemblyname> to get PublicKey
        public const string PublicKey = "0024000004800000940000000602000000240000525341310004000001000100c5ddf5d063ca8e695d4b8b5ad76634f148db9a41badaed8850868b75f916e313f15abb62601d658ce2bed877d73314d5ed202019156c21033983fed80ce994a325b5d4d93b0f63a86a1d7db49800aa5638bb3fd98f4a33cceaf8b8ba1800b7d7bff67b72b90837271491b61c91ef6d667be0521ce171f77e114fc2bbcfd185d3";
    }
}
