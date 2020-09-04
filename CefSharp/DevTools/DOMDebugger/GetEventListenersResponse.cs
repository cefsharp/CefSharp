// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// GetEventListenersResponse
    /// </summary>
    public class GetEventListenersResponse
    {
        /// <summary>
        /// Array of relevant listeners.
        /// </summary>
        public System.Collections.Generic.IList<EventListener> listeners
        {
            get;
            set;
        }
    }
}