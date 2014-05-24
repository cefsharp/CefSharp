using System;
using System.Diagnostics;
using System.ServiceModel;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class SubprocessProxy : ISubprocessProxy
    {
        public ISubprocessCallback Callback { get; private set; }

        public void Initialize()
        {
            Callback = OperationContext.Current.GetCallbackChannel<ISubprocessCallback>();

            CefRenderprocess.Instance.ServiceHost.Initialize(this);
        }

        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var result = CefRenderprocess.Instance.Browser.EvaluateScript(frameId, script, timeout);
            return result;
        }

        public void Terminate()
        {
            CefRenderprocess.Instance.Dispose();
        }
        
        public void RegisterJavascriptObjects(JavascriptObject obj)
        {
            CefRenderprocess.Instance.RegisterJavascriptObjects((JavascriptObjectWrapper)obj);
        }
    }
}
