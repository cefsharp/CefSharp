// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System.Collections.Specialized;

namespace CefSharp
{
    /// <inheritdoc/>
    public class Request : IRequest
    {
        internal Core.Request request = new Core.Request();

        /// <inheritdoc/>
        public UrlRequestFlags Flags
        {
            get { return request.Flags; }
            set { request.Flags = value; }
        }

        /// <inheritdoc/>
        public string Url
        {
            get { return request.Url; }
            set { request.Url = value; }
        }

        /// <inheritdoc/>
        public ulong Identifier
        {
            get { return request.Identifier; }
        }

        /// <inheritdoc/>
        public string Method
        {
            get { return request.Method; }
            set { request.Method = value; }
        }

        /// <inheritdoc/>
        public string ReferrerUrl
        {
            get { return request.ReferrerUrl; }
        }

        /// <inheritdoc/>
        public ResourceType ResourceType
        {
            get { return request.ResourceType; }
        }

        /// <inheritdoc/>
        public ReferrerPolicy ReferrerPolicy
        {
            get { return request.ReferrerPolicy; }
        }

        /// <inheritdoc/>
        public NameValueCollection Headers
        {
            get { return request.Headers; }
            set { request.Headers = value; }
        }

        /// <inheritdoc/>
        public IPostData PostData
        {
            get { return request.PostData; }
            set { request.PostData = value; }
        }

        /// <inheritdoc/>
        public TransitionType TransitionType
        {
            get { return request.TransitionType; }
        }

        /// <inheritdoc/>
        public bool IsDisposed
        {
            get { return request.IsDisposed; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get { return request.IsReadOnly; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            request.Dispose();
        }

        /// <inheritdoc/>
        public string GetHeaderByName(string name)
        {
            return request.GetHeaderByName(name);
        }

        /// <inheritdoc/>
        public void InitializePostData()
        {
            request.InitializePostData();
        }

        /// <inheritdoc/>
        public void SetHeaderByName(string name, string value, bool overwrite)
        {
            request.SetHeaderByName(name, value, overwrite);
        }

        /// <inheritdoc/>
        public void SetReferrer(string referrerUrl, ReferrerPolicy policy)
        {
            request.SetReferrer(referrerUrl, policy);
        }

        /// <summary>
        /// Used internally to get the underlying <see cref="IRequest"/> instance.
        /// Unlikely you'll use this yourself.
        /// </summary>
        /// <returns>the inner most instance</returns>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IRequest UnWrap()
        {
            return request;
        }

        /// <summary>
        /// Create a new <see cref="IRequest"/> instance
        /// </summary>
        /// <returns>Request</returns>
        public static IRequest Create()
        {
            return new CefSharp.Core.Request();
        }
    }
}
