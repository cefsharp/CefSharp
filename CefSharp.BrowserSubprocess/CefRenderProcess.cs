using System;
using System.Linq;
using System.Threading.Tasks;
using CefSharp.Internals;
using System.Collections.Generic;
using System.ServiceModel;
using TaskExtensions = CefSharp.Internals.TaskExtensions;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderProcess : CefSubProcess, IRenderProcess
    {
        private DuplexChannelFactory<IBrowserProcess> channelFactory;
        private int? parentBrowserId;
        private List<CefBrowserWrapper> browsers = new List<CefBrowserWrapper>();

        public CefRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            foreach(var browser in browsers)
            {
                browser.Dispose();
            }

            browsers = null;

            base.DoDispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserWrapper browser)
        {
            browsers.Add(browser);

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

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            browsers.Remove(browser);

            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(int browserId, long frameId, string script, TimeSpan? timeout)
        {
            var factory = RenderThreadTaskFactory;
            var browser = browsers.FirstOrDefault(x => x.BrowserId == browserId);
            if (browser == null)
            {
                return TaskExtensions.FromResult(new JavascriptResponse
                {
                    Success = false,
                    Message = string.Format("Browser with Id {0} not found in Render Sub Process.", browserId)
                });
            }

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
