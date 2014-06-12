using System.Collections.Specialized;

namespace CefSharp
{
    public interface IRequest
    {
        string Url { get; set; }
        string Method { get; }
        string Body { get; }
        NameValueCollection Headers { get; set; }
    }
}
