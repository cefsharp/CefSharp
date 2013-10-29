using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        static int Main(string[] args)
        {
            var hInstance = Process.GetCurrentProcess().Handle;
            LogCommandLine(args);
            MessageBox.Show("Please attach debugger now");

            return ExecuteCefRenderProcess(hInstance);
        }

        private static void LogCommandLine(string[] args)
        {
            OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));
        }

        private static int ExecuteCefRenderProcess(IntPtr hInstance)
        {
            var subprocessCefApp = new SubprocessCefApp();
            return GlobalMethods.CefExecuteProcess(hInstance, subprocessCefApp);
        }
    }
}
