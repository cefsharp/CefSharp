using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public delegate void OnRequestCompletedHandler();

    public interface ISchemeHandler
    {
        /// <summary>
        /// Processes a custom scheme-based request asynchronously. The implementing method should call the callback whenever the
        /// request is completed.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="response">The SchemeHandlerResponse object in which the handler is supposed to place the response
        /// information.</param>
        /// <param name="requestCompletedCallback">A callback which the handler is supposed to call once the processing is
        /// complete. The callback may be called on any thread.</param>
        /// <returns>true if the request is handled, false otherwise.</returns>
        bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback);
    }
}
