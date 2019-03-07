// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to provide handler implementations.
    /// Methods will be called by the process and/or thread indicated.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// Return the handler for functionality specific to the browser process.
        /// This method is called on multiple threads.
        /// </summary>
        IBrowserProcessHandler BrowserProcessHandler { get; }

        /// <summary>
        /// Provides an opportunity to register custom schemes. Do not keep a reference to the <paramref name="registrar"/> object.
        /// This method is called on the main thread for each process and the registered schemes should be the same across all processes.
        /// </summary>
        /// <param name="registrar">scheme registra</param>
        void OnRegisterCustomSchemes(ISchemeRegistrar registrar);        
    }
}
