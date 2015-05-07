namespace CefSharp.Example
{
    public class JsDialogHandler : IJsDialogHandler
    {
        public bool OnJSDialog(IWebBrowser browser, string originUrl, string acceptLang, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            return false;
        }

        public bool OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload)
        {
            //NOTE: Setting allowUnload to false will cancel the unload request
            allowUnload = false;

            //NOTE: Returning false will trigger the default behaviour, you need to return true to handle yourself.
            return false;
        }
    }
}
