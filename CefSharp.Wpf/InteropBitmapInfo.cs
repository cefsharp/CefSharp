using System.Windows.Interop;
using CefSharp.Internals;

namespace CefSharp.Wpf
{
    public class InteropBitmapInfo : BitmapInfo
    {
        // Cannot be InteropBitmap since we really don't want CefSharp to be dependent on WPF libraries.
        public InteropBitmap InteropBitmap { get; set; }

        public override void ClearBitmap()
        {
            InteropBitmap = null;
        }
    }
}
