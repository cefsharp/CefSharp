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

            var browserId = browser.IsPopup ? parentBrowserId.Value : browser.BrowserId;

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, browserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            var channelFactory = new DuplexChannelFactory<IBrowserProcess>(
                this,
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var proxy = channelFactory.CreateChannel();

            var clientChannel = ((IClientChannel)proxy);
            
            try
            {
                clientChannel.Open();

                proxy.Connect();

                var javascriptObject = proxy.GetRegisteredJavascriptObjects();

                if (javascriptObject.MemberObjects.Count > 0)
                {
                    Bind(javascriptObject, channelFactory.CreateChannel);
                }

                browser.ChannelFactory = channelFactory;

            }
            catch(Exception)
            {

            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            browsers.Remove(browser);

            var channelFactory = browser.ChannelFactory;

            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }

            browser.ChannelFactory = null;
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
                try
                {
                    var response = browser.DoEvaluateScript(frameId, script);

                    return response;
                }
                catch (Exception ex)
                {
                    return new JavascriptResponse
                    {
                        Success = false,
                        Message = ex.ToString()
                    };
                }

            }, TaskCreationOptions.AttachedToParent);

            return timeout.HasValue ? task.WithTimeout(timeout.Value) : task;
        }

        public IAsyncResult BeginEvaluateScriptAsync(int browserId, long frameId, string script, TimeSpan? timeout, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<JavascriptResponse>(state);
            var task = EvaluateScriptAsync(browserId, frameId, script, timeout);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(t.Result);
                }

                if (callback != null)
                {
                    callback(tcs.Task);
                }
            });
            return tcs.Task;
        }

        public JavascriptResponse EndEvaluateScriptAsync(IAsyncResult result)
        {
            return ((Task<JavascriptResponse>)result).Result;
        }
    }
}
