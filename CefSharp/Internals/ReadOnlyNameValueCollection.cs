// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Specialized;

namespace CefSharp.Internals
{
    public class ReadOnlyNameValueCollection : NameValueCollection
    {
        public ReadOnlyNameValueCollection()
        {
            IsReadOnly = true;
        }
    }
}
