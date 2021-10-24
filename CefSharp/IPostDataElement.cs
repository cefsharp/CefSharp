// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Class used to represent a single element in the request post data.
    /// The methods of this class may be called on any thread. 
    /// </summary>
    public interface IPostDataElement : IDisposable
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        string File { get; set; }

        /// <summary>
        /// Gets if the object is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Remove all contents from the post data element.
        /// </summary>
        void SetToEmpty();

        /// <summary>
        /// Gets the type of this <see cref="IPostDataElement"/>.
        /// </summary>
        PostDataElementType Type { get; }

        /// <summary>
        /// Gets or sets the bytes of this <see cref="IPostDataElement"/>.
        /// </summary>
        byte[] Bytes { get; set; }

        /// <summary>
        /// Used internally to get the underlying <see cref="IPostDataElement"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        IPostDataElement UnWrap();
    }
}
