using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class SubProcessProxy : ObjectBase, ISubProcessProxy
    {
        public ISubProcessCallback Callback { get; private set; }

        public void Initialize()
        {
            CefSubprocess.Instance.ServiceHost.Service = this;
            Callback = OperationContext.Current.GetCallbackChannel<ISubProcessCallback>();
        }

        public Task<object> EvaluateScript(long frameId, string script)
        {
            return CefSubprocess.Instance.Browser.EvaluateScript(frameId, script);
        }

        public void Terminate()
        {
            CefSubprocess.Instance.ServiceHost.Service = null;
            CefSubprocess.Instance.Dispose();
        }

        protected override void DoDispose(bool isDisposing)
        {
            Callback = null;
            base.DoDispose(isDisposing);
        }
    }
}
