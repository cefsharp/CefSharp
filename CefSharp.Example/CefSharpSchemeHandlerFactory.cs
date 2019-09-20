// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    public class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "custom";
        public const string SchemeNameTest = "test";

        private static readonly IDictionary<string, string> ResourceDictionary;

        static CefSharpSchemeHandlerFactory()
        {
            ResourceDictionary = new Dictionary<string, string>
            {
                { "/home.html", Resources.home_html },

                { "/assets/css/shCore.css", Resources.assets_css_shCore_css },
                { "/assets/css/shCoreDefault.css", Resources.assets_css_shCoreDefault_css },
                { "/assets/css/docs.css", Resources.assets_css_docs_css },
                { "/assets/js/application.js", Resources.assets_js_application_js },
                { "/assets/js/jquery.js", Resources.assets_js_jquery_js },
                { "/assets/js/shBrushCSharp.js", Resources.assets_js_shBrushCSharp_js },
                { "/assets/js/shBrushJScript.js", Resources.assets_js_shBrushJScript_js },
                { "/assets/js/shCore.js", Resources.assets_js_shCore_js },

                { "/bootstrap/bootstrap-theme.min.css", Resources.bootstrap_theme_min_css },
                { "/bootstrap/bootstrap.min.css", Resources.bootstrap_min_css },
                { "/bootstrap/bootstrap.min.js", Resources.bootstrap_min_js },

                { "/BindingTest.html", Resources.BindingTest },
                { "/BindingTestSingle.html", Resources.BindingTestSingle },
                { "/LegacyBindingTest.html", Resources.LegacyBindingTest },
                { "/PostMessageTest.html", Resources.PostMessageTest },
                { "/ExceptionTest.html", Resources.ExceptionTest },
                { "/PopupTest.html", Resources.PopupTest },
                { "/SchemeTest.html", Resources.SchemeTest },
                { "/TooltipTest.html", Resources.TooltipTest },
                { "/FramedWebGLTest.html", Resources.FramedWebGLTest },
                { "/MultiBindingTest.html", Resources.MultiBindingTest },
                { "/ScriptedMethodsTest.html", Resources.ScriptedMethodsTest },
                { "/ResponseFilterTest.html", Resources.ResponseFilterTest },
                { "/DraggableRegionTest.html", Resources.DraggableRegionTest },
                { "/DragDropCursorsTest.html", Resources.DragDropCursorsTest },
                { "/CssAnimationTest.html", Resources.CssAnimation },
                { "/CdmSupportTest.html", Resources.CdmSupportTest },
                { "/Recaptcha.html", Resources.Recaptcha },
                { "/UnicodeExampleGreaterThan32kb.html", Resources.UnicodeExampleGreaterThan32kb },
                { "/UnocodeExampleEqualTo32kb.html", Resources.UnocodeExampleEqualTo32kb },
                { "/JavascriptCallbackTest.html", Resources.JavascriptCallbackTest },
                { "/BindingTestsAsyncTask.html", Resources.BindingTestsAsyncTask }
            };
        }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            //Notes:
            // - The 'host' portion is entirely ignored by this scheme handler.
            // - If you register a ISchemeHandlerFactory for http/https schemes you should also specify a domain name
            // - Avoid doing lots of processing in this method as it will affect performance.
            // - Use the Default ResourceHandler implementation

            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            //Load a file directly from Disk
            if (fileName.EndsWith("CefSharp.Core.xml", StringComparison.OrdinalIgnoreCase))
            {
                //Convenient helper method to lookup the mimeType
                var mimeType = ResourceHandler.GetMimeType(".xml");
                //Load a resource handler for CefSharp.Core.xml
                //mimeType is optional and will default to text/html
                return ResourceHandler.FromFilePath("CefSharp.Core.xml", mimeType, autoDisposeStream: true);
            }

            if (fileName.EndsWith("Logo.png", StringComparison.OrdinalIgnoreCase))
            {
                //Convenient helper method to lookup the mimeType
                var mimeType = ResourceHandler.GetMimeType(".png");
                //Load a resource handler for Logo.png
                //mimeType is optional and will default to text/html
                return ResourceHandler.FromFilePath("..\\..\\..\\..\\CefSharp.WinForms.Example\\Resources\\chromium-256.png", mimeType, autoDisposeStream: true);
            }

            if (uri.Host == "cefsharp.com" && schemeName == "https" && (string.Equals(fileName, "/PostDataTest.html", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(fileName, "/PostDataAjaxTest.html", StringComparison.OrdinalIgnoreCase)))
            {
                return new CefSharpSchemeHandler();
            }

            if (string.Equals(fileName, "/EmptyResponseFilterTest.html", StringComparison.OrdinalIgnoreCase))
            {
                return ResourceHandler.FromString("", ".html");
            }

            string resource;
            if (ResourceDictionary.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                var fileExtension = Path.GetExtension(fileName);
                return ResourceHandler.FromString(resource, includePreamble: true, mimeType: ResourceHandler.GetMimeType(fileExtension));
            }

            return null;
        }
    }
}
