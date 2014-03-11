using CefSharp.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = _javaScriptProxy.BeginEvaluateScript(frameId, script, null, null);
            return Task<object>.Factory.FromAsync(result, _javaScriptProxy.EndEvaluateScript);
        }
    }
}
