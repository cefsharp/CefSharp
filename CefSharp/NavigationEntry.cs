// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Represents an entry in navigation history.
    /// </summary>
    public struct NavigationEntry
    {
        private DateTime completionTime;
        private string displayUrl;
        private int httpStatusCode;
        private string originalUrl;
        private string title;
        private TransitionType transitionType;
        private string url;
        private bool hasPostData;
        private bool isValid;
        private bool isCurrent;

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
        public NavigationEntry(bool isCurrent, DateTime completionTime, string displayUrl, int httpStatusCode, string originalUrl, string title, TransitionType transitionType, string url, bool hasPostData, bool isValid)
        {
            this.isCurrent = isCurrent;
            this.completionTime = completionTime;
            this.displayUrl = displayUrl;
            this.httpStatusCode = httpStatusCode;
            this.originalUrl = originalUrl;
            this.title = title;
            this.transitionType = transitionType;
            this.url = url;
            this.hasPostData = hasPostData;
            this.isValid = isValid;
        }

        /// <summary>
        /// Returns the time for the last known successful navigation completion.
        /// </summary>
        public DateTime CompletionTime
        {
            get { return completionTime; }
        }
          
        /// <summary>
        /// Returns a display-friendly version of the URL.
        /// </summary>
        public string DisplayUrl
        {
            get { return displayUrl; }
        }          
        
        /// <summary>
        /// Returns the HTTP status code for the last known successful navigation response.
        /// </summary>
        public int HttpStatusCode
        {
            get { return httpStatusCode; }
        }
          
        /// <summary>
        /// Returns the original URL that was entered by the user before any redirects.
        /// </summary>
        public string OriginalUrl
        {
            get { return originalUrl; }
        }
          
        /// <summary>
        /// Returns the title set by the page.
        /// </summary>
        public string Title
        {
            get { return title; }
        }
          
        /// <summary>
        /// Returns the transition type which indicates what the user did to move to this page from the previous page.
        /// </summary>
        public TransitionType TransitionType
        {
            get { return transitionType; }
        }
          
        /// <summary>
        /// Returns the actual URL of the page.
        /// </summary>
        public string Url
        {
            get { return url; }
        }
          
        /// <summary>
        /// Returns true if this navigation includes post data.
        /// </summary>
        public bool HasPostData
        {
            get { return hasPostData; }
        }
          
        /// <summary>
        /// Returns true if this object is valid.
        /// </summary>
        public bool IsValid
        {
            get {  return isValid;}
        }

        /// <summary>
        /// If true if this entry is the currently loaded navigation entry
        /// </summary>
        public bool IsCurrent
        {
            get { return isCurrent; }
        }
    }
}
