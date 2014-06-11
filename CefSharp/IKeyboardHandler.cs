namespace CefSharp
{
    public interface IKeyboardHandler
    {
        bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, int modifiers, bool isSystemKey);
        bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, int modifiers, bool isSystemKey, bool isKeyboardShortcut);
    }
}
