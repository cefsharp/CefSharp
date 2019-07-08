// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// Manages custom scheme registrations.
    /// </summary>
    public interface ISchemeRegistrar
    {
        /// <summary>
        /// Register a custom scheme. This method should not be called for the built-in
        /// HTTP, HTTPS, FILE, FTP, ABOUT and DATA schemes.
        ///
        /// See <see cref="SchemeOptions"/> for possible values for <paramref name="schemeOptions"/>
        ///
        /// This function may be called on any thread. It should only be called once
        /// per unique <paramref name="schemeName"/> value. 
        /// </summary>
        /// <param name="schemeName">scheme name</param>
        /// <param name="schemeOptions">scheme options</param>
        /// <returns>If <paramref name="schemeName"/> is already registered or if an error occurs this method will return false.</returns>
        bool AddCustomScheme(string schemeName, SchemeOptions schemeOptions);
    }
}
