// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Margin type for PDF printing.
    /// </summary>
    public enum CefPdfPrintMarginType
    {
        /// <summary>
        /// Default margins of 1cm (~0.4 inches)
        /// </summary>
        Default = 0,

        /// <summary>
        /// No margins.
        /// </summary>
        None,

        /// <summary>
        /// Custom margins.
        /// </summary>
        Custom
    }
}
