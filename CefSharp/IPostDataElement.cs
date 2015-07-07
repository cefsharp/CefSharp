// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IPostDataElement : IDisposable
    {
        string File { get; set; }
        bool IsReadOnly { get; }
        void SetToEmpty();
        int Type { get; }

        byte[] Bytes { get; set; }
    }
}
