namespace CefSharp.Internals
{
    public abstract class ManagedCefApp : DisposableResource
    {
        public abstract void OnBrowserCreated(CefBrowserBase cefBrowserWrapper);
    }
}
