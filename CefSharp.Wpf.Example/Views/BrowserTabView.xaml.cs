// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Example.JavascriptBinding;
using CefSharp.Example.ModelBinding;
using CefSharp.Example.PostMessage;
using CefSharp.Wpf.Example.Handlers;
using CefSharp.Wpf.Example.ViewModels;

namespace CefSharp.Wpf.Example.Views
{
    public partial class BrowserTabView : UserControl
    {
        //Store draggable region if we have one - used for hit testing
        private Region region;

        public BrowserTabView()
        {
            InitializeComponent();

            //browser.BrowserSettings.BackgroundColor = Cef.ColorSetARGB(0, 255, 255, 255);

            browser.RequestHandler = new ExampleRequestHandler();

            var bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                MethodInterceptor = new MethodInterceptorLogger() // intercept .net methods calls from js and log it
            };

            //To use the ResolveObject below and bind an object with isAsync:false we must set CefSharpSettings.WcfEnabled = true before
            //the browser is initialized.
            CefSharpSettings.WcfEnabled = true;

            //If you call CefSharp.BindObjectAsync in javascript and pass in the name of an object which is not yet
            //bound, then ResolveObject will be called, you can then register it
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "boundAsync2")
                {
                    repo.Register("boundAsync2", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                }
                else if (e.ObjectName == "bound")
                {
                    browser.JavascriptObjectRepository.Register("bound", new BoundObject(), isAsync: false, options: BindingOptions.DefaultBinder);
                }
                else if (e.ObjectName == "boundAsync")
                {
                    browser.JavascriptObjectRepository.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                }
            };

            browser.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
            {
                var name = e.ObjectName;

                Debug.WriteLine($"Object {e.ObjectName} was bound successfully.");
            };

            browser.DisplayHandler = new DisplayHandler();
            //This LifeSpanHandler implementaion demos hosting a popup in a ChromiumWebBrowser
            //instance, it's still considered Experimental
            //browser.LifeSpanHandler = new ExperimentalLifespanHandler();
            browser.MenuHandler = new MenuHandler();
            browser.AccessibilityHandler = new AccessibilityHandler();
            var downloadHandler = new DownloadHandler();
            downloadHandler.OnBeforeDownloadFired += OnBeforeDownloadFired;
            downloadHandler.OnDownloadUpdatedFired += OnDownloadUpdatedFired;
            browser.DownloadHandler = downloadHandler;

            //Read an embedded bitmap into a memory stream then register it as a resource you can then load custom://cefsharp/images/beach.jpg
            var beachImageStream = new MemoryStream();
            CefSharp.Example.Properties.Resources.beach.Save(beachImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            browser.RegisterResourceHandler(CefExample.BaseUrl + "/images/beach.jpg", beachImageStream, ResourceHandler.GetMimeType(".jpg"));

            var dragHandler = new DragHandler();
            dragHandler.RegionsChanged += OnDragHandlerRegionsChanged;

            browser.DragHandler = dragHandler;
            //browser.ResourceHandlerFactory = new InMemorySchemeAndResourceHandlerFactory();
            //You can specify a custom RequestContext to share settings amount groups of ChromiumWebBrowsers
            //Also this is now the only way to access OnBeforePluginLoad - need to implement IRequestContextHandler
            //browser.RequestContext = new RequestContext(new RequestContextHandler());
            //NOTE - This is very important for this example as the default page will not load otherwise
            //browser.RequestContext.RegisterSchemeHandlerFactory(CefSharpSchemeHandlerFactory.SchemeName, null, new CefSharpSchemeHandlerFactory());
            //browser.RequestContext.RegisterSchemeHandlerFactory("https", "cefsharp.example", new CefSharpSchemeHandlerFactory());

            //You can start setting preferences on a RequestContext that you created straight away, still needs to be called on the CEF UI thread.
            //Cef.UIThreadTaskFactory.StartNew(delegate
            //{
            //    string errorMessage;
            //    //Use this to check that settings preferences are working in your code

            //    var success = browser.RequestContext.SetPreference("webkit.webprefs.minimum_font_size", 24, out errorMessage);
            //});             

            browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();

            browser.LoadError += (sender, args) =>
            {
                // Don't display an error for downloaded files.
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                //Don't display an error for external protocols that we allow the OS to
                //handle in OnProtocolExecution().
                if (args.ErrorCode == CefErrorCode.UnknownUrlScheme && args.Frame.Url.StartsWith("mailto"))
                {
                    return;
                }

                // Display a load error message.
                var errorBody = string.Format("<html><body bgcolor=\"white\"><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              args.FailedUrl, args.ErrorText, args.ErrorCode);

                args.Frame.LoadHtml(errorBody, base64Encode: true);
            };

            CefExample.RegisterTestResources(browser);

            browser.JavascriptMessageReceived += OnBrowserJavascriptMessageReceived;
        }

        private void OnBrowserJavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            //Complext objects are initially expresses as IDicionary (in reality it's an ExpandoObject so you can use dynamic)
            if (typeof(System.Dynamic.ExpandoObject).IsAssignableFrom(e.Message.GetType()))
            {
                //You can use dynamic to access properties
                //dynamic msg = e.Message;
                //Alternatively you can use the built in Model Binder to convert to a custom model
                var msg = e.ConvertMessageTo<PostMessageExample>();
                var callback = msg.Callback;
                var type = msg.Type;
                var property = msg.Data.Property;

                callback.ExecuteAsync(type);
            }
            else if (e.Message is int)
            {
                e.Frame.ExecuteJavaScriptAsync("PostMessageIntTestCallback(" + (int)e.Message + ")");
            }

        }

        private void OnBeforeDownloadFired(object sender, DownloadItem e)
        {
            this.UpdateDownloadAction("OnBeforeDownload", e);
        }

        private void OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            this.UpdateDownloadAction("OnDownloadUpdated", e);
        }

        private void UpdateDownloadAction(string downloadAction, DownloadItem downloadItem)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                var viewModel = (BrowserTabViewModel)this.DataContext;
                viewModel.LastDownloadAction = downloadAction;
                viewModel.DownloadItem = downloadItem;
            });
        }

        private void OnBrowserMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(browser);

            if (region.IsVisible((float)point.X, (float)point.Y))
            {
                var window = Window.GetWindow(this);
                window.DragMove();

                e.Handled = true;
            }
        }

        private void OnDragHandlerRegionsChanged(Region region)
        {
            if (region != null)
            {
                //Only wire up event handler once
                if (this.region == null)
                {
                    browser.PreviewMouseLeftButtonDown += OnBrowserMouseLeftButtonDown;
                }

                this.region = region;
            }
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void OnTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

    }
}
