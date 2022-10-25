// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// Popup Creation options
    /// </summary>
    public enum PopupCreation
    {
        /// <summary>
        /// Popup creation is cancled, no further action will occur
        /// </summary>
        Cancel = 0,
        /// <summary>
        /// Popup creation will continue as per normal.
        /// </summary>
        Continue,
        /// <summary>
        /// Popup creation will continue with javascript disabled.
        /// </summary>
        ContinueWithJavascriptDisabled
    }
}
