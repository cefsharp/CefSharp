using System;

namespace CefSharp.Example
{
    public interface IExampleView
    {
        event Action<object, string> UrlActivated;
        event EventHandler BackActivated;
        event EventHandler ForwardActivated;
        event EventHandler ExitActivated;

        void SetTitle(string title);
        void SetTooltip(string tooltip);
        void SetAddress(string address);
        void SetCanGoBack(bool can_go_back);
        void SetCanGoForward(bool can_go_forward);
        void SetIsLoading(bool is_loading);
    }
}