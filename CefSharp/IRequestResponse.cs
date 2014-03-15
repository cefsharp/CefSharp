using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface IRequestResponse
    {
        /// cancel the request, return nothing
        void Cancel();

        /// the current request
        IRequest Request { get; }

        /// respond with redirection to the provided URL
        void Redirect(string url);

        /// respond with data from Stream
        void RespondWith(Stream stream, string mimeType);
    }
}
