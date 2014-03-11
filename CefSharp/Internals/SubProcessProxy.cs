using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class SubProcessProxy : ObjectBase, ISubProcessProxy
    {
        public ISubProcessCallback callback { get; private set; }

        public void Initialize()
        {
            CefSubprocessBase.Instance.ServiceHost.Service = this;
            callback = OperationContext.Current.GetCallbackChannel<ISubProcessCallback>();
        }

        public Task<object> EvaluateScript(long frameId, string script )
        {
            return CefSubprocessBase.Instance.Browser.EvaluateScript(frameId, script);
        }
        
        public void Terminate()
        {
            CefSubprocessBase.Instance.ServiceHost.Service = null;
            CefSubprocessBase.Instance.Dispose();
        }        
    }
}
