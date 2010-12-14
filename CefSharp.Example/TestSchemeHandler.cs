using System;
using System.IO;
using System.Text;
using CefSharp;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    public class TestSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            if(request.Url.EndsWith("SchemeTest.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.SchemeTest);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            if (request.Url.EndsWith("BindingTest.html", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Resources.BindingTest);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                return true;
            }

            return false;
        }
    }

    public class TestSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new TestSchemeHandler();
        }
    }
}