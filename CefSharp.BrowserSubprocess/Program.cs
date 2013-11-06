using CefSharp.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        static int Main(string[] args)
        {
            LogCommandLine(args);

            var subprocessCefApp = SubprocessCefApp.Instance;
            subprocessCefApp.ParentProcessId = FindParentProcessId(args);

            //MessageBox.Show("Please attach debugger now", null, MessageBoxButtons.OK, MessageBoxIcon.Information);
            var result = ExecuteCefRenderProcess();
            Kernel32.OutputDebugString("BrowserSubprocess shutting down.");
            return result;
        }

        private static void LogCommandLine(string[] args)
        {
            Kernel32.OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));
        }

        private static int? FindParentProcessId(IEnumerable<string> args)
        {
            // Format being parsed:
            // --channel=3828.2.1260352072\1102986608
            // We only really care about the PID (3828) part.
            var channelPrefix = "--channel=";
            var channelArgument = args.SingleOrDefault(arg => arg.StartsWith(channelPrefix));
            if (channelArgument == null) return null;

            var parentProcessId = channelArgument
                .Substring(channelPrefix.Length)
                .Split('.')
                .First();
            return int.Parse(parentProcessId);
        }

        private static int ExecuteCefRenderProcess()
        {
            var hInstance = Process.GetCurrentProcess().Handle;
            var subprocessCefApp = SubprocessCefApp.Instance;
            return GlobalMethods.CefExecuteProcess(hInstance, subprocessCefApp);
        }
    }
}
