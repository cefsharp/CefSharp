using System;
using System.ComponentModel;

namespace CefSharp.Example
{
    public class ExamplePresenter
    {
        private readonly IWebBrowser model;
        private readonly IExampleView view;
        private readonly Action<Action> gui_invoke;

        public ExamplePresenter(IWebBrowser model, IExampleView view,
            Action<Action> gui_invoke)
        {
            this.model = model;
            this.view = view;
            this.gui_invoke = gui_invoke;

            this.model.PropertyChanged += model_PropertyChanged;

            this.view.ExitActivated += view_ExitActivated;
            this.view.ForwardActivated += view_ForwardActivated;
            this.view.BackActivated += view_BackActivated;
            this.view.UrlActivated += view_UrlActivated;
        }

        private void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string @string;
            bool @bool;

            switch (e.PropertyName)
            {
                case "Title":
                    @string = model.Title;
                    gui_invoke(() => view.SetTitle(@string));
                    break;
                case "Tooltip":
                    @string = model.Tooltip;
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

        private void view_ExitActivated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}