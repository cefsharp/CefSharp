// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.IO;

namespace CefSharp
{
    public static class RequestContextExtensions
    {
        /// <summary>
        /// Load an extension from the given directory. manifest.json will be read from disk
        /// The loaded extension will be accessible in all contexts sharing the same storage (HasExtension returns true).
        /// However, only the context on which this method was called is considered the loader (DidLoadExtension returns true) and only the
        /// loader will receive IRequestContextHandler callbacks for the extension. <see cref="IExtensionHandler.OnExtensionLoaded"/> will be
        /// called on load success or <see cref="IExtensionHandler.OnExtensionLoadFailed"/> will be called on load failure.
        /// If the extension specifies a background script via the "background" manifest key then <see cref="IExtensionHandler.OnBeforeBackgroundBrowser"/>
        /// will be called to create the background browser. See that method for additional information about background scripts.
        /// For visible extension views the client application should evaluate the manifest to determine the correct extension URL to load and then pass
        /// that URL to the IBrowserHost.CreateBrowser* function after the extension has loaded. For example, the client can look for the "browser_action"
        /// manifest key as documented at https://developer.chrome.com/extensions/browserAction. Extension URLs take the form "chrome-extension:///".
        /// Browsers that host extensions differ from normal browsers as follows: - Can access chrome.* JavaScript APIs if allowed by the manifest.
        /// Visit chrome://extensions-support for the list of extension APIs currently supported by CEF. - Main frame navigation to non-extension
        /// content is blocked.
        /// - Pinch-zooming is disabled.
        /// - <see cref="IBrowserHost.GetExtension"/> returns the hosted extension.
        /// - <see cref="IBrowserHost.IsBackgroundHost"/> returns true for background hosts. See https://developer.chrome.com/extensions for extension implementation and usage documentation.
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension resources, manifest.json will be read from disk.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            if(!new Uri(rootDirectory).IsAbsoluteUri)
            {
                throw new ArgumentException("RootDirectory must be an absolute path(not relative)", "rootDirectory");
            }

            var manifest = Path.Combine(rootDirectory, "manifest.json");

            if (!File.Exists(manifest))
            {
                throw new FileNotFoundException("Extension manifest not found", "manifest.json");
            }

            requestContext.LoadExtension(rootDirectory, File.ReadAllText(manifest), handler);
        }

        /// <summary>
        /// Load an extension(s) from the given directory.
        /// The loaded extension will be accessible in all contexts sharing the same storage (HasExtension returns true).
        /// However, only the context on which this method was called is considered the loader (DidLoadExtension returns true) and only the
        /// loader will receive IRequestContextHandler callbacks for the extension. <see cref="IExtensionHandler.OnExtensionLoaded"/> will be
        /// called on load success or <see cref="IExtensionHandler.OnExtensionLoadFailed"/> will be called on load failure.
        /// If the extension specifies a background script via the "background" manifest key then <see cref="IExtensionHandler.OnBeforeBackgroundBrowser"/>
        /// will be called to create the background browser. See that method for additional information about background scripts.
        /// For visible extension views the client application should evaluate the manifest to determine the correct extension URL to load and then pass
        /// that URL to the IBrowserHost.CreateBrowser* function after the extension has loaded. For example, the client can look for the "browser_action"
        /// manifest key as documented at https://developer.chrome.com/extensions/browserAction. Extension URLs take the form "chrome-extension:///".
        /// Browsers that host extensions differ from normal browsers as follows: - Can access chrome.* JavaScript APIs if allowed by the manifest.
        /// Visit chrome://extensions-support for the list of extension APIs currently supported by CEF. - Main frame navigation to non-extension
        /// content is blocked.
        /// - Pinch-zooming is disabled.
        /// - <see cref="IBrowserHost.GetExtension"/> returns the hosted extension.
        /// - <see cref="IBrowserHost.IsBackgroundHost"/> returns true for background hosts. See https://developer.chrome.com/extensions for extension implementation and usage documentation.
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <param name="rootDirectory">absolute path to the directory that contains the extension(s) to be loaded.</param>
        /// <param name="handler">handle events related to browser extensions</param>
        public static void LoadExtensionsFromDirectory(this IRequestContext requestContext, string rootDirectory, IExtensionHandler handler)
        {
            requestContext.LoadExtension(Path.GetFullPath(rootDirectory), null, handler);
        }
    }
}
