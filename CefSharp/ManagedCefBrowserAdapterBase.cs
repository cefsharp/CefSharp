using CefSharp.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                _javaScriptProxy.Terminate();
            }
            DisposeMember( ref _javaScriptProxy );
            base.DoDispose(isDisposing);
        }

        public abstract string DevToolsUrl { get; }

        public void Error(Exception ex)
        {
        }
    }
}
