// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// Javascript Binding Settings
    /// </summary>
    public class JavascriptBindingSettings : FreezableBase
    {
        private bool alwaysInterceptAsynchronously;
        private bool legacyBindingEnabled;
        private string jsBindingGlobalObjectName;

        /// <summary>
        /// The Javascript methods that CefSharp provides in relation to JavaScript Binding are
        /// created using a Global (window) Object. Settings this property allows you to customise
        /// the name of this object. If not specified then both cefSharp and CefSharp objects will
        /// be created e.g. cefSharp.bindObjectAsync, CefSharp.BindObjectAsync.
        /// If specified then your custom name will be used, if the name starts with a lowercase letter
        /// then all the functions will be lowercase, e.g. myObjName.bindObjectAsync otherwise
        /// the functions will start with a uppercase letter e.g. MyObjName.BindObjectAsync
        /// </summary>
        /// <remarks>
        /// This object is also accessible through the window property. e.g. window.cefSharp.bindObjectAsync
        /// </remarks>
        public string JavascriptBindingApiGlobalObjectName
        {
            get { return jsBindingGlobalObjectName; }
            set
            {
                ThrowIfFrozen();

                if (!StringCheck.IsLettersAndNumbers(value))
                {
                    //TODO: See if there's a better suited Exception class for this.
                    throw new System.Exception("invalid or illegal characters used for binding property names. Alphanumeric and underscores characters only.");
                }

                jsBindingGlobalObjectName = value;
            }
        }

        /// <summary>
        /// Objects registered using <see cref="IJavascriptObjectRepository.Register"/>
        /// will be automatically bound when a V8Context is created. (Soon as the Javascript
        /// context is created for a browser). This behaviour is like that seen with Javascript
        /// Binding in version 57 and earlier.
        /// </summary>
        public bool LegacyBindingEnabled
        {
            get { return legacyBindingEnabled; }
            set
            {
                ThrowIfFrozen();

                legacyBindingEnabled = value;
            }
        }

        /// <summary>
        /// When using an <see cref="CefSharp.ModelBinding.IAsyncMethodInterceptor"/>
        /// the <see cref="CefSharp.ModelBinding.IAsyncMethodInterceptor.InterceptAsync(System.Func{object[], object}, object[], string)"/>
        /// method is call for all methods (the default is to call InterceptAsync only for methods that return a Task).
        /// This only applies when <see cref="BindingOptions.MethodInterceptor"/> is of type <see cref="CefSharp.ModelBinding.IAsyncMethodInterceptor"/>
        /// </summary>
        public bool AlwaysInterceptAsynchronously
        {
            get { return alwaysInterceptAsynchronously; }
            set
            {
                ThrowIfFrozen();

                alwaysInterceptAsynchronously = value;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JavascriptBindingSettings()
        {
            //Default to CefSharpSettings.LegacyJavascriptBindingEnabled
            //until it's eventually removed
            LegacyBindingEnabled = CefSharpSettings.LegacyJavascriptBindingEnabled;
        }
    }
}
