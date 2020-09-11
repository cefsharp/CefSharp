// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Details of a signed certificate timestamp (SCT).
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SignedCertificateTimestamp : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Validation status.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("status"), IsRequired = (true))]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Log name / description.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("logDescription"), IsRequired = (true))]
        public string LogDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Log ID.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("logId"), IsRequired = (true))]
        public string LogId
        {
            get;
            set;
        }

        /// <summary>
        /// Issuance date.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timestamp"), IsRequired = (true))]
        public long Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Hash algorithm.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hashAlgorithm"), IsRequired = (true))]
        public string HashAlgorithm
        {
            get;
            set;
        }

        /// <summary>
        /// Signature algorithm.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signatureAlgorithm"), IsRequired = (true))]
        public string SignatureAlgorithm
        {
            get;
            set;
        }

        /// <summary>
        /// Signature data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("signatureData"), IsRequired = (true))]
        public string SignatureData
        {
            get;
            set;
        }
    }
}