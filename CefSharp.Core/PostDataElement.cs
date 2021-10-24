// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

namespace CefSharp
{
    /// <summary>
    /// Class used to represent a single element in the request post data.
    /// The methods of this class may be called on any thread.
    /// </summary>
    public class PostDataElement : IPostDataElement
    {
        internal CefSharp.Core.PostDataElement postDataElement = new CefSharp.Core.PostDataElement();

        /// <inheritdoc/>
        public string File
        {
            get { return postDataElement.File; }
            set { postDataElement.File = value; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get { return postDataElement.IsReadOnly; }
        }

        /// <inheritdoc/>
        public PostDataElementType Type
        {
            get { return postDataElement.Type; }
        }

        /// <inheritdoc/>
        public byte[] Bytes
        {
            get { return postDataElement.Bytes; }
            set { postDataElement.Bytes = value; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            postDataElement.Dispose();
        }

        /// <inheritdoc/>
        public void SetToEmpty()
        {
            postDataElement.SetToEmpty();
        }

        /// <summary>
        /// Used internally to get the underlying <see cref="IPostDataElement"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IPostDataElement UnWrap()
        {
            return postDataElement;
        }

        /// <summary>
        /// Create a new instance of <see cref="IPostDataElement"/>
        /// </summary>
        /// <returns>PostDataElement</returns>
        public static IPostDataElement Create()
        {
            return new CefSharp.Core.PostDataElement();
        }
    }
}
