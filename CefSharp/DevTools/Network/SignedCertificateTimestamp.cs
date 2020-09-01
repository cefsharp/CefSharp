// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Details of a signed certificate timestamp (SCT).
    /// </summary>
    public class SignedCertificateTimestamp
    {
        /// <summary>
        /// Validation status.
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Origin.
        /// </summary>
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Log name / description.
        /// </summary>
        public string LogDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Log ID.
        /// </summary>
        public string LogId
        {
            get;
            set;
        }

        /// <summary>
        /// Issuance date.
        /// </summary>
        public long Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Hash algorithm.
        /// </summary>
        public string HashAlgorithm
        {
            get;
            set;
        }

        /// <summary>
        /// Signature algorithm.
        /// </summary>
        public string SignatureAlgorithm
        {
            get;
            set;
        }

        /// <summary>
        /// Signature data.
        /// </summary>
        public string SignatureData
        {
            get;
            set;
        }
    }
}