// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Console
{
    /// <summary>
    /// Console message.
    /// </summary>
    public class ConsoleMessage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Message source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("source"), IsRequired = (true))]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Message severity.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("level"), IsRequired = (true))]
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Message text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the message origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the resource that generated this message (1-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("line"), IsRequired = (false))]
        public int? Line
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the resource that generated this message (1-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("column"), IsRequired = (false))]
        public int? Column
        {
            get;
            set;
        }
    }
}