// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Return values for IResponseFilter
    /// </summary>
    public enum FilterStatus
    {
        /// <summary>
        /// Some or all of the pre-filter data was read successfully but more data is
        /// needed in order to continue filtering (filtered output is pending).
        /// </summary>
        NeedMoreData,
        
        /// <summary>
        /// Some or all of the pre-filter data was read successfully and all available filtered output has been written.
        /// </summary>
        Done,

        /// <summary>
        /// An error occurred during filtering.
        /// </summary>
        Error
    }
}
