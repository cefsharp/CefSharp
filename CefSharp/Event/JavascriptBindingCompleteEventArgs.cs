// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Event
{
    /// <summary>
    /// Event arguments for the <see cref="IJavascriptObjectRepository.ObjectBoundInJavascript"/> event
    /// </summary>
    public class JavascriptBindingCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// The javascript object repository, used to register objects
        /// </summary>
        public IJavascriptObjectRepository ObjectRepository { get; private set; }
        
        /// <summary>
        /// Name of the object
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// Was the object already bound. The default is false for the first js call to 
        /// CefSharp.BindObjectAsync, and subsiquently true if already bound in a given context.
        /// </summary>
        public bool AlreadyBound { get; private set; }

        /// <summary>
        /// Is the object cached
        /// </summary>
        public bool IsCached { get; private set; }

        public JavascriptBindingCompleteEventArgs(IJavascriptObjectRepository objectRepository, string name, bool alreadyBound, bool isCached)
        {
            ObjectRepository = objectRepository;
            ObjectName = name;
            AlreadyBound = alreadyBound;
            IsCached = isCached;
        }
    }
}
