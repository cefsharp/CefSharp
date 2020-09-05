// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// TargetInfo
    /// </summary>
    public class TargetInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("targetId"), IsRequired = (true))]
        public string TargetId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("title"), IsRequired = (true))]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the target has an attached client.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attached"), IsRequired = (true))]
        public bool Attached
        {
            get;
            set;
        }

        /// <summary>
        /// Opener target Id
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("openerId"), IsRequired = (false))]
        public string OpenerId
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the opened window has access to the originating window.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("canAccessOpener"), IsRequired = (true))]
        public bool CanAccessOpener
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("browserContextId"), IsRequired = (false))]
        public string BrowserContextId
        {
            get;
            set;
        }
    }
}