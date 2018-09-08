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
        Empty = 0,
        Bytes,
        File
    }
}
