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
        private int? parentBrowserId;
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

            if (parentBrowserId == null)
            {
                parentBrowserId = browser.BrowserId;
            }

            if (ParentProcessId == null || parentBrowserId == null)
            {
                return;
            }

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, parentBrowserId.Value);

            var binding = BrowserProcessServiceHost.CreateBinding();

            channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var proxy = CreateBrowserProxy();

            var clientChannel = ((IClientChannel)proxy);
            
            try
            {
                clientChannel.Open();

                proxy.Connect();

                var javascriptObject = proxy.GetRegisteredJavascriptObjects();

                if (javascriptObject.MemberObjects.Count > 0)
                {
                    Bind(javascriptObject);
                }

            }
            catch(Exception)
            {

            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper cefBrowserWrapper)
        {
            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(long frameId, string script, TimeSpan? timeout)
        {
            var factory = browser.RenderThreadTaskFactory;

            var task = factory.StartNew(() =>
            {
                return browser.DoEvaluateScript(frameId, script);
            }, TaskCreationOptions.AttachedToParent);

            return timeout.HasValue ? task.WithTimeout(timeout.Value) : task;
        }

        public override IBrowserProcess CreateBrowserProxy()
        {
            return channelFactory.CreateChannel();
        }
    }
}
