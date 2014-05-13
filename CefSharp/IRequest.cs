using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IRequest
    {
        string Url { get; set; }
        string Method { get; }
        string Body { get; }
        IHeaderDictionary Headers { get; set; }
    }
}
