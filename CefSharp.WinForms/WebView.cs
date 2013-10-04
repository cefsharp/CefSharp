// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace CefSharp.WinForms
{
    public class WebView : Control, IWebBrowserInternal, IRenderWebBrowser
    {
        private BrowserCore browserCore;
        private BrowserWrapper browserWrapper;
        private bool isOffscreenBrowserCreated;

        public BrowserSettings BrowserSettings { get; set; }

        public event ConsoleMessageEventHandler ConsoleMessage;
        public event PropertyChangedEventHandler PropertyChanged;
        public event LoadCompletedEventHandler LoadCompleted;
        public event LoadErrorEventHandler LoadError;

        public bool IsBrowserInitialized { get; private set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }

        public IntPtr FileMappingHandle { get; set; }

        public string Address
        {
            get
            {
                return browserCore.Address;
            }
            set
            {
                browserCore.Address = value;
            }
        }
        bool IWebBrowser.CanGoForward {
            get { return browserCore.CanGoForward;}
        }
        bool IWebBrowser.CanGoBack {
            get { return browserCore.CanGoBack; }
        }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string TooltipText { get; set; }

        public int BytesPerPixel
        {
            get { return PixelFormats.Bgr32.BitsPerPixel / 8; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            browserWrapper = new BrowserWrapper(this);
            browserWrapper.CreateBrowser(BrowserSettings ?? new BrowserSettings(), Handle, browserCore.Address);
            isOffscreenBrowserCreated = true;
        }

        private void OnAddressChanged()
        {

            if (!Cef.IsInitialized &&
                !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            if (browserCore == null)
            {
                browserCore = new BrowserCore(Address);
                browserCore.PropertyChanged += OnBrowserCorePropertyChanged;

                // TODO: Consider making the delay here configurable.
                //tooltipTimer = new DispatcherTimer(
                //    TimeSpan.FromSeconds(0.5),
                //    DispatcherPriority.Render,
                //    OnTooltipTimerTick,
                //    Dispatcher
                //);
            }

            browserCore.Address = browserCore.Address;

            if (isOffscreenBrowserCreated)
            {
                //browserWrapper.LoadUrl(Address);
            }
            else
            {
                //CreateOffscreenBrowser();
            }
        }

        private void OnBrowserCorePropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Address")
            {
                Address = browserCore.Address;
            }
        }

        public WebView(string address, BrowserSettings settings)
        {
            browserCore = new BrowserCore(address);
        }

        public void ExecuteScript(string script)
        {
            //cefBrowserWrapper.ExecuteScript(script);
        }

        public object EvaluateScript(string script)
        {
            return EvaluateScript(script, timeout: null);
        }

        public object EvaluateScript(string script, TimeSpan? timeout)
        {
            if (timeout == null)
            {
                timeout = TimeSpan.MaxValue;
            }

            //return cefBrowserWrapper.EvaluateScript(script, timeout.Value);
            return null;
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> GetBoundObjects()
        {
            throw new NotImplementedException();
        }

        public void ViewSource()
        {
            //cefBrowserWrapper.ViewSource();
        }

        public void LoadHtml(string html, string url)
        {
            //cefBrowserWrapper.LoadHtml(html, url);
        }

        public void Load(String url)
        {
            browserWrapper.LoadUrl(url);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void Back()
        {
            //cefBrowserWrapper.GoBack();
        }

        private bool CanGoBack()
        {
            return browserCore.CanGoBack;
        }

        private void Forward()
        {
            //cefBrowserWrapper.GoForward();
        }

        private bool CanGoForward()
        {
            return browserCore.CanGoForward;
        }

        public void Reload(bool ignoreCache)
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void ClearHistory()
        {
            throw new NotImplementedException();
        }

        public void ShowDevTools()
        {
            //var devToolsUrl = cefBrowserWrapper.DevToolsUrl;
            throw new NotImplementedException();
        }

        public void CloseDevTools()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void Cut()
        {
            throw new NotImplementedException();
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void SelectAll()
        {
            throw new NotImplementedException();
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        public void OnFrameLoadStart(string url)
        {
            browserCore.OnFrameLoadStart();
        }

        public void OnFrameLoadEnd(string url)
        {
            browserCore.OnFrameLoadEnd();

            if (LoadCompleted != null)
            {
                LoadCompleted(this, new LoadCompletedEventArgs(url));
            }
        }

        public void OnTakeFocus(bool next)
        {
            throw new NotImplementedException();
        }

        public void OnConsoleMessage(string message, string source, int line)
        {
            if (ConsoleMessage != null)
            {
                ConsoleMessage(this, new ConsoleMessageEventArgs(message, source, line));
            }
        }

        public void OnLoadError(string url, CefErrorCode errorCode, string errorText)
        {
            if (LoadError != null)
            {
                LoadError(url, errorCode, errorText);
            }
        }

        public void SetNavState(bool canGoBack, bool canGoForward)
        {
            browserCore.SetNavState(canGoBack, canGoForward);
        }

        public void OnInitialized()
        {
            browserCore.OnInitialized();
        }

        public void ClearBitmap()
        {
            //interopBitmap = null;
        }

        public void SetBitmap()
        {
            //var bitmap = interopBitmap;

            //lock (cefBrowserWrapper.BitmapLock)
            //{
            //    //if (bitmap == null)
            //    //{
            //    //    image.Source = null;
            //    //    GC.Collect(1);

            //    //    var stride = cefBrowserWrapper.BitmapWidth * BytesPerPixel;

            //    //    bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(FileMappingHandle,
            //    //        cefBrowserWrapper.BitmapWidth, cefBrowserWrapper.BitmapHeight, PixelFormats.Bgr32, stride, 0);
            //    //    image.Source = bitmap;
            //    //    interopBitmap = bitmap;
            //    //}

            //    interopBitmap.Invalidate();
            //}
        }

        public void SetAddress(string address)
        {
            browserCore.Address = address;
            PropertyChanged(this, new PropertyChangedEventArgs("Address"));
        }

        public void InvokeRenderAsync(Action callback)
        {

        }

        public void SetCursor(IntPtr handle)
        {
            //if (!Dispatcher.CheckAccess())
            //{
            //    Dispatcher.BeginInvoke((Action<IntPtr>)SetCursor, handle);
            //    return;
            //}

            //Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        public void SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }
    }
}
