namespace CefSharp
{
    public interface IKeyboardHandler
    {
        bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, int modifiers, bool isSystemKey);
    }
}
