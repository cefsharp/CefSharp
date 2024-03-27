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
using CefSharp.Fluent;
using CefSharp.Wpf.Example.Handlers;
using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Experimental;
using CefSharp.Wpf.Experimental.Accessibility;

namespace CefSharp.Wpf.Example.Views
{
    public partial class BrowserTabView : UserControl
    {
        //Store draggable region if we have one - used for hit testing
        private Region region;

        public BrowserTabView()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;

            browser.UsePopupMouseTransform();

            //browser.BrowserSettings.BackgroundColor = Cef.ColorSetARGB(0, 255, 255, 255);

            //Please remove the comments below to use the Experimental WpfImeKeyboardHandler.
            //browser.WpfKeyboardHandler = new CefSharp.Wpf.Experimental.WpfImeKeyboardHandler(browser);

            //Please remove the comments below to specify the color of the CompositionUnderline.
            //var transparent = Colors.Transparent;
            //var black = Colors.Black;
            //ImeHandler.ColorBKCOLOR = Cef.ColorSetARGB(transparent.A, transparent.R, transparent.G, transparent.B);
            //ImeHandler.ColorUNDERLINE = Cef.ColorSetARGB(black.A, black.R, black.G, black.B);

            browser.RequestHandler = new ExampleRequestHandler();

            //Test Handler allow All Permission
            browser.PermissionHandler = new ExamplePermissionHandler();

            var bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                MethodInterceptor = new MethodInterceptorLogger(), // intercept .net methods calls from js and log it
#if !NETCOREAPP
                PropertyInterceptor = new PropertyInterceptorLogger()
#endif
            };

            //To use the ResolveObject below and bind an object with isAsync:false we must set CefSharpSettings.WcfEnabled = true before
            //the browser is initialized.
#if !NETCOREAPP
            CefSharpSettings.WcfEnabled = true;
#endif

            //If you call CefSharp.BindObjectAsync in javascript and pass in the name of an object which is not yet
            //bound, then ResolveObject will be called, you can then register it
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;

                //When JavascriptObjectRepository.Settings.LegacyBindingEnabled = true
                //This event will be raised with ObjectName == Legacy so you can bind your
                //legacy objects
#if NETCOREAPP
                if (e.ObjectName == "Legacy")
                {
                    repo.Register("boundAsync", new AsyncBoundObject(), options: bindingOptions);
                }
                else
                {
                    if (e.ObjectName == "boundAsync")
                    {
                        repo.Register("boundAsync", new AsyncBoundObject(), options: bindingOptions);
                    }
                    else if (e.ObjectName == "boundAsync2")
                    {
                        repo.Register("boundAsync2", new AsyncBoundObject(), options: bindingOptions);
                    }
                }
#else
                if (e.ObjectName == "Legacy")
                {
                    repo.Register("bound", new BoundObject(), isAsync: false, options: BindingOptions.DefaultBinder);
                    repo.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                }
                else
                {
                    if (e.ObjectName == "bound")
                    {
                        repo.Register("bound", new BoundObject(), isAsync: false, options: bindingOptions);
                    }
                    else if (e.ObjectName == "boundAsync")
                    {
                        repo.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                    }
                    else if (e.ObjectName == "boundAsync2")
                    {
                        repo.Register("boundAsync2", new AsyncBoundObject(), isAsync: true, options: bindingOptions);
                    }
                }
#endif
            };

            browser.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
            {
                var name = e.ObjectName;

                Debug.WriteLine($"Object {e.ObjectName} was bound successfully.");
            };

            browser.DisplayHandler = new DisplayHandler();
            // This LifeSpanHandler implementaion demos hosting a popup in a ChromiumWebBrowser
            // instance, it's still considered Experimental. The ChromiumWebBrowser
            // is shown in a new Window. This could just as easily be a Tab/ContentControl/etc
            /*
            browser.LifeSpanHandler = CefSharp.Wpf.Experimental.LifeSpanHandler
                .Create()
                .OnPopupCreated((ctrl, targetUrl, targetFrameName, windowInfo) =>
                {
                    var windowX = (windowInfo.X == int.MinValue) ? double.NaN : windowInfo.X;
                    var windowY = (windowInfo.Y == int.MinValue) ? double.NaN : windowInfo.Y;
                    var windowWidth = (windowInfo.Width == int.MinValue) ? double.NaN : windowInfo.Width;
                    var windowHeight = (windowInfo.Height == int.MinValue) ? double.NaN : windowInfo.Height;

                    var popup = new System.Windows.Window
                    {
                        Left = windowX,
                        Top = windowY,
                        Width = windowWidth,
                        Height = windowHeight,
                        Content = ctrl,
                        Owner = Window.GetWindow(browser),
                        Title = targetFrameName
                    };

                    popup.Closed += (o, e) =>
                    {
                        var w = o as System.Windows.Window;
                        if (w != null && w.Content is IWebBrowser)
                        {
                            (w.Content as IWebBrowser)?.Dispose();
                            w.Content = null;
                        }
                    };
                })
                .OnPopupBrowserCreated((ctrl, browser) =>
                {
                    ctrl.Dispatcher.Invoke(() =>
                    {
                        var owner = System.Windows.Window.GetWindow(ctrl);

                        if (owner != null && owner.Content == ctrl)
                        {
                            owner.Show();
                        }
                    });
                })
                .OnPopupDestroyed((ctrl, popupBrowser) =>
                {
                    //If browser is disposed then we don't need to remove the tab
                    if (!ctrl.IsDisposed)
                    {
                        var owner = System.Windows.Window.GetWindow(ctrl);

                        if (owner != null && owner.Content == ctrl)
                        {
                            owner.Close();
                        }
                    }
                }).Build();
            */

            browser.MenuHandler = new MenuHandler(addDevtoolsMenuItems:true);

            //Enable experimental Accessibility support 
            browser.AccessibilityHandler = new AccessibilityHandler(browser);
            browser.IsBrowserInitializedChanged += (sender, args) =>
            {
                if ((bool)args.NewValue)
                {
                    //Uncomment to enable support
                    //browser.GetBrowserHost().SetAccessibilityState(CefState.Enabled);
                }
            };

            browser.DownloadHandler = DownloadHandler
                .Create()
                .CanDownload((chromiumWebBrowser, browser, url, requestMethod) =>
                {
                    //All all downloads
                    return true;
                })
                .OnBeforeDownload((chromiumWebBrowser, browser, downloadItem, callback) =>
                {
                    UpdateDownloadAction("OnBeforeDownload", downloadItem);

                    callback.Continue("", showDialog: true);

                }).OnDownloadUpdated((chromiumWebBrowser, browser, downloadItem, callback) =>
                {
                    UpdateDownloadAction("OnDownloadUpdated", downloadItem);
                })
                .Build();
            browser.AudioHandler = new CefSharp.Handler.AudioHandler();
            browser.JsDialogHandler = new Handlers.JsDialogHandler();

            //Read an embedded bitmap into a memory stream then register it as a resource you can then load custom://cefsharp/images/beach.jpg
            var beachImageStream = new MemoryStream();
            CefSharp.Example.Properties.Resources.beach.Save(beachImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            browser.RegisterResourceHandler(CefExample.BaseUrl + "/images/beach.jpg", beachImageStream, Cef.GetMimeType("jpg"));

            var dragHandler = new DragHandler();
            dragHandler.RegionsChanged += OnDragHandlerRegionsChanged;

            browser.DragHandler = dragHandler;
            //browser.ResourceHandlerFactory = new InMemorySchemeAndResourceHandlerFactory();
            //You can specify a custom RequestContext to share settings amount groups of ChromiumWebBrowsers
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
                //Aborted is generally safe to ignore
                //Actions like starting a download will trigger an Aborted error
                //which doesn't require any user action.
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
                var errorHtml = string.Format("<html><body><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              args.FailedUrl, args.ErrorText, args.ErrorCode);

                _ = args.Browser.SetMainFrameDocumentContentAsync(errorHtml);

                //AddressChanged isn't called for failed Urls so we need to manually update the Url TextBox
                Dispatcher.InvokeAsync(() =>
                {
                    var viewModel = (BrowserTabViewModel)this.DataContext;
                    viewModel.AddressEditable = args.FailedUrl;
                });
            };

            CefExample.RegisterTestResources(browser);

            browser.JavascriptMessageReceived += OnBrowserJavascriptMessageReceived;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //TODO: Ideally we'd be able to bind this directly without having to use codebehind
            var viewModel = e.NewValue as BrowserTabViewModel;

            if (viewModel != null)
            {

                browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = viewModel.LegacyBindingEnabled;
            }
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

                if (msg.Type == "Update")
                {
                    var callback = msg.Callback;
                    var type = msg.Type;
                    var property = msg.Data.Property;

                    callback.ExecuteAsync(type);
                }
            }
            else if (e.Message is int)
            {
                e.Frame.ExecuteJavaScriptAsync("PostMessageIntTestCallback(" + (int)e.Message + ")");
            }

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
