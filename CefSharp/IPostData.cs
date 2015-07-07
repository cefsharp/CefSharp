// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IPostData : IDisposable
    {
        bool AddElement(IPostDataElement element);
        IList<IPostDataElement> Elements { get; }
        bool IsReadOnly { get; }
        bool RemoveElement(IPostDataElement element);
        void RemoveElements();
    }
}
