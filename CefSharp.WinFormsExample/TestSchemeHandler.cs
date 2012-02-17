using System;
using System.IO;
using System.Text;
using CefSharp;
using CefSharp.Example.Properties;

namespace CefSharp.WinFormsExample
{
    public class TestSchemeHandler : ISchemeHandler
    {
        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            string resource = null;

            if (request.Url.EndsWith("SchemeTest.html", StringComparison.OrdinalIgnoreCase))
            {
                resource = Resources.SchemeTest;
            }
            else if (request.Url.EndsWith("BindingTest.html", StringComparison.OrdinalIgnoreCase))
            {
                resource = Resources.BindingTest;
            }
            else if (request.Url.EndsWith("TooltipTest.html", StringComparison.OrdinalIgnoreCase))
            {
                resource = Resources.TooltipTest;
            }
            else if (request.Url.EndsWith("PopupTest.html", StringComparison.OrdinalIgnoreCase))
            {
                resource = Resources.PopupTest;
            }

            if (!String.IsNullOrEmpty(resource))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(resource);
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