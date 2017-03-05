// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Resource type for a request.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// Top level page.
        /// </summary>
        MainFrame = 0,
        /// <summary>
        /// Frame or iframe.
        /// </summary>
        SubFrame,
        /// <summary>
        /// CSS stylesheet.
        /// </summary>
        Stylesheet,
        /// <summary>
        /// External script.
        /// </summary>
        Script,
        /// <summary>
        /// Image (jpg/gif/png/etc).
        /// </summary>
        Image,
        /// <summary>
        /// Font.
        /// </summary>
        FontResource,
        /// <summary>
        /// Some other subresource. This is the default type if the actual type is unknown.
        /// </summary>
        SubResource,
        /// <summary>
        /// Object (or embed) tag for a plugin, or a resource that a plugin requested.
        /// </summary>
        Object,
        /// <summary>
        /// Media resource.
        /// </summary>
        Media,
        /// <summary>
        /// Main resource of a dedicated worker.
        /// </summary>
        Worker,
        /// <summary>
        /// Main resource of a shared worker.
        /// </summary>
        SharedWorker,
        /// <summary>
        /// Explicitly requested prefetch.
        /// </summary>
        Prefetch,
        /// <summary>
        /// Favicon.
        /// </summary>
        Favicon,
        /// <summary>
        /// XMLHttpRequest.
        /// </summary>
        Xhr,
        /// <summary>
        /// A request for a ping
        /// </summary>
        Ping,
        /// <summary>
        /// Main resource of a service worker.
        /// </summary>
        ServiceWorker,
        /// <summary>
        /// A report of Content Security Policy violations.
        /// </summary>
        CspReport,
        /// <summary>
        /// A resource that a plugin requested.
        /// </summary>
        PluginResource
    }
}
