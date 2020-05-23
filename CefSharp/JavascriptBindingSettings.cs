// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    /// <summary>
    /// Javascript Binding Settings
    /// </summary>
    public class JavascriptBindingSettings : FreezableBase
    {
        /// <summary>
        /// Objects registered using <see cref="IJavascriptObjectRepository.Register(string, object, bool, BindingOptions)"/>
        /// will be automatically bound when a V8Context is created. (Soon as the Javascript
        /// context is created for a browser). This behaviour is like that seen with Javascript
        /// Binding in version 57 and earlier.
        /// </summary>
        public bool LegacyBindingEnabled { get; set; }

        public JavascriptBindingSettings()
        {
            //Default to CefSharpSettings.LegacyJavascriptBindingEnabled
            //until it's eventually removed
            LegacyBindingEnabled = CefSharpSettings.LegacyJavascriptBindingEnabled;
        }
    }
}
