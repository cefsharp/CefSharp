// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// DisplayFeature
    /// </summary>
    public class DisplayFeature : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Orientation of a display feature in relation to screen
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("orientation"), IsRequired = (true))]
        public string Orientation
        {
            get;
            set;
        }

        /// <summary>
        /// The offset from the screen origin in either the x (for vertical
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offset"), IsRequired = (true))]
        public int Offset
        {
            get;
            set;
        }

        /// <summary>
        /// A display feature may mask content such that it is not physically
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maskLength"), IsRequired = (true))]
        public int MaskLength
        {
            get;
            set;
        }
    }
}