using System;
using System.IO;
using System.Text;

namespace CefSharp.Example
{
    public class ExamplePresenter : IRequestHandler
    {
        private static readonly Uri resource_url = new Uri("http://test/resource/load");

        private readonly IWebBrowser model;
        private readonly Action<Action> uiThreadInvoke;

        public ExamplePresenter(IWebBrowser model, Action<Action> uiThreadInvoke)
        {
            this.model = model;
            this.uiThreadInvoke = uiThreadInvoke;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
                Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            //view.DisplayOutput(version);

            model.RequestHandler = this;
            //model.PropertyChanged += OnModelPropertyChanged;
            //model.ConsoleMessage += OnModelConsoleMessage;

            //// file
            //view.ShowDevToolsActivated += OnViewShowDevToolsActivated;
            //view.CloseDevToolsActivated += OnViewCloseDevToolsActivated;
            //view.ExitActivated += OnViewExitActivated;

            //// edit
            //view.UndoActivated += OnViewUndoActivated;
            //view.RedoActivated += OnViewRedoActivated;
            //view.CutActivated += OnViewCutActivated;
            //view.CopyActivated += OnViewCopyActivated;
            //view.PasteActivated += OnViewPasteActivated;
            //view.DeleteActivated += OnViewDeleteActivated;
            //view.SelectAllActivated += OnViewSelectAllActivated;

            //// test
            //view.TestResourceLoadActivated += OnViewTestResourceLoadActivated;
            //view.TestSchemeLoadActivated += OnViewTestSchemeLoadActivated;
            //view.TestExecuteScriptActivated += OnViewTestExecuteScriptActivated;
            //view.TestEvaluateScriptActivated += OnViewTestEvaluateScriptActivated;
            //view.TestBindActivated += OnViewTestBindActivated;
            //view.TestConsoleMessageActivated += OnViewTestConsoleMessageActivated;
            //view.TestTooltipActivated += OnViewTestTooltipActivated;
            //view.TestPopupActivated += OnViewTestPopupActivated;
            //view.TestLoadStringActivated += OnViewTestLoadStringActivated;
            //view.TestCookieVisitorActivated += OnViewTestCookieVisitorActivated;

            //// navigation
            // TODO: This one might actually be needed.
            //view.UrlActivated += OnViewUrlActivated;
            
            //view.ForwardActivated += OnViewForwardActivated;
            //view.BackActivated += OnViewBackActivated;
        }

        //private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case "Title":
        //            uiThreadInvoke(() => view.SetTitle(model.Title));
        //            break;

        //        case "Address":
        //            uiThreadInvoke(() => view.SetAddress(model.Address));
        //            break;

        //        case "CanGoBack":
        //            uiThreadInvoke(() => view.SetCanGoBack(model.CanGoBack));
        //            break;

        //        case "CanGoForward":
        //            uiThreadInvoke(() => view.SetCanGoForward(model.CanGoForward));
        //            break;

        //        case "IsLoading":
        //            uiThreadInvoke(() => view.SetIsLoading(model.IsLoading));
        //            break;
        //    }
        //}

    //    private void OnModelConsoleMessage(object sender, ConsoleMessageEventArgs e)
    //    {
    //        uiThreadInvoke(() => view.DisplayOutput(e.Message));
    //    }

    //    private void OnViewShowDevToolsActivated(object sender, EventArgs e)
    //    {
    //        model.ShowDevTools();
    //    }

    //    private void OnViewCloseDevToolsActivated(object sender, EventArgs e)
    //    {
    //        model.CloseDevTools();
    //    }

    //    private void OnViewExitActivated(object sender, EventArgs e)
    //    {
    //        var disposableModel = model as IDisposable;

    //        if (disposableModel != null)
    //        {
    //            disposableModel.Dispose();
    //        }

    //        Cef.Shutdown();
    //        Environment.Exit(0);
    //    }

    //    void OnViewUndoActivated(object sender, EventArgs e)
    //    {
    //        model.Undo();
    //    }

    //    void OnViewRedoActivated(object sender, EventArgs e)
    //    {
    //        model.Redo();
    //    }

    //    void OnViewCutActivated(object sender, EventArgs e)
    //    {
    //        model.Cut();
    //    }

    //    void OnViewCopyActivated(object sender, EventArgs e)
    //    {
    //        model.Copy();
    //    }

    //    void OnViewPasteActivated(object sender, EventArgs e)
    //    {
    //        model.Paste();
    //    }

    //    void OnViewDeleteActivated(object sender, EventArgs e)
    //    {
    //        model.Delete();
    //    }

    //    void OnViewSelectAllActivated(object sender, EventArgs e)
    //    {
    //        model.SelectAll();
    //    }

    //    private void OnViewTestResourceLoadActivated(object sender, EventArgs e)
    //    {
    //        //model.Load(resource_url);
    //    }

    //    private void OnViewTestSchemeLoadActivated(object sender, EventArgs e)
    //    {
    //        //model.Load(scheme_url);
    //    }

    //    private void OnViewTestExecuteScriptActivated(object sender, EventArgs e)
    //    {
    //        var script = String.Format("document.body.style.background = '{0}'",
    //            colors[color_index++]);
    //        if (color_index >= colors.Length)
    //        {
    //            color_index = 0;
    //        }

    //        view.ExecuteScript(script);
    //    }

    //    private void OnViewTestEvaluateScriptActivated(object sender, EventArgs e)
    //    {
    //        var rand = new Random();
    //        var x = rand.Next(1, 10);
    //        var y = rand.Next(1, 10);

    //        var script = String.Format("{0} + {1}", x, y);
    //        var result = view.EvaluateScript(script);
    //        var output = String.Format("{0} => {1}", script, result);

    //        uiThreadInvoke(() => view.DisplayOutput(output));
    //    }

    //    private void OnViewTestBindActivated(object sender, EventArgs e)
    //    {
    //        //model.Load(bind_url);
    //    }

    //    private void OnViewTestConsoleMessageActivated(object sender, EventArgs e)
    //    {
    //        var script = "console.log('Hello, world!')";
    //        view.ExecuteScript(script);
    //    }

    //    private void OnViewTestTooltipActivated(object sender, EventArgs e)
    //    {
    //        //model.Load(tooltip_url);
    //    }

    //    private void OnViewTestPopupActivated(object sender, EventArgs e)
    //    {
    //        //model.Load(popup_url);
    //    }

    //    private void OnViewTestLoadStringActivated(object sender, EventArgs e)
    //    {
    //        model.LoadHtml(string.Format("<html><body><a href='{0}'>CefSharp Home</a></body></html>", DefaultUrl));
    //    }

    //    private void OnViewTestCookieVisitorActivated(object sender, EventArgs e)
    //    {
    //        Cef.VisitAllCookies(this);
    //    }

        // TODO: This one might be needed...
        //private void OnViewUrlActivated(object sender, string address)
        //{
        //    model.Load(address);
        //}

    //    private void OnViewBackActivated(object sender, EventArgs e)
    //    {
    //        model.Back();
    //    }

    //    private void OnViewForwardActivated(object sender, EventArgs e)
    //    {
    //        model.Forward();
    //    }

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            // here you could do fun stuff when a plugin crashes
        }

        bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith(resource_url.ToString()))
            {
                Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                requestResponse.RespondWith(resourceStream, "text/html");
            }

            return false;
        }

        bool IRequestHandler.GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler)
        {
            handler = new DownloadHandler();
            return true;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policy_url, IWebPluginInfo info)
        {
            bool blockPluginLoad = false;

            // Enable next line to demo: Block any plugin with "flash" in its name
            // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y 
            //blockPluginLoad = info.Name.ToLower().Contains("flash");
            return blockPluginLoad;
        }

    //    bool ICookieVisitor.Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
    //    {
    //        Console.WriteLine("Cookie #{0}: {1}", count, cookie.Name);
    //        return true;
    //    }
    }
}
