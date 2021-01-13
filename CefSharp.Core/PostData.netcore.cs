// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <inheritdoc/>
    public class PostData : CefSharp.Core.PostData
    {
        /// <summary>
        /// Create a new instance of <see cref="IPostData"/>
        /// </summary>
        /// <returns>PostData</returns>
        public static IPostData Create()
        {
            return new CefSharp.Core.PostData();
        }
    }
}
