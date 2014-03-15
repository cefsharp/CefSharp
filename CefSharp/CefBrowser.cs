using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public abstract class CefBrowserBase : ObjectBase
    {
        public long BrowserId { get; set; }

        protected CefBrowserBase()
        {
        }

        public Task<object> EvaluateScript(long frameId, string script)
        {
            return CefManagedBase.Instance.RenderTaskFactory.StartNew( () => 
            {
                return DoEvaluateScript(frameId, script);
            } );
        }

        protected abstract object DoEvaluateScript(long frameId, string script);
    }
}
