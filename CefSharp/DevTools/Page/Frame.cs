// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Frame on the page.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Frame : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Frame unique identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Parent frame identifier.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parentId"), IsRequired = (false))]
        public string ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the loader associated with this frame.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("loaderId"), IsRequired = (true))]
        public string LoaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame's name as specified in the tag.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (false))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's URL without fragment.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's URL fragment including the '#'.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("urlFragment"), IsRequired = (false))]
        public string UrlFragment
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's security origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("securityOrigin"), IsRequired = (true))]
        public string SecurityOrigin
        {
            get;
            set;
        }

        /// <summary>
        /// Frame document's mimeType as determined by the browser.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mimeType"), IsRequired = (true))]
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// If the frame failed to load, this contains the URL that could not be loaded. Note that unlike url above, this URL may contain a fragment.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("unreachableUrl"), IsRequired = (false))]
        public string UnreachableUrl
        {
            get;
            set;
        }
    }
}