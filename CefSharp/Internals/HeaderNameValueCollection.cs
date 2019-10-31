// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;

namespace CefSharp.Internals
{
    /// <summary>
    /// A <see cref="NameValueCollection"/> implementation
    /// that can optionally be set to ReadOnly (used to represent the CefHeaderMap)
    /// </summary>
    public class HeaderNameValueCollection : NameValueCollection
    {
        public void SetReadOnly()
        {
            IsReadOnly = true;
        }
    }
}
