using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace CefSharp.Example
{
    public class ExamplePresenter : IBeforeResourceLoad
    {
        public static void Init()
        {
            Settings settings = new Settings();
            if (CEF.Initialize(settings))
            {
                CEF.RegisterScheme("test", new SchemeHandlerFactory());
                CEF.RegisterJsObject("bound", new BoundObject());
            }
        }

        private const string resource_url = "http://test/resource/load";
        private const string scheme_url = "test://test/SchemeTest.html";
        private const string bind_url = "test://test/BindingTest.html";
        private const string tooltips_url = "test://test/TooltipsTest.html";

        private int color_index = 0;
        private readonly string[] colors =
        {
            "red",
            "blue",
            "green",
        };

        private readonly IWebBrowser model;
        private readonly IExampleView view;
        private readonly Action<Action> gui_invoke;

        public ExamplePresenter(IWebBrowser model, IExampleView view,
            Action<Action> gui_invoke)
        {
            this.model = model;
            this.view = view;
            this.gui_invoke = gui_invoke;

            model.BeforeResourceLoadHandler = this;

            this.model.PropertyChanged += model_PropertyChanged;
            model.ConsoleMessage += model_ConsoleMessage;

            this.view.UrlActivated += view_UrlActivated;
            this.view.ForwardActivated += view_ForwardActivated;
            this.view.BackActivated += view_BackActivated;
            this.view.TestResourceLoadActivated += view_TestResourceLoadActivated;
            this.view.TestSchemeLoadActivated += view_TestSchemeLoadActivated;
            this.view.TestExecuteScriptActivated += view_TestExecuteScriptActivated;
            this.view.TestEvaluateScriptActivated += view_TestEvaluateScriptActivated;
            this.view.TestBindActivated += view_TestBindActivated;
            this.view.TestConsoleMessageActivated += view_TestConsoleMessageActivated;
            this.view.TestTooltipsActivated += view_TestTooltipsActivated;
            this.view.ExitActivated += view_ExitActivated;
        }

        private void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string @string = null;
            bool @bool = false;

            switch (e.PropertyName)
            {
                case "Title":
                    @string = model.Title;
                    gui_invoke(() => view.SetTitle(@string));
                    break;
                case "TooltipText":
                    @string = model.TooltipText;
                    gui_invoke(() => view.SetTooltip(@string));
                    break;
                case "Address":
                    @string = model.Address;
                    gui_invoke(() => view.SetAddress(@string));
                    break;
                case "CanGoBack":
                    @bool = model.CanGoBack;
                    gui_invoke(() => view.SetCanGoBack(@bool));
                    break;
                case "CanGoForward":
                    @bool = model.CanGoForward;
                    gui_invoke(() => view.SetCanGoForward(@bool));
                    break;
                case "IsLoading":
                    @bool = model.IsLoading;
                    gui_invoke(() => view.SetIsLoading(@bool));
                    break; 
            }
        }

        private void model_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            gui_invoke(() => view.DisplayOutput(e.Message));
        }

        private void view_UrlActivated(object sender, string url)
        {
            model.Load(url);
        }

        private void view_BackActivated(object sender, EventArgs e)
        {
            model.Back();
        }

        private void view_ForwardActivated(object sender, EventArgs e)
        {
            model.Forward();
        }

        private void view_TestResourceLoadActivated(object sender, EventArgs e)
        {
            model.Load(resource_url);
        }

        private void view_TestSchemeLoadActivated(object sender, EventArgs e)
        {
            model.Load(scheme_url);
        }

        private void view_TestExecuteScriptActivated(object sender, EventArgs e)
        {
            var script = String.Format("document.body.style.background = '{0}'",
                colors[color_index++]);
            if (color_index >= colors.Length)
            {
                color_index = 0;
            }

            view.ExecuteScript(script);
        }

        private void view_TestEvaluateScriptActivated(object sender, EventArgs e)
        {
            var rand = new Random();
            var x = rand.Next(1, 10);
            var y = rand.Next(1, 10);

            var script = String.Format("{0} + {1}", x, y);
            var result = view.EvaluateScript(script);
            var output = String.Format("{0} => {1}", script, result);

            gui_invoke(() => view.DisplayOutput(output));
        }

        private void view_TestBindActivated(object sender, EventArgs e)
        {
            model.Load(bind_url);
        }

        private void view_TestConsoleMessageActivated(object sender, EventArgs e)
        {
            var script = "console.log('Hello, world!')";
            view.ExecuteScript(script);
        }

        private void view_TestTooltipsActivated(object sender, EventArgs e)
        {
            model.Load(tooltips_url);
        }

        private void view_ExitActivated(object sender, EventArgs e)
        {
            CEF.Shutdown();
            System.Environment.Exit(0);
        }

        void IBeforeResourceLoad.HandleBeforeResourceLoad(IWebBrowser browserControl,
            IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            if (request.Url.StartsWith(resource_url))
            {
                Stream resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                requestResponse.RespondWith(resourceStream, "text/html");
            }
        }
    }
}