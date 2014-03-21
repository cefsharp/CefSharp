namespace CefSharp
{
    public abstract class CefAppBase : ObjectBase
    {
        public abstract void OnBrowserCreated(CefBrowserBase cefBrowserWrapper);
    }
}
