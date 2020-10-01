// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Default implementation of <see cref="IApp"/> which represents the CefApp class.
    /// </summary>
    /// <seealso cref="T:CefSharp.IApp"/>
    public class DefaultApp : IApp
    {
        /// <summary>
        /// Return the handler for functionality specific to the browser process. This method is called on multiple threads.
        /// </summary>
        /// <value>
        /// The browser process handler.
        /// </value>
        public IBrowserProcessHandler BrowserProcessHandler { get; private set; }
        /// <summary>
        /// Gets or sets the schemes.
        /// </summary>
        /// <value>
        /// The schemes.
        /// </value>
        public IEnumerable<CefCustomScheme> Schemes { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="browserProcessHandler">The browser process handler.</param>
        /// <param name="schemes">The schemes.</param>
        public DefaultApp(IBrowserProcessHandler browserProcessHandler, IEnumerable<CefCustomScheme> schemes)
        {
            BrowserProcessHandler = browserProcessHandler;
            Schemes = schemes;
        }

        /// <summary>
        /// Provides an opportunity to register custom schemes. Do not keep a reference to the <paramref name="registrar"/> object. This
        /// method is called on the main thread for each process and the registered schemes should be the same across all processes.
        /// 
        /// </summary>
        /// <param name="registrar">scheme registra.</param>
        void IApp.OnRegisterCustomSchemes(ISchemeRegistrar registrar)
        {
            OnRegisterCustomSchemes(registrar);
        }

        /// <summary>
        /// Provides an opportunity to register custom schemes. Do not keep a reference to the <paramref name="registrar"/> object. This
        /// method is called on the main thread for each process and the registered schemes should be the same across all processes.
        /// 
        /// </summary>
        /// <param name="registrar">scheme registra.</param>
        protected virtual void OnRegisterCustomSchemes(ISchemeRegistrar registrar)
        {
            //Possible we have duplicate scheme names, we'll only register the first one
            //Keep a list so we don't call AddCustomScheme twice for the same scheme name
            var registeredSchemes = new List<string>();

            foreach (var scheme in Schemes)
            {
                //We don't need to register http or https, they're built in schemes
                if (scheme.SchemeName == "http" || scheme.SchemeName == "https")
                {
                    continue;
                }

                //We've already registered this scheme name
                if (registeredSchemes.Contains(scheme.SchemeName))
                {
                    continue;
                }

                var success = registrar.AddCustomScheme(scheme.SchemeName, scheme.Options);
                if (success)
                {
                    registeredSchemes.Add(scheme.SchemeName);
                }
                else
                {
                    var msg = "CefSchemeRegistrar::AddCustomScheme failed for schemeName:" + scheme.SchemeName;
                    //TODO: Log error
                    //LOG(ERROR) << StringUtils::ToNative(msg).ToString();
                }
            }
        }
    }
}
