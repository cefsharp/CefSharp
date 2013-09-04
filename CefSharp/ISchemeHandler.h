#include "Stdafx.h"
#include "Request.h"

namespace CefSharp
{
    ref class SchemeHandlerResponse;
    public delegate void OnRequestCompletedHandler();

    public interface class ISchemeHandler
    {
        /// <summary>
        /// Processes a custom scheme-based request asynchronously. The implementing method should call the callback whenever the
        /// request is completed.
        /// </summary>
        /// <returns>true if the request is handled, false otherwise.</returns>
        bool ProcessRequestAsync(IRequest^ request, SchemeHandlerResponse^ response, OnRequestCompletedHandler^ callback);
    };
}
