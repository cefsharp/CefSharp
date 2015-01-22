namespace CefSharp.Example
{
    public class JsDialogHandler : IJsDialogHandler
    {
        public bool OnJSAlert(IWebBrowser browser, string url, string message)
        {
            return false;
        }

        public bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval)
        {
            retval = false;

            return false;
        }

        public bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result)
        {
            retval = false;
            result = null;

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
