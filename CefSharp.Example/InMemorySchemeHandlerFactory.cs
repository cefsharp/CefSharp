// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using CefSharp.Example.Properties;

namespace CefSharp.Example
{
    /// <summary>
    /// Demo ISchemeHandlerFactory reads that from resource in memory, could just
    /// as easily be from files on disk - only use this method if you already have pre loaded data, don't perform
    /// any web requests here or database lookups, best for static data only.
    /// </summary>
    public class InMemorySchemeHandlerFactory : ISchemeHandlerFactory
    {
        /// <summary>
        /// Just a simple dictionary for resource lookup
        /// </summary>
        private static readonly IDictionary<string, string> ResourceDictionary;

        static InMemorySchemeHandlerFactory()
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
                { "/ExceptionTest.html", Resources.ExceptionTest },
                { "/PopupTest.html", Resources.PopupTest },
                { "/SchemeTest.html", Resources.SchemeTest },
                { "/TooltipTest.html", Resources.TooltipTest },
                { "/DragDropCursorsTest.html", Resources.DragDropCursorsTest },
                { "/FramedWebGLTest.html", Resources.FramedWebGLTest },
                { "/MultiBindingTest.html", Resources.MultiBindingTest },
                { "/ScriptedMethodsTest.html", Resources.ScriptedMethodsTest },
                { "/ResponseFilterTest.html", Resources.ResponseFilterTest },
            };
        }

        /// <summary>
        /// The only method required to implement an ISchemeHandlerFactory
        /// </summary>
        /// <param name="browser">browser</param>
        /// <param name="frame">frame</param>
        /// <param name="schemeName">scheme name this handler was registered for</param>
        /// <param name="request">request, we'll use this to check the Url and load the appropriate resource</param>
        /// <returns>
        /// return null to invoke the default behaviour, this is useful when you register a handler on the http/https scheme
        /// if we have a string that represents our resource in the lookup dictionary then return it as an IResourceHandler
        /// </returns>
        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            //You can match the scheme/host if required, we don't need that in this example, so keeping it simple.
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;
            var extension = Path.GetExtension(fileName);

            string resource;
            if (ResourceDictionary.TryGetValue(fileName, out resource) && !string.IsNullOrEmpty(resource))
            {
                //For css/js/etc it's important to specify a mime/type, here we use the file extension to perform a lookup
                //there are overloads where you can specify more options including Encoding, mimeType
                return ResourceHandler.FromString(resource, extension);
            }

            return null;
        }
    }
}
