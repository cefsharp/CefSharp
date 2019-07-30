// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Default implementation of <see cref="IApp"/> which represents the CefApp class
    /// </summary>
    public class DefaultApp : IApp
    {
        public IBrowserProcessHandler BrowserProcessHandler { get; private set; }
        public IEnumerable<CefCustomScheme> Schemes { get; private set; }

        public DefaultApp(IBrowserProcessHandler browserProcessHandler, IEnumerable<CefCustomScheme> schemes)
        {
            BrowserProcessHandler = browserProcessHandler;
            Schemes = schemes;
        }

        void IApp.OnRegisterCustomSchemes(ISchemeRegistrar registrar)
        {
            OnRegisterCustomSchemes(registrar);
        }

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
