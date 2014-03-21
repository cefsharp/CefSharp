namespace CefSharp
{
    // Should rightfulyl live in the CefSharp.WinForms project, but the problem is that it's being used from the CefSharp project
    // so the dependency would go the wrong way... Has to be here for the time being.
    public interface IWinFormsWebBrowser : IWebBrowser
    {
        IMenuHandler MenuHandler { get; set; }
    }
}
