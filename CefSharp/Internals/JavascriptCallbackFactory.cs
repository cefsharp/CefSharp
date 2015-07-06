using System;

namespace CefSharp.Internals
{
    public class JavascriptCallbackFactory : IJavascriptCallbackFactory
    {
        private readonly WeakReference browserProcessServiceHost;

        public JavascriptCallbackFactory(WeakReference browserProcessServiceHost)
        {
            this.browserProcessServiceHost = browserProcessServiceHost;
        }

        public IJavascriptCallback Create(JavascriptCallback callback)
        {
            return new JavascriptCallbackProxy(callback.Id, callback.BrowserId, browserProcessServiceHost);
        }
    }
}
