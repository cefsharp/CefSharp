using System;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        static int Main(string[] args)
        {
            Kernel32.OutputDebugString("BrowserSubprocess starting up with command line: " + String.Join("\n", args));

            //MessageBox.Show("Please attach debugger now", null, MessageBoxButtons.OK, MessageBoxIcon.Information);

            int result = 0;

            using (var subprocess = new CefSubprocess(args))
            {
                result = subprocess.Run();
            }

            Kernel32.OutputDebugString("BrowserSubprocess shutting down.");
            return result;
        }
    }
}
