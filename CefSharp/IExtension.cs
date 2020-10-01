// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Object representing an extension. Methods may be called on any thread unless otherwise indicated.
    /// </summary>
    public interface IExtension
    {
        /// <summary>
        /// Returns the unique extension identifier. This is calculated based on the
        /// extension public key, if available, or on the extension path. See
        /// https://developer.chrome.com/extensions/manifest/key for details.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Returns the absolute path to the extension directory on disk. This value
        /// will be prefixed with PK_DIR_RESOURCES if a relative path was passed to
        /// IRequestContext.LoadExtension.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Returns the extension manifest contents as a CefDictionaryValue object. See
        /// https://developer.chrome.com/extensions/manifest for details.
        /// </summary>
        IDictionary<string, IValue> Manifest { get; }

        /// <summary>
        /// Returns true if this object is the same extension as that object.
        /// Extensions are considered the same if identifier, path and loader context
        /// match.
        /// </summary>
        /// <param name="that">extension to compare</param>
        /// <returns>return true if the same extension</returns>
        bool IsSame(IExtension that);

        /// <summary>
        /// Returns the request context that loaded this extension. Will return NULL
        /// for internal extensions or if the extension has been unloaded. See the
        /// CefRequestContext::LoadExtension documentation for more information about
        /// loader contexts. Must be called on the CEF UI thread.
        /// </summary>
        IRequestContext LoaderContext { get; }

        /// <summary>
        /// Returns true if this extension is currently loaded. Must be called on the
        /// CEF UI thread.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Unload this extension if it is not an internal extension and is currently
        /// loaded. Will result in a call to IExtensionHandler.OnExtensionUnloaded
        /// on success.
        /// </summary>
        void Unload();
    }
}
