// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Frame on the page.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Frame unique identifier.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Parent frame identifier.
        /// </summary>
        public string ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the loader associated with this frame.
        /// </summary>
        public string LoaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame's name as specified in the tag.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's URL without fragment.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's URL fragment including the '#'.
        /// </summary>
        public string UrlFragment
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's registered domain, taking the public suffixes list into account.
        public string DomainAndRegistry
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's security origin.
        /// </summary>
        public string SecurityOrigin
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's mimeType as determined by the browser.
        /// </summary>
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// If the frame failed to load, this contains the URL that could not be loaded. Note that unlike url above, this URL may contain a fragment.
        /// </summary>
        public string UnreachableUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether this frame was tagged as an ad.
        /// </summary>
        public string AdFrameType
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the main document is a secure context and explains why that is the case.
        /// </summary>
        public string SecureContextType
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether this is a cross origin isolated context.
        /// </summary>
        public string CrossOriginIsolatedContextType
        {
            get;
            set;
        }
    }
}