// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.ModelBinding;

namespace CefSharp
{
    /// <summary>
    /// Javascript binding options
    /// </summary>
    public class BindingOptions
    {
        /// <summary>
        /// Set of options with the default binding
        /// </summary>
        public static BindingOptions DefaultBinder
        {
            get { return new BindingOptions { Binder = new DefaultBinder() }; }
        }

        /// <summary>
        /// Model binder used for passing complex classes as params to methods
        /// </summary>
        public IBinder Binder { get; set; }

        /// <summary>
        /// Interceptor used for intercepting calls to the target object methods. For instance, can be used 
        /// for logging calls (from js) to .net methods.
        /// </summary>
        public IMethodInterceptor MethodInterceptor { get; set; }

#if !NETCOREAPP
        /// <summary>
        /// Interceptor used for intercepting get/set calls to the target object property. For instance, can be used 
        /// for logging calls to .net property (from js)
        /// </summary>
        public IPropertyInterceptor PropertyInterceptor { get; set; }
#endif

    }
}
