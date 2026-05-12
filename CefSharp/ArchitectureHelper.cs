using System;
using System.Reflection;
#if NETCOREAPP
using System.Runtime.InteropServices;
#endif

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
#if NETCOREAPP
            return MapArchitecture(RuntimeInformation.ProcessArchitecture.ToString());
#else
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            var runtimeInformationType = Type.GetType("System.Runtime.InteropServices.RuntimeInformation, System.Runtime.InteropServices.RuntimeInformation");

            if (runtimeInformationType != null)
            {
                var processArchitectureProperty = runtimeInformationType.GetProperty("ProcessArchitecture", flags);
                if (processArchitectureProperty != null)
                {
                    var processArchitecture = processArchitectureProperty.GetValue(null, null);
                    if (processArchitecture != null)
                    {
                        return MapArchitecture(processArchitecture.ToString());
                    }
                }
            }

            return Environment.Is64BitProcess ? X64 : X86;
#endif
        }

        private static string MapArchitecture(string architecture)
        {
            switch (architecture)
            {
                case "Arm64":
                case "arm64":
                    return Arm64;
                case "X64":
                case "x64":
                    return X64;
                case "X86":
                case "x86":
                    return X86;
                default:
                    return Environment.Is64BitProcess ? X64 : X86;
            }
        }
    }
}