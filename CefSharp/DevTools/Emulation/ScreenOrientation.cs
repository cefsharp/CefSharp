// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// Screen orientation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ScreenOrientation : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Orientation type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Orientation angle.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("angle"), IsRequired = (true))]
        public int Angle
        {
            get;
            set;
        }
    }
}