// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

using CefSharp.Event;
using CefSharp.ModelBinding;

namespace CefSharp
{
    /// <summary>
    /// Javascript object repository, object are registered for binding
    /// One repository per ChromiumWebBrowser instance
    /// </summary>
    public interface IJavascriptObjectRepository : IDisposable
    {
        /// <summary>
        /// Register a javascript object
        /// The equivilient to RegisterJsObject is isAsync = false
        /// The equivilient RegisterAsyncJsObject is isAsync = true
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="objectToBind">object</param>
        /// <param name="isAsync">is async</param>
        /// <param name="options">binding options</param>
        void Register(string name, object objectToBind, bool isAsync = false, BindingOptions options = null);
        /// <summary>
        /// Has bound objects
        /// </summary>
        bool HasBoundObjects { get; }
        /// <summary>
        /// Is object bound
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>true if object with matching name bound</returns>
        bool IsBound(string name);

        /// <summary>
        /// Event handler is called when an object with a given name is requested for binding and is not yet
        /// registered with the repository
        /// </summary>
        event EventHandler<JavascriptBindingEventArgs> ResolveObject;

        /// <summary>
        /// Event handler is triggered when a object has been successfully bound on javascript
        /// </summary>
        event EventHandler<JavascriptBindingEventArgs> ObjectBoundInJavascript;
    }
}
