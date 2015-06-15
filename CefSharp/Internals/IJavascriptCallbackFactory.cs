namespace CefSharp.Internals
{
    public interface IJavascriptCallbackFactory
    {
        IJavascriptCallback Create(JavascriptCallback callback);
    }
}
