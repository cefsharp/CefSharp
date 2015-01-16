using System.Windows.Media.Imaging;
using CefSharp.Internals;

namespace CefSharp.Wpf
{
	public abstract class WpfBitmapInfo : BitmapInfo
	{
		public abstract void Invalidate();
		public abstract BitmapSource CreateBitmap();
	}
}
