using System.Net;

namespace CefSharp
{
    public interface ICookieVisitor
    {
        bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie);
    }
}
