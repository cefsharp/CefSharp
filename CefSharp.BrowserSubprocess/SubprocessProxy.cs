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
            Debugger.Break();
            Callback = OperationContext.Current.GetCallbackChannel<ISubprocessCallback>();

            CefSubprocess.Instance.ServiceHost.Initialize(this);
        }

        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var result = CefSubprocess.Instance.Browser.EvaluateScript(frameId, script, timeout);
            return result;
        }

        public void Terminate()
        {
            CefSubprocess.Instance.Dispose();
        }
    }
}
