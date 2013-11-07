using System.Runtime.InteropServices;

namespace CefSharp.BrowserSubprocess
{
    internal class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);
    }
}