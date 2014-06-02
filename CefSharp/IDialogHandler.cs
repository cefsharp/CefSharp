using System.Collections.Generic;
namespace CefSharp
{
    public interface IDialogHandler
    {
        bool OnOpenFile(IWebBrowser browser, string title, string defaultFileName, List<string> acceptTypes, out List<string> result);
    }
}
