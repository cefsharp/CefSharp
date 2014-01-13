namespace CefSharp.WinForms.Example
{
    internal class MenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser)
        {
            // Return false if you want to disable the context menu.
            return true;
        }
    }
}