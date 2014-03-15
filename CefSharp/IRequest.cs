using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public interface IRequest
    {
        string Url { get; set; }
        string Method { get; }
        string Body { get; }
        IDictionary<string, String> Headers { get; set; }
    }
}
