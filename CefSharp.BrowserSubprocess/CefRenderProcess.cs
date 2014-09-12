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
        private CefBrowserWrapper browser;

        public CefRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            DisposeMember(ref browser);

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserWrapper cefBrowserWrapper)
        {
            browser = cefBrowserWrapper;

            if (ParentProcessId == null)
            {
                return;
            }

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, browser.BrowserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var proxy = CreateBrowserProxy();
            proxy.Connect();

            var javascriptObject = proxy.GetRegisteredJavascriptObjects();
            
            Bind(javascriptObject);
        }

        public Task<JavascriptResponse> EvaluateScript(long frameId, string script, TimeSpan timeout)
        {
            var factory = browser.RenderThreadTaskFactory;

            return factory.StartNew(() =>
            {
                return browser.DoEvaluateScript(frameId, script);
            }, TaskCreationOptions.AttachedToParent)
            .WithTimeout(timeout);
        }

        public override IBrowserProcess CreateBrowserProxy()
        {
            return channelFactory.CreateChannel();
        }
    }
}
