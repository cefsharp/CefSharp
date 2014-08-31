using CefSharp.Internals;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf.Example.Controls
{
    public class ChromiumWebBrowserWithSnapshotSupport : ChromiumWebBrowser
    {
        private volatile bool _isTakingSnapshot = false;
        private volatile int _snapshotWidth;
        private volatile int _snapshotHeight;
        private volatile string _snapshotFile;
        private ManualResetEventSlim _snapshotIsComplete = new ManualResetEventSlim(true);
        private readonly object _takingSnapshotLock = new object();

        public bool TakeSnapshot(out string imagePath, TimeSpan? timeout = null)
        {
            imagePath = null;
            bool result = false;

            if (!WebBrowser.IsBrowserInitialized || WebBrowser.IsLoading || _isTakingSnapshot)
                return result;

            var cancellationTokenSrc = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSrc.Token;
            try
            {
                var task = Task<string>.Factory.StartNew(() =>
                {
                    var size = Convert.ToString(EvaluateScript(
                        "''.concat(document.body.scrollHeight, ':', document.body.scrollWidth)", timeout));
                    cancellationToken.ThrowIfCancellationRequested();

                    var dimensions = size.Split(':').Select(int.Parse);
                    var tmpFileName = Path.GetTempFileName();
                    lock (_takingSnapshotLock)
                    {
                        _snapshotHeight = dimensions.First();
                        _snapshotWidth = dimensions.Last();

                        _snapshotFile = tmpFileName + ".png";
                        _snapshotIsComplete.Reset();

                        _isTakingSnapshot = !cancellationToken.IsCancellationRequested;
                        // it may take some time waiting on a lock
                        cancellationToken.ThrowIfCancellationRequested();
                        OnActualSizeChanged(this, EventArgs.Empty);
                        _snapshotIsComplete.Wait(cancellationToken);
                        return _snapshotFile;
                    }
                }, cancellationToken);

                if (timeout.HasValue)
                    result = task.Wait(timeout.Value);
                else
                {
                    task.Wait();
                    result = true;
                }

                if (result)
                    imagePath = task.Result;
                else
                    cancellationTokenSrc.Cancel();
            }
            finally
            {
                _isTakingSnapshot = false;
                OnActualSizeChanged(this, EventArgs.Empty);
            }

            return result;
        }

        private void SaveBitmapAsPng(BitmapSource bitmapSource, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
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
            {
                if (bitmapInfo.Height == _snapshotHeight && bitmapInfo.Width == _snapshotWidth)
                    SetSnapshotBitmap(bitmapInfo);
                else
                    OnActualSizeChanged(this, EventArgs.Empty);
            }
            else
            {
                base.InvokeRenderAsync(bitmapInfo);
            }
        }

        protected void SetSnapshotBitmap(BitmapInfo bitmapInfo)
        {
            var stride = bitmapInfo.Width * ((IRenderWebBrowser)this).BytesPerPixel;

            lock (bitmapInfo.BitmapLock)
            {
                var snapshotBitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                    bitmapInfo.Width, bitmapInfo.Height, PixelFormat, stride, 0);
                SaveBitmapAsPng(snapshotBitmap, _snapshotFile);
                _snapshotIsComplete.Set();
            }
        }

        #endregion Overridden members
    }
}
