using System.Runtime.InteropServices;

namespace CefSharp.Internals
{
    public class Kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);
    }
}