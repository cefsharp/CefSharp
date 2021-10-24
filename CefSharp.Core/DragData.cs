// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <summary>
    /// Used to represent Drag Data.
    /// </summary>
    public static class DragData
    {
        /// <summary>
        /// Create a new instance of <see cref="IDragData"/>
        /// </summary>
        /// <returns>DragData</returns>
        public static IDragData Create()
        {
            return Core.DragData.Create();
        }
    }
}
