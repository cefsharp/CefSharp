// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Describes the width and height dimensions of an entity.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Size : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Width in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (true))]
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Height in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (true))]
        public int Height
        {
            get;
            set;
        }
    }
}