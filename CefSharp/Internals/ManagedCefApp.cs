namespace CefSharp.Internals
{
    public abstract class ManagedCefApp : ObjectBase
    {
        public abstract void OnBrowserCreated(CefBrowserBase cefBrowserWrapper);
    }
}
