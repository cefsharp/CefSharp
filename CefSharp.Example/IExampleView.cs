using System;

namespace CefSharp.Example
{
    public interface IExampleView
    {
        event Action<object, string> UrlActivated;
        event EventHandler BackActivated;
        event EventHandler ForwardActivated;
        event EventHandler TestResourceLoadActivated;
        event EventHandler TestSchemeLoadActivated;
        event EventHandler TestExecuteScriptActivated;
        event EventHandler TestEvaluateScriptActivated;
        event EventHandler TestBindActivated;
        event EventHandler TestConsoleMessageActivated;
        event EventHandler ExitActivated;

        void SetTitle(string title);
        void SetTooltip(string tooltip);
        void SetAddress(string address);
        void SetCanGoBack(bool can_go_back);
        void SetCanGoForward(bool can_go_forward);
        void SetIsLoading(bool is_loading);

        void ExecuteScript(string script);
        object EvaluateScript(string script);
        void DisplayOutput(string output);
    }
}