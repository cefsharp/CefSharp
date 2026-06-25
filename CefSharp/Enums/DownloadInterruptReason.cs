// Copyright © 2026 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Download interrupt reasons.
    /// 
    /// For a complete up-to-date list, see the CEF source code
    /// (cef_download_interrupt_reason_t in include/internal/cef_types.h)
    /// and the Chromium source code (download/public/common/download_interrupt_reason_values.h).
    /// </summary>
    public enum DownloadInterruptReason
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,

        /// <summary>
        /// Generic file operation failure.
        /// </summary>
        FileFailed = 1,

        /// <summary>
        /// The file cannot be accessed due to security restrictions.
        /// </summary>
        FileAccessDenied = 2,

        /// <summary>
        /// There is not enough room on the drive.
        /// </summary>
        FileNoSpace = 3,

        /// <summary>
        /// The directory or file name is too long.
        /// </summary>
        FileNameTooLong = 5,

        /// <summary>
        /// The file is too large for the file system to handle.
        /// </summary>
        FileTooLarge = 6,

        /// <summary>
        /// The file contains a virus.
        /// </summary>
        FileVirusInfected = 7,

        /// <summary>
        /// The file was in use.
        /// Too many files are opened at once.
        /// We have run out of memory.
        /// </summary>
        FileTransientError = 10,

        /// <summary>
        /// The file was blocked due to local policy.
        /// </summary>
        FileBlocked = 11,

        /// <summary>
        /// An attempt to check the safety of the download failed due to unexpected
        /// reasons. See http://crbug.com/153212.
        /// </summary>
        FileSecurityCheckFailed = 12,

        /// <summary>
        /// An attempt was made to seek past the end of a file in opening
        /// a file (as part of resuming a previously interrupted download).
        /// </summary>
        FileTooShort = 13,

        /// <summary>
        /// The partial file didn't match the expected hash.
        /// </summary>
        FileHashMismatch = 14,

        /// <summary>
        /// The source and the target of the download were the same.
        /// </summary>
        FileSameAsSource = 15,

        /// <summary>
        /// Generic network failure.
        /// </summary>
        NetworkFailed = 20,

        /// <summary>
        /// The network operation timed out.
        /// </summary>
        NetworkTimeout = 21,

        /// <summary>
        /// The network connection has been lost.
        /// </summary>
        NetworkDisconnected = 22,

        /// <summary>
        /// The server has gone down.
        /// </summary>
        NetworkServerDown = 23,

        /// <summary>
        /// The network request was invalid. This may be due to the original URL or a
        /// redirected URL:
        /// - Having an unsupported scheme.
        /// - Being an invalid URL.
        /// - Being disallowed by policy.
        /// </summary>
        NetworkInvalidRequest = 24,

        /// <summary>
        /// The server indicates that the operation has failed (generic).
        /// </summary>
        ServerFailed = 30,

        /// <summary>
        /// The server does not support range requests.
        /// Internal use only:  must restart from the beginning.
        /// </summary>
        ServerNoRange = 31,

        /// <summary>
        /// The server does not have the requested data.
        /// </summary>
        ServerBadContent = 33,

        /// <summary>
        /// Server didn't authorize access to resource.
        /// </summary>
        ServerUnauthorized = 34,

        /// <summary>
        /// Server certificate problem.
        /// </summary>
        ServerCertProblem = 35,

        /// <summary>
        /// Server access forbidden.
        /// </summary>
        ServerForbidden = 36,

        /// <summary>
        /// Unexpected server response. This might indicate that the responding server
        /// may not be the intended server.
        /// </summary>
        ServerUnreachable = 37,

        /// <summary>
        /// The server sent fewer bytes than the content-length header. It may indicate
        /// that the connection was closed prematurely, or the Content-Length header was
        /// invalid. The download is only interrupted if strong validators are present.
        /// Otherwise, it is treated as finished.
        /// </summary>
        ServerContentLengthMismatch = 38,

        /// <summary>
        /// An unexpected cross-origin redirect happened.
        /// </summary>
        ServerCrossOriginRedirect = 39,

        /// <summary>
        /// The user canceled the download.
        /// </summary>
        UserCanceled = 40,

        /// <summary>
        /// The user shut down the browser.
        /// Internal use only:  resume pending downloads if possible.
        /// </summary>
        UserShutdown = 41,

        /// <summary>
        /// The browser crashed.
        /// Internal use only:  resume pending downloads if possible.
        /// </summary>
        Crash = 50
    }
}
