using CefSharp.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public abstract class ManagedCefBrowserAdapterBase : ObjectBase, ISubProcessCallback
    {    
        protected ISubProcessProxy _javaScriptProxy;

        public ManagedCefBrowserAdapterBase()
        {

        }

        protected override void DoDispose(bool isDisposing)
        {
            if ( _javaScriptProxy != null )
            {
                try
                {
                    _javaScriptProxy.Terminate();
                }
                catch
                {                 
                }
            }
            DisposeMember( ref _javaScriptProxy );
            base.DoDispose(isDisposing);
        }

        public abstract string DevToolsUrl { get; }

        public void Error(Exception ex)
        {
        }

        protected Task<object> DoEvalueteScript( long frameId, string script )
        {
            return Task<object>.Factory.StartNew( () => 
            {
                // TODO: Don't instantiate this on every request. The problem is that the CefBrowser is not set in our constructor.
                if (_javaScriptProxy == null)
                {
                    var serviceName = SubProcessProxySupport.GetServiceName(Process.GetCurrentProcess().Id, frameId);
                    var channelFactory = new DuplexChannelFactory<ISubProcessProxy>(this,
                        new NetNamedPipeBinding(),
                        new EndpointAddress(serviceName)
                        );

                    _javaScriptProxy = channelFactory.CreateChannel();
                }
                try
                {
                    return _javaScriptProxy.EvaluateScript(frameId, script).Result;
                }
                catch (Exception)
                {
                    _javaScriptProxy.Dispose();
                    _javaScriptProxy = null;
                    throw;
                }
            } );            
        }
    }
}
