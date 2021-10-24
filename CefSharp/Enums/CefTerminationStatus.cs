// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Process termination status values. 
    /// </summary>
    public enum CefTerminationStatus
    {
        /// <summary>
        /// Non-zero exit status.
        /// </summary>
        AbnormalTermination = 0,
        /// <summary>
        /// SIGKILL or task manager kill.
        /// </summary>
        ProcessWasKilled,
        /// <summary>
        /// Segmentation fault.
        /// </summary>
        ProcessCrashed,
        /// <summary>
        /// Out of memory. Some platforms may use ProcessCrashed instead.
        /// </summary>
        OutOfMemory
    }
}
