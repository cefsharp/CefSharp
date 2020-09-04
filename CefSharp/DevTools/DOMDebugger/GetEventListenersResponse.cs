// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMDebugger
{
    /// <summary>
    /// GetEventListenersResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetEventListenersResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<EventListener> listeners
        {
            get;
            set;
        }

        /// <summary>
        /// Array of relevant listeners.
        /// </summary>
        public System.Collections.Generic.IList<EventListener> Listeners
        {
            get
            {
                return listeners;
            }
        }
    }
}