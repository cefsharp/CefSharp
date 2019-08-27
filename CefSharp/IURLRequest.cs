using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public interface IURLRequest : IDisposable
    {
        /// <summary>
        /// True if the response was served from the cache.
        /// </summary>
        bool ResponseWasCached();
        
        /// <summary>
        /// The response, or null if no response information is available
        /// </summary>
        IResponse GetResponse();

        /// <summary>
        /// The request status.
        /// </summary>
        UrlRequestStatus GetRequestStatus();
    }
}
