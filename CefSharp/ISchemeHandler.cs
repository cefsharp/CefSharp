using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public interface ISchemeHandler
    {
        /// <summary>
        /// Processes a custom scheme-based request asynchronously. 
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="response">The SchemeHandlerResponse object in which the handler is supposed to place the response
        /// information.</param>
        /// <returns>the task that fullfils the request if the request is handled, null otherwise.
        /// the result of the task should be the provided filled ISchemeHandlerResponse
        /// </returns>
        Task ProcessRequestAsync(IRequest request, SchemeHandlerResponse response);
    }
}
