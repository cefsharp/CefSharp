using System.Drawing;
using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    public class GdiBitmapInfo : BitmapInfo
    {
        public Bitmap Bitmap { get; set; }

        public override void ClearBitmap()
        {
            Bitmap = null;
        }
    }
}
