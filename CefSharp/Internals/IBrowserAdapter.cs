using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// Interface used to break reference cycles in CefSharp.Core C++ code.
    /// This will ALWAYS be a ManagedCefBrowserAdapter instance.
    /// </summary>
    public interface IBrowserAdapter
    {
        Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout);
    }
}
