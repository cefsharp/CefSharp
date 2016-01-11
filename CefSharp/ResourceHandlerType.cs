// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public enum ResourceHandlerType
    {
        /// <summary>
        /// Resource is read from a Stream (Default)
        /// </summary>
        Stream = 0,

        /// <summary>
        /// Resource is read from a file on disk
        /// </summary>
        File = 1,
    }
}
