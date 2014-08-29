using System;
using System.Threading.Tasks;
using CefSharp.Internals;
using System.Collections.Generic;
using System.ServiceModel;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderProcess : CefSubProcess, IRenderProcess
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
        
        protected override void DoDispose(bool isDisposing)
        {
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

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, cefBrowserWrapper.BrowserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var proxy = CreateBrowserProxy();
            var javascriptObject = proxy.GetRegisteredJavascriptObjects();
            
            Bind(javascriptObject);
        }

        public Task<JavascriptResponse> EvaluateScript(long frameId, string script, TimeSpan timeout)
        {
            var task = Browser.EvaluateScript(frameId, script, timeout);

            return task;
        }

        public override IBrowserProcess CreateBrowserProxy()
        {
            return channelFactory.CreateChannel();
        }
    }
}
