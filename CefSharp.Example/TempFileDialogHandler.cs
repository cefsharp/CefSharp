using System.Collections.Generic;
using System.IO;

namespace CefSharp.Example
{
    public class TempFileDialogHandler : IDialogHandler
    {
        public bool OnFileDialog(IWebBrowser browser, CefFileDialogMode mode, string title, string defaultFileName, List<string> acceptTypes, out List<string> result)
        {
            result = new List<string> { Path.GetRandomFileName() };
            return true;
        }
    }
}
