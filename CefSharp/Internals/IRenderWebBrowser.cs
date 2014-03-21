using System;

namespace CefSharp.Internals
{
    public interface IRenderWebBrowser : IWebBrowserInternal
    {
        int BytesPerPixel { get; }

        int Width { get; }
        int Height { get; }

        void InvokeRenderAsync(BitmapInfo bitmapInfo);

        void SetCursor(IntPtr cursor);

        void ClearBitmap(BitmapInfo bitmapInfo);
        void SetBitmap(BitmapInfo bitmapInfo);

        void SetPopupIsOpen(bool show);
        void SetPopupSizeAndPosition(int width, int height, int x, int y);
    };
}
