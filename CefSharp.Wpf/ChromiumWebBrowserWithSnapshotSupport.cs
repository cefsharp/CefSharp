using CefSharp.Internals;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf
{
    public class ChromiumWebBrowserWithSnapshotSupport : ChromiumWebBrowser
    {
        private bool _isTakingSnapshot = false;
        private int _snapshotWidth;
        private int _snapshotHeight;
        private string _snapshotFile;
        private ManualResetEvent _snapshotIsComplete = new ManualResetEvent(true);

        public bool TakeSnapshot(out string imagePath)
        {
            imagePath = null;
            bool result = false;

            if (!WebBrowser.IsBrowserInitialized || WebBrowser.IsLoading || _isTakingSnapshot)
                return result;

            try
            {
                result = Task.Factory.StartNew(() =>
                {
                    var size = Convert.ToString(EvaluateScript(
                        "''.concat(document.body.scrollHeight, ':', document.body.scrollWidth)"));
                    var dimensions = size.Split(':').Select(int.Parse);
                    _snapshotHeight = dimensions.First();
                    _snapshotWidth = dimensions.Last();

                    _snapshotFile = Path.GetTempFileName() + ".jpg";
                    _isTakingSnapshot = true;
                    _snapshotIsComplete.Reset();
                    OnActualSizeChanged(this, EventArgs.Empty);
                    _snapshotIsComplete.WaitOne();
                }).Wait(5000);

                if (result)
                    imagePath = _snapshotFile;
            }
            finally
            {
                _isTakingSnapshot = false;
                OnActualSizeChanged(this, EventArgs.Empty);
            }

            return result;
        }

        private void SaveBitmapAsJpeg(BitmapSource bitmapSource, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
            }
        }

        #region Overridden members

        protected override int WidthToRender
        {
            get
            {
                if (_isTakingSnapshot)
                    return _snapshotWidth;
                else
                    return base.WidthToRender;
            }
        }

        protected override int HeightToRender
        {
            get
            {
                if (_isTakingSnapshot)
                    return _snapshotHeight;
                else
                    return base.HeightToRender;
            }
        }

        protected override void InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            if (_isTakingSnapshot)
                SetSnapshotBitmap(bitmapInfo);
            else
                base.InvokeRenderAsync(bitmapInfo);
        }

        protected void SetSnapshotBitmap(BitmapInfo bitmapInfo)
        {
            var stride = bitmapInfo.Width * ((IRenderWebBrowser)this).BytesPerPixel;

            lock (bitmapInfo.BitmapLock)
            {
                var snapshotBitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                    bitmapInfo.Width, bitmapInfo.Height, PixelFormat, stride, 0);
                SaveBitmapAsJpeg(snapshotBitmap, _snapshotFile);
                _snapshotIsComplete.Set();
            }
        }

        #endregion Overridden members
    }
}
