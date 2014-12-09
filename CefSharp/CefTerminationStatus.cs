// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public enum CefTerminationStatus
    {
        ///
        // Non-zero exit status.
        ///
        AbnormalTermination = 0,

        ///
        // SIGKILL or task manager kill.
        ///
        ProcessWasKilled,

        ///
        // Segmentation fault.
        ///
        ProcessCrashed
    }
}
