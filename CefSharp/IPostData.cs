// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IPostData : IDisposable
    {
        /// <summary>
        /// Adds the post data (key value pair) element. <param name="key"> should not be empty or null, value can be empty.
        /// </summary>
        bool AddElement(string key, string value);

        /// <summary>
        /// Adds the post data (full file path) element. <param name="fileName"> should not be empty or null, value can be empty.
        /// </summary>
        //bool AddElement(string fileName);

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
    }
}
