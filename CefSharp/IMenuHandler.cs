namespace CefSharp
{
    public interface IMenuHandler
    {
        bool OnBeforeContextMenu(IWebBrowser browser);
    }
}
