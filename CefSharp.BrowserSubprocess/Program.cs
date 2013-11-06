using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp.Wrappers;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        static int Main(string[] args)
        {
            var hInstance = Process.GetCurrentProcess().Handle;
            LogCommandLine(args);
            MessageBox.Show("Please attach debugger now", null, MessageBoxButtons.OK, MessageBoxIcon.Information);

            return ExecuteCefRenderProcess(hInstance);
        }

        private static void LogCommandLine(string[] args)
        {
            Kernel32.OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));
        }

        private static int ExecuteCefRenderProcess(IntPtr hInstance)
        {
            var subprocessCefApp = SubprocessCefApp.Instance;
            return GlobalMethods.CefExecuteProcess(hInstance, subprocessCefApp);
        }
    }
}
