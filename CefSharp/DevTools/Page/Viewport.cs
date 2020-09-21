// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Viewport for capturing screenshot.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Viewport : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// X offset in device independent pixels (dip).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("x"), IsRequired = (true))]
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Y offset in device independent pixels (dip).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("y"), IsRequired = (true))]
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle width in device independent pixels (dip).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (true))]
        public long Width
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle height in device independent pixels (dip).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (true))]
        public long Height
        {
            get;
            set;
        }

        /// <summary>
        /// Page scale factor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scale"), IsRequired = (true))]
        public long Scale
        {
            get;
            set;
        }
    }
}