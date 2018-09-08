// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Class used to represent a download item.
    /// </summary>
    public sealed class DownloadItem
    {
        /// <summary>
        /// Returns true if this object is valid. Do not call any other methods if this function returns false.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Returns true if the download is in progress.
        /// </summary>
        public bool IsInProgress { get; set; }

        /// <summary>
        /// Returns true if the download is complete.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Returns true if the download has been canceled or interrupted.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Returns a simple speed estimate in bytes/s.
        /// </summary>
        public Int64 CurrentSpeed { get; set; }

        /// <summary>
        /// Returns the rough percent complete or -1 if the receive total size is unknown.
        /// </summary>
        public int PercentComplete { get; set; }

        /// <summary>
        /// Returns the total number of bytes.
        /// </summary>
        public Int64 TotalBytes { get; set; }

        /// <summary>
        /// Returns the number of received bytes.
        /// </summary>
        public Int64 ReceivedBytes { get; set; }

        /// <summary>
        /// Returns the time that the download started
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Returns the time that the download ended
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Returns the full path to the downloaded or downloading file.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Returns the unique identifier for this download.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Returns the URL as it was before any redirects.
        /// </summary>
        public string OriginalUrl { get; set; }

        /// <summary>
        /// Returns the suggested file name.
        /// </summary>
        public string SuggestedFileName { get; set; }

        /// <summary>
        /// Returns the content disposition.
        /// </summary>
        public string ContentDisposition { get; set; }

        /// <summary>
        /// Returns the mime type.
        /// </summary>
        public string MimeType { get; set; }
    }
}
