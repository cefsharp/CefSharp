using System.Collections.Generic;
namespace CefSharp
{
    public interface IDialogHandler
    {
        bool OnOpenFile(IWebBrowser browser, string title, string default_file_name, List<string> accept_types, out List<string> result);
    }
}
