// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about the request initiator.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Initiator : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Type of this initiator.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator JavaScript stack trace, set for Script only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stack"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.StackTrace Stack
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator URL, set for Parser type or for Script type (when script is importing module) or for SignedExchange type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Initiator line number, set for Parser type or for Script type (when script is importing
        /// module) (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (false))]
        public long? LineNumber
        {
            get;
            set;
        }
    }
}