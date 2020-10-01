// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Resource on the page.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FrameResource : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Resource URL.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.ResourceType Type
        {
            get
            {
                return (CefSharp.DevTools.Network.ResourceType)(StringToEnum(typeof(CefSharp.DevTools.Network.ResourceType), type));
            }

            set
            {
                type = (EnumToString(value));
            }
        }

        /// <summary>
        /// Type of this resource.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        internal string type
        {
            get;
            set;
        }

        /// <summary>
        /// Resource mimeType as determined by the browser.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mimeType"), IsRequired = (true))]
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// last-modified timestamp as reported by server.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lastModified"), IsRequired = (false))]
        public long? LastModified
        {
            get;
            set;
        }

        /// <summary>
        /// Resource content size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentSize"), IsRequired = (false))]
        public long? ContentSize
        {
            get;
            set;
        }

        /// <summary>
        /// True if the resource failed to load.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("failed"), IsRequired = (false))]
        public bool? Failed
        {
            get;
            set;
        }

        /// <summary>
        /// True if the resource was canceled during loading.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("canceled"), IsRequired = (false))]
        public bool? Canceled
        {
            get;
            set;
        }
    }
}