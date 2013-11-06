using CefSharp.Internals.JavascriptBinding;
using System;

namespace CefSharp.BrowserSubprocess
{
    internal class JavascriptProxy : IJavascriptProxy
    {
        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var result = SubprocessCefApp.Instance.CefSubprocessWrapper.EvaluateScript(frameId, script, timeout);
            return result;
        }
    }
}