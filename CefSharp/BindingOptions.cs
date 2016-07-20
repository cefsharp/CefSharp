// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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
            get { return new BindingOptions { Binder = new DefaultBinder(new DefaultFieldNameConverter()) }; }
        }

        /// <summary>
        /// camel case the javascript names of properties/methods, defaults to true
        /// </summary>
        public bool CamelCaseJavascriptNames { get; set; }


        /// <summary>
        /// model binder used for passing complex classes as params to methods
        /// </summary>
        public IBinder Binder { get; set; }

        public BindingOptions()
        {
            CamelCaseJavascriptNames = true;
        }
    }
}
