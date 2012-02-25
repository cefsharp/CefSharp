using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        public ISchemeHandler Create()
        {
            return new SchemeHandler();
        }
    }

    class SchemeHandler: ISchemeHandler
    {
        private readonly IDictionary<string, string> resources;

        public SchemeHandler()
        {
            resources = new Dictionary<string, string>
            {
                { "BindingTest.html", Resources.BindingTest },
                { "PopupTest.html", Resources.PopupTest },
                { "SchemeTest.html", Resources.SchemeTest },
                { "TooltipTest.html", Resources.TooltipTest },
            };
        }

        public bool ProcessRequest(IRequest request, ref string mimeType, ref Stream stream)
        {
            var uri = new Uri(request.Url);
            var segments = uri.Segments;
            var file = segments[segments.Length - 1];

            string resource;
            if (resources.TryGetValue(file, out resource) &&
                !String.IsNullOrEmpty(resource))
            {
                var bytes = Encoding.UTF8.GetBytes(resource);
                stream = new MemoryStream(bytes);
                mimeType = "text/html";
                
                return true;
            }

            return false;
        }
    }
}
