using System.Collections.Generic;
namespace CefSharp
{
    public interface IDialogHandler
    {
        bool OnFileDialog(IWebBrowser browser, CefFileDialogMode mode, string title, string defaultFileName, List<string> acceptTypes, out List<string> result);
    }
}
