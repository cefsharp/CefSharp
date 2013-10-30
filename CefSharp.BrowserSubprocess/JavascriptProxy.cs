using CefSharp.Internals.JavascriptBinding;
using System;

namespace CefSharp.BrowserSubprocess
{
    internal class JavascriptProxy : IJavascriptProxy
    {
        public const String BaseAddress = "net.pipe://localhost";
        public const String ServiceName = "JavaScriptProxy";
        public const String Address = BaseAddress + "/" + ServiceName;

        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var cefFrame = SubprocessCefApp.Instance.CefBrowserWrapper.GetFrame(frameId);
            return null;
        }
    }
}