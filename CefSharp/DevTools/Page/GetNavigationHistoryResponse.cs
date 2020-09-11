// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetNavigationHistoryResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetNavigationHistoryResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal int currentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Index of the current navigation history entry.
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Page.NavigationEntry> entries
        {
            get;
            set;
        }

        /// <summary>
        /// Array of navigation history entries.
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Page.NavigationEntry> Entries
        {
            get
            {
                return entries;
            }
        }
    }
}