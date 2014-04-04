namespace CefSharp
{
    public abstract class CefBrowserBase : DisposableResource
    {
        public int BrowserId { get; set; }

        public abstract object EvaluateScript(int frameId, string script, double timeout);
    }
}
