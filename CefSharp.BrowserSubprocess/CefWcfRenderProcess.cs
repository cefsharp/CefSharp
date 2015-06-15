using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CefSharp.Internals;
using TaskExtensions = CefSharp.Internals.TaskExtensions;

namespace CefSharp.BrowserSubprocess
{
    public class CefWcfRenderProcess : CefRenderProcess, IRenderProcess
    {
        public CefWcfRenderProcess(IEnumerable<string> args) 
            : base(args)
        {
        }

        public override void OnBrowserCreated(CefBrowserWrapper browser)
        {
            base.OnBrowserCreated(browser);

            if (ParentProcessId == null || ParentBrowserId == null)
            {
                return;
            }

            var browserId = browser.IsPopup ? ParentBrowserId.Value : browser.BrowserId;

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, browserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            var channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var browserProcess = channelFactory.CreateChannel();
            var clientChannel = ((IClientChannel)browserProcess);

            try
            {
                clientChannel.Open();
                if (!browser.IsPopup)
                {
                    browserProcess.Connect();
                }

                browser.ChannelFactory = channelFactory;
                browser.BrowserProcess = browserProcess;
            }
            catch (Exception)
            {
            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            base.OnBrowserDestroyed(browser);

            var channelFactory = browser.ChannelFactory;

            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }

            var clientChannel = ((IClientChannel)browser.BrowserProcess);

            if (clientChannel.State == CommunicationState.Opened)
            {
                clientChannel.Close();
            }

            browser.ChannelFactory = null;
            browser.BrowserProcess = null;
            browser.JavascriptRootObject = null;
        }

        public void Done()
        {
            throw new NotImplementedException();
        }
    }
}
