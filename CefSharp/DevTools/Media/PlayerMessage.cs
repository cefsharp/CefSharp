// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Media
{
    /// <summary>
    /// Have one type per entry in MediaLogRecord::Type
    public class PlayerMessage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Keep in sync with MediaLogMessageLevel
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("level"), IsRequired = (true))]
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Message
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("message"), IsRequired = (true))]
        public string Message
        {
            get;
            set;
        }
    }
}