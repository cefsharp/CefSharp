namespace CefSharp.Internals
{
    public abstract class ManagedCefApp : DisposableResource
    {
        public virtual void OnBrowserCreated(CefBrowserBase cefBrowserWrapper)
        {
        }
    }
}
