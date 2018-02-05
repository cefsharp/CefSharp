// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp;
using System;
using System.Collections.Generic;

namespace CefSharp.Event
{
    /// <summary>
    /// Event arguments for the <see cref="IJavascriptObjectRepository.ResolveObject"/> and
    /// <see cref="IJavascriptObjectRepository.ObjectBoundInJavascript"/> events
    /// </summary>
    public class JavascriptBindingEventArgs : EventArgs
    {
        /// <summary>
        /// The javascript object repository, used to register objects
        /// </summary>
        public IJavascriptObjectRepository ObjectRepository { get; private set; }
        
        /// <summary>
        /// Name of the object
        /// </summary>
        public string ObjectName { get; private set; }

        public JavascriptBindingEventArgs(IJavascriptObjectRepository objectRepository, string name)
        {
            ObjectRepository = objectRepository;
            ObjectName = name;
        }
    }
}
