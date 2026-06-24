using System;
using System.Runtime.InteropServices;

namespace CefSharp
{
    internal static class ArchitectureHelper
    {
        internal const string Arm64 = "arm64";
        internal const string X64 = "x64";
        internal const string X86 = "x86";

        internal static string ProcessArchitecture
        {
            get { return processArchitecture.Value; }
        }

        internal static bool IsArm64Process
        {
            get { return string.Equals(ProcessArchitecture, Arm64, StringComparison.OrdinalIgnoreCase); }
        }

        private static readonly Lazy<string> processArchitecture = new Lazy<string>(ResolveProcessArchitecture);

        private static string ResolveProcessArchitecture()
        {
            return MapArchitecture(RuntimeInformation.ProcessArchitecture.ToString());
        }

        private static string MapArchitecture(string architecture)
        {
            var lower = architecture.ToLowerInvariant();

            if (string.Equals(lower, Arm64, StringComparison.Ordinal))
            {
                return Arm64;
            }

            if (string.Equals(lower, X64, StringComparison.Ordinal))
            {
                return X64;
            }

            if (string.Equals(lower, X86, StringComparison.Ordinal))
            {
                return X86;
            }

            return Environment.Is64BitProcess ? X64 : X86;
        }
    }
}