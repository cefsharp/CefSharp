// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Represents an entry in navigation history.
    /// </summary>
    public sealed class NavigationEntry
    {
        /// <summary>
        /// Returns the time for the last known successful navigation completion.
        /// </summary>
        public DateTime CompletionTime { get; private set; }

        /// <summary>
        /// Returns a display-friendly version of the URL.
        /// </summary>
        public string DisplayUrl { get; private set; }

        /// <summary>
        /// Returns the HTTP status code for the last known successful navigation response.
        /// </summary>
        public int HttpStatusCode { get; private set; }

        /// <summary>
        /// Returns the original URL that was entered by the user before any redirects.
        /// </summary>
        public string OriginalUrl { get; private set; }

        /// <summary>
        /// Returns the title set by the page.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Returns the transition type which indicates what the user did to move to this page from the previous page.
        /// </summary>
        public TransitionType TransitionType { get; private set; }

        /// <summary>
        /// Returns the actual URL of the page.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Returns true if this navigation includes post data.
        /// </summary>
        public bool HasPostData { get; private set; }

        /// <summary>
        /// Returns true if this object is valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// If true if this entry is the currently loaded navigation entry
        /// </summary>
        public bool IsCurrent { get; private set; }

        /// <summary>
        /// Returns the SSL information for this navigation entry.
        /// </summary>
        public SslStatus SslStatus { get; private set; }

        /// <summary>
        /// NavigationEntry
        /// </summary>
        /// <param name="completionTime">completionTime</param>
        /// <param name="displayUrl">displayUrl</param>
        /// <param name="httpStatusCode">httpStatusCode</param>
        /// <param name="originalUrl">originalUrl</param>
        /// <param name="title">title</param>
        /// <param name="transitionType">transitionType</param>
        /// <param name="url">url</param>
        /// <param name="hasPostData">hasPostData</param>
        /// <param name="isValid">isValid</param>
        /// <param name="isCurrent">is the current entry</param>
        /// <param name="sslStatus">the ssl status</param>
        public NavigationEntry(bool isCurrent, DateTime completionTime, string displayUrl, int httpStatusCode, string originalUrl, string title, TransitionType transitionType, string url, bool hasPostData, bool isValid, SslStatus sslStatus)
        {
            IsCurrent = isCurrent;
            CompletionTime = completionTime;
            DisplayUrl = displayUrl;
            HttpStatusCode = httpStatusCode;
            OriginalUrl = originalUrl;
            Title = title;
            TransitionType = transitionType;
            Url = url;
            HasPostData = hasPostData;
            IsValid = isValid;
            SslStatus = sslStatus;
        }
    }
}
