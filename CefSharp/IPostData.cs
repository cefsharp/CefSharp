// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IPostData : IDisposable
    {
        //bool AddElement(IPostDataElement element);

        /// <summary>
        /// Retrieve the post data elements.
        /// </summary>
        IList<IPostDataElement> Elements { get; }
        
        /// <summary>
        /// Returns true if this object is read-only.
        /// </summary>
        bool IsReadOnly { get; }
        //bool RemoveElement(IPostDataElement element);
        
        /// <summary>
        /// Remove all existing post data elements.
        /// </summary>
        void RemoveElements();

        /// <summary>
        /// Gets a value indicating whether the object has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Create a new <see cref="IPostDataElement"/> instance
        /// </summary>
        /// <returns></returns>
        IPostDataElement CreatePostDataElement();
    }
}
