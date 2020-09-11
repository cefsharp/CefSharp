// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Information about a signed exchange response.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SignedExchangeError : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Error message.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("message"), IsRequired = (true))]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// The index of the signature which caused the error.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signatureIndex"), IsRequired = (false))]
        public int? SignatureIndex
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.SignedExchangeErrorField? ErrorField
        {
            get
            {
                return (CefSharp.DevTools.Network.SignedExchangeErrorField? )(StringToEnum(typeof(CefSharp.DevTools.Network.SignedExchangeErrorField? ), errorField));
            }

            set
            {
                errorField = (EnumToString(value));
            }
        }

        /// <summary>
        /// The field which caused the error.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("errorField"), IsRequired = (false))]
        internal string errorField
        {
            get;
            set;
        }
    }
}