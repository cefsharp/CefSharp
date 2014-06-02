using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderProcess : CefSubProcess, IRenderprocess
    {
        private DuplexChannelFactory<IBrowserProcess> channelFactory;
        private CefBrowserBase browser;
        public CefBrowserBase Browser
        {
            get { return browser; }
        }
        

        public CefRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
        }

        public static new CefRenderProcess Instance
        {
            get { return (CefRenderProcess)CefSubProcess.Instance; }
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            //DisposeMember(ref renderprocess);
            DisposeMember(ref browser);

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserBase cefBrowserWrapper)
        {
            browser = cefBrowserWrapper;

            if (ParentProcessId == null)
            {
                return;
            }

            channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                new NetNamedPipeBinding(),
                new EndpointAddress(RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, cefBrowserWrapper.BrowserId))
            );

            channelFactory.Open();
            
            Bind(CreateBrowserProxy().GetRegisteredJavascriptObjects());
        }
        
        public object EvaluateScript(int frameId, string script, double timeout)
        {
            var result = Browser.EvaluateScript(frameId, script, timeout);
            return result;
        }

        public override IBrowserProcess CreateBrowserProxy()
        {
            return channelFactory.CreateChannel();
        }
    }
}
