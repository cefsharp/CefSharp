// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System.Collections.Generic;

namespace CefSharp
{
    /// <inheritdoc/>
    public class PostData : IPostData
    {
        private CefSharp.Core.PostData postData = new CefSharp.Core.PostData();

        /// <inheritdoc/>
        public IList<IPostDataElement> Elements
        {
            get { return postData.Elements; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get { return postData.IsReadOnly; }
        }

        /// <inheritdoc/>
        public bool IsDisposed
        {
            get { return postData.IsDisposed; }
        }

        /// <inheritdoc/>
        public bool HasExcludedElements
        {
            get { return postData.HasExcludedElements; }
        }

        /// <inheritdoc/>
        public bool AddElement(IPostDataElement element)
        {
            return postData.AddElement(element);
        }

        /// <inheritdoc/>
        public IPostDataElement CreatePostDataElement()
        {
            return new CefSharp.Core.PostDataElement();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            postData.Dispose();
        }

        /// <inheritdoc/>
        public bool RemoveElement(IPostDataElement element)
        {
            return postData.RemoveElement(element);
        }

        /// <inheritdoc/>
        public void RemoveElements()
        {
            postData.RemoveElements();
        }

        /// <summary>
        /// Used internally to get the underlying <see cref="IPostData"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IPostData UnWrap()
        {
            return postData;
        }

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
