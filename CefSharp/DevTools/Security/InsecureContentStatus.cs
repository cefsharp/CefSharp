// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Information about insecure content on the page.
    /// </summary>
    public class InsecureContentStatus
    {
        /// <summary>
        /// Always false.
        /// </summary>
        public bool RanMixedContent
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        public bool DisplayedMixedContent
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        public bool ContainedMixedForm
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        public bool RanContentWithCertErrors
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        public bool DisplayedContentWithCertErrors
        {
            get;
            set;
        }

        /// <summary>
        /// Always set to unknown.
        /// </summary>
        public string RanInsecureContentStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Always set to unknown.
        /// </summary>
        public string DisplayedInsecureContentStyle
        {
            get;
            set;
        }
    }
}