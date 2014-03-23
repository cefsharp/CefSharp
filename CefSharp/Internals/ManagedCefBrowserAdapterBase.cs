using CefSharp.Internals;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public abstract class ManagedCefBrowserAdapterBase : ObjectBase, ISubProcessCallback
    {
        private DuplexChannelFactory<ISubProcessProxy> javaScriptProxyFactory;

        protected ISubProcessProxy JavaScriptProxy { get; private set; }

        protected override void DoDispose(bool isDisposing)
        {
            if (JavaScriptProxy != null)
            {
                try
                {
                    JavaScriptProxy.Terminate();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    Abort();
                }
                JavaScriptProxy.Dispose();
                JavaScriptProxy = null;
            }
            base.DoDispose(isDisposing);
        }

        public abstract string DevToolsUrl { get; }

        protected abstract long FrameId { get; }
        public abstract int BrowserId { get; }

        public void Error(Exception ex)
        {
        }

        public Task<object> EvaluateScript(string script)
        {
            return Task<object>.Factory.StartNew(() =>
            {
                // TODO: Don't instantiate this on every request. The problem is that the CefBrowser is not set in our constructor.
                CreateChannel();
                try
                {
                    return JavaScriptProxy.EvaluateScript(FrameId, script).Result;
                }
                catch (Exception)
                {
                    Abort();
                    JavaScriptProxy = javaScriptProxyFactory.CreateChannel();
                    throw;
                }
            }, TaskCreationOptions.AttachedToParent);
        }

        private void CreateChannel()
        {
            CreateChannelFactory();

            if (JavaScriptProxy == null)
            {
                JavaScriptProxy = javaScriptProxyFactory.CreateChannel();
            }
        }

        private void CreateChannelFactory()
        {
            if (javaScriptProxyFactory != null)
            {
                return;
            }

            var serviceName = SubProcessProxySupport.GetServiceName(Process.GetCurrentProcess().Id, BrowserId);

            javaScriptProxyFactory = new DuplexChannelFactory<ISubProcessProxy>(this,
                        new NetNamedPipeBinding(),
                        new EndpointAddress(serviceName)
                        );
        }

        private void Abort()
        {
            (JavaScriptProxy as ICommunicationObject).Abort();
        }
    }
}
