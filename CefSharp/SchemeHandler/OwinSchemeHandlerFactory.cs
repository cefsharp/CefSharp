// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.SchemeHandler
{
    //Shorthand for Owin pipeline func
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// <see cref="ISchemeHandlerFactory"/> implementation that takes an OWIN  AppFunc
    /// and uses an <see cref="OwinResourceHandler"/> to fulfill each requests.
    /// </summary>
    public class OwinSchemeHandlerFactory : ISchemeHandlerFactory
    {
        private readonly AppFunc appFunc;

        public OwinSchemeHandlerFactory(AppFunc appFunc)
        {
            this.appFunc = appFunc;
        }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new OwinResourceHandler(appFunc);
        }
    }
}
