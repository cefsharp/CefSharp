// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// An offscreen instance of Chromium that you can use to take
    /// snapshots or evaluate JavaScript.
    /// </summary>
    public class ChromiumWebBrowser : IRenderWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

        /// <summary>Contains the last rendering from Chromium.</summary>
        private Bitmap bitmap;

        /// <summary>Need a lock because the caller may be asking for the bitmap
        /// while Chromium async rendering has returned on another thread.</summary>
        private readonly object bitmapLock = new object();

        /// <summary>Size of the Chromium viewport.
        /// 
        /// This must be set to something other than 0x0 otherwise Chromium will not render,
        /// and the ScreenshotAsync task will deadlock.</summary>
        private System.Drawing.Size size = new System.Drawing.Size(1366, 768);
        
        /// <summary>
        /// Create a new offscreen Chromium with the initial URL of "about:blank".
        /// </summary>
        /// <param name="browserSettings">The browser settings to use.  If null, the default settings are used.</param>
        public ChromiumWebBrowser(BrowserSettings browserSettings = null)
        {
            ResourceHandler = new DefaultResourceHandler();

            Cef.AddDisposable(this);

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);
            managedCefBrowserAdapter.CreateOffscreenBrowser(browserSettings ?? new BrowserSettings());
        }

        public void Dispose()
        {
            ResourceHandler = null;

            Cef.RemoveDisposable(this);

            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            if (managedCefBrowserAdapter != null)
            {
                if (!managedCefBrowserAdapter.IsDisposed)
                    managedCefBrowserAdapter.Dispose();
                managedCefBrowserAdapter = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get/set the size of the Chromium viewport, in pixels.
        /// 
        /// This also changes the size of the next screenshot.
        /// </summary>
        public System.Drawing.Size Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    managedCefBrowserAdapter.WasResized();
                }
            }
        }

        /// <summary>
        /// Fired by a separate thread when Chrome has re-rendered.
        /// This means that a Bitmap will be returned by ScreenshotOrNull().
        /// </summary>
        public event EventHandler NewScreenshot;

        /// <summary>
        /// Immediately returns a copy of the last rendering from Chrome,
        /// or null if no rendering has occurred yet.
        /// 
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        ///
        /// It is your responsibility to dispose the returned Bitmap.
        /// 
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        public Bitmap ScreenshotOrNull()
        {
            lock (bitmapLock)
            {
                if (bitmap == null)
                    return null;
                else
                    return new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Starts a task that waits for the next rendering from Chrome.
        /// 
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        /// 
        /// It is your responsibility to dispose the returned Bitmap.
        /// 
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        public Task<Bitmap> ScreenshotAsync()
        {
            // Try our luck and see if there is already a screenshot, to save us creating a new thread for nothing.
            var screenshot = ScreenshotOrNull();

            Task<Bitmap> task;

            if (screenshot != null)
            {
                var completionSource = new TaskCompletionSource<Bitmap>();
                completionSource.SetResult(screenshot);
                task = completionSource.Task;
            }
            else
            {
                // No existing screenshot, so wait for Chromium to render itself.

                // A wait handle, so the task knows when NewScreenshot has fired.
                var waitForBitmap = new EventWaitHandle(false, EventResetMode.AutoReset);

                EventHandler newScreenshot = null; // otherwise we cannot reference ourselves in the anonymous method below

                newScreenshot = (sender, e) =>
                {
                    // Chromium has rendered.  Tell the task about it.
                    NewScreenshot -= newScreenshot;
                    waitForBitmap.Set();
                };

                NewScreenshot += newScreenshot;

                task = new Task<Bitmap>(() =>
                {
                    // Wait in this thread for the NewScreenshot event to fire.
                    waitForBitmap.WaitOne();
                    return ScreenshotOrNull();
                });

                task.Start();
            }

            return task;
        }

        public IJsDialogHandler JsDialogHandler { get; set; }
        public IDialogHandler DialogHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }
        public IFocusHandler FocusHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDragHandler DragHandler { get; set; }
        public IResourceHandler ResourceHandler { get; set; }

        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler BrowserInitialized;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<NavStateChangedEventArgs> NavStateChanged;

        public void ShowDevTools()
        {
            throw new NotImplementedException();
        }

        public void CloseDevTools()
        {
            throw new NotImplementedException();
        }

        public string Address { get; set; }

        public bool CanGoBack { get; private set; }

        public bool CanGoForward { get; private set; }

        public Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout = null)
        {
            return managedCefBrowserAdapter.EvaluateScriptAsync(script, timeout);
        }

        public void ExecuteScriptAsync(string script)
        {
            managedCefBrowserAdapter.ExecuteScriptAsync(script);
        }

        public void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            managedCefBrowserAdapter.Find(identifier, searchText, forward, matchCase, findNext);
        }

        public void StopFinding(bool clearSelection)
        {
            managedCefBrowserAdapter.StopFinding(clearSelection);
        }

        public bool IsBrowserInitialized { get; private set; }

        public bool IsLoading { get; set; }

        public void Load(string url)
        {
            Address = url;
            managedCefBrowserAdapter.LoadUrl(Address);
        }

        public void LoadHtml(string html, string url)
        {
            LoadHtml(html, url, Encoding.UTF8);
        }

        public void LoadHtml(string html, string url, Encoding encoding)
        {
            var handler = ResourceHandler;
            if (handler == null)
            {
                throw new Exception("Implement IResourceHandler and assign to the ResourceHandler property to use this feature");
            }

            handler.RegisterHandler(url, CefSharp.ResourceHandler.FromString(html, encoding, true));

            Load(url);
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind);
        }

        public void Stop()
        {
            managedCefBrowserAdapter.Stop();
        }

        public string Title { get; set; }

        public string TooltipText { get; set; }

        public bool CanReload { get; private set; }

        public Task<string> GetSourceAsync()
        {
            var taskStringVisitor = new TaskStringVisitor();
            managedCefBrowserAdapter.GetSource(taskStringVisitor);
            return taskStringVisitor.Task;
        }

        public Task<string> GetTextAsync()
        {
            var taskStringVisitor = new TaskStringVisitor();
            managedCefBrowserAdapter.GetText(taskStringVisitor);
            return taskStringVisitor.Task;
        }

        bool IWebBrowser.Focus()
        {
            // no control to focus for offscreen browser
            return false;
        }

        public void Reload()
        {
            Reload(false);
        }

        public void Reload(bool ignoreCache)
        {
            managedCefBrowserAdapter.Reload(ignoreCache);
        }

        public void ViewSource()
        {
            managedCefBrowserAdapter.ViewSource();
        }

        public void Print()
        {
            managedCefBrowserAdapter.Print();
        }

        public void Back()
        {
            managedCefBrowserAdapter.GoBack();
        }

        public void Forward()
        {
            managedCefBrowserAdapter.GoForward();
        }

        public double ZoomLevel
        {
            get { return managedCefBrowserAdapter.GetZoomLevel(); }
            set { managedCefBrowserAdapter.SetZoomLevel(value); }
        }

        public void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY)
        {
            managedCefBrowserAdapter.OnMouseWheel(x, y, deltaX, deltaY);
        }

        #region IRenderWebBrowser (rendering to bitmap; derived from CefSharp.Wpf.ChromiumWebBrowser)
        /// <summary>The bitmap buffer is 32 BPP.</summary>
        int IRenderWebBrowser.BytesPerPixel
        {
            get { return 4; }
        }

        void IRenderWebBrowser.ClearBitmap(BitmapInfo bitmapInfo)
        {
            lock (bitmapLock) 
            {
                if (bitmap != null)
                    bitmap.Dispose();
                bitmap = null;
            }
        }

        int IRenderWebBrowser.Width
        {
            get { return size.Width; }
        }

        int IRenderWebBrowser.Height
        {
            get { return size.Height; }
        }

        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            ((IRenderWebBrowser)this).SetBitmap(bitmapInfo);
        }

        void IRenderWebBrowser.SetBitmap(BitmapInfo bitmapInfo)
        {
            lock (bitmapLock)
            {
                ((IRenderWebBrowser)this).ClearBitmap(bitmapInfo);
                lock (bitmapInfo.BitmapLock)
                {
                    var stride = bitmapInfo.Width * ((IRenderWebBrowser)this).BytesPerPixel;

                    bitmap = BitmapSourceToBitmap2(Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                        bitmapInfo.Width, bitmapInfo.Height, PixelFormats.Bgra32, stride, 0));

                    var handler = NewScreenshot;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// http://stackoverflow.com/a/5709472/450141
        /// </summary>
        /// <param name="srs">BitmapSource</param>
        /// <returns>Bitmap</returns>
        private static Bitmap BitmapSourceToBitmap2(BitmapSource srs)
        {
            var bmp = new Bitmap(
              srs.PixelWidth,
              srs.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            srs.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        void IRenderWebBrowser.SetCursor(IntPtr cursor)
        {
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool show)
        {
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
        }
        #endregion

        #region IWebBrowserInternal (notifications from CEF to C#; derived from CefSharp.Wpf.ChromiumWebBrowser)
        void IWebBrowserInternal.OnConsoleMessage(string message, string source, int line)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, new ConsoleMessageEventArgs(message, source, line));
            }
        }

        void IWebBrowserInternal.OnFrameLoadStart(string url, bool isMainFrame)
        {
            var handler = FrameLoadStart;
            if (handler != null)
            {
                handler(this, new FrameLoadStartEventArgs(url, isMainFrame));
            }
        }

        void IWebBrowserInternal.OnFrameLoadEnd(string url, bool isMainFrame, int httpStatusCode)
        {
            var handler = FrameLoadEnd;
            if (handler != null)
            {
                handler(this, new FrameLoadEndEventArgs(url, isMainFrame, httpStatusCode));
            }
        }

        void IWebBrowserInternal.OnInitialized()
        {
            IsBrowserInitialized = true;

            var handler = BrowserInitialized;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void IWebBrowserInternal.OnLoadError(string url, CefErrorCode errorCode, string errorText)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, new LoadErrorEventArgs(url, errorCode, errorText));
            }
        }

        void IWebBrowserInternal.OnStatusMessage(string value)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, new StatusMessageEventArgs(value));
            }
        }

        void IWebBrowserInternal.SetAddress(string address)
        {
            this.Address = address;
        }

        void IWebBrowserInternal.SetIsLoading(bool isloading)
        {
            IsLoading = isloading;
        }

        void IWebBrowserInternal.SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;

            var handler = NavStateChanged;
            if (handler != null)
            {
                handler(this, new NavStateChangedEventArgs(canGoBack, canGoForward, canReload));
            }
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            Title = title;
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }
        #endregion
    }
}
