// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Post data elements may represent either bytes or files.
    /// </summary>
    public enum PostDataElementType
    {
        /// <summary>
        /// An enum constant representing the empty option.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// An enum constant representing the bytes option.
        /// </summary>
        Bytes,
        /// <summary>
        /// An enum constant representing the file option.
        /// </summary>
        File
    }
}
