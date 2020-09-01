// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// An explanation of an factor contributing to the security state.
    /// </summary>
    public class SecurityStateExplanation
    {
        /// <summary>
        /// Security state representing the severity of the factor being explained.
        /// </summary>
        public string SecurityState
        {
            get;
            set;
        }

        /// <summary>
        /// Title describing the type of factor.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Short phrase describing the type of factor.
        /// </summary>
        public string Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Full text explanation of the factor.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// The type of mixed content described by the explanation.
        /// </summary>
        public string MixedContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Page certificate.
        /// </summary>
        public string Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Recommendations to fix any issues.
        /// </summary>
        public string Recommendations
        {
            get;
            set;
        }
    }
}